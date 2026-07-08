using ControlegastosAPI.Enums;
namespace ControlegastosAPI.Models;
public class Transaction
{
    public int Id {get;set;}
    public string Description {get;set;} = string.Empty;
    //decimal é melhor para dinheiro, porque não fica buscando aproximações igual o double
    public decimal Value {get; set;}
// criando o enum para só deixar duas opções de tipo de transação
    public TransactionType Type {get; set;}
    public int PersonId {get; set;}

    public Person Person {get; set;} = null!;
    }