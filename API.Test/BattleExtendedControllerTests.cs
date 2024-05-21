using System.Threading;
using API.Controllers;
using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class BattleExtendedControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;
    public BattleExtendedControllerTests()
    {
        this._repository = new Mock<IBattleOfMonstersRepository>();
    }
    [Fact]
    public async Task Post_OnNoMonsterFound_When_StartBattle_With_NonexistentMonster()
    {
        Battle b = new Battle()
        {
            MonsterA = 22,
            MonsterB = 23
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        BattleExtendedController sut = new BattleExtendedController(this._repository.Object);

        ActionResult result = await sut.Add(b);
        result.Should().BeOfType<NotFoundObjectResult>();
        NotFoundObjectResult objectResults = (NotFoundObjectResult)result;
        Assert.Equal($"The monster with ID = {22} not found.", objectResults.Value);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[0].Id,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[0].Id;
        Monster monsterA = monstersMock[0];
        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleExtendedController sut = new BattleExtendedController(this._repository.Object);

        ActionResult result = await sut.Add(b);
        result.Should().BeOfType<BadRequestObjectResult>();
        BadRequestObjectResult objectResults = (BadRequestObjectResult)result;
        Assert.Equal("Missing ID", objectResults.Value);
    }


    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirSpeedsSame_And_MonsterA_Has_Higher_Attack()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning_When_TheirSpeedsSame_And_MonsterB_Has_Higher_Attack()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirDefensesSame_And_MonsterA_Has_Higher_Speed()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Delete_OnSuccess_RemoveBattle()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Delete_OnNoBattleFound_Returns404()
    {
        // @TODO missing implementation
        Assert.True(false);
    }
}
