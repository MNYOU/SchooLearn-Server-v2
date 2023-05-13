using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IRatingManager
{
    IEnumerable<RatingApiModel>? GetGlobal(long userId, long? institutionId);

    IEnumerable<RatingApiModel>? GetByCondition(long userId, long? institutionId, long? groupId, long? subjectId,
        DateTime? from, DateTime? to);
}