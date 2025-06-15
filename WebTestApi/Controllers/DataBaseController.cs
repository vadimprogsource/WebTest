using System;
using Microsoft.AspNetCore.Mvc;
using Test.Repository.Domain;
using Microsoft.EntityFrameworkCore;
using Test.Api.Domain;

namespace TestWebApi.Controllers;

[Route("db")]
//[ApiExplorerSettings(IgnoreApi = true)]
public class DataBaseController : ControllerBase
{
    private readonly ForkDbContext dbContext;

    public DataBaseController(ForkDbContext context) =>dbContext = context;

    [HttpGet]
    public IActionResult GetScript()
    {
        return Ok(dbContext.Database.GenerateCreateScript());
        //return NotFound();
    }

    [HttpPut]
    public  IActionResult CreateDataBase()
    {
        dbContext.Database.EnsureCreated();
        return NoContent();
    }

    [HttpDelete]
    public IActionResult DeleteDataBase()
    {
        dbContext.Database.EnsureDeleted();
        return NoContent();
    }


}

