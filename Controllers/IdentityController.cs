using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MathGame.Models;
using MathGame.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MathGame.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class IdentityController : ControllerBase
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _config;
		private readonly IJWTGenerator _jwtToken;

		public IdentityController(
			UserManager<IdentityUser> userManager,
			 SignInManager<IdentityUser> signInManager,
			  RoleManager<IdentityRole> roleManager,
			  IConfiguration config,
			  IJWTGenerator jWTGenerator)
		{
			_roleManager = roleManager;
			_config = config;
			_signInManager = signInManager;
			_userManager = userManager;
			_jwtToken = jWTGenerator;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			var userFromDb = await _userManager.FindByNameAsync(model.Username);

			if (userFromDb == null)
			{
				return BadRequest();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, model.Password, false);


			if (!result.Succeeded)
			{
				return BadRequest();
			}

			IList<Claim> claims = await _userManager.GetClaimsAsync(userFromDb);

			var token = _jwtToken.GenerateToken(userFromDb, claims);

			HttpContext.Session.SetString("token", token);
			HttpContext.Session.SetString("PlayerName", userFromDb.UserName);
			HttpContext.Session.SetString("Username", userFromDb.UserName);
			HttpContext.Session.SetString("Email", userFromDb.Email);

			//TODO : are these needed?
			Response.Cookies.Append("token", token);
			Response.Cookies.Append("PlayerName", userFromDb.UserName);
			Response.Cookies.Append("Username", userFromDb.UserName);
			Response.Cookies.Append("Email", userFromDb.Email);

			return RedirectToAction("Index", "Home");
        }

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var userToCreate = new IdentityUser
			{
				Email = model.Email,
				UserName = model.Username,
			};

			var result = await _userManager.CreateAsync(userToCreate, model.Password);

            if (result.Succeeded)
            {
				return Ok(result);
            }

			return BadRequest(result);
		}
	}
}