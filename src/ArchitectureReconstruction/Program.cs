using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureReconstructionPresentation.Services;
using LibGit2Sharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ArchitectureReconstructionPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //EvolutionAnalysis.Timeline();
            /*
            using (var repo = new Repository("/Users/bjergfelt/Documents/GitHub/ITU/ArchReconstrution/src/Blazorise"))
            {
                EvolutionAnalysis.ResetToState(repo);
            }
             */
            
             CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}