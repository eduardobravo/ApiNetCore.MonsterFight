using System.Threading;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Lib.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BattleExtendedController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;
    public BattleExtendedController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Add([FromBody] Battle battle)
    {
        if (battle.MonsterA == null || battle.MonsterB == null)
            return BadRequest("Missing ID");

        var existingMonsterA = await _repository.Battles.FindAsync((int)battle.MonsterA);
        var existingMonsterB = await _repository.Battles.FindAsync((int)battle.MonsterB);

        if (existingMonsterA == null)
        {
            return NotFound($"The monster with ID = {battle.MonsterA} not found.");
        }

        if (existingMonsterB == null)
        {
            return NotFound($"The monster with ID = {battle.MonsterB} not found.");
        }

        battle.Winner = BattleWin(battle.MonsterARelation, battle.MonsterBRelation);

        await _repository.Battles.AddAsync(battle);
        await _repository.Save();
        return Ok(battle);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Remove(int id)
    {
        var existingBattle = await _repository.Battles.FindAsync(id);

        if (existingBattle == null)
        {
            return NotFound($"The battle with ID = {id} not found.");
        }

        await _repository.Battles.RemoveAsync(id);
        await _repository.Save();
        return Ok();
    }

    private int BattleWin(Monster monsterA, Monster monsterB)
    {
        int hpA = monsterA.Hp;
        int hpB = monsterB.Hp;
        int winId = 0;
        int damage = 1;

        while(hpA >= 0 && hpB >= 0) {
            if (monsterA.Speed >= monsterB.Speed)
            {
                damage = (monsterA.Attack == monsterB.Defense) ? damage : (monsterA.Attack - monsterB.Defense);
                hpB = hpB - damage;

                if (hpB <= 0) 
                { 
                    winId = (int)monsterA.Id;
                    break;
                }

                damage = (monsterB.Attack == monsterA.Defense) ? damage : (monsterB.Attack - monsterA.Defense);
                hpA = hpA - damage;

                if (hpA <= 0) 
                { 
                    winId = (int)monsterB.Id;
                    break;
                }
            }
            else
            {
                damage = (monsterB.Attack == monsterA.Defense) ? damage : (monsterB.Attack - monsterA.Defense);
                hpA = hpA - damage;

                if (hpA <= 0)
                {
                    winId = (int)monsterB.Id;
                    break;
                }

                damage = (monsterA.Attack == monsterB.Defense) ? damage : (monsterA.Attack - monsterB.Defense);
                hpB = hpB - damage;

                if (hpB <= 0)
                {
                    winId = (int)monsterA.Id;
                    break;
                }
            }
        }
        return winId;
    }
}
