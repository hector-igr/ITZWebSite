using ForgeLibs.Models.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interfaces
{
    public interface ForgeDesktop
    {
        IEnumerable<ForgeElement> ForgeElements { get; set; }
    }
}
