using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

public class MiddlewareConsultaPop
{
    private readonly RequestDelegate next; //guarda a referencia do proximo middleware
    public MiddlewareConsultaPop(RequestDelegate nextMiddleware){
        next=nextMiddleware;
    }
    public MiddlewareConsultaPop(){
        
    }
    public async Task Invoke(HttpContext context){
        
        string[] segmentos =context.Request.Path.ToString().Split("/", System.StringSplitOptions.RemoveEmptyEntries);
        if(segmentos.Length == 2 && segmentos[0] == "pop"){
            var localidade = HttpUtility.UrlDecode(segmentos[1]);//decodifica do padrao de url para texto comum
            var populacao = (new Random()).Next(999,9999999);
            context.Response.ContentType = "text/html; charset=utf-8"; 
            StringBuilder html = new StringBuilder();
            html.Append($"<h3>População de {localidade.ToUpper()}</h3>");
            html.Append($"<p>{populacao:N0} habitantes</p>");
      
            await context.Response.WriteAsync(html.ToString());
        }
        else if (next!= null){
            await next(context);
        }
    }
}