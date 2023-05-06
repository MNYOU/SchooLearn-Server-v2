namespace Dal.Entities;

public class FileData
{
    public long Id { get; set; }

    public string FileName { get; set; }

    public byte[] Content { get; set; }

    public string ContentType { get; set; }
}