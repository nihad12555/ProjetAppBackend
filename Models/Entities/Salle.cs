

namespace AppBackend.Models.Entities;

public class Salle
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Equipment { get; set; }
    public bool IsAvailable { get; set; } = true;
}