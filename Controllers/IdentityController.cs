using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using MathGame.Models;
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

		public IdentityController(
			UserManager<IdentityUser> userManager,
			 SignInManager<IdentityUser> signInManager,
			  RoleManager<IdentityRole> roleManager,
			  IConfiguration config)
		{
			_roleManager = roleManager;
			_config = config;
			_signInManager = signInManager;
			_userManager = userManager;
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

			HttpContext.Session.SetString("token", "token");
			HttpContext.Session.SetString("PlayerName", userFromDb.UserName);
			return RedirectToAction("Index", "Game");
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