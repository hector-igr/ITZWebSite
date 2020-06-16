using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interop
{
    
    public class GoldenLayoutInterop
    {
        private readonly IJSRuntime JS;
        public GoldenLayoutInterop(IJSRuntime js)
        {
            this.JS = js;
        }

        public Task<bool> LoadGoldenLayout(string containerName)
        {
            return JS.InvokeAsync<bool>("goldenLayout.ini", containerName).AsTask();
        }
    }
}
