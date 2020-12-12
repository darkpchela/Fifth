using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameManageService gameManageService;

        public HomeController(ILogger<HomeController> logger, IGameManageService gameManageService)
        {
            _logger = logger;
            this.gameManageService = gameManageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Game()
        {
            return View();
        }

        [HttpPost]
        public async void CreateGame(GameSessionVM createGameVM)
        {
            await gameManageService.CreateGameAsync(createGameVM);
            //return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
