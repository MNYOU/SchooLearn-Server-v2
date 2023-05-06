namespace Logic.ApiModels;

public record TaskPreviewApiModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public float Scores { get; set; }
}