using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interop
{
    public class ForgeInterop
    {
        private readonly IJSRuntime JS;
        public ForgeInterop(IJSRuntime js)
        {
            this.JS = js;
        }

        public ValueTask<object> LoadDocument(string viewId, string token, string urn, object forgeViewer)
        {
            return JS.InvokeAsync<object>("forgeFunctions.initialize", viewId, token, 
                urn, DotNetObjectReference.Create(forgeViewer));
        }

        //public Task<object> LoadDocument(string viewId, string token, string urn, string onSelectHandlers)
        //{
        //    return JS.InvokeAsync<object>("forgeFunctions.initialize", viewId, token, urn, onSelectHandlers);
        //}

        public ValueTask<object> IsolateElement(string viewerId, int[] ids)
        {
            return JS.InvokeAsync<object>("forgeFunctions.isolateElements", viewerId, ids);
        }

        public ValueTask<object> ChangeColorElement(string viewerId, int[] ids, string rgbStr)
        {
            return JS.InvokeAsync<object>("forgeFunctions.changeColor", 
                viewerId, ids, rgbStr);
        }
    }
}
