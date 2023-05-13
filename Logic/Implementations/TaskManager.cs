using AutoMapper;
using Dal.Entities;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Task = Dal.Entities.Task;

namespace Logic.Implementations;

public class TaskManager : ITaskManager
{
    private readonly IMapper _mapper;
    private readonly Random _random;
    private readonly ITaskRepository _repository;
    private readonly IGroupRepository _groupRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherManager _teacherManager;
    private readonly IStudentManager _studentManager;

    public TaskManager(IMapper mapper, ITaskRepository repository, IStudentManager studentManager,
        ITeacherManager teacherManager, IGroupRepository groupRepository, Random random,
        ISubjectRepository subjectRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _studentManager = studentManager;
        _teacherManager = teacherManager;
        _groupRepository = groupRepository;
        _random = random;
        _subjectRepository = subjectRepository;
    }

    public Task? Get(long id)
    {
        return _repository.Tasks
            .Include(t => t.Subject)
            .Include(t => t.Difficulty)
            .Include(t => t.Groups)
            .FirstOrDefault(t => t.Id == id);
    }

    public TaskResponseModel? GetAny(long institutionId, string? subjectName, string? difficultyName)
    {
        var tasks = _repository.Tasks
            .Where(t => t.IsPublic && !t.IsExtended && t.InstitutionId == institutionId);
        if (subjectName != null)
        {
            tasks = tasks
                .Where(t => t.Subject.Name == subjectName);
        }

        if (difficultyName != null)
        {
            tasks = tasks
                .Where(t => t.Difficulty.Name == difficultyName);
        }

        return _mapper.Map<TaskResponseModel>(tasks
            .OrderBy(t => _random.Next(int.MaxValue))
            .FirstOrDefault());
    }

    public async Task<IEnumerable<TaskResponseModel>> GetCurrentForStudentAsync(long studentId)
    {
        var student = await _studentManager.GetAsync(studentId);
        if (student is null)
            return Array.Empty<TaskResponseModel>();
        var solvedTasks = _repository.SolvedTasks
            .Where(st => st.StudentId == studentId)
            .Select(t => t.Task);
        return _groupRepository.GroupStudents
            .Where(gs => gs.StudentId == studentId && gs.IsApproved)
            .SelectMany(gs => gs.Group.Tasks)
            .Where(t => !IsExpired(t) && !solvedTasks.Contains(t))
            .Include(t => t.Difficulty)
            .Include(t => t.Difficulty)
            .OrderBy(t => t.CreationDateTime)
            .Take(20)
            .Select(t => _mapper.Map<TaskResponseModel>(t));
    }

    public async Task<IEnumerable<TaskResponseModel>> GetExpiredTasksAsync(long studentId, DateTime? period)
    {
        var student = await _studentManager.GetAsync(studentId);
        if (student is null)
            return Array.Empty<TaskResponseModel>();

        period ??= DateTime.Now.AddMonths(-1);
        return _groupRepository.GroupStudents
            .Where(gs => gs.StudentId == studentId && gs.IsApproved)
            .SelectMany(gs => gs.Group.Tasks)
            .Where(t => !IsExpired(t, period.Value))
            .Include(t => t.Difficulty)
            .Include(t => t.Difficulty)
            .OrderBy(t => t.CreationDateTime)
            .Select(t => _mapper.Map<TaskResponseModel>(t));
    }

    public async Task<IEnumerable<TaskFullApiModel>> GetAssignedTasksAsync(long teacherId, DateTime? period)
    {
        period ??= DateTime.MinValue;
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        if (teacher is null)
            return Array.Empty<TaskFullApiModel>();
        return _repository.Tasks
            .Where(t => t.TeacherId == teacherId && !IsExpired(t) && t.Deadline >= period.Value)
            .OrderBy(t => t.CreationDateTime)
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .Select(t => _mapper.Map<TaskFullApiModel>(t));
    }

    public async Task<IEnumerable<TaskFullApiModel>> GetOutdatedTasksAsync(long teacherId, DateTime? period)
    {
        var teacher = await _teacherManager.GetAsync(teacherId);
        if (teacher is null)
            return Array.Empty<TaskFullApiModel>();
        period ??= DateTime.Now.AddMonths(-1);
        return _repository.Tasks
            .Where(t => t.TeacherId == teacherId && !IsExpired(t, period.Value))
            .OrderBy(t => t.Deadline)
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .Select(t => _mapper.Map<TaskFullApiModel>(t));
    }

