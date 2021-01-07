using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Interop
{
	public class SelectDropDownInterop
	{
		public static async ValueTask<int> GetSelectedIndex(IJSRuntime js, string id)
		{
			return await js.InvokeAsync<int>("GetSelectedIndex", id);
		}

		public static async void ChangeSelectIndexById(IJSRuntime js, string id, int indx)
		{
			await js.InvokeVoidAsync("ChangeSelectIndexById", id, indx);
		}

		public static async void ChangeSelectIndexByClass(IJSRuntime js, string classId, int indx)
		{
			await js.InvokeVoidAsync("ChangeSelectIndexByClass", classId, indx);
		}

		public static async void DisableDropdown(IJSRuntime js, string id)
		{
			await js.InvokeVoidAsync("DisableById", id);
		}

		public static async void EnableDropdown(IJSRuntime js, string id)
		{
			await js.InvokeVoidAsync("EnableById", id);
		}

	}
}
