using Dal.Entities;
using Logic.ApiModels;
using Task = Dal.Entities.Task;

// using Task = System.Threading.Tasks.Task;

namespace Logic.Helpers;

public static class ConvertHelper
{
    public static SolvedTaskPreviewModel ConvertToPreview(SolvedTask task)
    {
        var previewTask = new SolvedTaskPreviewModel()
        {
            Id = task.TaskId,
            Name = task.Task.Name ?? "",
            Scores = task.Scores
        };
        return previewTask;
    }
    
    public static SolvedTaskApiModel ConvertToSolved(SolvedTask solvedTask)
    {
        var task = solvedTask.Task;
        var apiModel = new SolvedTaskApiModel()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            CreationDateTime = task.CreationDateTime,
            Deadline = task.Deadline,
            Difficulty = task.Difficulty.Name,
            Subject = task.Subject.Name,
            IsExtended = task.IsExtended,
            IsPublic = task.IsPublic,
            ReceivedAnswer = solvedTask.Answer,
            Scores = solvedTask.Scores,
        };
        return apiModel;
    }

    public static Task ConvertToTask(TaskApiModel model, Difficulty difficulty, Subject subject)
    {
        return new Task()
        {
            Id = model.Id ?? 0,
            Name = model.Name,
            Description = model.Description,
            DifficultyId = difficulty.Id,
            SubjectId = subject.Id,
            Deadline = model.Deadline.ToUniversalTime(),
            CreationDateTime = model.CreationDateTime?.ToUniversalTime() ?? DateTime.Now.ToUniversalTime(),
            IsPublic = model.IsPublic,
            IsExtended = model.IsExtended,
            Answer = model.Answer,
            Groups = new List<Group>(),
            SolvedTasks = new List<SolvedTask>(),
        };
    }

    public static TaskResponseModel? ConvertTaskToResponse(Task? task)
    {
        if (task is null) return null;
        var responseModel = new TaskResponseModel()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Answer = task.Answer,
            IsPublic = task.IsPublic,
            IsExtendedTask = task.IsExtended,
            CreationDateTime = task.CreationDateTime,
            Deadline = task.Deadline
        };
        if (task.Subject != null)
            responseModel.Subject = new SubjectApiModel() { Id = task.Subject.Id, Name = task.Subject.Name };
        if (task.Difficulty != null)
            responseModel.Difficulty = new Difficulty() { Id = task.Difficulty.Id, Name = task.Difficulty.Name };
        if (task.Teacher != null)
        {
            responseModel.Teacher = new TeacherApiModel() { Id = task.TeacherId};
            if (task.Teacher.User != null)
                responseModel.Teacher.Nickname = task.Teacher.User.Nickname;
        }
        if (task.Institution != null)
            responseModel.Institution = new InstitutionApiModel() { Id = task.Institution.Id, Name = task.Institution.Name };
        return responseModel;
    }
}