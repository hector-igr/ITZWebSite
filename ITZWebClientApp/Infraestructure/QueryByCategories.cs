using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Infraestructure
{
    public class QueryByCategories
    {
        public List<string> Categories { get; set; } = new List<string> { };
        public List<string> Properties { get; set; } = new List<string> { };
        public List<string> GroupParameters { get; set; } = new List<string> { };

    }
}