    public IEnumerable<StudentApiModel> GetStudentsWhoCompletedTask(long teacherId, long taskId)
    {
        // это повзоляет получать только тех студентов, которые есть в группах преподавателя
        var students = _teacherManager
            .GetMyGroups(teacherId)
            .SelectMany(g => g.GroupsStudent.Select(gs => gs.Student))
            .Distinct();
        return _repository.SolvedTasks
            .Where(t => t.TaskId == taskId && students.Any(s => t.StudentId == s.UserId))
            .OrderBy(t => t.SolveTime)
            .Select(t => new StudentApiModel());
    }

    public IEnumerable<TaskPreviewApiModel> GetSolvedTasksPreviewAsync(long studentId)
    {
        // TODO нужно создать карту для TaskPreviewApiModel, которая будет вытягивать Scores из сложности
        var tasks = _repository.SolvedTasks
            .Where(st => st.StudentId == studentId && st.IsChecked == true && !st.Task.IsExtended)
            .OrderBy(st => st.SolveTime)
            .Include(st => st.Task)
            .ThenInclude(t => t.Difficulty)
            .Select(st => _mapper.Map<TaskPreviewApiModel>(st.Task));
        return tasks;
    }

    public SolvedTaskApiModel? GetSolvedTask(long studentId, long taskId)
    {
        // TODO нужно создать карту для SolvedTaskApiModel, которая будет вытягивать Scores из сложности
        var solvedTask = _repository.SolvedTasks
            .Include(st => st.Task)
            .Include(st => st.Task.Subject)
            .Include(st => st.Task.Difficulty)
            .FirstOrDefault(st => st.StudentId == studentId && st.TaskId == taskId);
        return _mapper.Map<SolvedTaskApiModel>(solvedTask);
    }

    private async Task<Task?> MapTaskAsync(TaskApiModel model, long institutionId, long teacherId)
    {
        var difficulty = await _repository.Difficulties.FirstOrDefaultAsync(d => d.Name == model.Difficulty);
        var subject = await _subjectRepository.Subjects.FirstOrDefaultAsync(d => d.Name == model.Subject);
        if (difficulty == null || subject == null) return null;
        var task = _mapper.Map<Task>(model);
        task.InstitutionId = institutionId;
        task.TeacherId = teacherId;
        return CheckTaskForCorrectness(task) ? task : null;
    }


