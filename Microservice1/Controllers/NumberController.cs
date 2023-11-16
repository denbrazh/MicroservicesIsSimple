using Microservice1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservice1.Controllers;

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

    
    [HttpGet("1.SimpleString")]
    public async Task<string> SimpleString()
    {
        return $"Я строка из статики!";
    }
    
    [HttpGet("2.StringFromDb")]
    public async Task<string> StringFromDatabase()
    {
        int id = 5;
        var row = await _contextSQL.Strochkis.FindAsync(id);
        return $"{row.Stroka}";
    }
    
    [HttpGet("3.StringFromIntegration")]
    public async Task<string> StringFromIntegration()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/string");
        return $"Привет строка из микросервиса:  {row}";
    }
    
    [HttpGet("4.Hello_microservices")]
    public async Task<string> Get()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:2222/api/getString");
        return $"Первый! {row}";
    }
    
    [HttpGet("5.Microservers_And_DB_Postgre")]
    public async Task<string> GetStringFromDb()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:2222/api/fromDB");
        int a = 1;
        var strocka = await _context.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("6.Microservers_And_DB_SQLS")]
    public async Task<string> GetStringFromMsDb()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:2222/api/fromMsDB");
        int a = 2;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("7.Microservers_And_2DB")]
    public async Task<string> GetStringFrom2DB()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:2222/api/fromPostgre");
        int a = 2;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("8.Problems_Microservers")]
    public async Task<string> GetLaggyStoredProcedure()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/getLaggyProcedure");
        int a = 2;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("9.Exception")]
    public async Task<string> GetExceptionFromDB()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:3333/api/getExceptionProcedure");
        int a = 2;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("10.Exception_Microservers")]
    public async Task<string> GetExceptionFromMS()
    {
        using var client = new HttpClient();
        var row = await client.GetStringAsync("http://localhost:2222/api/NotExistEndpoint");
        int a = 2;
        var strocka = await _contextSQL.Strochkis.FindAsync(a);
        return $"{strocka?.Stroka} {row}";
    }
    
    [HttpGet("stroka/{id}")]
    public async Task<ActionResult<Strochki>> GetStrochkaItem(int id)
    {
        var strocka = await _context.Strochkis.FindAsync(id);

        if (strocka == null)
        {
            return NotFound();
        }

        return strocka;
    }
    
    [HttpGet("MSstroka/{id}")]
    public async Task<ActionResult<Strochki>> GetMsStrochkaItem(int id)
    {
        var strocka = await _contextSQL.Strochkis.FindAsync(id);

        if (strocka == null)
        {
            return NotFound();
        }

        return strocka;
    }
    
    [HttpPost("Post")]
    public async Task<string> PostStrokaItem(Strochki strocka)
    {
        _context.Strochkis.Add(strocka);
        await _context.SaveChangesAsync();

        return "Succesfully@!";
    }
    
    [HttpPost("MSPost")]
    public string PostMsStrokaItem(Strochki strocka)
    {
        _contextSQL.Strochkis.Add(strocka);
        _contextSQL.SaveChanges();

        return "Succesfully@!";
    }
    
    [HttpPut("put/{id}")]
    public async Task<IActionResult> PutStrokaItem(int id, Strochki strocka)
    {
        if (id != strocka.Id)
        {
            return BadRequest();
        }

        _context.Entry(strocka).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPut("MSput/{id}")]
    public async Task<IActionResult> PutMsStrokaItem(int id, Strochki strocka)
    {
        if (id != strocka.Id)
        {
            return BadRequest();
        }

        _contextSQL.Entry(strocka).State = EntityState.Modified;
        await _contextSQL.SaveChangesAsync();

        return NoContent();
    }
}