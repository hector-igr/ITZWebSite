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

        public async Task LoadDataAsync()
        {
            Console.WriteLine("JS_ItzModelRespository.LoadDataAsync()");
            projects = await client.GetJsonAsync<Project[]>("/db/hgr_projects2.json");
            models = await client.GetJsonAsync<ForgeModel[]>("/db/forgeModels.json");
            imagesInfo = await client.GetJsonAsync<MediaMetadata[]>("/db/imageInfo2.json");
			
            string json = await client.GetStringAsync("/db/appList.json");
            dynamic o = JsonConvert.DeserializeObject(json);
            json = JsonConvert.SerializeObject(o["apps"]);
            appInfo = JsonConvert.DeserializeObject<AppInfo[]>(json);
        }
        public IEnumerable<Project> Projects => this.projects;
        public IEnumerable<ForgeModel> ForgeModels => this.models;
        public IEnumerable<MediaMetadata> ImageInfo => this.imagesInfo;
		public IEnumerable<AppInfo> AppsInfo => this.appInfo;
        public ForgeModel GetForgeModelAsync(int id) => models.FirstOrDefault(x => x.ProjectId == id);

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

        public Task SaveForgeModelAsync(ForgeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
