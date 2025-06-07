﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApi.Controllers;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class IndexController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return  File("/index.html","text/html");
    }
}

