using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Helpers;
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
            .Include(t => t.Institution)
            .Include(t => t.Groups)
            .FirstOrDefault(t => t.Id == id);
    }

    public TaskResponseModel? GetForResponse(long id)
    {
        return _mapper.Map<TaskResponseModel>(Get(id));
    }

    public TaskResponseModel? GetAny(long userId, long? subjectId, long? difficultyId, IAccountManager accountManager)
    {
        var user = accountManager.Get(userId);
        if (user is null) return null;
        var tasks = _repository.Tasks
            .Where(t => t.IsPublic && !t.IsExtended && t.InstitutionId == user.InstitutionId);
        if (subjectId != null)
        {
            tasks = tasks
                .Where(t => t.SubjectId == subjectId);
        }

        if (difficultyId != null)
        {
            tasks = tasks
                .Where(t => t.DifficultyId == difficultyId);
        }

        return _mapper.Map<TaskResponseModel>(tasks
            .OrderBy(t => _random.Next(int.MaxValue))
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .FirstOrDefault());
    }

    public TaskResponseModel? GetForStudent(long studentId, long taskId)
    {
        var student = _studentManager.Get(studentId);
        var task = GetForResponse(taskId);
        if (student is null || task is null) return null;
        if (student.InstitutionId == task.Institution.Id)
            return task;
        return null;
    }

    public TaskResponseModel? GetForTeacher(long teacherId, long taskId)
    {
        var teacher = _teacherManager.Get(teacherId);
        var task = GetForResponse(taskId);
        if (teacher is null || task is null) return null;
        if (teacher.InstitutionId == task.Institution.Id)
            return task;
        return null;
    }

    public IEnumerable<SubjectApiModel> GetSubjects()
    {
        return _subjectRepository.Subjects
            .Select(s => _mapper.Map<SubjectApiModel>(s));
    }

    public IEnumerable<Difficulty> GetAvailableDifficulties()
    {
        return _repository.Difficulties;
    }

    public async Task<IEnumerable<TaskPreviewApiModel>> GetCurrentForStudentAsync(long studentId, long? groupId)
    {
        if (!_groupRepository.GroupStudents.Any(gs =>
                gs.GroupId == groupId && gs.StudentId == studentId && gs.IsApproved))
            return Array.Empty<TaskPreviewApiModel>();
        var solvedTasks = _repository.SolvedTasks
            .Where(st => st.StudentId == studentId)
            .Select(t => t.Task);
        return _repository.Tasks
            .Where(t => t.Groups.Any(g => g.Id == groupId) && !solvedTasks.Any(st => st.Id == t.Id))
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .OrderBy(t => t.CreationDateTime)
            .Take(20)
            .Select(t => _mapper.Map<TaskPreviewApiModel>(t));
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

    public async Task<IEnumerable<TaskPreviewApiModel>> GetAssignedTasksAsync(long teacherId, long? groupId,
        DateTime? period)
    {
        period ??= DateTime.MinValue;
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        if (teacher is null)
            return Array.Empty<TaskPreviewApiModel>();
        return _repository.Tasks
            .Where(t => t.TeacherId == teacherId && (groupId == null || t.Groups.Any(g => g.Id == groupId)) &&
                        !IsExpired(t) && t.Deadline >= period.Value)
            .OrderBy(t => t.CreationDateTime)
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .Select(t => _mapper.Map<TaskPreviewApiModel>(t));
    }

    public async Task<IEnumerable<TaskResponseModel>> GetOutdatedTasksAsync(long teacherId, DateTime? period)
    {
        var teacher = await _teacherManager.GetAsync(teacherId);
        if (teacher is null)
            return Array.Empty<TaskResponseModel>();
        period ??= DateTime.Now.AddMonths(-1);
        return _repository.Tasks
            .Where(t => t.TeacherId == teacherId && !IsExpired(t, period.Value))
            .OrderBy(t => t.Deadline)
            .Include(t => t.Difficulty)
            .Include(t => t.Subject)
            .Select(t => _mapper.Map<TaskResponseModel>(t));
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

    public int GetCountOfSolved(long studentId, long groupId)
    {
        return _repository.SolvedTasks
            .Count(st => st.StudentId == studentId && st.Task.Groups.Any(g => g.Id == groupId) &&
                         st.Student.GroupsStudent.Any(gs =>
                             gs.IsApproved &&
                             gs.GroupId == groupId));
    }

    public IEnumerable<SolvedTaskPreviewModel> GetSolvedTasksPreviewAsync(long studentId, long? groupId)
    {
        if (!_groupRepository.GroupStudents.Any(gs =>
                gs.GroupId == groupId && gs.StudentId == studentId && gs.IsApproved))
            return Array.Empty<SolvedTaskPreviewModel>();
        var tasks = _repository.SolvedTasks
            .Where(st => st.StudentId == studentId && st.IsChecked == true && !st.Task.IsExtended &&
                         (groupId == null || st.Task.Groups.Any(g => g.Id == groupId)))
            .OrderBy(st => st.SolveTime)
            .Include(st => st.Task)
            .Select(st => ConvertHelper.ConvertToPreview(st));
        return tasks;
    }

    public SolvedTaskApiModel? GetSolvedTask(long studentId, long taskId)
    {
        var solvedTask = _repository.SolvedTasks
            .Include(st => st.Task)
            .Include(st => st.Task.Subject)
            .Include(st => st.Task.Difficulty)
            .FirstOrDefault(st => st.StudentId == studentId && st.TaskId == taskId);
        if (solvedTask is null) return null;
        return ConvertHelper.ConvertToSolved(solvedTask);
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
        if (!CanStudentSolveTask(studentId, model.TaskId)) return false;
        var task = _repository.Tasks
            .Include(t => t.Difficulty)
            .FirstOrDefault(t => t.Id == model.TaskId);
        if (task is null || task.IsExtended) return false;
        var result = CheckAnswer(task, model.Answer);
        if (!result) return result;
        var solvedTask = new SolvedTask
        {
            Answer = model.Answer,
            SolveTime = DateTime.Now,
            StudentId = studentId,
            Task = task,
            Scores = task.Difficulty.Scores,
            IsChecked = true
        };
        _repository.SolvedTasks.Add(solvedTask);
        _repository.SaveChanges();
        return result;
    }

    public IEnumerable<SolvedExtendedTaskPreview> GetUnchecked(long teacherId, long? groupId)
    {
        var students = _groupRepository.Groups
            .Where(g => (groupId == null || g.Id == groupId) && g.TeacherId == teacherId)
            .SelectMany(g => g.GroupsStudent)
            .Where(gs => gs.IsApproved)
            .Select(gs => gs.Student)
            .Distinct();
        return _repository.SolvedTasks
            .Where(st => !st.IsChecked && st.Task.IsExtended && students.Any(s => s.UserId == st.StudentId))
            .Select(st => new SolvedExtendedTaskPreview(st.StudentId, st.TaskId));
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
        if (!CanStudentSolveTask(studentId, taskId)) return false;
        var task = Get(taskId);
        if (task is null || !task.IsExtended) return false;
        var bytes = await GetByteArrayFromFileAsync(fileAnswer);
        var solvedTask = new SolvedTask
        {
            StudentId = studentId,
            TaskId = taskId,
            SolveTime = DateTime.Now,
            FileAnswer = new FileData()
            {
                Content = bytes,
                FileName = ".pdf",
                ContentType = "application/pdf",
            },
        };
        task.SolvedTasks.Add(solvedTask);
        await _repository.SaveChangesAsync();
        return true;
    }

    private bool CanStudentSolveTask(long studentId, long taskId)
    {
        var student = _studentManager.GetWithDetails(studentId);
        var task = Get(taskId);
        if (task is null || IsExpired(task) || student is null ||
            student.InstitutionId != task.InstitutionId) return false;
        if (_repository.SolvedTasks.Any(st => st.TaskId == taskId && st.StudentId == studentId)) return false;
        if (task is { IsPublic: true, IsExtended: false }) return true;
        var groupsContainsTask = _groupRepository.GroupStudents
            .Where(gs => gs.IsApproved && gs.StudentId == studentId)
            .SelectMany(gs => gs.Group.Tasks)
            .Any(t => t.Id == taskId);
        return groupsContainsTask;
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
        // здесь можно сравнивать теоритический ответ и ответ, полученный от chatGPT
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