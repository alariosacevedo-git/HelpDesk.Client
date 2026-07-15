using ElmahCore;
using HelpDesk.Client.Business.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Client.UI.ViewComponents
{
	public class UserSessionViewComponent : ViewComponent
	{
		private readonly ILogger<UserSessionViewComponent> _logger;
		public UserSessionViewComponent(ILogger<UserSessionViewComponent> logger)
		{
			try
			{
				_logger = logger;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error: " + ex.Message);
				ElmahExtensions.RiseError(new Exception(ex.Message));
			}
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			UserSessionViewModel model = new UserSessionViewModel();
			try
			{
				model.UserId = Request.Cookies["UserId"];
				model.UserName = Request.Cookies["UserName"];
			}
			catch (Exception ex)
			{
				_logger.LogError("Error: " + ex.Message);
				ElmahExtensions.RiseError(new Exception(ex.Message));
			}
			return await Task.FromResult((IViewComponentResult)View("Index", model));
		}
	}
}
