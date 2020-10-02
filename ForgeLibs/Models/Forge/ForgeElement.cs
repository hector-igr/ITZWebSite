using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ForgeLibs.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using ForgeLibs.ViewModels;
using System.Threading.Tasks;

namespace ForgeLibs.Models.Forge
{
    public class ForgeElement
    {
        public int RevitId { get; set; }
        /// <summary>
        /// Node Id from the serialize data in the forge platform
        /// </summary>
        //public int NodeId { get; set; }
        public int ObjectId { get; set; }
        //public string ExternalId { get; set; }
        public string Category { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
		public string OmmiCode { get; set; }
		public ForgeElement Parent { get; set; }
        public IEnumerable<ForgeElement> Children { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public IEnumerable<ForgeTableByCategoryVM> Grouping { get; set; }
        public ForgeTableByCategoryVM ElementProperties { get; set; }

        public static string SerializeForgeElements(IEnumerable<ForgeElement> forgeElements)
        {
            StringBuilder sb = new StringBuilder();
            string[] headers = new string[] { "RevitId","ObjectId","Category","OmniCode" , "TypeName","Name","FullName","Parent_RevitId","Properties","Children"};
            sb.AppendLine(string.Join("\t", headers));
            foreach (ForgeElement elem in forgeElements)
            {
                try
                {
                    List<string> properties = new List<string>();
                    foreach (var property in elem.Properties)
                    {
                        properties.Add($"{property.Key}:::{property.Value}");
                    }
                    string dic = string.Join(";", properties);
                    string children_Nodeld = string.Join(";", elem.Children?.Select(x => x.RevitId));
                    object[] values = new object[] { elem.RevitId, elem.ObjectId, elem.Category, elem.OmmiCode, elem.TypeName, elem.Name, elem.FullName, elem.Parent?.RevitId, dic, children_Nodeld };
                    sb.AppendLine(string.Join("\t", values));
                }
                catch (Exception ex)
                {

                }
                
            }
            return sb.ToString();
        }

        public static ForgeElement ParseForgeElement(string line)
        {
            object[] values = line.Split('\t');
            try
            {
                string properties = values[7].ToString();
                string[] propertyValues = properties.Split(';');
                Dictionary<string, object> dic = new Dictionary<string, object>();
                char[] delimiters = new char[] { ':', ':', ':' };
                foreach (var value in propertyValues)
                {
                    var pair = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (pair.Length > 1)
                    {
                        string key = pair[0];
                        string v = pair.Length > 1 ? pair[1] : "";
                        if (!string.IsNullOrWhiteSpace(v)) dic.Add(key, v);
                    }
                }

                ForgeElement forgeElement = new ForgeElement()
                {
                    RevitId = Convert.ToInt32(values[0]),
                    ObjectId = Convert.ToInt16(values[1]),
                    Category = values[2].ToString(),
                    TypeName = values[3].ToString(),
                    Name = values[4].ToString(),
                    FullName = values[5].ToString(),
                    Properties = dic
                };

                return forgeElement;
                
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static IEnumerable<ForgeElement> ParseForgeElements(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    ForgeElement forgeElement = ParseForgeElement(line);
                    yield return forgeElement;
                }
            }
        }

        ///// <summary>
        ///// BLAZOR NOT SUPPORTS THREADING :(
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static List<ForgeElement> ParseForgeElementsAsync(string str)
        //{
        //    List<ForgeElement> forgeElements = new List<ForgeElement>();
        //    var lines = str.Split('\n');
        //    Console.WriteLine($"lines : {lines.Length}");
        //    int numberOfThreads = 3;
        //    int numberOfLines = lines.Length / numberOfThreads;

        //    int startIndex = 0;
        //    List<Thread> threads = new List<Thread>();
        //    for (int i = 0; i < numberOfThreads; i++)
        //    {
        //        IEnumerable<string> lineStack = lines.Skip(i * startIndex).Take(numberOfLines);
        //        Thread t = new Thread(() =>
        //            forgeElements.AddRange(ParseForgeElements(lineStack))
        //        );
        //        threads.Add(t);
        //        t.Start();
        //    }

        //    for (int i = 0; i < threads.Count; i++)
        //    {
        //        threads[i].Join();
        //    }

        //    return forgeElements;
        //}

        public static List<ForgeElement> ParseForgeElements(string str, int maxElements = int.MaxValue)
        {
            List<ForgeElement> forgeElements = new List<ForgeElement>();
            var lines = str.Split('\n').ToList();
            lines.RemoveAt(0);
            Console.WriteLine($"Parsing '{lines.Count}' forge elements");
            if (maxElements != int.MaxValue)
			{
                Console.WriteLine($"Parsing limit to '{maxElements}'");
                lines = lines.Take(maxElements).ToList();
            }

            forgeElements.AddRange(ParseForgeElements(lines));
            //for (int i = 1; i < lines.Length; i++)
            //{
            //    string line = lines[i];
            //    if (!string.IsNullOrEmpty(line))
            //    {
            //        ForgeElement forgeElement = ParseForgeElement(line);
            //        forgeElements.Add(forgeElement);
            //    }
            //    //if(i % maxElements == 0)
            //    //{
            //    //    Console.WriteLine($"\tline : {i}");
            //    //    break;
            //    //}
            //}

            ////Console.WriteLine($"line : {i}");
            ////foreach (ForgeElement el in forgeElements)
            ////{
            ////    el.GetParent(forgeElements);
            ////    el.GetChildren(forgeElements);
            ////}

            return forgeElements;
        }

        public static async Task<List<ForgeElement>> ParseForgeElementsAsync(string str)
        {
            List<ForgeElement> forgeElements = new List<ForgeElement>();
            var lines = str.Split('\n').ToList();
            Console.WriteLine($"lines : {lines.Count}");
            lines.RemoveAt(0);
			for (int l = 0; l < lines.Count; l++)
			{
                string line = lines[l];
                ForgeElement forgeElement = ParseForgeElement(line);
                forgeElements.Add(forgeElement);
                if (l % 100 == 0)
				{
                    Console.WriteLine("Yielding ... " + forgeElements.Count);
                    await Task.Yield();
				}
            }

            lines = lines.ToList();
            forgeElements.AddRange(ParseForgeElements(lines));
            return forgeElements;
        }


        /// <summary>
        /// Gets ForgeElements using json from Autodesk Forge
        /// </summary>
        /// <param name="json"></param>
        /// <param name="omni"></param>
        /// <returns></returns>
        public static IEnumerable<ForgeElement> GetForgeElements(string json, OmniClassRepository omni)
        {
            List<ForgeElement> forgeElements = new List<ForgeElement>();
            JObject desJson = (JObject)JsonConvert.DeserializeObject(json);
            var pr = desJson["data"]["collection"];
            foreach (JToken item in desJson["data"]["collection"])
            {
                string name = "", externalId = "", category = "", omniCode = "";
                int objId = -1;
                Dictionary<string, object> properties = new Dictionary<string, object>();
                Stopwatch watch = Stopwatch.StartNew();
                foreach (JProperty prop in item)
                {
                    string propName = prop.Name;
                    JToken value = prop.Value;
                    switch (propName)
                    {
                        case "name":
                            name = value.ToString();
                            break;
                        case "objectid":
                            objId = Convert.ToInt32(value);
                            break;
                        case "externaId":
                            externalId = value.ToString();
                            break;
                        case "properties":
                            foreach (JProperty grp in value)
                            {
                                foreach (JObject parameters in grp.Children())
                                {
                                    foreach (var param in parameters)
                                    {
                                        string paramName = param.Key;
                                        if (!string.IsNullOrWhiteSpace(param.Value.ToString()))
                                        {
                                            properties[paramName] = param.Value;
                                            if (paramName == OmniClassRepository.OmniClassNumber)
                                            {
                                                omniCode = param.Value.ToString();
                                                var cat = omni.GetCategory(omniCode);
                                                if (!string.IsNullOrEmpty(cat))
                                                {
                                                    category = cat;
                                                    
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                watch.Stop();
                if (name.Contains('[') && name.Contains(']'))
                {
                    var str = name.Split('[', ']');
                    var id = Convert.ToInt32(str[1]);
                    if (name.ToUpper().Contains("FLOOR"))
                    {
                        category = "Losas";
                    }
                    else if (name.ToUpper().Contains("WALL"))
                    {
                        category = "Muros";
                    }
                    else if (name.ToUpper().Contains("RAILING"))
                    {
                        category = "Barandal";
                    }
                    else if (name.ToUpper().Contains("STAIR"))
                    {
                        category = "Escalera";
                    }
                    string typeName = str[0];
                    string fullName = "";
                    if (properties.ContainsKey("Type Name"))
                    {
                        fullName = $"{typeName} - {properties["Type Name"].ToString()}";
                    }

                    ForgeElement newElement = new ForgeElement()
                    {
                        //NodeId = Convert.ToInt32(item.Name),
                        ObjectId = objId,
                        Category = category,
                        Name = name,
                        Properties = properties,
                        RevitId = id,
                        TypeName = typeName,
                        FullName = fullName,
                        OmmiCode = omniCode
                    };
                    forgeElements.Add(newElement);
                }
            }
            foreach (ForgeElement el in forgeElements)
            {
                el.GetParent(forgeElements);
                el.GetChildren(forgeElements);
            }

            return forgeElements;
        }

        public void LoadProperties(OmniClassRepository repo)
        {
            this.ElementProperties = ForgeTableByCategoryVM.GetModelByCategory(repo, new ForgeElement[] { this }, this.Category);
        }

        public void LoadGroups(OmniClassRepository repo)
        {
            List<ForgeTableByCategoryVM> viewModels = new List<ForgeTableByCategoryVM>();
            var groups = this.Children.GroupBy(x => x.Category);
            Console.WriteLine("LoadGroups()" + groups.Count());
            foreach (var grp in groups)
            {
                viewModels.Add(ForgeTableByCategoryVM.GetModelByCategory(repo, grp, grp.Key));
            }
            this.Grouping = viewModels;
        }

        public void GetParent(IEnumerable<ForgeElement> elements)
        {
            if (this.Properties.ContainsKey("AS_ParentId"))
            {
                string str = this.Properties["AS_ParentId"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Replace("_", "");
                    var parentId = Convert.ToInt32(str);
                    this.Parent = elements.FirstOrDefault(x => x.RevitId == parentId);

                }
            }
        }

        public void GetChildren(IEnumerable<ForgeElement> elements)
        {
            List<ForgeElement> children = new List<ForgeElement>();
            if (this.Properties.ContainsKey("AS_ChildrenId"))
            {
                string str = this.Properties["AS_ChildrenId"].ToString();
                var values = str.Split(',');
                foreach (string item in values)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var id = item.Replace("_", "");
                        var childId = Convert.ToInt32(id);
                        ForgeElement el = elements.FirstOrDefault(x => x.RevitId == childId);
                        if(el != null) children.Add(el);
                    }
                }
            }
            this.Children = children;
        }

        public string GetPropertyValue(string property, bool removeUnit = false)
        {
            if (!string.IsNullOrEmpty(property) && this.Properties.ContainsKey(property))
            {
                string value = this.Properties[property].ToString();
                if (removeUnit)
                {
                    Regex regex = new Regex(@"(-|\+|-\s|\+\s)?[0-9.]+");
                    string numb = regex.Match(value).Value;
                    return numb;
                }
                else
                {
                    return value;
                }
            }
            return "";
        }
        public string GetUnit(string property)
        {
            if (!string.IsNullOrEmpty(property) && this.Properties.ContainsKey(property))
            {
                string value = this.Properties[property].ToString();
                string[] values = value.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length > 1)
                {
                    return values[1];
                }
                //Regex regex = new Regex(@"([0-9\.])\w+");
                //var numbers = regex.Match(value).Value;
                //string unit = "";
                //if(numbers.Length > 0)
                //{
                //    unit = value.Replace(numbers, "");
                //}
                //return unit;
            }
            return "";
        }

        public static string CalculateTotal(IEnumerable<ForgeElement> forgeElements, string property)
        {
            double total = 0.00;
            if (forgeElements.Count() == 0 || string.IsNullOrEmpty(property)) return "";
            string unit = "";

            foreach (var item in forgeElements)
            {
                string str = item.GetPropertyValue(property, true);
                if (!string.IsNullOrEmpty(str))
                {
                    total += Convert.ToDouble(str);
                }
            }
            Regex regex = new Regex(@"(-|\+|-\s|\+\s)?[0-9.]+");
            var val = forgeElements.First().GetPropertyValue(property, false);
            if (!string.IsNullOrEmpty(val))
            {
                unit = val.Replace(regex.Match(val).Value, "");
            }
            return $"{total.ToString("#,#.##")} {unit}";
        }
    }
}
