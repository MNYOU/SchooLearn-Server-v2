using System.ComponentModel.DataAnnotations;

namespace Logic.ApiModels;

public class SubjectApiModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "Название предмета обязательно")]
    public string Name { get; set; }
}

