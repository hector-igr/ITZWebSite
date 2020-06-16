using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ForgeLibs.Models.Charts
{
    public class BarChart
    {
        public string Title { get; set; }
        public List<string> Labels { get; private set; } = new List<string>();
        public IList<ChartDataSet> Sets { get; private set; } = new List<ChartDataSet>();
        public string Unit { get; set; }

        public object JSON
        {
            get
            {
                dynamic config = new JObject();
                dynamic data = new JObject();
                data.labels = new JArray(this.Labels);
                List<dynamic> sets = new List<dynamic>();
                foreach (var dataset in Sets)
                {
                    dynamic set = new JObject();
                    set.label = dataset.Name;
                    set.data = new JArray(dataset.Data);
                    set.backgroundColor = new JArray(dataset.Background);
                    set.borderColor = new JArray(dataset.BorderColor);
                    set.borderWidth = 1;
                    JArray idsGroup = new JArray();
                    foreach (var ids in dataset.RelatedObjectsIds)
                    {
                        idsGroup.Add(new JArray(ids));
                    }
                    set.ids = idsGroup;
                    sets.Add(set);
                }
                data.datasets = new JArray(sets);
                config.data = data;
                config.units = Unit;
                //config.options = new JObject();
                //config.options.scales = new JObject();

                //dynamic scaleLabel = new JObject();
                //scaleLabel.labelString = Unit;

                //config.options.scales.yAxes = new JArray(scaleLabel);

                return config;
            }
        }

        public BarChart(string title)
        {
            this.Title = title;
        }

        public void RandomAllColors()
        {
            for (int i = 0; i < this.Sets.Count; i++)
            {
                ChartDataSet data = this.Sets[i];
                for (int l = 0; l < data.Labels.Count; l++)
                {
                    var color = ChartUtils.RandomColor(Color.White, Color.Black, 120);
                    data.Background[l] = ChartUtils.ColorRgbaName(color);
                    data.BorderColor[l] = ChartUtils.ColorRgbaName(Color.FromArgb(255, color.R, color.G, color.B));
                }
            }
        }

        public void SetLabels()
        {
            for (int ii = 0; ii < this.Sets.Count; ii++)
            {
                ChartDataSet data = this.Sets[ii];
                string background = data.Background.FirstOrDefault(x => !string.IsNullOrEmpty(x));
                string borderColor = data.BorderColor.FirstOrDefault(x => !string.IsNullOrEmpty(x));
                for (int i = 0; i < this.Labels.Count; i++)
                {
                    if (data.Labels.Count > i)
                    {
                        if (this.Labels[i] != data.Labels[i])
                        {
                            InsertBlank(ref data, i, background, borderColor);
                        }
                    }
                    else
                    {
                        InsertBlank(ref data, i, background, borderColor);
                    }
                }
            }
        }

        public void InsertBlank(ref ChartDataSet dataSet, int indx, string backgroundColor, string borderColor)
        {
            dataSet.Background.Insert(indx, backgroundColor);
            dataSet.BorderColor.Insert(indx, borderColor);
            dataSet.Labels.Insert(indx, "");
            dataSet.Data.Insert(indx, 0.00);
        }
    }
}
