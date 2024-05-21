using System.Diagnostics;
using System.Text;
using API.Controllers;
using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class MonsterExtendedControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;

    public MonsterExtendedControllerTests()
    {
        this._repository = new Mock<IBattleOfMonstersRepository>();
    }

    [Fact]
    public async Task Post_OnSuccess_ImportCsvToMonster()
    {
        this._repository
            .Setup(x => x.Monsters.GetAllAsync());

        MonsterController monster = new MonsterController(this._repository.Object);

        using FileStream stream = System.IO.File.OpenRead("../../../Files/monsters-correct.csv");
        IFormFile file = new FormFile(stream, 0, stream.Length, stream.Name, stream.Name);

        ActionResult result = await monster.ImportCsv(file);
        result.Should().BeOfType<OkResult>();
        OkResult objectResults = (OkResult)result;
        objectResults.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    {
        this._repository
            .Setup(x => x.Monsters.GetAllAsync());

        MonsterController monster = new MonsterController(this._repository.Object);
        
        using FileStream stream = System.IO.File.OpenRead("../../../Files/monsters-empty-monster.csv");
        IFormFile file = new FormFile(stream, 0, stream.Length, stream.Name, stream.Name);
        
        ActionResult result = await monster.ImportCsv(file);
        result.Should().BeOfType<BadRequestObjectResult>();
        BadRequestObjectResult objectResults = (BadRequestObjectResult)result;
        objectResults.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    {
        this._repository
            .Setup(x => x.Monsters.GetAllAsync());

        MonsterController monster = new MonsterController(this._repository.Object);
        
        using FileStream stream = System.IO.File.OpenRead("../../../Files/monsters-wrong-column.csv");
        IFormFile file = new FormFile(stream, 0, stream.Length, stream.Name, stream.Name);
        
        ActionResult result = await monster.ImportCsv(file);
        result.Should().BeOfType<BadRequestObjectResult>();
        BadRequestObjectResult objectResults = (BadRequestObjectResult)result;
        objectResults.StatusCode.Should().Be(400);
    }
}
