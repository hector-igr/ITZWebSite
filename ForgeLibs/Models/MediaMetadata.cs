using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeLibs.Models
{
    public class MediaMetadata
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public int ProjectId { get; set; }
        public int Order { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        private string PredominantColorsRaw { get; set; }
        public string[] PredominantColors { get { return PredominantColorsRaw.Split(','); } }
        public string URL { get; set; }
        public int Rating { get; set; }
    }
}
