using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace MapStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            bool makemap = false;
            if (makemap)
            {
                Bitmap bitmap = new Bitmap(@"..\..\SimpleRiskMap.png");
                Console.WriteLine(bitmap.Width + " by " + bitmap.Height);
                Map map = new Map(bitmap);
                string[][] tnames = new string[3][];

                tnames[0] = new string[] {"A", "B"};
                tnames[1] = new string[] {"C", "D", "E"};
                tnames[2] = new string[] {"F", "G", "H"};
                int[] x = {182,73, 507, 719, 626, 270, 447, 386};
                int[] y = {83, 241, 79, 76, 236, 379, 390, 545};
                string[] cnames = { "Blue", "Red", "Green" };
                int count = 0;
                for (int i = 0; i < cnames.Length; ++i )
                {
                    string c = cnames[i];
                    Continent continent = new Continent(c, 1, i);
                    map.addContinent(continent);
                    for (int j = 0; j < tnames[i].Length; ++j )
                    {
                        string t = tnames[i][j];
                        Territory territory = new Territory(x[count], y[count], t, count, continent);
                        //continent.addTerritory(territory);
                        map.addTerritory(territory, continent);
                        count++;
                    }
                }

                string[,] relations = new string[,] { { "A", "B" }, { "A", "C" }, { "B", "F" }, { "C", "F" }, { "C", "E" }, { "C", "D" }, { "D", "E" }, { "E", "G" }, { "F", "G" }, { "F", "H" }, { "G", "H" } };
                for (int i = 0; i < relations.GetLength(0); ++i)
                {
                    string a = relations[i,0];
                    string b = relations[i,1];
                    map.getTerritory(a).addNeighbor(map.getTerritory(b));
                    map.getTerritory(b).addNeighbor(map.getTerritory(a));
                }

                map.saveMap(@"..\..\SimpleRisk.map");
            }
            else
            {
                Map map = Map.loadMap(@"..\..\SimpleRisk.map");
                List<Territory> territories = map.getAllTerritories();
                foreach (Territory t in territories)
                {
                    Console.WriteLine(t.getName() + "@(" + t.returnX() + ", " + t.returnY() + ")");
                }
            }
        }
    }
}
