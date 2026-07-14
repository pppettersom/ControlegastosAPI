using ControlegastosAPI.Data;
using ControlegastosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ControlegastosAPI.Enums;
using ControlegastosAPI.DTOs;

namespace ControlegastosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponseDto>> CreateTransaction(TransactionCreateDto transaction)
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
        if (person == null)
        {
            return NotFound("A pessoa não foi encontrada!");
        }
        if (person.Age < 18 && transaction.Type == TransactionType.Income)
        {
            return BadRequest("Pessoas menores de idade não podem cadastrar receita");
        }
        Transaction dbTransaction = new Transaction();
        dbTransaction.Description = transaction.Description;
        dbTransaction.Value = transaction.Value;
        dbTransaction.Type = transaction.Type;
        dbTransaction.PersonId = transaction.PersonId;
        _context.Transactions.Add(dbTransaction);

        await _context.SaveChangesAsync();
        TransactionResponseDto responseTransaction = new TransactionResponseDto();
        responseTransaction.Id = dbTransaction.Id;
        responseTransaction.Description = dbTransaction.Description;
        responseTransaction.Value = dbTransaction.Value;
        responseTransaction.Type = dbTransaction.Type;
        responseTransaction.PersonId = dbTransaction.PersonId;
        return Ok(responseTransaction);

    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetTransactions()
    {
        //retorna a lista das transações junto com as pessoas
        List<Transaction>? transactions = await _context.Transactions.ToListAsync();
        List<TransactionResponseDto> response = new();
        foreach (Transaction transaction in transactions)
        {
            TransactionResponseDto responseDto = new TransactionResponseDto();
            responseDto.Id = transaction.Id;
            responseDto.Description = transaction.Description;
            responseDto.Type = transaction.Type;
            responseDto.Value = transaction.Value;
            responseDto.PersonId = transaction.PersonId;
            response.Add(responseDto);
        }
        return Ok(response);

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
    public async Task<ActionResult<TransactionResponseDto>> PutTransaction(int id, TransactionUpdateDto transaction)
    {
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

        TransactionResponseDto responseTransaction = new TransactionResponseDto();
        responseTransaction.Id = dbTransaction.Id;
        responseTransaction.Description = dbTransaction.Description;
        responseTransaction.Value = dbTransaction.Value;
        responseTransaction.Type = dbTransaction.Type;
        responseTransaction.PersonId = dbTransaction.PersonId;
        return Ok(responseTransaction);

    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction(int id)
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