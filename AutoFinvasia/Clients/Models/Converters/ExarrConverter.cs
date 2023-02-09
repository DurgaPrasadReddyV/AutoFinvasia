using System.Text.Json.Serialization;
using System.Text.Json;

namespace AutoFinvasia.Clients.Models.Converters
{
    internal class ExarrConverter : JsonConverter<Exarr>
    {
        public override bool CanConvert(Type t) => t == typeof(Exarr);

        public override Exarr Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "BSE":
                    return Exarr.Bse;
                case "CDS":
                    return Exarr.Cds;
                case "NFO":
                    return Exarr.Nfo;
                case "NSE":
                    return Exarr.Nse;
            }
            throw new Exception("Cannot unmarshal type Exarr");
        }

        public override void Write(Utf8JsonWriter writer, Exarr value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Exarr.Bse:
                    JsonSerializer.Serialize(writer, "BSE", options);
                    return;
                case Exarr.Cds:
                    JsonSerializer.Serialize(writer, "CDS", options);
                    return;
                case Exarr.Nfo:
                    JsonSerializer.Serialize(writer, "NFO", options);
                    return;
                case Exarr.Nse:
                    JsonSerializer.Serialize(writer, "NSE", options);
                    return;
            }
            throw new Exception("Cannot marshal type Exarr");
        }
    }
}
