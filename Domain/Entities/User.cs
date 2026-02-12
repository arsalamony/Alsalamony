namespace Domain.Entities;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public decimal? Latitude { get; set; } = null;

    public decimal? Longitude { get; set; } = null;

    public DateTime? DateOfLastLocation { get; set; } = DateTime.Now;

    public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
}
