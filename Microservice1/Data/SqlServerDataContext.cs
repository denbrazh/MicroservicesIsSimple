using Microsoft.EntityFrameworkCore;

namespace Microservice1.Data;

public class SqlServerDataContext: DbContext
{
    private readonly IConfiguration _configuration;

    public SqlServerDataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
    }
    
    public DbSet<Strochki> Strochkis { get; set; }
}