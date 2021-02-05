using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace semLinqTask
{
    class Program
    {
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
                        Country = elements[10],
                        State = elements[11],
                        ZipCode = elements[12]
                    };
                    We.Add(we);
                }
            }
            Console.Write("Numbers of distinct cities:");
            Console.WriteLine(We.Select(e => e.City).ToList().Distinct().Count());
            foreach (var item in We.Select(e => e.StartTime).Select(e => e.Year).Distinct().ToArray())
            {
                Console.WriteLine($"Number of date in {item}: {We.Where(e => e.StartTime.Year == item).Count()}");
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
        public string Country { get; set; }
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