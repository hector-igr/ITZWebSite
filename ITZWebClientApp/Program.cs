using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ITZWebClientApp.Infraestructure.Data;
using ITZWebClientApp.Infraestructure.StateManagement;

using Serilog;
using Serilog.Core;
using System.Net.Http;
using Microsoft.JSInterop;

namespace ITZWebClientApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddSingleton<ForgeLibs.Data.OmniClassRepository>();
            builder.Services.AddSingleton<IItzRepository, JS_ItzModelRespository>();
            builder.Services.AddSingleton<ForgeQueryState>();

            var host = builder.Build();
            var omni = host.Services.GetRequiredService<ForgeLibs.Data.OmniClassRepository>();
            await omni.LoadFileAsync();

            var repo = host.Services.GetRequiredService<IItzRepository>();
            await repo.LoadDataAsync();

            //         var cat = omni.GetCategory("MX.03.01");
            //         Console.WriteLine(cat);
            //var prop = omni.GetParameters("MX.03.01")[0];
            //Console.WriteLine(prop);
            //var prop2 = omni.GetGroupingParameters("MX.03.01")[0];
            //Console.WriteLine(prop2);
            await host.RunAsync();
        }
    }
}
