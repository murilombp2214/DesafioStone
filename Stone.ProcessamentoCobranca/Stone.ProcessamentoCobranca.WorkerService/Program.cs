using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterServices()
                            .AddUtil(isDevelopment: true)
                            .AddHostedService<Worker>();
                });
    }
}
