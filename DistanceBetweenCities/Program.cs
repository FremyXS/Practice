using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DistanceBetweenCities
{
    class Program
    {
        public static string[] Sityes =
        {
            "Москва", "Питер", "Челябинск", "Уфа", "Оренбург", "Новгород", "Екатеринбург", "Тагил", "Коноха"
        };
        static void Main(string[] args)
        {
            Distance.AddNewData();

            do
            {
                var sityOne = Console.ReadLine();
                var sityTwo = Console.ReadLine();

                Console.WriteLine(Distance.GetDistance(sityOne, sityTwo));

            } while (true);
        }
    }
    public class Distance
    {
        public string SityOne { get; }
        public string SityTwo { get; }
        public int CountDistance { get; }
        public Distance(string sityOne, string sityTwo, int countDistance)
        {
            SityOne = sityOne;
            SityTwo = sityTwo;
            CountDistance = countDistance;
        }
        public static List<Distance> Distances { get; private set; } = new List<Distance>();
        public static void AddNewData()
        {
            var countStr = new Random().Next(40);

            for(var i = 0; i < countStr; i++)
                 Distances.Add(SetDistance());

            RecordData();
        }
        private static Distance SetDistance()
        {
            Distance distance = null;
            var t = false;

            do
            {
                distance = new Distance
                (
                    Program.Sityes[new Random().Next(Program.Sityes.Length)],
                    Program.Sityes[new Random().Next(Program.Sityes.Length)],
                    new Random().Next()
                );

                if (distance.SityOne != distance.SityTwo
                && !Distances.Any(el => el.SityOne == distance.SityOne && el.SityTwo == distance.SityTwo)
                && !Distances.Any(el => el.SityOne == distance.SityTwo && el.SityTwo == distance.SityOne))
                    t = true;

            } while (!t);

            return distance;
        }
        private static void RecordData()
            => File.WriteAllLines(@"../../../info.txt",
                Distances.Select(el => el.SityOne + ";" + el.SityTwo + ";" + el.CountDistance.ToString()).ToArray());
        public static string GetDistance(string sityOne, string sityTwo)
            => Distances.FirstOrDefault(el => el.SityOne == sityOne && el.SityTwo == sityTwo).CountDistance.ToString();
    }
}
