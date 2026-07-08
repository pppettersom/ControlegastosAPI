//importando o AppDbContext
using ControlegastosAPI.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
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

app.MapControllers();

app.Run();

