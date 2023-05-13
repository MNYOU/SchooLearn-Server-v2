namespace Logic.ApiModels;

public class RatingApiModel
{
    public int Place { get; set; }
    
    public StudentApiModel Student { get; set; }

    public float Scores { get; set; }
}