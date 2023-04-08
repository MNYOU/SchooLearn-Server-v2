using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dal.Entities;

public class Subject
{
    // составной ключ из id and teacherId
    // или реализовать через многие ко многим
    // наиболее рационально сделать так: каждый учитель создает свои предметы сам, а в них группы
    
    public long Id { get; set; }
    
    public string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<Teacher> Teachers { get; set; }

    public ICollection<Group> Groups { get; set; }

    public ICollection<Task> Tasks { get; set; }
}