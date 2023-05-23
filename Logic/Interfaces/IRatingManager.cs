using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IRatingManager
{
    RatingApiModel GetMyRating(long userId, long? subjectId, long? groupId,
        DateTime? from, DateTime? to);

    IEnumerable<RatingApiModel>? GetGlobal(long userId, long? institutionId);

    IEnumerable<RatingApiModel>? GetByCondition(long userId, long? institutionId, long? subjectId, long? groupId,
        DateTime? from, DateTime? to);
}