using System.Runtime.InteropServices.JavaScript;
using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IStudentManager
{
    Task<bool> Register(User user);
    
    bool JoinInGroup(long key);
    
    // is't good?
    IEnumerable<TaskApiModel> GetMyTasks();
    // как реализовать задания с развернутыми ответами
    IEnumerable<TaskCustomApiModel> GetTaskWithAnswer();

    // TODO как быть с авторизационной сессией

    // вопрос на подумать
    // вступление в группу, выход из нее должно быть здесь?
    // а общие манипуляции должные быть там?
}