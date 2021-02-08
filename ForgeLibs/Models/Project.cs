using Newtonsoft.Json;

namespace ForgeLibs.Models
{
	public class Project
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string CoverImageName { get; set; }
        public string Job { get; set; }
        [JsonProperty(PropertyName = "Model")]
        public string ModelRepo { get; set; }
        public int Importance { get; set; }
		public string URL { get; set; }
	}
}
