using ControlegastosAPI.Data;
using Microsoft.AspNetCore.Mvc;
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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
    public async Task<ActionResult<List<Person>>> GetPersons()
    {   //retorna uma lista com as pessoas cadastradas
        return await _context.Persons.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
        //procura a pessoa pelo id, pode ser nulo (?) porque depois fazemos as validações
        Person? person = await _context.Persons.FindAsync(id);
        
        if (person == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }

        return Ok (person);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Person>> PutPerson (int id, Person person)
    {
        if(id != person.Id)
        {
            return BadRequest("O ID da URL é diferente do ID fornecido");
        }

        Person? dbPerson = await _context.Persons.FindAsync(id);

        if(dbPerson == null)
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
        return Ok(dbPerson);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerson (int id)
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
    public async Task<ActionResult<Person>> CreatePerson(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return BadRequest("O nome é obrigatório!");
        }
        if (person.Age < 0)
        {
            return BadRequest("A idade não pode ser negativa");
        }

        _context.Persons.Add(person);

        await _context.SaveChangesAsync();

        return Ok(person);
    }
}


