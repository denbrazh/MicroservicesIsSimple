using Microservice1.Data;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddDbContext<MsSqlDbContext>();
builder.Services.AddDbContext<SqlServerDataContext>();

builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
    {
        b
            .AddZipkinExporter()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddNpgsql()
            .AddConsoleExporter()
            .ConfigureResource(resource => resource
                .AddService(serviceName: builder.Environment.ApplicationName));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();