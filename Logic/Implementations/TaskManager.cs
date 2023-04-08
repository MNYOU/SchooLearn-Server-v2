using Dal.EFCore;
using Dal.Repositories;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = Dal.Entities.Task;

namespace Logic.Implementations;

public class TaskManager: ITaskManager
{
    private readonly ITaskRepository _taskRepository;
    public TaskManager(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<Task?> GetAny()
    {
        var task = new Task() { Id = 5, Name = "найти корень числа", Description = "нати корень числа pi" };
        _taskRepository.Tasks.Add(task);
        await _taskRepository.SaveChangesAsync();
        task = _taskRepository.Tasks.FirstOrDefault();
        // var task = _taskRepository.Tasks.FirstOrDefault();
        return task;
    }

    public async Task<IEnumerable<Task>> GetCurrentForStudent(long studentId)
    {
        throw new NotImplementedException();
    }

    private bool CheckTaskForCorrectness(Task task)
    {
        throw new NotImplementedException();
    }
    
    public bool TryAddTaskInRepository()
    {
        
        throw new NotImplementedException();
    }
}