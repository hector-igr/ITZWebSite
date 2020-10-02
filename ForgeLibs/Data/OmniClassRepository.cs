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
        //public const string OmniClassAbsolutePath = @"D:\Desarrollos\ASPNETCORE\ForgeViewer\ForgeViewerClient\wwwroot\models_metadata/OmniClass.txt";
        public const string OmniClassTitle = "OmniClass Title";
        public const string OmniClassNumber = "OmniClass Number";
        public const string OminiClassCategoryId = "RevitCategoryId";
        public const string OmniClassParameters = "OmniClassParameters";
        public const string OmniClassGroupingParameters = "OmniClassGroupingParameters";

        private DataTable _Table;
        private HttpClient _client;

        public OmniClassRepository()
        {

        }

        public OmniClassRepository(HttpClient client)
        {
            this._client = client;
        }

        public async Task<bool> LoadFileAsync()
        {
            
            var str = await _client.GetStringAsync(OmniClassPath);
            _Table = StringToDataTable(str, '\t');
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

            //if (this._client != null)
            //{
            //    Console.WriteLine("BY CLIENT");
               
            //}
            //else
            //{
            //    Console.WriteLine("BY IO");
            //    using (StreamReader sr = new StreamReader(OmniClassAbsolutePath))
            //    {
            //        _Table = StringToDataTable(sr.ReadToEnd(), '\t');
            //    }
            //}
            
            //Console.WriteLine("columns count : " + _Table.Columns.Count);
            //Console.WriteLine("rows count : " + _Table.Rows.Count);
            //Console.WriteLine(this.GetGroup("MX.01"));
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
                //try
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
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
                            table.Rows.Add(row);
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return table;
        }

        /// <summary>
        /// Gets first Omni Title on the herarchy
        /// </summary>
        /// <param name="omniNumber"></param>
        /// <returns></returns>
        public string GetGroup(string omniNumber)
        {
            for (int i = 0; i < _Table.Rows.Count; i++)
            {
                string number = (string)_Table.Rows[i][OmniClassNumber];
                if (omniNumber == number)
                {
                    if (omniNumber.Count(x => x == '.') == 1)
                    {
                        return (string)_Table.Rows[i][OmniClassTitle];
                    }
                    else
                    {
                        for (int ii = i; ii < i; ii--)
                        {
                            string title = (string)_Table.Rows[i][OmniClassTitle];
                            if (title.Count(x => x == '.') <= 0)
                            {
                                return (string)_Table.Rows[ii][OmniClassTitle];
                            }
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Get Parameters for quantity take off
        /// </summary>
        /// <param name="omniNumber"></param>
        /// <returns></returns>
        public string[] GetParameters(string omniNumber)
        {
            for (int r = 0; r < _Table.Rows.Count; r++)
            {
                string title = (string)_Table.Rows[r][OmniClassNumber];
                if (title == omniNumber)
                {
                    string str = (string)_Table.Rows[r][OmniClassParameters];
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
            for (int r = 0; r < _Table.Rows.Count; r++)
            {
                string title = (string)_Table.Rows[r][OmniClassNumber];
                if (title == omniNumber)
                {
                    string str = (string)_Table.Rows[r][OmniClassGroupingParameters];
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
        public string GetOmniClassNumber(string omniClassTitle)
        {
            for (int r = 0; r < _Table.Rows.Count; r++)
            {
                string title = (string)_Table.Rows[r][OmniClassTitle];
                if (title == omniClassTitle)
                {
                    string code = (string)_Table.Rows[r][OmniClassNumber];
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
            for (int i = 0; i < _Table.Rows.Count; i++)
            {
                string number = (string)_Table.Rows[i][OmniClassNumber];
                if (omniNumber == number) return (string)_Table.Rows[i][OmniClassTitle];
            }
            return "";
        }
    }
}
