using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CriacaoEndPoint
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMiddleware<MiddlewareConsultaPop>();//esses middlewares decidem como agir a cada requisicao do pipeline
            app.UseMiddleware<MiddlewareConsultaCep>();//esses, de agora,no caso, verificam a url para saber se agem ou nao
            app.Use(async (context,next)=>{
                context.Response.ContentType = "text/plain; charset=utg-8";
                await context.Response.WriteAsync("Middleware terminal alcancado.");
            });

          
        }
    }
}
