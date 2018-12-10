using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geocoding;
using Geocoding.Google;
using System.Linq;
using OpenWeatherMap.Standard;

namespace Botwyn.Handlers
{
    public static class WeatherHandler
    {
        public static async Task<string> GetWeather(string address)
        {
            IGeocoder geocoder = new GoogleGeocoder();
            IEnumerable<Address> addresses = await geocoder.GeocodeAsync(address);
            var georesult = addresses.First();

            Forecast forecast = new Forecast();
            var result = forecast.GetWeatherDataByCityNameAsync("a3222c3f65f66410ab8e7604f6d6a8d2", address, "gb", WeatherUnits.imperial);
            var test = Convert.ToString(result.Result.main.temp);

            return test;
        }
    }
}
