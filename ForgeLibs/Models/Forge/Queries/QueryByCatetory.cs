using ForgeLibs.Data;
using ForgeLibs.Models.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForgeLibs.Models.Forge.Queries
{
	public class QueryByCatetory : IForgeQuery
	{
		public OmniClassRepository Omni { get; set; }
		public string Category { get; set; }
		public string Property { get; set; }
		public string Group { get; set; }

		public QueryType Query { get; } = QueryType.ByCategory;
		public IEnumerable<ForgeElement> ForgeElements { get ; set ; }
		public IEnumerable<string> Categories { get; set; }

		public QueryByCatetory(IEnumerable<ForgeElement> elements, OmniClassRepository omni)
		{
			this.ForgeElements = elements;
			this.Omni = omni;

			this.Categories = elements.Select(x => $"{x.Category}")
				.Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x);
		}
		public BarChartData GetChartData()
		{
			try
			{
				Console.WriteLine($"{Category}-{Property}-{Group}");
				if (!string.IsNullOrEmpty(Category) && !string.IsNullOrEmpty(Property))
				{
					BarChartData data = ForgeModelUtils.GetBarChart(Omni, ForgeElements, Category, Property, Group);
					return data;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			
			return null;
		}

		public string[] GetParametersByCategories()
		{
			string omniNumber = Omni.GetOmniClassNumberByClassFullTitle(Category);
			Console.WriteLine($"omni number : {omniNumber}");
			var parameters = Omni.GetParameters(omniNumber).ToList();
			Console.WriteLine($"parameters : {string.Join(",", parameters)}");
			parameters.Insert(0, "Count");
			return parameters.ToArray();
		}
		public string[] GetGroupParametersByCategories()
		{
			string omniNumber = Omni.GetOmniClassNumberByClassFullTitle(Category);
			var parameters = Omni.GetGroupingParameters(omniNumber).ToList();
			//parameters.Insert(0, "Count");
			return parameters.ToArray();
		}
	}
}
