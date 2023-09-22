using BL.Setup.Code.Repositories;
using BL.Setup.Infrastructure;
using BL.Setup.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BL.Setup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseRepository databaseRepo;

        public HomeController(ILogger<HomeController> logger, DatabaseRepository databaseRepository)
        {
            _logger = logger;
            databaseRepo = databaseRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult OneClickSetup()
        {
            try
            {
                // 1. Delete Database
                this.databaseRepo.DeleteDB();

                // 2. Create Database
                this.databaseRepo.CreateDB();

                // 3. Generate Schema
                this.databaseRepo.GenerateSchema();

                TempData[TempDataKeys.OneClickMsg] = "One Click Setup completed.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData[TempDataKeys.OneClickError] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}