using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interop
{
    public class ForgeInterop
    {
        //private readonly IJSRuntime JS;
        //public ForgeInterop(IJSRuntime js)
        //{
        //    this.JS = js;
        //}

        public static ValueTask<object> LoadDocument(IJSRuntime js, string viewId, string token, string urn, object forgeViewer)
        {
            return js.InvokeAsync<object>("forgeFunctions.initialize", viewId, token, 
                urn, DotNetObjectReference.Create(forgeViewer));
        }

        //public Task<object> LoadDocument(string viewId, string token, string urn, string onSelectHandlers)
        //{
        //    return JS.InvokeAsync<object>("forgeFunctions.initialize", viewId, token, urn, onSelectHandlers);
        //}

        public static ValueTask<object> IsolateElement(IJSRuntime js, string viewerId, int[] ids)
        {
            return js.InvokeAsync<object>("forgeFunctions.isolateElements", viewerId, ids);
        }

        public static ValueTask<object> ChangeColorElement(IJSRuntime js, string viewerId, int[] ids, string rgbStr)
        {
            return js.InvokeAsync<object>("forgeFunctions.changeColor", 
                viewerId, ids, rgbStr);
        }

        public static ValueTask<object> Resize(IJSRuntime js, string viewId)
		{
            return js.InvokeAsync<object>("forgeFunctions.resize", viewId);
        }

        public static ValueTask<object> ResetOverrideColors(IJSRuntime js, string viewId)
		{
            return js.InvokeAsync<object>("forgeFunctions.resetOverrideColors", viewId);
        }

        public static void ShowAll(IJSRuntime js, string viewId)
        {
            js.InvokeVoidAsync("forgeFunctions.showAll", viewId);
        }

        public static void FitToView(IJSRuntime js, string viewId, int[] ids = null)
        {
            js.InvokeVoidAsync("forgeFunctions.fitToView", viewId, ids);
        }
    }
}
