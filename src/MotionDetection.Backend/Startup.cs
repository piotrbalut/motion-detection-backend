using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Database;
using MotionDetection.Backend.Models.Jwt;
using MotionDetection.Backend.Models.Mailgun;
using MotionDetection.Backend.Models.Plivo;
using MotionDetection.Backend.Services;

namespace MotionDetection.Backend
{
	public class Startup
	{
		private const string ConfigurationPlivoAuthKey = "PlivoAuth";
		private const string ConfigurationPlivoSettingsKey = "PlivoSettings";
		private const string ConfigurationJwtAuthenticationKey = "JwtAuthentication";
		private const string ConfigurationMailgunKey = "Mailgun";
		private const string ConfigurationJwtAuthenticationIssuerKey = "JwtAuthentication:JwtIssuer";
		private const string ConfigurationJwtAuthenticationKeyKey = "JwtAuthentication:JwtKey";
		private const string DefaultRequestCulture = "en-US";
		private const bool AcceptLanguageHeaderRequestCultureProvider = false;

		private readonly CultureInfo[] _supportedCultures =
		{
			new CultureInfo("en-US"),
			new CultureInfo("pl-PL")
		};

		public Startup(
			IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		public void ConfigureServices(
			IServiceCollection services)
		{
			ConfigureDbContexts(services);
			ConfigureConfiguration(services);
			ConfigureCustomServices(services);
			
			services.AddIdentity<User, IdentityRole>()
			        .AddEntityFrameworkStores<CameraDbContext>()
			        .AddDefaultTokenProviders();

			services.AddAutoMapper();

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
							ValidIssuer = Configuration.GetSection(ConfigurationJwtAuthenticationIssuerKey).Value,
							ValidAudience = Configuration.GetSection(ConfigurationJwtAuthenticationIssuerKey).Value,
							IssuerSigningKey = new SymmetricSecurityKey(
								Encoding.UTF8.GetBytes(Configuration.GetSection(ConfigurationJwtAuthenticationKeyKey).Value)),
							ClockSkew = TimeSpan.Zero
						};
					});

			services.AddLocalization();
			services.Configure<RequestLocalizationOptions>(
				options =>
				{
					options.DefaultRequestCulture = new RequestCulture(DefaultRequestCulture, DefaultRequestCulture);
					options.SupportedCultures = _supportedCultures;

					if (!AcceptLanguageHeaderRequestCultureProvider)
					{
						//https://stackoverflow.com/questions/44480759/asp-net-core-default-language-is-always-english
						options.RequestCultureProviders = new List<IRequestCultureProvider>
						{
							new QueryStringRequestCultureProvider(),
							new CookieRequestCultureProvider()
						};
					}
				});

			services.AddMvc();
		}

		private void ConfigureDbContexts(IServiceCollection services)
			=> services.AddDbContext<CameraDbContext>();

		private void ConfigureConfiguration(
			IServiceCollection services)
		{
			services.Configure<PlivoAuth>(Configuration.GetSection(ConfigurationPlivoAuthKey));
			services.Configure<PlivoSettings>(Configuration.GetSection(ConfigurationPlivoSettingsKey));
			services.Configure<JwtAuthentication>(Configuration.GetSection(ConfigurationJwtAuthenticationKey));
			services.Configure<MailgunSettings>(Configuration.GetSection(ConfigurationMailgunKey));
        }

		private void ConfigureCustomServices(
			IServiceCollection services)
		{
			services.AddSingleton<ICustomDateTime, CustomDateTime>();
			services.AddSingleton<IMailService, MailService>();
			services.AddSingleton<ISmsService, SmsService>();
			services.AddSingleton<IJwtService, JwtService>();
			services.AddSingleton<ICommonResource, CommonResource>();
        }

        public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			CameraDbContext dbContext
		)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			if (!AcceptLanguageHeaderRequestCultureProvider)
			{
				app.UseRequestLocalization();
			}

			app.UseMvc();

			//https://stackoverflow.com/questions/35797628/ef7-generates-wrong-migrations-with-sqlite
			//dbContext.Database.EnsureCreated();
			//Init database
			//DbInitializer.Initialize(dbContext);
		}
	}
}