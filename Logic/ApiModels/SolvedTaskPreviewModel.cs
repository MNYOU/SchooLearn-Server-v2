namespace Logic.ApiModels;

public record SolvedTaskPreviewModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public float Scores { get; set; }
}