
global using API.Services.Flights;
global using API.Repositories.Flights;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using API.Models;
global using API.Dtos.Flights;
global using API.Shared.Dtos;
global using FluentValidation;
global using API.Hubs;
global using API.Services.Notifications;

using API.Data;
using FluentValidation.AspNetCore;
using API.Validators;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("FlightsConnection"), sql =>
        sql.MigrationsAssembly(typeof(DataContext).Assembly.FullName)
    )
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IFlightsService, FlightsService>();
builder.Services.AddScoped<IFlightsRepository, FlightsRepository>();
builder.Services.AddScoped<DbContext, DbContext>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateFlightDtoValidator>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowCors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
