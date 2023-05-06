namespace Logic.ApiModels;

public record FileAnswer
{
    public long Id { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }

    public byte[] Content { get; set; }
}