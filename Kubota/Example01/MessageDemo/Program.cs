using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;
using System.Xml.Serialization;
using System.IO;

namespace MessageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"../../bonge.txt";
            //Message2 m = new Message2(); //MainState.Initialize, "gonbe");
            ArmyPlacementMessage m = new ArmyPlacementMessage(MainState.Initialize, "gonbe");
            m.territory_army.Add(new ArmyPlacement("PA", "gonbe", 5));
            m.territory_army.Add(new ArmyPlacement("Snyder", "gonbe", 3));
            XmlSerializer ser = new XmlSerializer(typeof(ArmyPlacementMessage));
            Console.Out.WriteLine(ser);
            TextWriter writer = new StreamWriter(filename);
            string s = File.ReadAllText(@"../../bonge.txt");
            Database_Controller Session = new Database_Controller();


            ser.Serialize(writer, m);
            writer.Close();

            {
                ArmyPlacementMessage myObject;
                XmlSerializer mySerializer = new XmlSerializer(typeof(ArmyPlacementMessage));
                FileStream myFileStream = new FileStream(filename, FileMode.Open);
                myObject = (ArmyPlacementMessage)mySerializer.Deserialize(myFileStream);
                myFileStream.Close();
                Console.WriteLine("Message content for " + myObject.playerName + ", " + myObject.state);
                foreach (ArmyPlacement a in myObject.territory_army)
                {
                    Console.WriteLine(a.territory + ", " + a.owner + ", " + a.numArmies);
                }
            }
        }
    }
}
