using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shortid;
using shortid.Configuration;
using System;
using System.Net.Http;
using System.Text;

namespace NBomberTest
{
    public class Program
    {

        private static string GetValueFromResponce(string responceResult, string value)
        {
            return JObject.Parse(responceResult)
                        ?.GetValue(value)
                        ?.ToString();
        }

        static void Main(string[] args)
        {
            using var httpClient = new HttpClient();

            var options = new GenerationOptions
            {
                Length = 8
            };

            var getUrlStep = Step.Create("get-url",
                timeout: TimeSpan.FromSeconds(5),
                execute: async context =>
            {
                try
                {
                    var url = context.Data["url"];

                    var response = await httpClient.GetAsync($"https://localhost:5001/api/Url/get-url?ShortUrl={url}");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            });

            var updateUrlStep = Step.Create("update-url", async context =>
            {
                try
                {
                    var id = context.Data["id"];

                    var jsonString = JsonConvert.SerializeObject(new { urlId = id });
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"https://localhost:5001/api/Url/update-url", httpContent);
                    var result = response.Content.ReadAsStringAsync().Result;

                    context.Data["url"] = GetValueFromResponce(result, "shortUrl");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            });

            var createUrlStep = Step.Create("create-url", async context =>
            {
                try
                {
                    var jsonString = JsonConvert.SerializeObject(new 
                    { 
                        longUrl = $"https://www.google.com/{ShortId.Generate(options)}"
                    });
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://localhost:5001/api/Url/create-url", httpContent);
                    var result = response.Content.ReadAsStringAsync().Result;

                    context.Data["url"] = GetValueFromResponce(result, "shortUrl");
                    context.Data["id"] = GetValueFromResponce(result, "id");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            });

            var deleteUrlStep = Step.Create("delete-url", async context =>
            {
                try
                {
                    var id = context.Data["id"];

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("https://localhost:5001/api/Url/delete-url"),
                        Content = new StringContent(JsonConvert.SerializeObject(new { id }), Encoding.UTF8, "application/json")
                    };

                    var response = await httpClient.SendAsync(request);

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            });

            var firstScenario = ScenarioBuilder.CreateScenario("firstScenario", createUrlStep, updateUrlStep, getUrlStep, deleteUrlStep)
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(
                    Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(30))
                );

            var redisScenario = ScenarioBuilder.CreateScenario("redisScenario", createUrlStep, getUrlStep, getUrlStep, deleteUrlStep)
               .WithWarmUpDuration(TimeSpan.FromSeconds(10))
               .WithLoadSimulations(
                   Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(20))
               );

            NBomberRunner
                .RegisterScenarios(redisScenario)
                .WithReportFormats(ReportFormat.Txt)
                .Run();
        }
    }
}
