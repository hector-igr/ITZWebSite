using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ForgeLibs
{
    public class NetStandardsUtils
    {
        public static string RandomWord()
        {
            var w1 = RandomLetter();
            var w2 = RandomLetter();
            var w3 = RandomLetter();
            var w4 = RandomInteger().ToString();

            return $"{w1}{w2}{w3}{w4}";
        }

        public static char RandomLetter()
        {
            Thread.Sleep(3);
            Random rnd = new Random();
            int i = rnd.Next(0, 26);
            char let = (char)('a' + i);
            return let;
        }

        public static int RandomInteger()
        {
            Thread.Sleep(3);
            Random rnd = new Random();
            int i = rnd.Next(0, 1000);
            return i;
        }


    }
}
