//importando Models para que o banco tenha acesso as classes Person
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlegastosAPI.Data;

//o AppDbContext está aproveitando a classe DbContext 
public class AppDbContext : DbContext
{
    //construtor chama o DbContextOptions que é o objeto de configurações, o : base(options) repassa para o construtor as options
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Person> Persons {get; set;}
    public DbSet<Transaction> Transactions {get;set;}
    
    
    //Método para deletar as transações quando as pessoas forem deletadas

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Person)
        .WithMany(p =>p.Transactions)
        .HasForeignKey(t => t.PersonId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}