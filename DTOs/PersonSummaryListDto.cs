namespace ControlegastosAPI.DTOs;

public class PersonSummaryListDto
{
    public List<PersonSummaryDto> Persons {get; set;} =[];
    public decimal TotalIncome {get;set;}
    public decimal TotalExpense {get;set;}
    public decimal Balance {get;set;}
}