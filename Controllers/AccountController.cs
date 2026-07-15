using ElmahCore;
using HelpDesk.Client.Business.Observers;
using HelpDesk.Client.Business.Services;
using HelpDesk.Client.Business.Settings;
using HelpDesk.Client.Business.ViewModels;
using HelpDesk.Client.Dto.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HelpDesk.Client.UI.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IAccountService _accountService;
		private readonly IEmailService _emailService;
		private readonly HelpDeskApiSettings _helpDeskApiSettings;

		public AccountController(ILogger<AccountController> logger, IAccountService accountService, IEmailService emailService, IOptions<HelpDeskApiSettings> helpDeskApiSettings)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			_emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
			_helpDeskApiSettings = helpDeskApiSettings?.Value ?? throw new ArgumentNullException(nameof(helpDeskApiSettings));
		}

		private void ReportError(Exception ex)
		{
			try
			{
				_logger.LogError(ex, "AccountController error");
				ElmahExtensions.RiseError(ex);
			}
			catch
			{
				// swallow - logging should not throw
			}
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View(new UserLoginViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserLoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				model.UserDto = await _accountService.Login(model.UserLogin);

				var user = new UserSubject("1", "alex@oldmail.com");

				user.Attach(new EmailObserver());
				user.Attach(new AuditLogObserver());

				user.UpdateEmail("alex@newmail.com");

				if (model.UserDto is null)
				{
					ModelState.AddModelError(string.Empty, "Invalid credentials.");
					return View(model);
				}

				var claims = new[]
				{
					new Claim(ClaimTypes.Sid, model.UserDto.Id ?? string.Empty),
					new Claim(ClaimTypes.Name, string.Concat(model.UserDto.FirstName, " ", model.UserDto.LastName))
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

				var options = new CookieOptions
				{
					Expires = DateTimeOffset.UtcNow.AddHours(8),
					HttpOnly = true,
					Secure = Request.IsHttps,
					SameSite = SameSiteMode.Strict
				};

				Response.Cookies.Append("UserId", model.UserDto.Id ?? string.Empty, options);
				Response.Cookies.Append("UserName", model.UserDto.UserName ?? string.Empty, options);

				return RedirectToAction(nameof(DashBoardController.Index), "DashBoard");
			}
			catch (Exception ex)
			{
				ReportError(ex);
				ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				Response.Cookies.Delete("UserId");
				Response.Cookies.Delete("UserName");
			}
			catch (Exception ex)
			{
				ReportError(ex);
			}
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View(new UserRegistrationViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UserRegistrationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				_logger.LogInformation("Request sent to WebApiUrl: {Url} started at: {Now}", _helpDeskApiSettings.WebApiUrl, DateTime.UtcNow);

				


				model.IdentityResult = await _accountService.Create(model);

				if (model.IdentityResult.Errors.Any() || !string.IsNullOrEmpty(model.HttpClientError))
				{
					ModelState.AddModelError(string.Empty, "Registration failed.");
					return View(model);
				}

				var userDto = await _accountService.FindUserByEmail(model.UserRegistration.Email);
				if (string.IsNullOrEmpty(userDto?.UserName) || !string.IsNullOrEmpty(model.HttpClientError))
				{
					ModelState.AddModelError(string.Empty, "Unable to retrieve created user.");
					return View(model);
				}

				var userToken = await _accountService.GenerateEmailConfirmationToken(userDto);
				if (string.IsNullOrEmpty(userToken) || !string.IsNullOrEmpty(model.HttpClientError))
				{
					ModelState.AddModelError(string.Empty, "Unable to generate confirmation token.");
					return View(model);
				}

				var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token = userToken, email = userDto.Email }, Request.Scheme);
				var isEmailConfirmationSent = await _emailService.EmailConfirmation(userDto.Email, userToken, confirmationLink);
				model.IsUserCreated = isEmailConfirmationSent;

				_logger.LogInformation("Request sent to WebApiUrl: {Url} completed at: {Now}", _helpDeskApiSettings.WebApiUrl, DateTime.UtcNow);

				return View(model);
			}
			catch (Exception ex)
			{
				ReportError(ex);
				ModelState.AddModelError(string.Empty, "An error occurred while processing registration.");
				return View(model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var model = new EmailConfirmedViewModel();
			try
			{
				var emailConfirmed = new EmailConfirmedDto { Email = email, Token = token };
				var isEmailConfirmed = await _accountService.EmailConfirmed(emailConfirmed);
				model.Email = email;
				ViewBag.IsConfirmed = isEmailConfirmed;
			}
			catch (Exception ex)
			{
				ReportError(ex);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View(new ForgotPasswordViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var userDto = await _accountService.FindUserByEmail(model.ForgotPassword.Email);
				if (userDto != null)
				{
					var token = await _accountService.GeneratePasswordResetToken(userDto);
					if (!string.IsNullOrEmpty(token))
					{
						var confirmationLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = userDto.Email }, Request.Scheme);
						await _emailService.ResetPassword(userDto.Email, token, confirmationLink);
					}
				}
			}
			catch (Exception ex)
			{
				ReportError(ex);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ResetPassword(string token, string email)
		{
			var model = new ResetPasswordViewModel();
			model.ResetPassword.Token = token;
			model.ResetPassword.Email = email;
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				// Expect service to handle reset logic; keep the controller thin
				// e.g. await _accountService.ResetPassword(model.ResetPassword);
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}
			catch (Exception ex)
			{
				ReportError(ex);
				ModelState.AddModelError(string.Empty, "An error occurred while resetting the password.");
				return View(model);
			}
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}
	}
}
