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


}
