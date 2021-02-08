using System.Collections.Generic;
using System.Drawing;

namespace ForgeLibs.Models.Charts
{
	public class ChartDataSet
    {
        public string Name { get; set; }
        public List<IEnumerable<int>> RelatedObjectsIds { get; set; } = new List<IEnumerable<int>>();
        public List<double> Data { get; set; } = new List<double>();
        public List<string> Background { get; } = new List<string>();
        public List<string> BorderColor { get; } = new List<string>();
        public List<string> Labels { get; } = new List<string>();

        public ChartDataSet(string datasetName, Color color, Dictionary<string, double> data)
        {
            this.Name = datasetName;
            foreach (var d in data)
            {
                this.Labels.Add(d.Key);
                this.Data.Add(d.Value);
                this.Background.Add(ChartUtils.ColorRgbaName(color));
                this.BorderColor.Add(ChartUtils.ColorRgbaName(Color.FromArgb(255, color.R, color.G, color.B)));
            }
        }
        public ChartDataSet(string datasetName, Color color, Dictionary<string, double> data, Dictionary<string, IEnumerable<int>> ids)
        {
            this.Name = datasetName;
            foreach (var d in data)
            {
                this.Labels.Add(d.Key);
                var v = ids.ContainsKey(d.Key) ? ids[d.Key] : new int[0];
                this.RelatedObjectsIds.Add(v);
                this.Data.Add(d.Value);
                this.Background.Add(ChartUtils.ColorRgbaName(color));
                this.BorderColor.Add(ChartUtils.ColorRgbaName(Color.FromArgb(255, color.R, color.G, color.B)));
            }
        }

        public ChartDataSet(string datasetName, Dictionary<string, Color> colorMapping, Dictionary<string, double> data)
        {
            this.Name = datasetName;
            foreach (var d in data)
            {
                this.Labels.Add(d.Key);
                this.Data.Add(d.Value);
                Color color = colorMapping[d.Key];
                this.Background.Add(ChartUtils.ColorRgbaName(color));
                this.BorderColor.Add(ChartUtils.ColorRgbaName(Color.FromArgb(255, color.R, color.G, color.B)));
            }
        }
        public ChartDataSet(string datasetName, Dictionary<string, Color> colorMapping, Dictionary<string, double> data, int overrideTransparency)
        {
            this.Name = datasetName;
            foreach (var d in data)
            {
                this.Labels.Add(d.Key);
                this.Data.Add(d.Value);
                Color color = colorMapping[d.Key];
                color = Color.FromArgb(overrideTransparency, color);
                this.Background.Add(ChartUtils.ColorRgbaName(color));
                this.BorderColor.Add(ChartUtils.ColorRgbaName(Color.FromArgb(255, color.R, color.G, color.B)));
            }
        }
    }
}
