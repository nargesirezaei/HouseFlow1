﻿using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseFlowPart1.Controllers;


public class HomeController : Controller
{
    private readonly ILogService logService;
    private readonly IHouseService houseService;

    public HomeController(ILogService logService, IHouseService houseService)
    {
        this.logService = logService;
        this.houseService = houseService;
    }

    public async Task<IActionResult> Index()
    {
        List<HouseImagesViewModel> homesWithImages = await houseService.GetAllHousesWithImages();
        return View(homesWithImages);
    }

    private string GenerateLogMessage() {
        // Extract request details from HttpContext
        
        var request = HttpContext.Request;
        var method = request.Method;
        var path = request.Path;
        var queryString = request.QueryString.ToString();
        var userAgent = request.Headers["User-Agent"].ToString();

        // Construct the log message with request details
        return $"{method} {path}{queryString} - User-Agent: {userAgent}";
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        logService.WriteExceptionAsync(GenerateLogMessage(), "5XX");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
    public new IActionResult NotFound()
    {
        // Custom logic for handling not found requests
        logService.WriteExceptionAsync(GenerateLogMessage(), "404");
        return View();
    }
}

