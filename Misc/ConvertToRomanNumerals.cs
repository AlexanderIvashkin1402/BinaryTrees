using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
    public static class ConvertToRomanNumerals
    {
        private static List<RomanNumeral> RomanNumerals = new List<RomanNumeral>
        {
            new() { Number = 1, Numeral ="I", Sub = 0},
            new() { Number = 5, Numeral = "V", Sub = -1},
            new() { Number = 10, Numeral = "X", Sub = -2},
            new() { Number = 50, Numeral = "L", Sub = -1},
            new() { Number = 100, Numeral = "C", Sub = -2},
            new() { Number = 500, Numeral = "D", Sub = -1},
            new() { Number = 1000, Numeral = "M", Sub = -2}
        };

        public static string ConvertToRoman(int num)
        {
            var rni = RomanNumerals.Count() - 1;

            var result = new StringBuilder();

            while (num > 0 && rni >= 0)
            {
                var rn = RomanNumerals[rni];

                while (num >= rn.Number)
                {
                    result.Append(rn.Numeral);
                    num -= rn.Number;
                }

                // Symbols are placed from left to right in order of value, starting
                // with the largest. However, in a few specific cases, to avoid four
                // characters being repeated in succession (such as IIII or XXXX),
                // subtractive notation is used where two symbols are used which
                // indicate a smaller number being subtracted from a larger number;
                // for example, 900 is represented as CM, which means 100 subtracted
                // from 1000.

                if (rn.Sub != 0)
                {
                    var rnj = rni + rn.Sub;
                    var rnx = RomanNumerals[rnj];

                    if (num >= (rn.Number - rnx.Number))
                    {
                        result.Append($"{rnx.Numeral}{rn.Numeral}");
                        num -= (rn.Number - rnx.Number);
                    }
                }

                --rni;
            }

            return result.ToString();
        }    

        public static string ConvertToRomanSimple(int num)
        {
            var result = "";

            while (num >= 1000)
            {
                result += "M";
                num -= 1000;
            }

            if (num >= 900)
            {
                result += "CM";
                num -= 900;
            }

            if (num >= 500)
            {
                result += "D";
                num -= 500;
            }

            if (num >= 400)
            {
                result += "DC";
                num -= 400;
            }

            while (num >= 100)
            {
                result += "C";
                num -= 100;
            }

            if (num >= 90)
            {
                result += "XC";
                num -= 90;
            }

            if (num >= 50)
            {
                result += "L";
                num -= 50;
            }

            if (num >= 40)
            {
                result += "XL";
                num -= 40;
            }

            while (num > 10)
            {
                result += "X";
                num -= 10;
            }

            if (num == 9)
            {
                result += "IX";
                num -= 9;
            }

            if (num >= 5)
            {
                result += "V";
                num -= 5;
            }

            if (num == 4)
            {
                result += "IV";
                num -= 4;
            }

            while (num > 0)
            {
                result += "I";
                num--;
            }

            return result;
        }
    }

    public class RomanNumeral
    {
        public int Number { get; set; }
        public string Numeral { get; set; }
        public int Sub { get; set; }
    }
}
