using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IGameManageService gameManageService;
        private readonly ICookieAuthenticationService authenticationService;

        public HomeController(IGameManageService gameManageService, ICookieAuthenticationService authenticationService)
        {
            this.gameManageService = gameManageService;
            this.authenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Game(int id)
        {
            HttpContext.Session.SetInt32("gameId", id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(GameSessionVM createGameVM)
        {
            if (!User.Identity.IsAuthenticated && !await authenticationService.SignInAsync(createGameVM.UserName, "") && !await authenticationService.SignUpAsync(createGameVM.UserName, ""))
                return RedirectToAction(nameof(Index));

            int id = await gameManageService.CreateGameAsync(createGameVM);
            var res = User.Identity.IsAuthenticated;

            return RedirectToAction(nameof(Game), id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
