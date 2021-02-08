using ForgeLibs.Configuration;
using ForgeLibs.Models;
using ForgeLibs.Models.Forge;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITZWebClientApp.Infraestructure.Data
{
    public interface IItzRepository
    {
		IEnumerable<Project> Projects { get; }
        IEnumerable<ForgeModel> ForgeModels { get; }
        IEnumerable<MediaMetadata> ImageInfo { get; }
        IEnumerable<AppInfo> AppsInfo { get; }

        Task LoadDataAsync(IConfiguration config);
        Task LoadDataAsync(ITZ_WebConfig config);
        ForgeModel GetForgeModelAsync (int id);
        //Task SaveForgeModelAsync(ForgeModel model);
        //Task<bool >DeleteForgeModelAsync(int id);
    }
}
