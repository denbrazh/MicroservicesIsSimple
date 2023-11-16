using Microservice3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Microservice3.Controllers;

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
    public string Get()
    {
        return $"Третий!";
    }
    
    [HttpGet("getStoredProcedure")]
    public string GetSP()
    {
        var somevalues =_contextSQL.Strochkis.FromSqlRaw("EXEC [dbo].[GetAllStrochkis]").ToList();
        var res = somevalues[4].Stroka;
        return $"{res}";
    }
    
    [HttpGet("getLaggyProcedure")]
    public string GetLaggySP()
    {
        var somevalues =_contextSQL.Strochkis.FromSqlRaw("EXEC [dbo].[GetStrochkaDelayByID] 7").ToList();
        var res = somevalues.First().Stroka;
        return $"{res}";
    }
    
    [HttpGet("getExceptionProcedure")]
    public string GetBaggySP()
    {
        var somevalues =_contextSQL.Strochkis.FromSqlRaw("EXEC [dbo].[RaiseException]").ToList();
        var res = somevalues.First().Stroka;
        return $"{res}";
    }
    
    
    [HttpGet("string")]
    public async Task<string> GetStringDb()
    {
        return "Я строка из микросервиса";
    }
    
    [HttpGet("fromDB")]
    public async Task<string> GetFromDb()
    {
        int a = 3;
        var strochka = await _context.Strochkis.FindAsync(a);
        return strochka.Stroka;
    }
    [HttpGet("fromMsDB")]
    public async Task<string> GetFromMsDb()
    {
        int a = 4;
        var strochka = await _contextSQL.Strochkis.FindAsync(a);
        return strochka.Stroka;
    }
}