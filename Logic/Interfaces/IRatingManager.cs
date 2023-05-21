using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IRatingManager
{
    RatingApiModel GetMyRating(long userId);
    
    IEnumerable<RatingApiModel>? GetGlobal(long userId, long? institutionId);

    IEnumerable<RatingApiModel>? GetByCondition(long userId, long? institutionId, long? groupId, long? subjectId,
        DateTime? from, DateTime? to);
}