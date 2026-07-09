using ControlegastosAPI.Data;
using Microsoft.AspNetCore.Mvc;
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;
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
    {
        return await _context.Persons.ToListAsync();
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


