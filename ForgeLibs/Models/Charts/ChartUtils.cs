using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ForgeLibs.Models.Charts
{
    public class ChartUtils
    {
        public static Color RandomColor(Color color1, Color color2, int trans = 255)
        {
            int r1 = color1.R;
            int g1 = color1.G;
            int b1 = color1.B;
            int a1 = color1.A;

            int r2 = color2.R;
            int g2 = color2.G;
            int b2 = color2.B;
            int a2 = color2.A;

            int r = r1 > r2 ? r1 : r2;
            int g = g1 > g2 ? g1 : g2;
            int b = b1 > b2 ? b1 : b2;
            int a = a1 > a2 ? a1 : a2;

            int r0 = r1 < r2 ? r1 : r2;
            int g0 = g1 < g2 ? g1 : g2;
            int b0 = b1 < b2 ? b1 : b2;
            int a0 = a1 < a2 ? a1 : a2;

            Thread.Sleep(8);
            int seed = (int)DateTime.Now.Ticks;
            Random random = new Random(seed);

            //trans = (trans == 0) ? random.Next(a0, b) : trans;
            Color color = Color.FromArgb(trans, random.Next(r0, r), random.Next(g0, g), random.Next(b0, b));
            return color;
        }

        public static string ColorRgbaName(Color color)
        {

            return $"rgba({color.R},{color.G},{color.B}, {color.A / 255.00})";
        }
    }
}
