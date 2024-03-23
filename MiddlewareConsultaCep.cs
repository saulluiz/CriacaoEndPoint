using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

public class MiddlewareConsultaCep
{
    private readonly RequestDelegate next; //guarda a referencia do proximo middleware
    public MiddlewareConsultaCep(RequestDelegate nextMiddleware)
    {
        next = nextMiddleware;
    }
    public MiddlewareConsultaCep(){

    }
    public async Task Invoke(HttpContext context)
    {


                string cep = context.Request.RouteValues["cep"] as string;
                Console.WriteLine(cep);

       
            
          
            var objetoCep = await ConsultaCep(cep);
            context.Response.ContentType = "text/html; charset=utf-8";
            
            StringBuilder html = new StringBuilder();
            html.Append($"<h3>CEP {objetoCep.CEP}</p>");
            html.Append($"<p>Logradouro: {objetoCep.Logradouro}</p>");
            html.Append($"<p>Bairro: {objetoCep.Bairro}</p>");
            html.Append($"<p>Cidade/UF: {objetoCep.Localidade}/{objetoCep.Estado}</p>");
            string localidade = HttpUtility.UrlEncode($"{objetoCep.Localidade}-{objetoCep.Estado}");
            html.Append($"<p><a href='/pop/{localidade}'>Consultar Populacao</a></p>");
               
            await context.Response.WriteAsync(html.ToString());

        
         if (next != null)
        {
            //Quando o endpoint nao leva cumpre o caminho cep e valor, vamos ao proximo middleware. No caso, o proximo confere
            //se o endp[oint tem o caminho especifico e, se nao tiver, vai ao proximo middleware tmb
            await next(context);
        }

    }
    private async Task<jsonCep> ConsultaCep(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json";
        var cliente = new HttpClient();

        cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta Cep");
        var response = await cliente.GetAsync(url);
        var dadosCEP = await response.Content.ReadAsStringAsync();
        dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
        return JsonConvert.DeserializeObject<jsonCep>(dadosCEP);
    }
}