using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class RatingManager : IRatingManager
{
    private readonly IAccountManager _accountManager;
    private readonly ITaskRepository _taskRepository;
    private readonly IGroupRepository _groupRepository;

    public RatingManager(ITaskRepository taskRepository, IAccountManager accountManager,
        IGroupRepository groupRepository)
    {
        _taskRepository = taskRepository;
        _accountManager = accountManager;
        _groupRepository = groupRepository;
    }

    public RatingApiModel GetMyRating(long userId, long? subjectId, long? groupId,
        DateTime? from,
        DateTime? to)
    {
        var user = _accountManager.GetWithDetails(userId);
        RatingApiModel? ratingModel = null;
        if (user is { InstitutionId: not null })
        {
            ratingModel = GetByCondition(userId, user.InstitutionId, subjectId, groupId, from, to)
                ?.FirstOrDefault(r => r.Nickname == user.Nickname);
        }
        
        return ratingModel ?? new RatingApiModel() { Place = 0, Nickname = user?.Nickname ?? "", Scores = 0 };
    }

    public IEnumerable<RatingApiModel>? GetGlobal(long userId, long? institutionId)
    {
        var user = _accountManager.GetWithDetails(userId);
        if (user is null) return null;
        if (user.InstitutionId != null)
            institutionId = user.InstitutionId;
        else if ((int)user.Role > 1)
            institutionId = 0;

        var solvedTasks = _taskRepository.SolvedTasks
            .Include(st => st.Student)
            .ThenInclude(s => s.User)
            .Where(st => st.Student.InstitutionId == institutionId);
        var ratingModels = CalculateRating(solvedTasks);
        return ratingModels;
    }

    public IEnumerable<RatingApiModel>? GetByCondition(long userId, long? institutionId, long? subjectId, long? groupId,
        DateTime? from,
        DateTime? to)
    {
        var user = _accountManager.GetWithDetails(userId);
        if (user is null) return null;
        if (user.InstitutionId != null)
            institutionId = user.InstitutionId;
        else if ((int)user.Role > 1)
            institutionId = null;

        var solved = _taskRepository.SolvedTasks
                .Include(st => st.Student)
                .ThenInclude(s => s.User)
                // .Include(st => st.)
                .Include(st => st.Task)
                .ThenInclude(t => t.Subject)
                .Where(st => st.Student.InstitutionId == institutionId)
            .AsEnumerable();
            ;
        if (groupId != null)
        {
            var groupStudents = _groupRepository.GroupStudents
                .Where(gs => gs.GroupId == groupId && gs.IsApproved)
                .ToList();
            var groupTasks = _taskRepository.Tasks
                .Where(t => t.Groups.Any(g => g.Id == groupId))
                .ToList();
            solved = solved
                .Where(st => groupStudents.Any(g => st.StudentId == g.StudentId))
                .Where(st => groupTasks.Any(t => st.TaskId == t.Id));
        }
        else if (subjectId != null)
        {
            solved = solved.Where(st => st.Task.SubjectId == subjectId);
        }

        if (from != null)
        {
            solved = solved.Where(st => st.SolveTime >= from);
        }

        if (to != null)
        {
            solved = solved.Where(st => st.SolveTime <= to);
        }

        // solved = solved
            // .Include(st => st.Student)
            // .ThenInclude(s => s.User);

        var ratingModels = CalculateRating(solved.ToList());
        return ratingModels;
    }

    private IEnumerable<RatingApiModel> CalculateRating(IEnumerable<SolvedTask> filteredSolvedTasks)
    {
        var solvedTasks = filteredSolvedTasks.ToList();
        var usersWithScores = new Dictionary<string, float>();
        foreach (var solvedTask in solvedTasks)
        {
            var nickname = solvedTask.Student.User.Nickname;
            usersWithScores.TryAdd(nickname, 0);
            usersWithScores[nickname] += solvedTask.Scores;
        }

        var ratingModels = usersWithScores
            .OrderByDescending(u => u.Value)
            .Select((u, i) => new RatingApiModel() { Place = i + 1, Nickname = u.Key, Scores = u.Value });
        return ratingModels;
    }
}