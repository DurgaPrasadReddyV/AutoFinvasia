using System.Text.Json.Serialization;

namespace AutoFinvasia.Clients.Models
{
    public class ConnectMessage
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string uid;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string actid;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string susertoken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string t;
    }
}
