using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interop
{
    public class Utils_DOM
    {
        public static Task<object> MakeDragableElement(IJSRuntime js, string id)
        {
            return js.InvokeAsync<object>("dragableElement.ini", id).AsTask();
        }

        public static Task<Object> RegisterOnIni(IJSRuntime js)
        {
            return js.InvokeAsync<object>("ForgeAppDOM.Ini").AsTask();
        }

        public static Task<Object> RegisterBasicPageRoutines(IJSRuntime js)
        {
            return js.InvokeAsync<object>("ForgeAppDOM.SubscriveVideoHover").AsTask();
        }
    }
}
