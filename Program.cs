using System;
using System.IO;
using System.Linq;

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

            WeatherEvent we = new WeatherEvent()
            {
                EventId = "W-1",
                Type = WeatherEventType.Rain,
                Severity = Severity.Light,
                StartTime = DateTime.Now

            };

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
    }

    enum Severity
    {
        Unknown,
        Light,
        Severe,
    }
}