using ControlegastosAPI.Models;
namespace ControlegastosAPI.DTOs;

public class PersonResponseDto
{
    public int Id {get;set;}
    public string Name {get;set;} = string.Empty;
    public int Age {get; set;}
}