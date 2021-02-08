using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Text;
using ForgeLibs.Models;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using ForgeLibs.Models.Forge;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using ForgeLibs.Configuration;

namespace ITZWebClientApp.Infraestructure.Data
{
    public class JS_ItzModelRespository : IItzRepository
    {
        private Project[] projects;
        private ForgeModel[] models;
        private MediaMetadata[] imagesInfo;
        private AppInfo[] appInfo;

        HttpClient client { get; set; }
        public JS_ItzModelRespository(HttpClient client)
        {
            this.client = client;
        }

        public async Task LoadDataAsync(IConfiguration configuration)
        {
			//Console.WriteLine("JS_ItzModelRespository.LoadDataAsync()");
			projects = JsonConvert.DeserializeObject<Project[]>(await client.GetStringAsync(configuration["DbFiles:PROJECTLIST"]));
			models = JsonConvert.DeserializeObject<ForgeModel[]>(await client.GetStringAsync(configuration["DbFiles:FORGEMODELS"]));
			imagesInfo = await client.GetJsonAsync<MediaMetadata[]>(configuration["DbFiles:PROJECTMEDIAINFO"]);
			string json = await client.GetStringAsync(configuration["DbFiles:APPLIST"]);
			dynamic o = JsonConvert.DeserializeObject(json);
			json = JsonConvert.SerializeObject(o["apps"]);
			appInfo = JsonConvert.DeserializeObject<AppInfo[]>(json);
		}

        public IEnumerable<Project> Projects => this.projects;
        public IEnumerable<ForgeModel> ForgeModels => this.models;
        public IEnumerable<MediaMetadata> ImageInfo => this.imagesInfo;
		public IEnumerable<AppInfo> AppsInfo => this.appInfo;
        public ForgeModel GetForgeModelAsync(int id) => models.FirstOrDefault(x => x.ProjectId == id);

		public async Task LoadDataAsync(ITZ_WebConfig config)
		{
			//Console.WriteLine("JS_ItzModelRespository.LoadDataAsync()");
			projects = JsonConvert.DeserializeObject<Project[]>(await client.GetStringAsync(config.ProjectList));
			models = JsonConvert.DeserializeObject<ForgeModel[]>(await client.GetStringAsync(config.ForgeModels));

			imagesInfo = await client.GetJsonAsync<MediaMetadata[]>(config.ProjectMediaInfo);
			string json = await client.GetStringAsync(config.AppList);
			dynamic o = JsonConvert.DeserializeObject(json);
			json = JsonConvert.SerializeObject(o["apps"]);
			appInfo = JsonConvert.DeserializeObject<AppInfo[]>(json);
		}

		//public async Task<bool> DeleteForgeModelAsync(int id)
		//{
		//    ForgeModel model = models.FirstOrDefault(x => x.Id == id);
		//    if(model != null)
		//    {
		//        //models.Remove(model);
		//        return true;
		//    }
		//    return false;
		//}

		//public Task SaveForgeModelAsync(ForgeModel model)
		//{
		//    throw new NotImplementedException();
		//}
	}
}
