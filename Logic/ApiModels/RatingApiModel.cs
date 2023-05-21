namespace Logic.ApiModels;

public class RatingApiModel
{
    public long Place { get; set; }
    
    public StudentApiModel Student { get; set; }

    public float Scores { get; set; }
}