using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using DatisBasePay.Services;
using DatisBasePay.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace DatisBasePay
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
						 .SetBasePath(env.ContentRootPath)
						 .AddJsonFile("appsettings.json")
						 .AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddSingleton(Configuration);
			services.AddScoped<IEmployeeData, SqlEmployeeData>();
			services.AddDbContext<DatisBasePayDbContext>(options =>
				   options.UseSqlServer(Configuration.GetConnectionString("DatisBasePay")));
			services.AddIdentity<User, IdentityRole>()
				   .AddEntityFrameworkStores<DatisBasePayDbContext>();
			services.AddMvc().AddJsonOptions(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
			});
		}

		// This method gets called by the runtime.
		// Use this method to configure the HTTP request pipeline.
		public void Configure(
		    IApplicationBuilder app,
		    IHostingEnvironment env,
		    ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(new ExceptionHandlerOptions
				{
					ExceptionHandler = context => context.Response.WriteAsync("An error has occurred.")
				});
			}

			app.UseFileServer();
			app.UseNodeModules(env.ContentRootPath);
			app.UseIdentity();
			app.UseMvc(ConfigureRoutes);
		}

		private void ConfigureRoutes(IRouteBuilder routeBuilder)
		{
			routeBuilder.MapRoute("Default",
			    "{controller=Home}/{action=Index}/{id?}");
		}
	}
}
