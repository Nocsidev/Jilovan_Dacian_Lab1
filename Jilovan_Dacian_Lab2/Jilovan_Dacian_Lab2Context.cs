using Microsoft.EntityFrameworkCore;

public class Jilovan_Dacian_Lab2Context(DbContextOptions<Jilovan_Dacian_Lab2Context> options) : DbContext(options)
{
    public DbSet<Jilovan_Dacian_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Jilovan_Dacian_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Jilovan_Dacian_Lab2.Models.Author> Author { get; set; } = default!;
}
