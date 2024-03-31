using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MathGame.Token
{
	public interface IJWTGenerator
	{
		string GenerateToken(IdentityUser user, IList<Claim> claims);
	}
}