    public async Task<bool> TryAddTaskInRepositoryAsync(long teacherId, TaskApiModel model)
    {
        var teacher = await _teacherManager.GetAsync(teacherId);
        if (teacher is null) return false;
        var task = await MapTaskAsync(model, teacher.InstitutionId, teacherId);
        if (task is null) return false;
        await _repository.Tasks.AddAsync(task);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TryAddTaskForGroupAsync(long teacherId, long groupId, TaskApiModel model)
    {
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        var group = teacher?.Groups.FirstOrDefault(g => g.Id == groupId);
        if (teacher is null || group is null) return false;
        var task = await MapTaskAsync(model, teacher.InstitutionId, teacherId);
        if (task is null) return false;
        group.Tasks.Add(task);
        await _groupRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTaskAsync(long teacherId, TaskApiModel model)
    {
        var teacher = await _teacherManager.GetAsync(teacherId);
        var task = await _repository.Tasks.FirstOrDefaultAsync(t => t.Id == model.Id && t.TeacherId == teacherId);
        if (teacher != null && task != null)
        {
            var newTask = await MapTaskAsync(model, teacher.InstitutionId, teacherId);
            if (newTask == null) return false;
            _repository.Tasks.Update(newTask);
            await _repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public bool DeleteTask(long teacherId, long taskId)
    {
        var task = _repository.Tasks.FirstOrDefault(t => t.Id == taskId && t.TeacherId == teacherId);
        if (task != null)
        {
            _repository.Tasks.Remove(task);
            return true;
        }

        return false;
    }


    public async Task<bool> TryAddTaskForGroupsAsync(long teacherId, long taskId,
        IEnumerable<GroupApiModel> groupApiModels)
    {
        var task = _repository.Tasks.FirstOrDefault(t => t.Id == taskId && t.TeacherId == teacherId);
        if (task != null)
        {
            var groups = _groupRepository.Groups
                .Where(g => g.TeacherId == teacherId && groupApiModels.Any(gap => gap.Name == g.Name));
            foreach (var group in groups)
            {
                task.Groups.Add(group);
            }

            await _repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public bool CheckAndPointTask(TaskWithAnswerRequest model, long studentId)
    {
        var task = _repository.Tasks
            .Include(t => t.Difficulty)
            .FirstOrDefault(t => t.Id == model.TaskId);
        if (task is null || IsExpired(task) || task.IsExtended ||
            _repository.SolvedTasks.Any(st => st.TaskId == model.TaskId && st.StudentId == studentId)) return false;
        var result = CheckAnswer(task, model.Answer);
        if (!result) return result;
        var student = _studentManager.Get(studentId);
        if (student is null) return result;
        var solvedTask = new SolvedTask
        {
            Answer = model.Answer,
            SolveTime = DateTime.Now,
            Student = student,
            Task = task,
            Scores = task.Difficulty.Scores,
            IsChecked = true
        };
        student.SolvedTasks.Add(solvedTask);
        _repository.SaveChanges();
        return result;
    }

    public bool GetUnchecked(long teacherId)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> RateAnswerAsync(long teacherId, long studentId, long taskId, byte percentages)
    {
        var solvedTask = await _repository.SolvedTasks
            .Include(st => st.Student)
            .Include(st => st.Task)
            .ThenInclude(st => st.Difficulty)
            .FirstOrDefaultAsync(st => st.TaskId == taskId && st.StudentId == studentId);
        if (solvedTask is null || solvedTask.Task.TeacherId != teacherId || percentages > 100)
            return false;
        solvedTask.Scores = CalculateScores(solvedTask.Task.Difficulty, percentages);
        await _repository.SaveChangesAsync();
        return true;
    }


    public async Task<bool> UploadAnswerToTaskAsync(long studentId, long taskId, IFormFile fileAnswer)
    {
        // TODO если в разных группах одинаковое задание
        // проверять, что у ученика есть эта задача в выданных
        return false;
        // var solvedTask = await _repository.SolvedTasks
        //     .FirstOrDefaultAsync(st => st.StudentId == studentId && st.TaskId == taskId);
        // if (solvedTask != null) return false;
        // var task = Get(taskId);
        // // TODO задание должно приходить с группами
        // if (task is null || !task.IsExtended || IsExpired(task)) return false;
        // var student = await _studentManager.GetWithDetailsAsync(studentId);
        // if (student is null) return false;
        // var studentHasTask = student.GroupsStudent
        //     .Where(gs => gs.IsApproved)
        //     .SelectMany(gs => gs.Group.Tasks)
        //     .Any(t => t.Id == taskId);
        // var bytes = await GetByteArrayFromFileAsync(fileAnswer);
        // solvedTask = new SolvedTask
        // {
        //     StudentId = student.UserId,
        //     TaskId = task.Id,
        //     SolveTime = DateTime.Now,
        //     FileAnswer = new FileData()
        //     {
        //         Content = bytes,
        //         FileName = ".pdf",
        //         ContentType = "application/pdf",
        //     },
        // };
        // task.SolvedTasks.Add(solvedTask);
        // await _repository.SaveChangesAsync();
        // return true;
    }

    private bool CanStudentSolveTask(long studentId, long taskId)
    {
        return false;
        // // если задание решено успешно, его больше нельзя решать
        // // а если он получил не макс балл за расширенное задание, то?
        // var task = _studentManager.Get(studentId)?.GroupsStudent
        //     .Where(gs => gs.IsApproved)
        //     .SelectMany(gs => gs.Group.Tasks)
        //     .FirstOrDefault(t => t.Id == taskId);
        // if ()
        // var solvedTasks = _repository.SolvedTasks
        //     .Where(st => st.StudentId == studentId)
        //     .Select(st => st.Task);
        // return false;
    }

    public FileAnswer? DownloadAnswer(long studentId, long taskId)
    {
        var solvedTask = _repository.SolvedTasks
            .FirstOrDefault(st => st.TaskId == taskId && st.StudentId == studentId && st.Task.IsExtended);
        return _mapper.Map<FileAnswer>(solvedTask?.FileAnswer);
    }

    private async Task<byte[]> GetByteArrayFromFileAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        if (file.Length < 5242880)
            return memoryStream.ToArray();
        return Array.Empty<byte>();
    }

    public static bool CheckAnswer(Task task, string answer)
    {
        if (task is null || answer == "" || task.Answer is null)
            return false;
        var expectedAnswer = task.Answer;
        // answer = answer.Split().Join()
        return string.Equals(expectedAnswer, answer, StringComparison.CurrentCultureIgnoreCase);
    }

    public static bool IsExpired(Task task)
    {
        return IsExpired(task, DateTime.Now);
    }

    public static bool IsExpired(Task task, DateTime dateTime)
    {
        return dateTime > task.Deadline;
    }

    public static bool CheckTaskForCorrectness(Task task)
    {
        //TODO здесь можно сравнивать теоритический ответ и ответ, полученный от chatGPT
        if (task is { IsExtended: false, Answer: null or "" })
            return false;
        return true;
    }

    public static float CalculateScores(byte scores, byte percentages)
    {
        return (float)Math.Round(percentages * (scores / 100.0), 2);
    }

    public static float CalculateScores(Difficulty difficulty, byte percentages)
    {
        return CalculateScores(difficulty.Scores, percentages);
    }
}