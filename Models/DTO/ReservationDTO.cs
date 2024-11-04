public class ReservationDto
{
    public int Id { get; set; }
    public int SalleId { get; set; }
    public string UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
