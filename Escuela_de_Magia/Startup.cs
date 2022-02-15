using Escuela_de_Magia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

namespace Escuela_de_Magia
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
            services.AddControllers();
            //services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Hogwarts"));
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Hogwarts")));

            //Se agrega en generador de Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "Api Howarts_Test", Version = "v1" });
            });

            // Configuracion de CORS
            services.AddCors(op =>
            {
                op.AddPolicy(name: "MyPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("PUT", "POST", "GET", "DELETE");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Habilitar swagger
            app.UseSwagger();

            //indica la ruta para generar la configuración de swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Howarts_Test");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy"); // Habilito el uso de CORS

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
