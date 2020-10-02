using ForgeLibs.Data;
using ForgeLibs.Models.Charts;
using ForgeLibs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgeLibs.Models.Forge
{
	public static class ForgeModelUtils
	{
        public static BarChartData GetBarChart(OmniClassRepository omni, IEnumerable<ForgeElement> ForgeElems, string category, string property, string groupby = "")
        {
            ForgeTableByCategoryVM vm = ForgeTableByCategoryVM.GetModelByCategory(omni, ForgeElems, category);
            Dictionary<string, double> data = new Dictionary<string, double>();
            Dictionary<string, IEnumerable<int>> ids = new Dictionary<string, IEnumerable<int>>();
            System.Drawing.Color color = ChartUtils.RandomColor(System.Drawing.Color.Black, System.Drawing.Color.White, 150);
            IEnumerable<ForgeElement> preFilteredElems;
            bool toCount = property == "Count" ? true : false;
            if (groupby != "Count")
            {
                preFilteredElems = vm.Elements.ToList();
            }
            else
            {
                preFilteredElems = vm.Elements.Where(x => x.Properties.Keys.Contains(property)).ToList();
            }

            if (string.IsNullOrEmpty(groupby))
            {
                if(property == "Count")
				{
                    data[property] = preFilteredElems.Count();
                    ids[property] = preFilteredElems.Select(x => x.ObjectId);
                    
                }
				else
				{
                    var values = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                       .Select(x => x.GetPropertyValue(property, true)).Select(x => Convert.ToDouble(x));
                    double total = values.Sum(x => x);
                    data[property] = total;
                    ids[property] = preFilteredElems.Select(x => x.ObjectId);
                }
                
            }
            else
            {
                if (groupby == "Nombre")
                {
                    if(toCount)
					{
                        data = preFilteredElems
                        .GroupBy(x => x.FullName)
                        .ToDictionary(x => x.Key, y => Convert.ToDouble(y.Count()));
                        ids = preFilteredElems.GroupBy(x => x.FullName).ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                    }
                    else
					{
                        data = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                        .GroupBy(x => x.FullName)
                        .ToDictionary(x => x.Key, y => y.Sum(z => double.Parse(z.GetPropertyValue(property, true))));
                        ids = preFilteredElems.GroupBy(x => x.FullName).ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                    }
                }
                else
                {
                    if(toCount)
					{
                        data = preFilteredElems
                        //.Where(x => x.Properties[groupby] != null)
                        //.Where(x => x.Properties.Keys.Contains(groupby))
                        .GroupBy(x =>
                        {
                            if(!x.Properties.Keys.Contains(groupby))
							{
                                return "-";
							}
                            return x.Properties[groupby].ToString();
                        }
                        )
                        .OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, y => Convert.ToDouble(y.Count()));
                        
                        ids = preFilteredElems
                            //.Where(x => x.Properties[groupby] != null)
                            //.Where(x => x.Properties.Keys.Contains(groupby))
                            //.GroupBy(x => x.Properties[groupby].ToString())
                            .GroupBy(x =>
                                {
                                    if (!x.Properties.Keys.Contains(groupby))
                                    {
                                        return "-";
                                    }
                                    return x.Properties[groupby].ToString();
                                }
                            )
                            .OrderBy(x => x.Key)
                            .ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                    }
					else
					{
                        //Console.WriteLine("SECOND BLOCK");
                        data = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                            .Where(x => x.Properties[groupby] != null).GroupBy(x => x.Properties[groupby].ToString())
                            .OrderBy(x => x.Key)
                            .ToDictionary(x => x.Key, y => y.Sum(z =>
                            {
                                double dbl = double.Parse(z.GetPropertyValue(property, true));
                                //Debug.WriteLine($"{y.Key} - [{z.RevitId}] {property} - {dbl}");
                                return dbl;
                            }));
                        //Console.WriteLine("SECOND BLOCK . . . . ");
                        ids = preFilteredElems.Where(x => x.Properties[groupby] != null).GroupBy(x => x.Properties[groupby].ToString())
                            .OrderBy(x => x.Key)
                            .ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                    }
                }
            }

            ChartDataSet set = new ChartDataSet(category, color, data, ids);
            BarChartData chart = new BarChartData(category);
            chart.Unit = toCount ? "pza" : preFilteredElems.First().GetUnit(property);
            chart.Sets.Add(set);
            chart.Labels.AddRange(data.Keys);
            chart.RandomAllColors();
            return chart;
            //return JsonConvert.SerializeObject(chart.JSON);
        }
    }
}
