namespace Dal.Entities;

public class Application
{
    public long Id { get; set; }

    public bool IsReviewed { get; set; }
    
    public long InstitutionId { get; set; }
    
    public Institution Institution { get; set; }
    
    public bool ApplicationResult { get; set; }
}