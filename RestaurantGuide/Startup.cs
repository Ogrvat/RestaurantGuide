using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantGuide.Models;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RestaurantGuide
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string connection = Configuration.GetConnectionString("DefaultConnection");
			
			//1. Ахитектурно предусмотреть, что-бы изменение хранилища данных не потребовало переписывания всего приложения.
			services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection)); 

			services.AddControllers();
			//2. Описание доступных методов API должно быть легко получено клиентом, а так-же клиент должен иметь возможность проверить работу метод.
			services.AddSwaggerGen(c=> 
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("../swagger/v1/swagger.json", "RestaurantGuide API ");
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
