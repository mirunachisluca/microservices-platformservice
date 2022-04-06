using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configuration

var isProduction = builder.Environment.IsProduction();

if (isProduction)
{
    Console.WriteLine("--> Using SQL Server database");

    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnection")));
}
else
{
    Console.WriteLine("--> Using in-memory database");

    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PlatformDb"));
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

Console.WriteLine($"--> CommandService endpoint: {builder.Configuration["CommandServiceURL"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, isProduction);

app.Run();
