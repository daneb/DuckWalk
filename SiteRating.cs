using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DuckWalk
{
     public class SiteRating
    {

        string API_KEY = string.Empty;
        string PROVIDER_API = "https://api.abuseipdb.com/api/v2/check";

        public SiteRating()
        {
            API_KEY = Environment.GetEnvironmentVariable("ABUSEIPDB_API_KEY") ?? "f644c5a2ade5a89400165121695a9f239e678d22a4bd12337091263e1d1487cbf20fad9169207bc0";
        }

        private string GetIP(string site)
        {
            var baseUrl = ExtractDomainNameFromURL(site);
            return Dns.GetHostEntry(baseUrl).AddressList[0].ToString();
        }

        private string ExtractDomainNameFromURL(string Url)
        {
            if (!Url.Contains("://"))
                Url = "http://" + Url;

            return new Uri(Url).Host;
        }

        public string RateSite(string site)
        {
            string rating = null;
            string ipAddress = GetIP(site);

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

                    rating = result.abuseConfidenceScore.ToString();
                }
            }

            return rating;

        }
    }
}
