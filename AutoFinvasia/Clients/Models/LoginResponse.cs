using System.Text.Json.Serialization;
using System.Text.Json;
using AutoFinvasia.Clients.Models.Converters;

namespace AutoFinvasia.Clients.Models
{
    public partial class LoginResponse
    {
        [JsonPropertyName("request_time")]
        public string RequestTime { get; set; }

        [JsonPropertyName("actid")]
        public string Actid { get; set; }

        [JsonPropertyName("access_type")]
        public string[] AccessType { get; set; }

        [JsonPropertyName("uname")]
        public string Uname { get; set; }

        [JsonPropertyName("prarr")]
        public Prarr[] Prarr { get; set; }

        [JsonPropertyName("stat")]
        public string Stat { get; set; }

        [JsonPropertyName("susertoken")]
        public string Susertoken { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("brnchid")]
        public string Brnchid { get; set; }

        [JsonPropertyName("totp_set")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TotpSet { get; set; }

        [JsonPropertyName("orarr")]
        public string[] Orarr { get; set; }

        [JsonPropertyName("exarr")]
        public Exarr[] Exarr { get; set; }

        [JsonPropertyName("values")]
        public string[] Values { get; set; }

        [JsonPropertyName("mws")]
        public Mws Mws { get; set; }

        [JsonPropertyName("brkname")]
        public string Brkname { get; set; }

        [JsonPropertyName("lastaccesstime")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Lastaccesstime { get; set; }
    }

    public partial class Mws
    {
        [JsonPropertyName("1")]
        public The1[] The1 { get; set; }

        [JsonPropertyName("ashi")]
        public The1[] Ashi { get; set; }
    }

    public partial class The1
    {
        [JsonPropertyName("exch")]
        public Exarr Exch { get; set; }

        [JsonPropertyName("token")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Token { get; set; }

        [JsonPropertyName("tsym")]
        public string Tsym { get; set; }

        [JsonPropertyName("instname")]
        public Instname Instname { get; set; }

        [JsonPropertyName("pp")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Pp { get; set; }

        [JsonPropertyName("ls")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Ls { get; set; }

        [JsonPropertyName("ti")]
        public string Ti { get; set; }
    }

    public partial class Prarr
    {
        [JsonPropertyName("prd")]
        public string Prd { get; set; }

        [JsonPropertyName("s_prdt_ali")]
        public string SPrdtAli { get; set; }

        [JsonPropertyName("exch")]
        public Exarr[] Exch { get; set; }
    }

    public enum Exarr { Bse, Cds, Nfo, Nse };

    public enum Instname { A, Be, Eq };
}
