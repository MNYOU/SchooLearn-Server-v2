using Dal.Entities;
using Logic.ApiModels;
using Microsoft.AspNetCore.Http;
using Task = Dal.Entities.Task;

namespace Logic.Interfaces;

public interface ITaskManager
{
    //TODO нужно сделать получение развернутых заданий для учителя 

    Task? Get(long id);

    Task<TaskResponseModel> GetAny();

    /// <summary>
    /// Возвращает невыполненные задания для студента
    /// </summary>
    /// <param name="studentId">Идентификатор студента</param>
    /// <returns>Перечисление заданий</returns>
    Task<IEnumerable<TaskResponseModel>> GetCurrentForStudentAsync(long studentId);

    /// <summary>
    /// Возвращает список выданных заданий учителем
    /// </summary>
    /// <param name="teacherId">Идентификатор учителя</param>
    /// <returns>Перечисление заданий</returns>
    /// эти задания должны быть с ответом
    Task<IEnumerable<TaskResponseModel>> GetAssignedTasksAsync(long teacherId);

    // groups??
    IEnumerable<StudentApiModel> GetStudentsWhoCompletedTask(long teacherId, long taskId);

    Task<IEnumerable<TaskPreviewApiModel>> GetSolvedTasksAsync(long studentId);

    SolvedTaskApiModel? GetSolvedTask(long studentId, long taskId);

    /// <summary>
    /// Добавляет новое задание для группы
    /// </summary>
    /// <param name="model">Задание</param>
    /// <param name="teacherId">Идентификатор учителя</param>
    /// <param name="groupId">Идентификатор группы</param>
    /// <returns>Результат действия</returns>
    Task<bool> TryAddTaskForGroupAsync(TaskApiModel model, long teacherId, long groupId);

    // данный метод невозможно заставить работать
    Task<bool> TryAddTaskForGroupsAsync(TaskApiModel model, long teacherId, IEnumerable<GroupApiModel> groupApiModels);

    /// <summary>
    /// Проверяет задание и записывает его в выполненные для студента
    /// </summary>
    /// <param name="model">Задание с ответом</param>
    /// <param name="studentId">Идентификатор студента</param>
    /// <returns>Результат проверки ответа</returns>
    bool CheckAndPointTask(TaskWithAnswerRequest model, long studentId);

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