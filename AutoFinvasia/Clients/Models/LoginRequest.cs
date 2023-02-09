using System.Text.Json.Serialization;

namespace AutoFinvasia.Clients.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("apkversion")]
        public string ApkVersion { get; set; } = string.Empty;

        [JsonPropertyName("uid")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("pwd")]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonPropertyName("factor2")]
        public string Factor2Code { get; set; } = string.Empty;

        [JsonPropertyName("imei")]
        public string IMEI { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("vc")]
        public string VendorCode { get; set; } = string.Empty;

        [JsonPropertyName("appkey")]
        public string AppKeyHash { get; set; } = string.Empty;
    }
}
