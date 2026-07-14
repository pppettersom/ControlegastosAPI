using ControlegastosAPI.Data;
using Microsoft.AspNetCore.Mvc;
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;
using ControlegastosAPI.DTOs;
using ControlegastosAPI.Enums;
namespace ControlegastosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly AppDbContext _context;

    public PersonController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonResponseDto>>> GetPersons()
    {   //retorna uma lista com as pessoas cadastradas
        List<Person>? persons = await _context.Persons.ToListAsync();
        List<PersonResponseDto> response = new();
        foreach (Person person in persons)
        {
            PersonResponseDto responseDto = new PersonResponseDto();

            responseDto.Id = person.Id;
            responseDto.Name = person.Name;
            responseDto.Age = person.Age;
            response.Add(responseDto);
        }

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonResponseDto>> GetPerson(int id)
    {
        //procura a pessoa pelo id, pode ser nulo (?) porque depois fazemos as validações
        Person? person = await _context.Persons.FindAsync(id);

        if (person == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }
        PersonResponseDto responseDto = new PersonResponseDto();
        responseDto.Id = person.Id;
        responseDto.Name = person.Name;
        responseDto.Age = person.Age;

        return Ok(responseDto);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonResponseDto>> PutPerson(int id, PersonUpdateDto person)
    {

        Person? dbPerson = await _context.Persons.FindAsync(id);

        if (dbPerson == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return BadRequest("O nome não pode ser nulo ou vazio!");
        }
        if (person.Age < 0)
        {
            return BadRequest("A idade não pode ser negativa");
        }
        dbPerson.Name = person.Name;
        dbPerson.Age = person.Age;

        await _context.SaveChangesAsync();

        PersonResponseDto responseDto = new PersonResponseDto();
        responseDto.Id = dbPerson.Id;
        responseDto.Name = dbPerson.Name;
        responseDto.Age = dbPerson.Age;

        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerson(int id)
    {
        Person? person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<PersonResponseDto>> CreatePerson(PersonCreateDto person)
    {
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return BadRequest("O nome é obrigatório!");
        }
        if (person.Age < 0)
        {
            return BadRequest("A idade não pode ser negativa");
        }
        Person dbPerson = new Person();
        dbPerson.Name = person.Name;
        dbPerson.Age = person.Age;

        _context.Persons.Add(dbPerson);

        await _context.SaveChangesAsync();
        PersonResponseDto responseDto = new PersonResponseDto();
        responseDto.Id = dbPerson.Id;
        responseDto.Name = dbPerson.Name;
        responseDto.Age = dbPerson.Age;
        return Ok(responseDto);
    }
    [HttpGet("{id}/summary")]
    public async Task<ActionResult<PersonSummaryDto>> GetSummary(int id)
    {
        Person? person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return NotFound("Pessoa não encontrada!");
        }

        var incomes = await _context.Transactions.Where(t => t.PersonId == id && t.Type == TransactionType.Income).ToListAsync();
        var expenses = await _context.Transactions.Where(t => t.PersonId == id && t.Type == TransactionType.Expense).ToListAsync();
        decimal totalIncomes = incomes.Sum(t => t.Value);
        decimal totalExpenses = expenses.Sum(t => t.Value);
        decimal balance = totalIncomes - totalExpenses;

        PersonSummaryDto personSummary = new PersonSummaryDto();
        personSummary.Name = person.Name;
        personSummary.TotalIncome = totalIncomes;
        personSummary.TotalExpense = totalExpenses;
        personSummary.Balance = balance;

        return Ok(personSummary);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<PersonSummaryListDto>> GetSummaryList()
    {
        List<Person> persons = await _context.Persons.ToListAsync();
        List<PersonSummaryDto> listSummary = new();
        decimal totalIncomes = 0;
        decimal totalExpenses = 0;
        foreach (Person person in persons)
        {
            PersonSummaryDto responseDto = new PersonSummaryDto();
            var incomes = await _context.Transactions.Where(t => t.PersonId == person.Id && t.Type == TransactionType.Income).ToListAsync();
            var expenses = await _context.Transactions.Where(t => t.PersonId == person.Id && t.Type == TransactionType.Expense).ToListAsync();
            decimal totalIncomesDto = incomes.Sum(t => t.Value);
            decimal totalExpensesDto = expenses.Sum(t => t.Value);
            decimal balance = totalIncomesDto - totalExpensesDto;
            responseDto.Name = person.Name;
            responseDto.TotalIncome = totalIncomesDto;
            responseDto.TotalExpense = totalExpensesDto;
            responseDto.Balance = balance;
            listSummary.Add(responseDto);

            totalIncomes += totalIncomesDto;
            totalExpenses +=totalExpensesDto;

        }
        
        PersonSummaryListDto summary = new PersonSummaryListDto ();
        summary.Persons = listSummary;
        summary.TotalIncome = totalIncomes;
        summary.TotalExpense = totalExpenses;
        summary.Balance = totalIncomes - totalExpenses;

        return Ok(summary);

    }
}


