using Logic.ApiModels;
using Logic.Interfaces;

namespace Logic.Implementations;

public class RatingManager: IRatingManager
{
    public RatingApiModel GetMyRating(long userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RatingApiModel>? GetGlobal(long userId, long? institutionId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RatingApiModel>? GetByCondition(long userId, long? institutionId, long? groupId, long? subjectId, DateTime? from,
        DateTime? to)
    {
        throw new NotImplementedException();
    }
}