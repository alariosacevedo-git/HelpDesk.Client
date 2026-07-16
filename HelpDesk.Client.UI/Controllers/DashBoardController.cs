using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Client.UI.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ILogger<DashBoardController> _logger;
        public DashBoardController(ILogger<DashBoardController> logger)
        {
            try
            {
                _logger = logger;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return View();
        }
    }
}
