using Microservice3.Data;
using Microsoft.EntityFrameworkCore;

namespace Microservice3.Data;

public class MsSqlDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MsSqlDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("MsSql"));
    }
    
    public DbSet<Strochki> Strochkis { get; set; }
}