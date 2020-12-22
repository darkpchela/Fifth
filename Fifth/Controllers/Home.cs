using AutoMapper;
using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Controllers
{
    public class Home : Controller
    {
        private readonly IGamesManager gamesManager;
        private readonly IAppAuthenticationService authenticationService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITagsProvider tagsProvider;

        public Home(IGamesManager gamesManager, IAppAuthenticationService authenticationService, IUnitOfWork unitOfWork, ITagsProvider tagsProvider,IMapper mapper)
        {
            this.gamesManager = gamesManager;
            this.authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.tagsProvider = tagsProvider;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> _GamesTable(string tagsJson)
        {
            IEnumerable<SessionData> sessions = await tagsProvider.GetSessionsByTag(tagsJson);
            var VMs = mapper.Map<IEnumerable<SessionVM>>(sessions);
            return PartialView(VMs);
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await tagsProvider.GetAllTags();
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
                return PartialView("_CreateGame", createGameVM);
            int id = await gamesManager.CreateGameAsync(createGameVM, HttpContext.User.Identity.Name);
            if (id == -1)
            {
                ModelState.AddModelError("", "Name is already taken!");
                return PartialView("_CreateGame", createGameVM);
            }
            return PartialView("_GameCreated", id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}