using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Controllers
{
    public class Home : Controller
    {
        private readonly IGamesManager gamesManager;
        private readonly IMapper mapper;
        private readonly ITagsProvider tagsProvider;

        public Home(IGamesManager gamesManager, ITagsProvider tagsProvider, IMapper mapper)
        {
            this.gamesManager = gamesManager;
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
            var allowed = sessions.Where(s => !s.Started);
            var VMs = mapper.Map<IEnumerable<SessionVM>>(allowed.ToList());
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