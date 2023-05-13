namespace Logic.Helpers;

public static class FileHelper
{
    public static string GenerateFileName(Random random, long fileId, string fileExtension)
    {
        var datetime = DateTime.Now;
        var number = random.NextInt64(fileId) * random.Next(1, 10);
        var fileName = $"default_file_{datetime}_{number}{fileExtension}";
        return fileName;
    }
}