namespace Dal.Entities;

public class Admin
{
    public long UserId { get; set; }
    public User User { get; set; } // один ко одному

    public long InstitutionId { get; set; }

    public Institution Institution { get; set; }
}