using System.ComponentModel;
using System.Runtime.Intrinsics;

namespace UnionFind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var map = new Dictionary<string, int>
            {
                { "E", 0 },
                { "F", 1 },
                { "I", 2 },
                { "D", 3 },
                { "C", 4 },
                { "A", 5 },
                { "J", 6 },
                { "L", 7 },
                { "G", 8 },
                { "K", 9 },
                { "B", 10 },
                { "H", 11 }
            };

            var uf = new UnionFind(map.Count);

            uf.Unify(map["C"], map["K"]);
            uf.Unify(map["F"], map["E"]);
            uf.Unify(map["A"], map["J"]);
            uf.Unify(map["A"], map["B"]);
            uf.Unify(map["C"], map["D"]);
            uf.Unify(map["D"], map["I"]);
            uf.Unify(map["L"], map["F"]);
            uf.Unify(map["C"], map["A"]);
            uf.Unify(map["A"], map["B"]);
            uf.Unify(map["H"], map["G"]);
            uf.Unify(map["H"], map["F"]);
            uf.Unify(map["H"], map["B"]);

            var sides = new List<Side>
            {
                new Side { V1 = "I", V2= "J", Lenght = 0 },
                new Side { V1 = "A", V2= "E", Lenght = 1 },
                new Side { V1 = "C", V2= "I", Lenght = 1 },
                new Side { V1 = "E", V2= "F", Lenght = 1 },
                new Side { V1 = "G", V2= "H", Lenght = 1 },
                new Side { V1 = "B", V2= "D", Lenght = 2 },
                new Side { V1 = "C", V2= "J", Lenght = 2 },
                new Side { V1 = "D", V2= "E", Lenght = 2 },
                new Side { V1 = "D", V2= "H", Lenght = 2 },
                new Side { V1 = "A", V2= "D", Lenght = 4 },
                new Side { V1 = "B", V2= "C", Lenght = 4 },
                new Side { V1 = "C", V2= "H", Lenght = 4 },
                new Side { V1 = "G", V2= "I", Lenght = 4 },
                new Side { V1 = "A", V2= "B", Lenght = 5 },
                new Side { V1 = "D", V2= "F", Lenght = 5 },
                new Side { V1 = "H", V2= "I", Lenght = 6 },
                new Side { V1 = "F", V2= "G", Lenght = 7 },
                new Side { V1 = "D", V2= "G", Lenght = 11 }
            };

            var map1 = new Dictionary<string, int>();
            var vertices = new List<string>();

            foreach(var side in sides)
            {
                if (!vertices.Contains(side.V1)) vertices.Add(side.V1);
                if (!vertices.Contains(side.V2)) vertices.Add(side.V2);
            }

            for(int i = 0; i < vertices.Count; i++)
            {
                map1[vertices[i]] = i;
            }

            var uf1 = new UnionFind(vertices.Count);

            var totalLenght = 0;

            foreach (var side in sides)
            {
                if (uf1.Unify(map1[side.V1], map1[side.V2], false))
                {
                    totalLenght += side.Lenght;
                    Console.WriteLine($"Side {side.V1}-{side.V2} Length: {side.Lenght}");
                }
            }

            Console.WriteLine($"Total Lenght: {totalLenght}");
        }
    }

    public class Side
    {
        public string V1 { get; set; }
        public string V2 { get; set; }
        public int Lenght { get; set; }
    }
}