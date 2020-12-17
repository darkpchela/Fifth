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
using Fifth.Models;
using System.Linq;
using AutoMapper;

namespace Fifth.Controllers
{
    public class Home : Controller
    {
        private readonly IGameProccessManager gameManageService;
        private readonly ITagCrudService tagCrudService;
        private readonly IAppAuthenticationService authenticationService;
        private readonly ISessionTagCrudService sessionTagCrudService;
        private readonly IGamesCrudService gamesCrudService;
        private readonly IMapper mapper;

        public Home(IGameProccessManager gameManageService, IAppAuthenticationService authenticationService, ITagCrudService tagCrudService, ISessionTagCrudService sessionTagCrudService,
            IGamesCrudService gamesCrudService ,IMapper mapper)
        {
            this.gameManageService = gameManageService;
            this.authenticationService = authenticationService;
            this.tagCrudService = tagCrudService;
            this.sessionTagCrudService = sessionTagCrudService;
            this.gamesCrudService = gamesCrudService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _GamesTable(string tagsJson)
        {
            List<SessionData> sessions;
            if (string.IsNullOrEmpty(tagsJson))
                sessions = (await gamesCrudService.GetAllGamesAsync()).Select(g => g.Data).Where(g => !g.Started).ToList();
            else
            {
                var inputTags = JsonConvert.DeserializeObject<Tag[]>(tagsJson).Select(t => t.Id);
                sessions = (await sessionTagCrudService.GetSessionsByTagAsync(inputTags)).ToList();
            }
            var VMs = mapper.Map<IEnumerable<GameSessionVM>>(sessions);
            return PartialView(VMs);
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await tagCrudService.GetAll();
            return Json(tags);
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
            if (!ModelState.IsValid)
                return PartialView("_CreateGame" ,createGameVM);
            createGameVM.Username = HttpContext.User.Identity.Name;
            int id = await gameManageService.OpenGameAsync(createGameVM);
            return PartialView("_GameCreated", id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}