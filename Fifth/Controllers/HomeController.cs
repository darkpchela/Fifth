using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Game(int id)
        {
            HttpContext.Session.SetInt32("gameId", id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(GameSessionVM createGameVM)
        {
            int id = await gameManageService.CreateGameAsync(createGameVM);
            return RedirectToAction(nameof(Game), id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
