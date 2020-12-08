using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ForgeLibs.Models.Forge
{
    public class ForgeModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string BucketKey { get; set; }
        public string ObjectId { get; set; }
        public string ObjectKey { get; set; }
        //public string Sha1 { get; set; }
        //public long Size { get; set; }
        //public string FilePath { get; set; }
        //public string BaseURN64 { get; set; }
        //public string Metadata { get; set; }
        public string[] ViewGuids { get; set; }

        public ForgeSchedule[] Schedules { get; set; }
    }

    public class ForgeSchedule
	{
        public DateTime Now { get; set; } = DateTime.Now;
		public DateTime? FirstDate { get; set; }
		public DateTime? LastDate { get; set; }
		public string Address { get; set; }
		public double Version { get; set; }
        public string JSON { get; set; }
		public List<dynamic> Tasks { get; set; }

		public async Task LoadjSONfromAddress(HttpClient httpClient)
		{
            if(!string.IsNullOrEmpty(Address))
			{
                var response = await httpClient.GetAsync(Address);
                if(response.IsSuccessStatusCode)
				{
                    JSON = await response.Content.ReadAsStringAsync();
                    Tasks = JsonConvert.DeserializeObject<List<dynamic>>(JSON);
                    FirstDate = Tasks.OrderBy(x => (DateTime?)x.pStart).FirstOrDefault().pStart;
                    LastDate = Tasks.OrderBy(x => (DateTime?)x.pEnd).LastOrDefault().pEnd;
                    //Console.WriteLine($"FirstDate : {FirstDate} , LastDate: {LastDate}");
                }
            }
			else
			{
                Console.WriteLine("Cant load JSON from address because the address is empty");
            }
		}

        public void FilterJSON(DateTime startDate, DateTime endDate)
		{
            if (!string.IsNullOrEmpty(JSON))
			{
                //Console.WriteLine("JSON before filter : ");
                //Console.WriteLine(JSON);
                var tasks = Tasks.Where(x => (DateTime)x.pStart >= startDate && (DateTime)x.pEnd <= endDate).ToList();
                JSON = JsonConvert.SerializeObject(tasks);
                //Console.WriteLine("JSON after filter : ");
                //Console.WriteLine(JSON);
            }
			else
			{
                Console.WriteLine("JSON dont loaded!");
			}
        }


	}
}
