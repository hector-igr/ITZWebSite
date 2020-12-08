using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace ForgeLibs.Data
{
    public class OmniClassRepository
    {
        public const string OmniClassPath = @"models_metadata/OmniClass.txt";
        public const string OmniClassAbsolutePath = @"D:\Desarrollos\ASPNETCORE\ForgeViewer\ForgeViewerClient\wwwroot\models_metadata/OmniClass.txt";

        public const string OmniClassTitle = "OmniClass Title";
        public const string OmniClassNumber = "OmniClass Number";
        public const string OminiClassCategoryId = "RevitCategoryId";
        public const string OmniClassParameters = "OmniClassParameters";
        public const string OmniClassGroupingParameters = "OmniClassGroupingParameters";
        public const string OmniClassFullTitle = "OmniClass Full Title";
        public const string OmniHierarchy = "Desgloce";
        public const string OmniDiscipline = "Discipline";

		private DataTable _table;
        

        private HttpClient _client;

        public OmniClassRepository()
        {

        }

        public OmniClassRepository(HttpClient client)
        {
            this._client = client;
        }

        public bool LoadFromPath()
		{
            var str = File.ReadAllText(OmniClassAbsolutePath);
            _table = StringToDataTable(str, '\t');
            if (!string.IsNullOrEmpty(str))
            {
                Console.WriteLine("Load Omni File Success");
                return true;
            }
            else
            {
                Console.WriteLine("Load Omni File Failed");
                return false;
            }
        }

        public async Task<bool> LoadFileAsync()
        {
            var str = await _client.GetStringAsync(OmniClassPath);
            _table = StringToDataTable(str, '\t');
            if(!string.IsNullOrEmpty(str))
			{
                Console.WriteLine("Load Omni File Success");
                return true;
			}
			else
			{
                Console.WriteLine("Load Omni File Failed");
                return false;
			}
        }

        public static DataTable StringToDataTable(string stream, char separator)
        {
            
            DataTable table = new DataTable();
            using (StringReader sr = new StringReader(stream))
            {
                string str = sr.ReadLine();
                while (string.IsNullOrEmpty(str))
                {
                    str = sr.ReadLine();
                }
                var headers = str.Split(separator);
                foreach (string header in headers)
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        string h = header;
                        DataColumn col = new DataColumn(h, typeof(object));
                        table.Columns.Add(col);
                    }
                }
                //Add Extra columns for full category name, discipline
                DataColumn col1 = new DataColumn(OmniClassFullTitle, typeof(string));
                table.Columns.Add(col1);
                DataColumn col2 = new DataColumn(OmniDiscipline, typeof(string));
                table.Columns.Add(col2);
                List<string> catHierarchy = new List<string>();

                //Append data from stream
                int lineCount = 0;
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        if(lineCount++ == 0) continue;

                        var values = line.Split(separator);
                        DataRow row = table.NewRow();
                        int col = 0;
                        int length = table.Columns.Count < values.Length ? table.Columns.Count : values.Length;
                        bool hasValue = false;
                        for (int i = 0; i < length; i++)
                        {
                            row[i] = values[i];
                            col++;
                            if (!string.IsNullOrEmpty(values[i])) hasValue = true;
                        }
                        if (hasValue)
						{
                            //add fully category name using the hierarchy three of the omninclass number
                            string omNumber = (string)row[OmniClassNumber];
                            int level = Convert.ToInt32(row[OmniHierarchy]);
                            if (catHierarchy.Count >= level)
                            {
                                catHierarchy = catHierarchy.Take(level - 2).ToList();
                            }

                            catHierarchy.Add((string)row[OmniClassTitle]);
                            row[OmniDiscipline] = catHierarchy[0];
                            
                            row[OmniClassFullTitle] = string.Join("-", catHierarchy).Replace(catHierarchy[0] + "-", "");


                            if((string)row[OmniDiscipline] == (string)row[OmniClassFullTitle])
							{
                                row[OmniClassFullTitle] = $"{row[OmniClassFullTitle]} (no classified)";
                            }

                            //row[OmniClassFullTitle] = string.Join("-", catHierarchy);


                            table.Rows.Add(row);
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return table;
        }

		///// <summary>
		///// Gets first Omni Title on the herarchy
		///// </summary>
		///// <param name="omniNumber"></param>
		///// <returns></returns>
		//public string GetGroupBy(string omniNumber)
		//{
		//	for (int i = 0; i < _table.Rows.Count; i++)
		//	{
		//		string number = (string)_table.Rows[i][OmniClassNumber];
		//		if (omniNumber == number)
		//		{
		//			if (omniNumber.Count(x => x == '.') == 1)
		//			{
		//				return (string)_table.Rows[i][OmniClassTitle];
		//			}
		//			else
		//			{
		//				for (int ii = i; ii < i; ii--)
		//				{
		//					string title = (string)_table.Rows[i][OmniClassTitle];
		//					if (title.Count(x => x == '.') <= 0)
		//					{
		//						return (string)_table.Rows[ii][OmniClassTitle];
		//					}
		//				}
		//			}
		//		}
		//	}
		//	return "";
		//}

		///// <summary>
		///// Gets first Omni Title on the herarchy
		///// </summary>
		///// <param name="omniNumber"></param>
		///// <returns></returns>
		//public string GetGroupByFullTitle(string omniNumber)
  //      {
  //          for (int i = 0; i < _table.Rows.Count; i++)
  //          {
  //              string number = (string)_table.Rows[i][OmniClassNumber];
  //              if (omniNumber == number)
  //              {
  //                  if (omniNumber.Count(x => x == '.') == 1)
  //                  {
  //                      return (string)_table.Rows[i][OmniClassFullTitle];
  //                  }
  //                  else
  //                  {
  //                      for (int ii = i; ii < i; ii--)
  //                      {
  //                          string title = (string)_table.Rows[i][OmniClassFullTitle];
  //                          if (title.Count(x => x == '.') <= 0)
  //                          {
  //                              return (string)_table.Rows[ii][OmniClassFullTitle];
  //                          }
  //                      }
  //                  }
  //              }
  //          }
  //          return "";
  //      }

        /// <summary>
        /// Get Parameters for quantity take off
        /// </summary>
        /// <param name="omniNumber"></param>
        /// <returns></returns>
        public string[] GetParameters(string omniNumber)
        {
            for (int r = 0; r < _table.Rows.Count; r++)
            {
                string title = (string)_table.Rows[r][OmniClassNumber];
                if (title == omniNumber)
                {
                    string str = (string)_table.Rows[r][OmniClassParameters];
                    if (!string.IsNullOrEmpty(str))
					{
                        var paramNames = str.Split(',').OrderBy(x => x).ToList();
                        paramNames.RemoveAll(x => string.IsNullOrEmpty(x));
                        return paramNames.ToArray();
                    }
                }
            }
            return new string[0];
        }

        /// <summary>
        /// Get Parameters for grouping
        /// </summary>
        /// <param name="omniNumber"></param>
        /// <returns></returns>
        public string[] GetGroupingParameters(string omniNumber)
        {
            for (int r = 0; r < _table.Rows.Count; r++)
            {
                string title = (string)_table.Rows[r][OmniClassNumber];
                if (title == omniNumber)
                {
                    string str = (string)_table.Rows[r][OmniClassGroupingParameters];
                    if(!string.IsNullOrEmpty(str))
					{
                        var paramNames = str.Split(',').OrderBy(x => x).ToList();
                        paramNames.RemoveAll(x => string.IsNullOrEmpty(x));
                        paramNames.Add("Nombre");
                        return paramNames.ToArray();
                    }
                }
            }
            return new string[] { "Nombre" };
        }

        /// <summary>
        /// Get Title Id of OmniClassTable
        /// </summary>
        /// <param name="omniClassTitle"></param>
        /// <returns></returns>
        public string GetOmniClassNumberByClassTitle(string omniClassTitle)
        {
            for (int r = 0; r < _table.Rows.Count; r++)
            {
                string title = (string)_table.Rows[r][OmniClassTitle];
                if (title == omniClassTitle)
                {
                    string code = (string)_table.Rows[r][OmniClassNumber];
                    return code;
                }
            }
            return "";
        }

        /// <summary>
        /// Get Title Id of OmniClassTable
        /// </summary>
        /// <param name="omniClassTitle"></param>
        /// <returns></returns>
        public string GetOmniClassNumberByClassFullTitle(string omniClassFullTitle)
        {
            for (int r = 0; r < _table.Rows.Count; r++)
            {
                string title = (string)_table.Rows[r][OmniClassFullTitle];
                if (title == omniClassFullTitle)
                {
                    string code = (string)_table.Rows[r][OmniClassNumber];
                    return code;
                }
            }
            return "";
        }

        /// <summary>
        /// Get Category Name from omniclass table
        /// </summary>
        /// <param name="omniNumber"></param>
        /// <returns></returns>
        public string GetCategory(string omniNumber)
        {
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                string number = (string)_table.Rows[i][OmniClassNumber];
                if (omniNumber == number) return (string)_table.Rows[i][OmniClassFullTitle];
            }
            return "";
        }
    }
}
