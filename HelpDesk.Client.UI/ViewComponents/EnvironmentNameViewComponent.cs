using ElmahCore;
using HelpDesk.Client.Business.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Client.UI.ViewComponents
{
	public class EnvironmentNameViewComponent : ViewComponent
	{
		private readonly ILogger<EnvironmentNameViewComponent> _logger;
		private IConfiguration _configuration { get; }
		private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment { get; set; }
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EnvironmentNameViewComponent(ILogger<EnvironmentNameViewComponent> logger, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
		{
			try
			{
				_logger = logger;
				_configuration = configuration;
				_hostingEnvironment = hostingEnvironment;
				_httpContextAccessor = httpContextAccessor;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error: " + ex.Message);
				ElmahExtensions.RiseError(new Exception(ex.Message));
			}
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			EnvironmentNameViewModel model = new EnvironmentNameViewModel();
			var output = string.Empty;
			try
			{
				model.EnvironmentName = _hostingEnvironment.EnvironmentName.ToString();
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
