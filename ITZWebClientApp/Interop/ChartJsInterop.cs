using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;


namespace ITZWebClientApp.Interop
{
    public class ChartJsInterop
    {
        private readonly IJSRuntime Js;
        public ChartJsInterop(IJSRuntime js)
        {
            this.Js = js;
        }
        #region BarChart

        public ValueTask<bool> LoadBarChart(string viewElementId, string chartName)
        {
            return Js.InvokeAsync<bool>("chartJS.setBarChart", chartName, viewElementId, null);
        }

        public ValueTask<bool> LoadBarChart(string viewElementId, string chartName, Pages.ForgeDomain forgeDomain)
        {
            return Js.InvokeAsync<bool>("chartJS.setBarChart", chartName, viewElementId, DotNetObjectReference.Create(forgeDomain));
        }

        public ValueTask<object> UpdateBarData(string chartName, string data)
        {
            return Js.InvokeAsync<object>("chartJS.updateChart", chartName, data);
        }

        public ValueTask<object> GetSelectedIds(string chartName)
        {
            return Js.InvokeAsync<object>("chartJS.getSelectedIds", chartName);
        }
        #endregion

        #region PieChart

        public ValueTask<bool> LoadPieChart(string viewElementId, string chartName, Pages.ForgeDomain forgeDomain, bool reset)
        {
            return Js.InvokeAsync<bool>("chartJsPie.setPieChart", chartName, viewElementId, DotNetObjectReference.Create(forgeDomain), reset);
        }

        public Task UpdatePieChart(string chartName, string data)
        {
            return Js.InvokeAsync<bool>("chartJsPie.updatePieChart", chartName, data).AsTask();
        }
        #endregion

    }
}
