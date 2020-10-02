using ForgeLibs.Models.Charts;
using ForgeLibs.Models.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeLibs.Models.Forge.Queries
{
	public interface IForgeQuery
	{
		QueryType Query { get; }
		BarChartData GetChartData();
		IEnumerable<ForgeElement> ForgeElements { get; set; }

	}
}
