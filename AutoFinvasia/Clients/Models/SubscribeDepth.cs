using System.Text.Json.Serialization;

namespace AutoFinvasia.Clients.Models
{
    public class SubscribeDepth
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string k;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string t;
    }
}
