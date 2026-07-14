//importando o AppDbContext
using System.Security.Authentication.ExtendedProtection;
using ControlegastosAPI.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// CORS da a permissão para o frontend acessar a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
//Configruando o AppDbContext como um serviço da aplicação
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite( //mostrando que vai usar SQLite
        builder.Configuration.GetConnectionString("DefaultConnection") //Procurano appsettings a conexão
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ReactPolicy");
app.MapControllers();

app.Run();

