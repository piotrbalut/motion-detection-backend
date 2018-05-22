using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Jwt;

namespace MotionDetection.Backend.Services
{
	public class JwtService : IJwtService
	{
		private readonly IOptions<JwtAuthentication> _jwtAuthentication;

		public JwtService(
			IOptions<JwtAuthentication> jwtAuthentication)
		{
			_jwtAuthentication = jwtAuthentication;
		}

		#region IJwtService Members

		public async Task<object> GenerateTokenAsync(
			string email,
			IdentityUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Value.JwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtAuthentication.Value.JwtExpireDays));

			var token = new JwtSecurityToken(
					_jwtAuthentication.Value.JwtIssuer,
					_jwtAuthentication.Value.JwtIssuer,
					claims,
					expires: expires,
					signingCredentials: creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		#endregion
	}
}