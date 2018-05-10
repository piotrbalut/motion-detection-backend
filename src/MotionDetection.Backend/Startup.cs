using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Jwt;
using MotionDetection.Backend.Models.Plivo;
using MotionDetection.Backend.Services;

namespace MotionDetection.Backend
{
	public class Startup
	{
		public Startup(
			IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(
			IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>();
			services.AddDbContext<CameraDbContext>();

			services.Configure<PlivoAuth>(Configuration.GetSection("PlivoAuth"));
			services.Configure<PlivoSettings>(Configuration.GetSection("PlivoSettings"));
			services.Configure<JwtAuthentication>(Configuration.GetSection("JwtAuthentication"));
			services.AddSingleton<ISmsService, SmsService>();
			services.AddSingleton<IJwtService, JwtService>();

			// ===== Add Identity ========
			services.AddIdentity<IdentityUser, IdentityRole>()
			        .AddEntityFrameworkStores<ApplicationDbContext>()
			        .AddDefaultTokenProviders();

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			services
				.AddAuthentication(
					options =>
					{
						options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
						options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
						options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					})
				.AddJwtBearer(
					cfg =>
					{
						cfg.RequireHttpsMetadata = false;
						cfg.SaveToken = true;
						cfg.TokenValidationParameters = new TokenValidationParameters
						{
							ValidIssuer = Configuration.GetSection("JwtAuthentication:JwtIssuer").Value,
							ValidAudience = Configuration.GetSection("JwtAuthentication:JwtIssuer").Value,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwtAuthentication:JwtKey").Value)),
							ClockSkew = TimeSpan.Zero
						};
					});

			services.AddMvc();
		}

		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ApplicationDbContext dbContext
		)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// ===== Use Authentication ======
			app.UseAuthentication();
			app.UseMvc();

			// ===== Create tables ======
			dbContext.Database.EnsureCreated();
		}
	}
}