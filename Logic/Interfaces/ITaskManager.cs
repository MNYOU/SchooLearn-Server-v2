using Task = Dal.Entities.Task;

namespace Logic.Interfaces;

public interface ITaskManager
{
    Task<Task?> GetAny();

    Task<IEnumerable<Task>> GetCurrentForStudent(long studentId);

    bool TryAddTaskInRepository();
}