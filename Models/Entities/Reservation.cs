namespace AppBackend.Models.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int SalleId { get; set; }
    public string UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Salle Salle { get; set; }
    public User User { get; set; }
}