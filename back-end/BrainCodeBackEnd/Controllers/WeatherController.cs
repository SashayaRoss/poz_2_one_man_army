﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BrainCodeBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainCodeBackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Weather")]
    public class WeatherController : Controller
    {
        public static readonly HttpClient httpClient = new HttpClient();
        public WeatherController()
        {
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "https://api.opencagedata.com/geocode/v1");

        }

        [HttpGet("{filter}")]
        public async Task<string> GetWeather(string filter)
        {
            var filters = filter.Split(",");

            float dl;
            float.TryParse(filters[0], NumberStyles.Any, CultureInfo.InvariantCulture, out dl);
            float sr;
            float.TryParse(filters[1], NumberStyles.Any, CultureInfo.InvariantCulture, out sr);

            var response =
                SendGetRequest(
                    $"http://api.openweathermap.org/data/2.5/forecast?lat={sr}&lon={dl}&appid=4aefe593e29e3bb1224da04a5fef6b30");

            string responseBody = await response.Content.ReadAsStringAsync();

            var weatherModel = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherRoot>(responseBody);

            //http://api.openweathermap.org/data/2.5/forecast?lat=35&lon=139&appid=4aefe593e29e3bb1224da04a5fef6b30
            return responseBody;
        }

        private HttpResponseMessage SendGetRequest(string url)
        {
            try
            {
                return httpClient.GetAsync(url).Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}