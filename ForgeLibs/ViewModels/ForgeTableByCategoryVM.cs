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
    public interface IForgeElementTableVM
	{
        string Guid { get; }
        IEnumerable<ForgeElement> Elements { get; set; }
    }
    public class ForgeTableByCategoryVM : IForgeElementTableVM
    {
        public string Guid { get; private set; } = System.Guid.NewGuid().ToString();
        public IEnumerable<ForgeElement> Elements { get; set; }
        public string Category { get; set; }
        public string[] Properties { get; set; }
        public string[] GroupingProperties { get; set; }
		

		/// <summary>
		/// Get View Model from a omniclass, forge elements array and a category
		/// </summary>
		/// <param name="repository"></param>
		/// <param name="forgeElements"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		public static ForgeTableByCategoryVM GetModelByCategory(OmniClassRepository repository, 
            IEnumerable<ForgeElement> forgeElements, string category)
        {
            List<ForgeElement> filteredElements = new List<ForgeElement>();
            foreach (ForgeElement el in forgeElements)
            {
                if (el.Category == category) filteredElements.Add(el);
            }
            string omniNumber = repository.GetOmniClassNumberByClassFullTitle(category);
            string[] parameters = repository.GetParameters(omniNumber);
            string[] groupingProperties = repository.GetGroupingParameters(omniNumber);
            ForgeTableByCategoryVM vm = new ForgeTableByCategoryVM
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
            //Console.WriteLine(string.Join(",", this.Elements.GroupBy(x => x.FullName).Select(x=> x.Key)));
            return this.Elements.GroupBy(x => x.FullName);
        }
    }
}
