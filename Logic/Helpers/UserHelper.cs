using Dal.Entities;
using Dal.Enums;

namespace Logic.Helpers;

public static class UserHelper
{
    public static bool HasAccessToUsers(User user, long? institutionId, Role role)
    {
        if (institutionId is null && user.Role != Role.Admin && user.Role !=  Role.ProjectManager)
            return false;
        var hasAccess = false;
        var roleWeight = (int)user.Role;
        switch (role)
        {
            case Role.SuperManager:
                hasAccess = roleWeight == 0;
                break;
            case Role.ProjectManager:
                hasAccess = roleWeight <= 1;
                break;
            case Role.Admin:
                hasAccess = roleWeight switch
                {
                    <= 1 => true,
                    2 => user.InstitutionId == institutionId,
                    _ => false
                };
                break;
            case Role.Teacher:
            case Role.Student:
                hasAccess = roleWeight <= 1 || user.InstitutionId == institutionId;
                break;
            default:
                hasAccess = false;
                break;
        }

        return hasAccess;
    }
    
    public static bool IsPartiallyEqual(string partialName, string fullName)
    {
        var words = partialName.Split().Select(s => s.ToLower()).ToArray();
        if (words.Length == 0) return false;
        var sample = fullName.Split().Select(s => s.ToLower()).ToList();
        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (i == words.Length - 1)
            {
                if (!sample.Any(s => s.Contains(word)))
                    return false;
            }
            else if (sample.Contains(word))
                sample.Remove(word);
            else
                return false;
        }

        return partialName == fullName;
    }
}