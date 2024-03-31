using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MathGame.Token
{
	public class JWTGenerator : IJWTGenerator
	{
		private readonly IConfiguration _config;

		public JWTGenerator(IConfiguration config)
		{
			_config = config;

		}
		public string GenerateToken(IdentityUser user, IList<Claim> claims)
		{
			claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.UserName));
			claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds,
				Issuer = _config["Token:Issuer"],
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
    }
}