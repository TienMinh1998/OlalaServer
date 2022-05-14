using APIProject.CronJob;
using APIProject.Job;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject
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
                // Add the required Quartz.NET services
                services.AddQuartz(q =>
                {
                    // Use a Scoped container to create jobs. I'll touch on this later
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();
                    q.AddJobAndTrigger<Jobclass>(hostContext.Configuration);
                });

                // Add the Quartz.NET hosted service

                services.AddQuartzHostedService(
                    q => q.WaitForJobsToComplete = true);

                // other config
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSentry(o =>
                    {
                        o.Dsn = "https://77022c7ab8614e8590cbfccc5af85a36@o1237928.ingest.sentry.io/6406971";
                        // When configuring for the first time, to see what the SDK is doing:
                        o.Debug = true;
                        // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                        // We recommend adjusting this value in production.
                        o.TracesSampleRate = 1.0;
                    });
                });
    }
}
