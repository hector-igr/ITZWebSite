using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeLibs.Models.Forge
{
    public class ForgeObjects
    {
        public _Items[] Items { get; set; }

        public class _Items
        {
            public string BucketKey { get; set; }
            public string ObjectKey { get; set; }
            public string ObjectId { get; set; }
            public string Sha1 { get; set; }
            public long Size { get; set; }
            public string Location { get; set; }
        }
        
    }
}
