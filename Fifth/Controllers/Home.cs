﻿using Fifth.Interfaces;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fifth.Controllers
{
    public class Home : Controller
    {
        private readonly IGameProccessManager gameManageService;
        private readonly IAppAuthenticationService authenticationService;

        public Home(IGameProccessManager gameManageService, IAppAuthenticationService authenticationService)
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGame(CreateGameVM createGameVM)
        {
            createGameVM.Username = HttpContext.User.Identity.Name;
            //var a = JsonConvert.DeserializeObject<Tag[]>(createGameVM.Tags);
            int id = await gameManageService.OpenGameAsync(createGameVM);
            return RedirectToAction(nameof(Game), new { id = id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class Tag
        {
            public int? Id { get; set; }
            public string Value { get; set; }
        }
    }
}