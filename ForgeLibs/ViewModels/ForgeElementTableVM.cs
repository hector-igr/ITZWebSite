using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForgeLibs.Models.Forge;
using ForgeLibs.Data;
using ForgeLibs.Models.Charts;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ForgeLibs.ViewModels
{
    public class ForgeElementTableVM
    {
        public string Guid { get; private set; } = System.Guid.NewGuid().ToString();
        public string Category { get; set; }
        public ForgeElement[] Elements { get; set; }
        public string[] Properties { get; set; }
        public string[] GroupingProperties { get; set; }

        /// <summary>
        /// Get View Model from a omniclass, forge elements array and a category
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="forgeElements"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static ForgeElementTableVM GetModelByCategory(OmniClassRepository repository, 
            IEnumerable<ForgeElement> forgeElements, string category)
        {
            List<ForgeElement> filteredElements = new List<ForgeElement>();
            foreach (ForgeElement el in forgeElements)
            {
                if (el.Category == category) filteredElements.Add(el);
            }
            string omniNumber = repository.GetOmniClassNumber(category);
            string[] parameters = repository.GetParameters(omniNumber);
            string[] groupingProperties = repository.GetGroupingParameters(omniNumber);
            ForgeElementTableVM vm = new ForgeElementTableVM
            {
                Category = category,
                Elements = filteredElements.ToArray(),
                Properties = parameters,
                GroupingProperties = groupingProperties
            };
            return vm;
        }

        public IEnumerable<IGrouping<string, ForgeElement>> GroupElementsByProperties(string property)
        {
            return this.Elements.GroupBy(x => x.Properties[property].ToString());
        }
        public IEnumerable<IGrouping<string, ForgeElement>> GroupElementsByFullName()
        {
            return this.Elements.GroupBy(x => x.FullName);
        }

        public static string GetBarChart(OmniClassRepository omni, IEnumerable<ForgeElement> ForgeElems, string category, 
            string property, string groupby = "")
        {
            ForgeElementTableVM vm = ForgeElementTableVM.GetModelByCategory(omni, ForgeElems, category);
            Dictionary<string, double> data = new Dictionary<string, double>();
            Dictionary<string, IEnumerable<int>> ids = new Dictionary<string, IEnumerable<int>>();
            System.Drawing.Color color = ChartUtils.RandomColor(System.Drawing.Color.Black, System.Drawing.Color.White, 150);
            IEnumerable<ForgeElement> preFilteredElems;
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
                var values = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                    .Select(x => x.GetPropertyValue(property, true)).Select(x => Convert.ToDouble(x));
                double total = values.Sum(x => x);
                data[property] = total;
                ids[property] = preFilteredElems.Select(x => x.ObjectId);
            }
            else
            {
                if (groupby == "Nombre")
                {
                    data = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                        .GroupBy(x => x.FullName)
                        .ToDictionary(x => x.Key, y => y.Sum(z => double.Parse(z.GetPropertyValue(property, true))));
                    ids = preFilteredElems.GroupBy(x => x.FullName)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                }
                else
                {
                    data = preFilteredElems.Where(x => !string.IsNullOrEmpty(x.GetPropertyValue(property, true)))
                        .Where(x => x.Properties[groupby] != null).GroupBy(x => x.Properties[groupby].ToString())
                        .OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, y => y.Sum(z =>
                        {
                            double dbl = double.Parse(z.GetPropertyValue(property, true));
                            //Debug.WriteLine($"{y.Key} - [{z.RevitId}] {property} - {dbl}");
                            return dbl;
                        }));
                    //Debug.WriteLine($"__________");
                    ids = preFilteredElems.Where(x => x.Properties[groupby] != null).GroupBy(x => x.Properties[groupby].ToString())
                        .OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.ObjectId));
                }
            }

            ChartDataSet set = new ChartDataSet(category, color, data, ids);
            BarChart chart = new BarChart(category);
            chart.Unit = preFilteredElems.First().GetUnit(property);
            chart.Sets.Add(set);
            chart.Labels.AddRange(data.Keys);
            chart.RandomAllColors();
            return JsonConvert.SerializeObject(chart.JSON);
        }
    }
}
