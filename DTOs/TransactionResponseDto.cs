using ControlegastosAPI.Enums;

namespace ControlegastosAPI.DTOs;

public class TransactionResponseDto
{
    public int Id {get;set;}
    public string Description {get;set;} = string.Empty;
    public decimal Value {get; set;}
    public TransactionType Type {get;set;}
    public int PersonId {get; set;}
}