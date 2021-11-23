using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net.Http;
using System.Text;

namespace NBomberTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using var httpClient = new HttpClient();

            var getUrlStep = Step.Create("get-url", async context =>
            {
                try
                {
                    var response = await httpClient.GetAsync($"https://localhost:5001/api/Url/get-url?ShortUrl=http://localhost:5001/sfdA__Fd");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            });

            var getUrlScenario = ScenarioBuilder.CreateScenario("get-url", getUrlStep);

            NBomberRunner
                .RegisterScenarios(getUrlScenario)
                .Run();
        }
    }
}
