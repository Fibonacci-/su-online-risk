using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileIOExample
{
    enum Gender { Female, Male }
    [Serializable]
    class Person
    {
        public string name;
        public Gender gender;
        public int id;
        public Person(string name, Gender gender, int id)
        {
            this.name = name;
            this.gender = gender;
            this.id = id;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filename = Path.GetFullPath(@"..\..\Program.cs");
            Console.Out.WriteLine("Absolute path of this program is " + filename);
            string[] lines = File.ReadAllLines(filename);
            foreach (string s in lines)
            {
                Console.Out.WriteLine(s);
            }

            StreamReader reader = new StreamReader(filename);

            string tempname = Path.GetTempFileName();
            Console.Out.WriteLine(tempname);
            StreamWriter writer = new StreamWriter(tempname);
            while (reader.EndOfStream == false)
            {
                string s = reader.ReadLine();
                writer.WriteLine(s);
            }
            reader.Close();
            writer.Close();

            Person tom = new Person("Tom Sawyer", Gender.Male, 12345);
            Person jane = new Person("Jane Smith", Gender.Female, 34567);
            List<Person> persons = new List<Person>();
            persons.Add(tom);
            persons.Add(jane);

            //we will save this list to a file using serialization
            Stream stream = File.Create("persons.data");
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, persons); //save the persons list in a file
            stream.Close();

            Stream stream2 = File.OpenRead("persons.data");
            List<Person> person2 = (List<Person>) formatter.Deserialize(stream2);
            foreach (Person person in person2)
            {
                Console.Out.WriteLine(person.name + " " + person.gender + " " + person.id);
            }
            stream2.Close();
        }
    }
}
