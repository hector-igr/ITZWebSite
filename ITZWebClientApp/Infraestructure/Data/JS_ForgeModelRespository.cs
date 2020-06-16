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

        HttpClient client;

        public JS_ForgeModelRespository(HttpClient client)
        {
            this.client = client;
            //GetDataFromIO(client);
            //GetData(client);
        }

        public void GetDataFromIO()
        {
            Console.WriteLine("JS_ForgeModelRespository.GetDataFromIO()");
            using (StreamReader sr = new StreamReader("./db/hgr_projects.json"))
            {
                string json = sr.ReadToEnd();
                //Console.Write(json);
                projects = JsonConvert.DeserializeObject<Project[]>(json);
            }
            using (StreamReader sr = new StreamReader("./db/imageInfo.json"))
            {
                string json = sr.ReadToEnd();
                //Console.Write(json);
                imagesInfo = JsonConvert.DeserializeObject<MediaMetadata[]>(json);
            }
        }

        public async Task GetDataAsync()
        {
            Console.WriteLine("JS_ForgeModelRespository.GetData()");
            string str = await client.GetStringAsync("/db/hgr_projects.json");

            projects = JsonConvert.DeserializeObject<Project[]>(str);
            //projects = await client.GetJsonAsync<Project[]>("/db/projects.json");
            
            str = await client.GetStringAsync("/db/imageInfo.json");
            imagesInfo = JsonConvert.DeserializeObject<MediaMetadata[]>(str);
        }

        public IEnumerable<Project> Projects => this.projects;

        public IEnumerable<ForgeModel> ForgeModels => this.models;

        public IEnumerable<MediaMetadata> ImageInfo => this.imagesInfo;

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
