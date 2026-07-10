namespace ControlegastosAPI.DTOs;

public class PersonSummaryDto
{
    public string Name {get;set;} = string.Empty;
    public decimal TotalIncome {get;set;}
    public decimal TotalExpense {get;set;}
    public decimal Balance {get; set;}

}