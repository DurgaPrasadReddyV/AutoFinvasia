using AutoFinvasia.Clients.Models;
using AutoFinvasia.Clients.Models.Converters;
using AutoFinvasia.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AutoFinvasia.Clients
{
    public class FinvasiaApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly FinvasiaApiOptions _finvasiaApiOptions;

        public FinvasiaApiClient(HttpClient client, IOptions<FinvasiaApiOptions> finvasiaApiOptions)
        {
            _finvasiaApiOptions = finvasiaApiOptions.Value;
            _client = client;
            _client.BaseAddress = new Uri(_finvasiaApiOptions.BaseAddress);
            _client.Timeout = new TimeSpan(0, 0, _finvasiaApiOptions.Timeout);
            _client.DefaultRequestHeaders.Clear();
            _jsonOptions = new JsonSerializerOptions()
            {
                Converters =
                {
                    new ExarrConverter(),
                    new InstnameConverter(),
                    new DateOnlyConverter(),
                    new IsoDateTimeOffsetConverter(),
                    new ParseStringConverter(),
                    new TimeOnlyConverter()
                }
            };
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest jData)
        {
            HttpContent content = new StringContent("jData=" + JsonSerializer.Serialize(jData, _jsonOptions));
            var httpResponse = await _client.PostAsync("NorenWClientTP/QuickAuth", content);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
        }
    }
}
