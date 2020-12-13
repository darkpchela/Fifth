using Fifth.Interfaces;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fifth.Controllers
{
    public class Home : Controller
    {
        private readonly IGameManageService gameManageService;
        private readonly IAppAuthenticationService authenticationService;

        public Home(IGameManageService gameManageService, IAppAuthenticationService authenticationService)
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
            int id = await gameManageService.CreateGameAsync(createGameVM);
            return RedirectToAction(nameof(Game), new { id = id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}