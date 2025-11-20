using Microsoft.EntityFrameworkCore;
using SalaCine.Api.Data;
using SalaCine.Api.Repository;
using SalaCine.Api.Services.Implementations;
using SalaCine.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "SalaCine API",
        Version = "v1",
        Description = "API REST para gestión de películas y salas de cine"
    });
});

builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository and Service
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IPeliculaService, PeliculaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalaCine API v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
