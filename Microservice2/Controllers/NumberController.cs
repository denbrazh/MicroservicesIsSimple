using Microservice2.Data;
using Microsoft.AspNetCore.Mvc;

namespace Microservice2.Controllers;

[ApiController]
[Route("api")]
public class NumberController : ControllerBase
{
    private readonly ILogger<NumberController> _logger;
    private readonly AppDbContext _context;
    private readonly MsSqlDbContext _contextSQL;

    public NumberController(ILogger<NumberController> logger, AppDbContext context, MsSqlDbContext contextSql)
    {
        _logger = logger;
        _context = context;
        _contextSQL = contextSql;
    }

    [HttpGet("getString")]
    public async Task<string> Get()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/getString");
        return $"Второй! {row}";
    }
    
    [HttpGet("fromDB")]
    public async Task<string> GetFromDb()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/fromDB");
        int a = 2;
        var strocka = await _context.Strochkis.FindAsync(a);
        return $"{strocka.Stroka} {row}";
    }
    
    [HttpGet("fromMsDB")]
    public async Task<string> GetFromMsDb()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/fromMsDB");
        int a = 3;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka.Stroka} {row}";
    }
    
    [HttpGet("fromPostgre")]
    public async Task<string> GetFromPostgre()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/fromMsDB");
        int a = 4;
        var strocka = await _context.Strochkis.FindAsync(a);
        return $"{strocka.Stroka} {row}";
    }
}