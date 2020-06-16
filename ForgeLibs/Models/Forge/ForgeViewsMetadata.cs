using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeLibs.Models.Forge
{
    public class ForgeViewsMetadata
    {
        public _Data Data { get; set; }
        
        public class _Data
        {
            public string Type { get; set; }
            public _Metadata[] Metadata { get; set; }

            public class _Metadata
            {
                public string Name { get; set; }
                public string Role { get; set; }
                public string Guid { get; set; }
            }
        }
    }
}
