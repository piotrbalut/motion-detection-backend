using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MotionDetection.Backend.Helpers
{
	public static class JwtHelper
	{
		public static async Task<object> GenerateToken(
			string email,
			IdentityUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration.JwtExpireDays));

			var token = new JwtSecurityToken(
					Configuration.JwtIssuer,
					Configuration.JwtIssuer,
					claims,
					expires: expires,
					signingCredentials: creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
        }
	}
}