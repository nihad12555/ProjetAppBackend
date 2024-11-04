using AppBackend.Data;
using AppBackend.Models.Entities;
using AppBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ReservationService _reservationService;
    private readonly RoomAccessService _roomAccessService;

    public ReservationController(AppDbContext context, ReservationService reservationService, RoomAccessService roomAccessService)
    {
        _context = context;
        _reservationService = reservationService;
        _roomAccessService = roomAccessService;
    }

    // Méthode pour récupérer toutes les réservations
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        return await _context.Reservations.ToListAsync();
    }

    // Méthode pour créer une réservation après vérification des conflits et des autorisations
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        // Vérifier si l'utilisateur a le droit de réserver cette salle
        var userId = User.Identity.Name; // Utilisez l'identité de l'utilisateur authentifié
        var hasAccess = await _roomAccessService.HasAccessToRoom(userId, reservation.SalleId);
        if (!hasAccess)
        {
            return Forbid("Vous n'avez pas l'autorisation de réserver cette salle.");
        }

        // Vérifier si la salle est disponible avant de créer la réservation
        var isAvailable = await _reservationService.CheckRoomAvailability(reservation.SalleId, reservation.StartTime, reservation.EndTime);
        if (!isAvailable)
        {
            return Conflict("Conflit de réservation : la salle est déjà réservée pour le créneau sélectionné.");
        }

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
    }
}
