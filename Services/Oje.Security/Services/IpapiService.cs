using Newtonsoft.Json;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System.Net;

namespace Oje.Security.Services
{
    public class IpapiService : IIpapiService
    {
        public async Task<IpDetectionServiceVM> Validate(string ip)
        {
            IpDetectionServiceVM result = null;
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("http://api.ipapi.com");
                HttpResponseMessage response = await client.GetAsync("/"+ ip + "?access_key=7443268245a2a43c4b97309fbc6cf43c&format=1");
                response.EnsureSuccessStatusCode();
                string tempResult = await response.Content.ReadAsStringAsync();
                try { result = JsonConvert.DeserializeObject<IpDetectionServiceVM>(tempResult); } catch { };
            }

            return result;
        }
    }
}
