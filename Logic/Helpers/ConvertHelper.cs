using Dal.Entities;
using Logic.ApiModels;

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
}