using ElmahCore;
using HelpDesk.Client.Business.ViewModels;
using HelpDesk.Client.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelpDesk.Client.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;


		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			HomeViewModel model = null;
			try
			{
				model = new HomeViewModel();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error: " + ex.Message);
				ElmahExtensions.RiseError(new Exception(ex.Message));
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(HomeViewModel model)
		{
			try
			{

			}
			catch (Exception ex)
			{
				_logger.LogError("Error: " + ex.Message);
				ElmahExtensions.RiseError(new Exception(ex.Message));
			}
			return View(model);
		}



		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
