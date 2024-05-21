using System.Globalization;
using API.Models;
using CsvHelper;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MonsterExtendedController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;

    public MonsterExtendedController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Monster> monsters = await _repository.Monsters.GetAllAsync();
        return Ok(monsters);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update(int id, [FromBody] Monster monster)
    {
        var existingMonster = await _repository.Monsters.FindAsync(id);

        if (existingMonster == null)
        {
            return NotFound($"The monster with ID = {id} not found.");
        }

        _repository.Monsters.Update(id, monster);
        await _repository.Save();
        return Ok();
    }
}
