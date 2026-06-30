using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TPI.Aplication.Abstractions;

namespace TPI.Infraestructure.ExternalServices
{
    public class DolarService : IDolarService
    {
        private readonly HttpClient _httpClient;

        public DolarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetOfficialBuyRateAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.bluelytics.com.ar/v2/latest");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<BluelyticsResponse>(json, options);

                if (data?.Oficial == null)
                {
                    throw new InvalidOperationException("La respuesta de la API externa no contiene los datos del dólar oficial.");
                }

                return data.Oficial.ValueBuy;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo obtener la cotización del dólar de la API externa.", ex);
            }
        }
    }

    public class BluelyticsResponse
    {
        [JsonPropertyName("oficial")]
        public DolarInfo Oficial { get; set; } = null!;
    }

    public class DolarInfo
    {
        [JsonPropertyName("value_avg")]
        public decimal ValueAvg { get; set; }

        [JsonPropertyName("value_sell")]
        public decimal ValueSell { get; set; }

        [JsonPropertyName("value_buy")]
        public decimal ValueBuy { get; set; }
    }
}
