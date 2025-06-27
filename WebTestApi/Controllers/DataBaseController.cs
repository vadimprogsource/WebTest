﻿using System;
using Microsoft.AspNetCore.Mvc;
using Test.Repository.Domain;
using Microsoft.EntityFrameworkCore;
using Test.Api.Domain;

namespace TestWebApi.Controllers;

[Route("db")]
//[ApiExplorerSettings(IgnoreApi = true)]
public class DataBaseController(ForkDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetScript()
    {
        return Ok(context.Database.GenerateCreateScript());
        //return NotFound();
    }

    [HttpPut]
    public  IActionResult CreateDataBase()
    {
        context.Database.EnsureCreated();
        return NoContent();
    }

    [HttpDelete]
    public IActionResult DeleteDataBase()
    {
        context.Database.EnsureDeleted();
        return NoContent();
    }


}

