using System.Security.Cryptography;

namespace Logic.Helpers;

public static class Generator
{
    public static string GenerateInvitationCode(IEnumerable<string> occupiedCodes, int length=9)
    {
        var newCode = GetRandomAlphanumericString(length);
        var codes = occupiedCodes.ToList();
        while (codes.Contains(newCode))
        {
            newCode = GetRandomAlphanumericString(length);
        }

        return newCode;
    }
    
    public static string GetRandomAlphanumericString(int length)
    {
        const string alphanumericCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789";
        return GetRandomString(length, alphanumericCharacters);
    }

    public static string GetRandomString(int length, IEnumerable<char> characterSet)
    {
        if (length < 0)
            throw new ArgumentException("length must not be negative", "length");
        if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
            throw new ArgumentException("length is too big", "length");
        if (characterSet == null)
            throw new ArgumentNullException("characterSet");
        var characterArray = characterSet.Distinct().ToArray();
        if (characterArray.Length == 0)
            throw new ArgumentException("characterSet must not be empty", "characterSet");

        var bytes = new byte[length * 8];
        RandomNumberGenerator.Create().GetBytes(bytes);
        // new RNGCryptoServiceProvider().GetBytes(bytes);
        var result = new char[length];
        for (var i = 0; i < length; i++)
        {
            var value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }
        return new string(result);
    }
}