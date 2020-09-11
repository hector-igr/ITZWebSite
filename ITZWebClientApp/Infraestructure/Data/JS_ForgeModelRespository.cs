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

namespace ITZWebClientApp.Infraestructure.Data
{
    public class JS_ForgeModelRespository : IForgeModelRepository
    {
        Project[] projects;
        ForgeModel[] models;
        MediaMetadata[] imagesInfo;
        AppInfo[] appInfo;

        HttpClient client { get; set; }

        public JS_ForgeModelRespository(HttpClient client)
        {
            this.client = client;
        }

        public async Task GetDataAsync()
        {
            Console.WriteLine("JS_ForgeModelRespository.GetData()");
            string str = await client.GetStringAsync("/db/hgr_projects2.json");

            projects = JsonConvert.DeserializeObject<Project[]>(str);
            //projects = await client.GetJsonAsync<Project[]>("/db/projects.json");
            
            str = await client.GetStringAsync("/db/imageInfo2.json");
            imagesInfo = JsonConvert.DeserializeObject<MediaMetadata[]>(str);

            str = await client.GetStringAsync("/db/appList.json");
            dynamic o = JsonConvert.DeserializeObject(str);
            str = JsonConvert.SerializeObject(o["apps"]);
            appInfo = JsonConvert.DeserializeObject<AppInfo[]>(str);
        }


        public IEnumerable<Project> Projects => this.projects;

        public IEnumerable<ForgeModel> ForgeModels => this.models;

        public IEnumerable<MediaMetadata> ImageInfo => this.imagesInfo;

		public AppInfo[] AppsInfo => this.appInfo;

		public async Task<bool> DeleteForgeModelAsync(int id)
        {
            ForgeModel model = models.FirstOrDefault(x => x.Id == id);
            if(model != null)
            {
                //models.Remove(model);
                return true;
            }
            return false;
        }

        public async Task<ForgeModel> GetForgeModelAsync(int id)
        {
            Console.WriteLine("JS_ForgeModelRespository.GetForgeModelAsync()");
            models = await client.GetJsonAsync<ForgeModel[]>("db/models.json");
            ForgeModel model = models.FirstOrDefault(x => x.Id == id);
            return model;
        }


        public Task SaveForgeModelAsync(ForgeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
