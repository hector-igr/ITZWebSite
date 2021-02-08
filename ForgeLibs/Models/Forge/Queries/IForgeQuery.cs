using ForgeLibs.Models.Charts;
using System.Collections.Generic;

namespace ForgeLibs.Models.Forge.Queries
{
	public interface IForgeQuery
	{
		QueryType Query { get; }
		BarChartData GetChartData();
		IEnumerable<ForgeElement> ForgeElements { get; set; }

	}
}
