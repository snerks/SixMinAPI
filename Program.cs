using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinApi.Data;
using SixMinApi.Dtos;
using SixMinApi.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SqlConnectionStringBuilder? sqlConBuilder = new();
sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
sqlConBuilder.UserID = builder.Configuration["UserId"];
sqlConBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// string[]? summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     return (WeatherForecast[])Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateTime.Now.AddDays(index),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
// })
// .WithName("GetWeatherForecast");

app.MapGet("api/v1/commands", async (ICommandRepo repo, IMapper mapper) =>
{
    global::System.Collections.Generic.IEnumerable<global::SixMinApi.Models.Command>? commands = await repo.GetAllCommands();
    return Results.Ok(mapper.Map<IEnumerable<SixMinApi.Dtos.CommandReadDto>>(commands));
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    global::SixMinApi.Models.Command? command = await repo.GetCommandById(id);
    return command != null ? Results.Ok(mapper.Map<CommandReadDto>(command)) : Results.NotFound();
});

app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto cmdCreateDto) =>
{
    global::SixMinApi.Models.Command? commandModel = mapper.Map<Command>(cmdCreateDto);

    await repo.CreateCommand(commandModel);
    await repo.SaveChanges();

    global::SixMinApi.Dtos.CommandReadDto? cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/v1/commands/{cmdReadDto.Id}", cmdReadDto);
});

app.MapPut("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto cmdUpdateDto) =>
{
    global::SixMinApi.Models.Command? command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    _ = mapper.Map(cmdUpdateDto, command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, int id) =>
{
    global::SixMinApi.Models.Command? command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.Run();

// internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }