using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ForgeLibs.Models.Forge
{
    public class ForgeModel
    {
        //public int Id { get; set; }
        public int ProjectId { get; set; }
        public string BucketKey { get; set; }
        public string ObjectId { get; set; }
        public string ObjectKey { get; set; }
        //public string Sha1 { get; set; }
        //public long Size { get; set; }
        //public string FilePath { get; set; }
        //public string BaseURN64 { get; set; }
        //public string Metadata { get; set; }
        //public string[] ViewGuids { get; set; }

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
        public Dictionary<int, int[]> ForgeTaskMapping { get; set; } = new Dictionary<int, int[]>();
		public List<dynamic> Tasks { get; set; }

		public async Task LoadjSONfromAddress(HttpClient httpClient)
		{
            if (!string.IsNullOrEmpty(Address))
            {
                var response = await httpClient.GetAsync(Address);
                if (response.IsSuccessStatusCode)
                {
                    JSON = await response.Content.ReadAsStringAsync();
                    Tasks = JsonConvert.DeserializeObject<List<dynamic>>(JSON);
                    FirstDate = Tasks.OrderBy(x => (DateTime?)x.pStart).FirstOrDefault().pStart;
                    LastDate = Tasks.OrderBy(x => (DateTime?)x.pEnd).LastOrDefault().pEnd;
                    //Console.WriteLine($"FirstDate : {FirstDate} , LastDate: {LastDate}");
                }

                string mapPath = Path.Combine(Path.GetDirectoryName(Address), $"{ Path.GetFileNameWithoutExtension(Address)}_map.json");
                Console.WriteLine(mapPath);
                response = await httpClient.GetAsync(mapPath);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(content);
                    dynamic[] mapping = JsonConvert.DeserializeObject<dynamic[]>(content);

                    //int counter = 0;
					foreach (dynamic item in mapping)
					{
      //                if(counter++ < 10)
						//{
      //                      Console.WriteLine(item["id"]);
      //                      Console.WriteLine(item["elements"]);
      //                  }
                        int taskId = item["id"];
                        int[] elements = ((JArray)item["elements"]).ToObject<int[]>();
                        ForgeTaskMapping.Add(taskId, elements);
      //                if (counter++ < 10)
						//{
      //                      Console.WriteLine(".... ok");
						//}
                    }

                    //Console.WriteLine(JsonConvert.SerializeObject(ForgeTaskMapping));
                }
				else
				{
                    Console.WriteLine($"File is {mapPath} not founded");
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

        public IEnumerable<dynamic> GetFilterTasks(DateTime startDate, DateTime endDate)
		{
           
            return Tasks.Where(x => (DateTime)x.pStart >= startDate && (DateTime)x.pEnd <= endDate);
        }

        public IEnumerable<dynamic> GetPresentTasks(DateTime date)
		{
            return Tasks.Where(x => (DateTime)x.pStart <= date && date <= (DateTime)x.pEnd);
        }

        public IEnumerable<dynamic> GetPastTasks(DateTime date)
        {
            return Tasks.Where(x => (DateTime)x.pStart < date && (DateTime)x.pEnd < date);
        }

        public IEnumerable<dynamic> GetFutureTasks(DateTime date)
        {
            return Tasks.Where(x => (DateTime)x.pEnd > date && (DateTime)x.pStart > date);
        }

        public IEnumerable<int> GetTaskObjectsIds(IEnumerable<dynamic> tasks)
		{
			foreach (dynamic task in tasks)
			{
                yield return task.pID;
            }
        }


        public IEnumerable<int> GetRevitIds(int[] tasksIds)
		{
			foreach (var pair in ForgeTaskMapping)
			{
                int taskId = pair.Key;
                if (tasksIds.Contains(taskId))
				{
                    int[] ids = pair.Value;
					foreach (int id in ids)
					{
                        yield return id;
					}
				}
			}
		}

        public IEnumerable<int> GetAllRevitIds()
		{
            foreach (var pair in ForgeTaskMapping)
            {
                int[] ids = pair.Value;
                foreach (int id in ids)
                {
                    yield return id;
                }
            }
        }
    }
}
