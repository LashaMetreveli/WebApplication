using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc; // for ActionResult
using Newtonsoft.Json; // for JsonConvert

public class WeatherController : Controller
{
    private readonly string apiKey = "d9d898e2d5fc4d07b1482107242004";

    public async Task<ActionResult> Index()
    {
        string[] locations = { "New York", "London", "Tokyo", "Sydney", "Tbilisi", "Chicago" }; // Add more locations as needed

        List<WeatherInfo> weatherInfos = new List<WeatherInfo>();

        // Get current server time
        DateTime serverTime = DateTime.Now;

        using (HttpClient httpClient = new HttpClient())
        {
            foreach (string location in locations)
            {
                string apiUrl = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={location}";

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic weatherData = JsonConvert.DeserializeObject(jsonResponse);

                        var weatherInfo = new WeatherInfo
                        {
                            Location = weatherData.location.name,
                            CurrentTemperature = weatherData.current.temp_c.ToString(),
                            // Add more properties as needed
                        };

                        weatherInfos.Add(weatherInfo);
                    }
                    else
                    {
                        // Handle API error response
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
            }
        }

        // Pass server time and weather information list to the view
        ViewBag.ServerTime = serverTime;
        return View(weatherInfos);
    }
}
