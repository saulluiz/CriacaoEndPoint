using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            /*    app.UseMiddleware<MiddlewareConsultaPop>();//esses middlewares decidem como agir a cada requisicao do pipeline
               app.UseMiddleware<MiddlewareConsultaCep>();//esses, de agora,no caso, verificam a url para saber se agem ou nao

                   ## A utilizacao de middlewares dessa forma faz com que a aplicacao fique refem de atualizar os endpoints sempre
                       pois qualquer alteracao na url quebraria o codigo. 

    */

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
/*                 endpoints.MapGet("{p1}/{p2}/{p3}", async context =>
 */                endpoints.MapGet("{arq}/{arquivo}.{ext}", async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("reqeuisicao foi roteada.\n");
                    foreach(var item in context.Request.RouteValues){
                        await context.Response.WriteAsync($"{item.Key}:{item.Value}");
                        //percorre os valores de rota, que sao divididos por barras, e os printa na tela
                    }

                });
                endpoints.MapGet("rota", async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("Requisicao foi roteada");
                });
                endpoints.MapGet("hab/{*local=São%20Paulo-SP}",EndpointConsultaPop.Endpoint).WithMetadata(new RouteNameMetadata("consultapop"));
                endpoints.MapGet("cep/{cep?}", EndpointConsultaCep.Endpoint);
                //se o endpoint corresponder a string, acontece o delegate(No caso, middleware)




            });
            app.Use(async (context, next) =>
            {
                context.Response.ContentType = "text/plain; charset=utg-8";
                await context.Response.WriteAsync("Middleware terminal alcancado.");
            });


        }
    }
}
