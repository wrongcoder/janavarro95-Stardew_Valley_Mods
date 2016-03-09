using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Event;
using Storm.StardewValley.Wrapper;
using Storm;
using Microsoft.Xna.Framework;
namespace MoreRain
{
    [Mod]
    public class MoreRain : DiskResource
    {
        public const int weather_sunny = 0;
        public const int weather_rain = 1;
      //public const int weather_debris = 2;
        public const int weather_lightning = 3;
       // public const int weather_festival = 4;
       // public const int weather_snow = 5;
       // public const int weather_wedding = 6;

        public Config ModConfig { get; private set; }
        public bool RainUpdate = false;


        [Subscribe]
        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public void InitializeCallback(InitializeEvent @event)
        {
            var configLocation = Path.Combine(PathOnDisk, "Config.json");
            if (!File.Exists(configLocation))
            {
                Console.WriteLine("The config file for MoreRain was not found, guess I'll create it...");
                ModConfig = new Config();
                ModConfig.RainChance = 30;
                ModConfig.ThunderChance = 10;
                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig)));
                Console.WriteLine("The config file for MoreRain has been loaded.\n\t RainChance: {0}, ThunderChance: {1}",
                    ModConfig.RainChance, ModConfig.ThunderChance);
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Console.WriteLine("The config file for MoreRain has been loaded.\n\tRainChance: {0}, ThunderChance: {1}",
                   ModConfig.RainChance, ModConfig.ThunderChance);
            }

            Console.WriteLine("MoreRain Initialization Completed");
        }

        [Subscribe]
        public void UpdateCallback(Storm.StardewValley.Event.NewDayEvent @event)
        {
            
          //  Console.WriteLine(ModConfig.RainChance);
          // Console.WriteLine(ModConfig.ThunderChance);
           
                Random random = new Random();
                int randomNumber = random.Next(0, 100); //sets ran variable to some num between 0 and 100
                Random thunder_random = new Random();
                int thunder_randomNumber = random.Next(0, 100);

            if (randomNumber <= ModConfig.RainChance) //if the random variable is less than or equal to the chance for rain.
            {
           
                @event.Root.WeatherForTomorrow=weather_rain;
                     Console.WriteLine("It will rain tomorrow.");
                    }
                    else
                    {
                @event.Root.WeatherForTomorrow = weather_sunny;
                Console.WriteLine("It will not rain tomorrow.");
                    }

                    if (@event.Root.WeatherForTomorrow==weather_rain)
                    {
                        if (randomNumber <= ModConfig.ThunderChance) //if the random variable is less than or equal to the chance for rain.
                        {
                    @event.Root.WeatherForTomorrow=weather_lightning; //sets rainy weather tomorrow
                            Console.WriteLine("It will be stormy tomorrow.");
                        }
                        else
                        {
                            Console.WriteLine("There will be no lightning tomorrow.");
                        }
                    }
            Console.WriteLine("RainMod has updated.");
        }
                
        }
    

        public class Config
        {
            public int RainChance { get; set; }
            public int ThunderChance { get; set; }
        }
    }