using ControlegastosAPI.Data;
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ControlegastosAPI.Enums;

namespace ControlegastosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionController (AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        if (transaction.Value <= 0)
        {
            return BadRequest("O valor não pode ser negativo!");
        }
        if (string.IsNullOrWhiteSpace(transaction.Description))
        {
            return BadRequest("A descrição não pode estar vazia!");
        }
        Person? person = await _context.Persons.FindAsync(transaction.PersonId);
        if(person == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }
        if (person.Age < 18 && transaction.Type == TransactionType.Income)
        {
            return BadRequest("Pessoas menores de idade não podem cadastrar receita");
        }

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions()
    {
        //retorna a lista das transações junto com as pessoas
         return await _context.Transactions.Include(t => t.Person).ToListAsync();

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        Transaction? transaction = await _context.Transactions.Include(t => t.Person).FirstOrDefaultAsync(t => t.Id == id);
        if (transaction == null)
        {
            return NotFound("Não existe transação cadastrada");
        }

        return Ok(transaction);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Transaction>> PutTransaction (int id, Transaction transaction)
    {
        if(id != transaction.Id)
        {
            return BadRequest("O Id da URL é diferente do Id fornecido");
        }

        Transaction? dbTransaction = await _context.Transactions.FindAsync(id);
        
        if (dbTransaction == null)
        {
            return NotFound("Não existe transação cadastrada");
        }
        if (transaction.Value <= 0)
        {
            return BadRequest("O valor não pode ser negativo!");
        }
        if (string.IsNullOrWhiteSpace(transaction.Description))
        {
            return BadRequest("A descrição não pode estar vazia!");
        }

        Person? person = await _context.Persons.FindAsync(transaction.PersonId);

        if (person == null)
        {
            return NotFound("A pessoa não foi encontrada");
        }

        if (person.Age < 18 && transaction.Type == TransactionType.Income)
        {
            return BadRequest("Pessoas menores de idade não podem cadastrar receita");
        }

        dbTransaction.Description = transaction.Description;
        dbTransaction.Value = transaction.Value;
        dbTransaction.Type = transaction.Type;
        dbTransaction.PersonId = transaction.PersonId;

        await _context.SaveChangesAsync();
        return Ok(dbTransaction);

    }    
    [HttpDelete ("{id}")]
    public async Task<ActionResult> DeleteTransaction (int id)
    {
        Transaction? transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound("A transação não existe!");
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}