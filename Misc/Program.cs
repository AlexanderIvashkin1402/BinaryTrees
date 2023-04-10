using CSharp_Examples;

namespace Misc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var test = new TestEnumerable();
            //test.Test();

            Console.WriteLine("Numbers to Roman Numbers");
            Console.WriteLine($"1973 -> {ConvertToRomanNumerals.ConvertToRoman(1973)}");
            Console.WriteLine($"2023 -> {ConvertToRomanNumerals.ConvertToRoman(2023)}");
        }
    }
}