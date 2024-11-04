using AppBackend.Data;
using AppBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Services;

public class ReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    // Vérifie si la salle est disponible pour un créneau donné
    public async Task<bool> CheckRoomAvailability(int salleId, DateTime startTime, DateTime endTime)
    {
        return !await _context.Reservations
            .AnyAsync(r => r.SalleId == salleId &&
                           r.StartTime < endTime &&
                           r.EndTime > startTime);
    }

    // Réserve une salle si elle est disponible
    public async Task<string> ReserveRoom(int salleId, DateTime startTime, DateTime endTime, string userId)
    {
        var isAvailable = await CheckRoomAvailability(salleId, startTime, endTime);

        if (!isAvailable)
        {
            return "Conflit de réservation : la salle est déjà réservée pour le créneau sélectionné.";
        }

        var reservation = new Reservation
        {
            SalleId = salleId,
            StartTime = startTime,
            EndTime = endTime,
            UserId = userId
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return "Réservation réussie.";
    }
}