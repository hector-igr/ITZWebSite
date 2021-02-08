using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ITZWebClientApp.Infraestructure.Data;
using ITZWebClientApp.Infraestructure.StateManagement;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Net.Http;
using Microsoft.JSInterop;
using ForgeLibs.Configuration;
using System.Configuration;
using System.Reflection;
using Microsoft.Extensions.Configuration.Json;


namespace ITZWebClientApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<ForgeLibs.Data.OmniClassRepository>();
            builder.Services.AddSingleton<IItzRepository, JS_ItzModelRespository>();
            builder.Services.AddSingleton<ForgeQueryState>();

			//AD OTHER CONFIG FILES
			//var http = new HttpClient()
			//{
			//	BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
			//};
			//using var response = await http.GetAsync("someOtherSettings.json");
			//using var stream = await response.Content.ReadAsStreamAsync();
			//builder.Configuration.AddJsonStream(stream);

			await LoadConfig_1(builder);
		}

		private static async Task LoadConfig_1(WebAssemblyHostBuilder builder)
		{
			ITZ_WebConfig webConfig = builder.Configuration.GetSection("DbFiles").Get<ITZ_WebConfig>();
			builder.Services.AddSingleton(x => webConfig);

			WebAssemblyHost host = builder.Build();
			var repo = host.Services.GetRequiredService<IItzRepository>();
			await repo.LoadDataAsync(webConfig);

			var omni = host.Services.GetRequiredService<ForgeLibs.Data.OmniClassRepository>();
			await omni.LoadFileAsync();
			await host.RunAsync();
		}

		//TRY TO USE IOPTIONS .. FAILED
		private static async Task LoadConfig_2(WebAssemblyHostBuilder builder)
		{
			ITZ_WebConfig webConfig = builder.Configuration.GetSection("DbFiles").Get<ITZ_WebConfig>();
			Console.WriteLine(webConfig.ProjectMediaInfo);
			builder.Services.Configure<ITZ_WebConfig>(x => builder.Configuration.Bind(webConfig));

			WebAssemblyHost host = builder.Build();

			var repo = host.Services.GetRequiredService<IItzRepository>();
			await repo.LoadDataAsync(webConfig);

			var omni = host.Services.GetRequiredService<ForgeLibs.Data.OmniClassRepository>();
			await omni.LoadFileAsync();
			await host.RunAsync();
		}
	}
}
