using Dal.Entities;
using Logic.ApiModels;
using Microsoft.AspNetCore.Http;
using Task = Dal.Entities.Task;

namespace Logic.Interfaces;

public interface ITaskManager
{
    Task? Get(long id);
    
    TaskResponseModel? GetAny(long institutionId, string? subjectName, string? difficultyName);

    /// <summary>
    /// Возвращает невыполненные задания для студента
    /// </summary>
    /// <param name="studentId">Идентификатор студента</param>
    /// <returns>Перечисление заданий</returns>
    Task<IEnumerable<TaskResponseModel>> GetCurrentForStudentAsync(long studentId);
    
    Task<IEnumerable<TaskResponseModel>> GetExpiredTasksAsync(long studentId, DateTime? period);

    Task<IEnumerable<TaskFullApiModel>> GetAssignedTasksAsync(long teacherId, DateTime? period);

    Task<IEnumerable<TaskFullApiModel>> GetOutdatedTasksAsync(long teacherId, DateTime? period);
    
    IEnumerable<StudentApiModel> GetStudentsWhoCompletedTask(long teacherId, long taskId);

    IEnumerable<TaskPreviewApiModel> GetSolvedTasksPreviewAsync(long studentId);

    SolvedTaskApiModel? GetSolvedTask(long studentId, long taskId);

    Task<bool> TryAddTaskInRepositoryAsync(long teacherId, TaskApiModel model);
    
    /// <summary>
    /// Добавляет новое задание для группы
    /// </summary>
    /// <param name="model">Задание</param>
    /// <param name="teacherId">Идентификатор учителя</param>
    /// <param name="groupId">Идентификатор группы</param>
    /// <returns>Результат действия</returns>
    Task<bool> TryAddTaskForGroupAsync(long teacherId, long groupId, TaskApiModel model);

    Task<bool> TryAddTaskForGroupsAsync(long teacherId, long taskId, IEnumerable<GroupApiModel> groupApiModels);

    Task<bool> UpdateTaskAsync(long teacherId, TaskApiModel model);

    bool DeleteTask(long teacherId, long taskId);

    /// <summary>
    /// Проверяет задание и записывает его в выполненные для студента
    /// </summary>
    /// <param name="model">Задание с ответом</param>
    /// <param name="studentId">Идентификатор студента</param>
    /// <returns>Результат проверки ответа</returns>
    bool CheckAndPointTask(TaskWithAnswerRequest model, long studentId);

    bool GetUnchecked(long teacherId);

    /// <summary>
    /// Ставит оценку ученику после проверки учителем
    /// </summary>
    /// <param name="teacherId">Идентификатор учителя</param>
    /// <param name="studentId">Идентификатор студента</param>
    /// <param name="taskId">Идентификатор задания</param>
    /// <param name="percentages">Оценка</param>
    /// <returns>Результат операции</returns>
    Task<bool> RateAnswerAsync(long teacherId, long studentId, long taskId, byte percentages);

    /// <summary>
    /// Загружает выполненное задание на сервер
    /// </summary>
    /// <param name="studentId">Идентификатор студента</param>
    /// <param name="taskId">Идентификатор задания</param>
    /// <param name="fileAnswer">Ответ в виде файла</param>
    /// <returns>Результат операции</returns>
    Task<bool> UploadAnswerToTaskAsync(long studentId, long taskId, IFormFile fileAnswer);

    FileAnswer? DownloadAnswer(long studentId, long taskId);
}