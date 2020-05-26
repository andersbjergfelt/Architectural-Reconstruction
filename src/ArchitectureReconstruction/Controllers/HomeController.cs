using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ArchitectureReconstructionPresentation.Models;
using ArchitectureReconstructionPresentation.Services;

namespace ArchitectureReconstructionPresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public string GetResult()
        {
            DataGathering dg = new DataGathering();

            
            //dg.GenerateResultFile();
            //dg.dependenciesGraph();
            //string allText = System.IO.File.ReadAllText(@"wwwroot//allresults.json");

            
           string allText = JsonSerializer.Serialize(EvolutionAnalysis.Timeline(), new JsonSerializerOptions(){WriteIndented = true});
            return allText;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}