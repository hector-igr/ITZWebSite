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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewElementId"></param>
        /// <param name="chartName"></param>
        /// <param name="barType">Chose 'bar' or 'horizontalBar'</param>
        /// <param name="forgeDomain"></param>
        /// <returns></returns>
        public ValueTask<bool> LoadBarChart(string viewElementId, string chartName, string barType, bool keepAspectRatio, Components.Charts.BarChart barChartComponent = null)
        {
            var result = Js.InvokeAsync<bool>("chartJS.setBarChart", chartName, viewElementId, barType, keepAspectRatio, DotNetObjectReference.Create(barChartComponent));
            return result;
        }

        public ValueTask<object> UpdateBarData(string chartName, string data)
        {
            return Js.InvokeAsync<object>("chartJS.updateChart", chartName, data);
        }

        public ValueTask<object> GetSelectedIds(string chartName)
        {
            return Js.InvokeAsync<object>("chartJS.getSelectedIds", chartName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartName"></param>
        /// <param name="axy">xAxy or yAxy</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="beginAtZero"></param>
        /// <param name="axyIndex">index of axy options</param>
        /// <param name="functionTicks">body of the function to change labels</param>
        public ValueTask<object> SetMinAndMax(string chartName, string axy, double min, double max, bool beginAtZero, int axyIndex = 0, string functionTicks = "")
		{
            return Js.InvokeAsync<object>("chartJS.setMinAndMax", chartName, axy,  min, max, beginAtZero, axyIndex, functionTicks);
        }

        public ValueTask<object> SetAspectRatio(string chartName, bool maintainAspectRatio)
		{
            return Js.InvokeAsync<object>("chartJS.setAspectRatio", chartName, maintainAspectRatio);
        }

        #endregion

        #region PieChart

        public ValueTask<bool> LoadPieChart(string viewElementId, string chartName, bool reset, Components.Charts.BarChart barChartComponent = null)
        {
            if(barChartComponent != null)
			{
                return Js.InvokeAsync<bool>("chartJsPie.setPieChart", chartName, viewElementId, reset, DotNetObjectReference.Create(barChartComponent));
            }
			else
			{
                return Js.InvokeAsync<bool>("chartJsPie.setPieChart", chartName, viewElementId, reset);
            }
            
        }

        public Task UpdatePieChart(string chartName, string data)
        {
            return Js.InvokeAsync<bool>("chartJsPie.updatePieChart", chartName, data).AsTask();
        }
        #endregion

    }
}
