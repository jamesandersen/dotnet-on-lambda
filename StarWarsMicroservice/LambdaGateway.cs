using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;
using StarWarsMicroservice;
using System.IO;

namespace StarWarsMicroservice
{
    public class LambdaGateway : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseApiGateway()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
        }
    }
}