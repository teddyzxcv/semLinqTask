using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace semLinqTask
{
    class Program
    {
        static string NumbersOfDistinctCities(List<WeatherEvent> we)
        {
            return we.Select(e => e.City)
                        .ToList()
                        .Distinct()
                        .Count()
                        .ToString();
            // Or
            /* var result = from e in we
                          select e.City;
             return result.Distinct().Count().ToString();*/

        }

        static string EachYearDataCount(List<WeatherEvent> we)
        {
            int[] distinctyears = we.Select(e => e.StartTime).Select(e => e.Year).Distinct().ToArray();
            // Or
            /* var result = from e in we
                          select e.StartTime.Year;
             int[] distinctyears = result.Distinct().ToArray();*/
            List<string> lines = new List<string>();
            string output = String.Empty;
            foreach (var item in distinctyears)
            {
                lines.Add($"Numbers of data in {item}: {we.Where(e => e.StartTime.Year == item).Count()}");
            }
            return String.Join('\n', lines);


        }

        static string NumbersOfEventsIn(int year, List<WeatherEvent> we)
        {
            return we.Where(e => e.StartTime.Year == year).Count().ToString();
            // Or
            /*return (from e in we
                    where e.StartTime.Year == year).Count().ToString();*/
        }
        static string NumbersOfDistinctState(List<WeatherEvent> we)
        {
            return we.Select(e => e.State)
                       .ToList()
                       .Distinct()
                       .Count()
                       .ToString();
            // Or
            /*return (from e in we
                    select e.State).Distinct().Count().ToString();*/
        }

        static Dictionary<string, int> DictionaryOfRainingCitiesIn(int year, List<WeatherEvent> we)
        {
            Dictionary<string, int> raininglist = new Dictionary<string, int>();
            var cityweatherlist = we.Where(e => e.StartTime.Year == year).Where(e => e.Type == WeatherEventType.Rain)
                            .Select(e => e.City)
                            .GroupBy(e => e)
                            .Where(e => e.Count() > 1)
                            .Select(y => new { Elements = y.Key, Counter = y.Count() }).ToList();
            // Or
            /* var cityweatherlist = (from t in (from e in we
                                               where e.StartTime.Year == year && e.Type == WeatherEventType.Rain
                                               select e.City).GroupBy(e => e)
                                    where t.Count() > 1
                                    select new { Elements = t.Key, Counter = t.Count() }).ToList();*/
            for (int i = 0; i < cityweatherlist.Count; i++)
            {
                raininglist.Add(cityweatherlist.ToList()[i].Elements, cityweatherlist.ToList()[i].Counter);
            }
            return raininglist;
        }

        static string LongestSnowEventIn(int year, List<WeatherEvent> we)
        {
            WeatherEvent maxspancity = we.Where(e => e.StartTime.Year == year)
                    .Where(e => e.Type == WeatherEventType.Snow)
                    .OrderByDescending(e => e.EndTime - e.StartTime)
                    .ToList()[0];
            //Or
            /*WeatherEvent maxspancity = (from e in we
                                        where e.StartTime.Year == year
                                        orderby e.EndTime - e.StartTime descending).ToList();*/
            string snowtimespanday = (maxspancity.EndTime - maxspancity.StartTime).ToString("%d");
            string snowtimespanprecise = (maxspancity.EndTime - maxspancity.StartTime).ToString(@"hh\:mm\:ss");
            return $"{maxspancity.City} and was continued {snowtimespanday} day(s) {snowtimespanprecise}";
        }
        static void Main(string[] args)
        {
            //Нужно дополнить модель WeatherEvent, создать список этого типа List<>
            //И заполнить его, читая файл с данными построчно через StreamReader
            //Ссылка на файл https://www.kaggle.com/sobhanmoosavi/us-weather-events

            //Написать Linq-запросы, используя синтаксис методов расширений
            //и продублировать его, используя синтаксис запросов
            //(возможно с вкраплениями методов расширений, ибо иногда первого может быть недостаточно)

            //0. Linq - сколько различных городов есть в датасете.
            //1. Сколько записей за каждый из годов имеется в датасете.
            //Потом будут еще запросы
            List<WeatherEvent> We = new List<WeatherEvent>();
            using (StreamReader sr = new StreamReader("WeatherEvents_Jan2016-Dec2020.csv"))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] elements = line.Split(',');
                    WeatherEvent we = new WeatherEvent()
                    {
                        EventId = elements[0],
                        Type = (WeatherEventType)Enum.Parse(typeof(WeatherEventType), elements[1]),
                        Severity = (Severity)Enum.Parse(typeof(Severity), elements[2]),
                        StartTime = DateTime.Parse(elements[3]),
                        EndTime = DateTime.Parse(elements[4]),
                        TimeZone = elements[5],
                        AirportCode = elements[6],
                        LocationLat = elements[7],
                        LocationLng = elements[8],
                        City = elements[9],
                        County = elements[10],
                        State = elements[11],
                        ZipCode = elements[12]
                    };
                    We.Add(we);
                }
            }
            int[] distinctyears = We.Select(e => e.StartTime).Select(e => e.Year).Distinct().ToArray();
            Console.WriteLine($"Numbers of distinct cities: {NumbersOfDistinctCities(We)}");
            Console.WriteLine(EachYearDataCount(We));
            Console.WriteLine($"Numbers of weather event in 2018: {NumbersOfEventsIn(2018, We)} ");
            Console.WriteLine($"Numbers of state in dataset: {NumbersOfDistinctState(We)}");
            Console.WriteLine($"Numbers of city in dataset: {NumbersOfDistinctCities(We)}");
            Console.WriteLine("Top 3 by rain numbers in different city");
            var RainingDic = DictionaryOfRainingCitiesIn(2019, We).OrderByDescending(e => e.Value).ToList();
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"{i + 1} place : {RainingDic[i].Key}");
                Console.WriteLine($", numbers of rain: {RainingDic[i].Value}");
            }
            foreach (var item in distinctyears)
            {
                Console.WriteLine($"Longest snow in {item} was at {LongestSnowEventIn(item, We)}");
            }
        }
    }

    //Дополнить модеь, согласно данным из файла 
    class WeatherEvent
    {
        public string EventId { get; set; }
        public WeatherEventType Type { get; set; }
        public Severity Severity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeZone { get; set; }
        public string AirportCode { get; set; }
        public string LocationLat { get; set; }
        public string LocationLng { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    //Дополнить перечисления
    enum WeatherEventType
    {
        Unknown,
        Snow,
        Fog,
        Rain,
        Cold,
        Storm,
        Precipitation,
        Hail
    }
    enum Severity
    {
        Unknown,
        Light,
        Severe,
        Moderate,
        Heavy,
        UNK,
        Other
    }
}