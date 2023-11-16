using Microsoft.EntityFrameworkCore;

namespace Microservice2.Data;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("MicroWebDatabase"));
    }
    
    public DbSet<Strochki> Strochkis { get; set; }

}