using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DuckWalk
{
     public class SiteRating
    {

        string API_KEY = string.Empty;
        string PROVIDER_API = "https://api.abuseipdb.com/api/v2/check";

        public SiteRating()
        {
            API_KEY = Environment.GetEnvironmentVariable("ABUSEIPDB_API_KEY") ?? null;
        }

        private string GetIP(string site)
        {
            try
            {
                var baseUrl = ExtractDomainNameFromURL(site);
                return Dns.GetHostEntry(baseUrl).AddressList[0].ToString();

            }
            catch (System.Net.Sockets.SocketException err)
            {
                MessageBox.Show($"Error: {err.Message}", "Search");
            }

            return null;
        }

        private string ExtractDomainNameFromURL(string Url)
        {
            if (!Url.Contains("://"))
                Url = "http://" + Url;

            return new Uri(Url).Host;
        }

        public string RateSite(string site)
        {
            string ipAddress = GetIP(site);
            string ratingImage = String.Empty;
            int abuseConfidenceScore = 0;

            if (ipAddress != null)
                abuseConfidenceScore = QueryRatingAPI(ipAddress);
            
            ratingImage = GetRatingImage(abuseConfidenceScore);

            return ratingImage;

        }

        private int QueryRatingAPI(string ipAddress)
        {
            int rating = 0;
            
            using (var httpClient = new HttpClient())
            {

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"{PROVIDER_API}?ipAddress={ipAddress}&maxAgeInDays=90"))
                {
                    request.Headers.TryAddWithoutValidation("Key", API_KEY);
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    var response = Task.Run(() => httpClient.SendAsync(request)).Result;
                    response.EnsureSuccessStatusCode();

                    var json = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                    Data result = JsonConvert.DeserializeObject<Data>(json);

                    rating = result.abuseConfidenceScore;
                }
            }

            return rating;

        }

        public string GetRatingImage(int abuseConfidenceScore)
        {
            var imagePath = abuseConfidenceScore switch
            {
                var n when n >= 80 => "/Images/icons8-e-cute-48.png",
                var n when n >= 60 => "/Images/icons8-d-cute-48.png",
                var n when n >= 40 => "/Images/icons8-c-cute-48.png",
                var n when n >= 20 => "/Images/icons8-b-cute-48.png",
                var n when n >= 0 => "/Images/icons8-a-cute-48.png",
                _ => "/Images/icons8-question-mark-48.png"
            };

            return imagePath;

        }
    }
}
