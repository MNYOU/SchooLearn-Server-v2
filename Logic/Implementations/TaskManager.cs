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
    private readonly ITaskRepository _repository;
    private readonly ITeacherManager _teacherManager;
    private readonly IStudentManager _studentManager;
    private readonly IGroupRepository _groupRepository;

    public TaskManager(IMapper mapper, ITaskRepository repository, IStudentManager studentManager,
        ITeacherManager teacherManager, IGroupRepository groupRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _studentManager = studentManager;
        _teacherManager = teacherManager;
        _groupRepository = groupRepository;
    }

    public async Task<TaskResponseModel> GetAny()
    {
        var task = _mapper.Map<TaskResponseModel>(_repository.Tasks.FirstOrDefault() ?? new Task()
        {
            Name = "Теоретическая геометрия", Description = "Найдите количество углов у круга",
            Difficulty = new Difficulty() { Name = "Невозможно", Scores = 100 }
        });
        return task;
    }

    public Task? Get(long id)
    {
        return _repository.Tasks
            .Include(t => t.Subject)
            .Include(t => t.Difficulty)
            .Include(t => t.Groups)
            .FirstOrDefault(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskResponseModel>> GetCurrentForStudentAsync(long studentId)
    {
        var student = await _studentManager.GetAsync(studentId);
        if (student is null)
            return Array.Empty<TaskResponseModel>();
        return student.GroupsStudent
            .SelectMany(g => g.Tasks)
            .Select(t => _mapper.Map<TaskResponseModel>(t));
    }

    public async Task<IEnumerable<TaskResponseModel>> GetAssignedTasksAsync(long teacherId)
    {
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        if (teacher is null)
            return Array.Empty<TaskResponseModel>();
        var now = DateTime.Now;
        return teacher.Groups
            .SelectMany(g => g.Tasks)
            // тут возможна неправильная фильтрация
            .Where(t => DateTime.Compare(now, t.ExecutionPeriod) >= 0)
            .Select(t => _mapper.Map<TaskResponseModel>(t));
    }

    public IEnumerable<StudentApiModel> GetStudentsWhoCompletedTask(long teacherId, long taskId)
    {
        var students = _teacherManager
            .GetMyGroups(teacherId)
            .SelectMany(g => g.GroupsStudent)
            .Distinct();
        return _repository.SolvedTasks
            .Where(t => t.TaskId == taskId)
            .Where(t => students.Any(s => t.StudentId == s.UserId))
            .OrderBy(t => t.SolveTime)
            .Select(t => new StudentApiModel());
    }

    public async Task<IEnumerable<TaskPreviewApiModel>> GetSolvedTasksAsync(long studentId)
    {
        // сортировка по дате
        var tasks = _repository.SolvedTasks
            .Where(s => s.StudentId == studentId)
            .Select(s => s.Task)
            .Include(t => t.Subject)
            .Include(t => t.Difficulty)
            .Select(s => _mapper.Map<TaskPreviewApiModel>(s));
        return tasks;
    }
    
    public SolvedTaskApiModel? GetSolvedTask(long studentId, long taskId)
    { 
        var solvedTask = _repository.SolvedTasks
            .Include(st => st.Task.Subject)
            .Include(st => st.Task.Difficulty)
            .FirstOrDefault(st => st.StudentId == studentId && st.TaskId == taskId);
        return _mapper.Map<SolvedTaskApiModel>(solvedTask?.Task);
    }

    public async Task<bool> TryAddTaskForGroupAsync(TaskApiModel model, long teacherId, long groupId)
    {
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        var group = teacher?.Groups.FirstOrDefault(g => g.Id == groupId);
        if (group is null) return false;
        var task = _mapper.Map<Task>(model);
        group.Tasks.Add(task);
        await _groupRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TryAddTaskForGroupsAsync(TaskApiModel model, long teacherId,IEnumerable<GroupApiModel> groupApiModels)
    {
        // не оптимизированно
        // использование GroupApiModel кажется излишним 
        var teacher = await _teacherManager.GetWithDetailsAsync(teacherId);
        var groups = teacher?.Groups
            .Where(g => groupApiModels.Any(apiModel => g.Id == apiModel.Id));
        if (groups is null) return false;
        var task = _mapper.Map<Task>(model);
        if (CheckTaskForCorrectness(task)) return false;
        task.Groups
            .ToList()
            .AddRange(groups);
        await _repository.Tasks.AddAsync(task);
        await _repository.SaveChangesAsync();
        return true;
    }

    public bool CheckAndPointTask(TaskWithAnswerRequest model, long studentId)
    {
        var task = Get(model.TaskId);
        if (model.Answer == "" || task is null) return false;
        var result = task.Answer == model.Answer;
        if (!result) return result;
        var student = _studentManager.Get(studentId);
        if (student is null) return result;
        var solvedTask = new SolvedTask
        {
            Answer = model.Answer,
            SolveTime = DateTime.Now,
            Student = student,
            Task = task
        };
        student.SolvedTasks.Add(solvedTask);
        return result;
    }

    public async Task<bool> RateAnswerAsync(long teacherId, long studentId, long taskId, byte percentages)
    {
        var student = await _studentManager.GetAsync(studentId);
        var group = student?.GroupsStudent.FirstOrDefault(g => g.Tasks.Any(t => t.Id == taskId));
        if (student is null || group is null || group.TeacherId != teacherId)
            return false;
        var solvedTask = _repository.SolvedTasks
            .Include(st => st.Task)
            .ThenInclude(t => t.Difficulty)
            .FirstOrDefault(st => st.StudentId == studentId && st.TaskId == taskId);
        if (solvedTask is null || percentages is < 0 or > 100) return false;
        var dif = solvedTask.Task.Difficulty;
        solvedTask.Scores = (float)Math.Round(percentages * (dif.Scores / 100.0), 2);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UploadAnswerToTaskAsync(long studentId, long taskId, IFormFile fileAnswer)
    {
        // мануально устанавливать имя файла
        var task = Get(taskId);
        if (task is null || !task.IsExtended) return false;
        var student = await _studentManager.GetAsync(studentId);
        if (student is null) return false;
        var contentType = "application/pdf";
        var bytes = await GetByteArrayFromFileAsync(fileAnswer);
        var solvedTask = new SolvedTask()
        {
            StudentId = student.UserId,
            TaskId = task.Id,
            SolveTime = DateTime.Now,
            FileAnswer = new FileData()
            {
                Content = bytes,
                FileName = "defaultFileName",
                ContentType = contentType,
            },
        };
        task.SolvedTasks.Add(solvedTask);
        await _repository.SaveChangesAsync();
        return true;
    }

    public FileAnswer? DownloadAnswer(long studentId, long taskId)
    {
        var solvedTask = _repository.SolvedTasks
            .FirstOrDefault(st => st.TaskId == taskId && st.StudentId == studentId);
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


    private bool CheckTaskForCorrectness(Task task)
    {
        return true;
    }
}