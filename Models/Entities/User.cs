using Microsoft.AspNetCore.Identity;

namespace AppBackend.Models.Entities;

public class User : IdentityUser
{
	public string Role { get; set; }
}