using AppBackend.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AppBackend.Models.Entities;
using AppBackend.Services;

public class RoomAccessService
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public RoomAccessService(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // Vérifie si l'utilisateur a accès à la salle en fonction de son rôle
    public async Task<bool> HasAccessToRoom(string userId, int salleId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null || user.Role != "Admin") // Adaptez selon vos règles de gestion des rôles
        {
            return false; // L'utilisateur n'est pas autorisé à réserver la salle
        }

        return true; // L'utilisateur a l'autorisation de réserver la salle
    }

    // Réserve la salle avec vérification des autorisations
    public async Task<string> ReserveRoomWithAccessCheck(string userId, int salleId, DateTime startTime, DateTime endTime)
    {
        // Vérifiez si l'utilisateur a accès à la salle
        var hasAccess = await HasAccessToRoom(userId, salleId);

        if (!hasAccess)
        {
            return "Vous n'avez pas l'autorisation de réserver cette salle.";
        }

        // Si l'utilisateur a accès, procédez avec la réservation
        var reservationService = new ReservationService(_context); // Vous pouvez aussi injecter ReservationService
        var result = await reservationService.ReserveRoom(salleId, startTime, endTime, userId);
        return result;
    }
}
