using System.Text.Json.Serialization;
using System.Text.Json;

namespace AutoFinvasia.Clients.Models.Converters
{
    internal class InstnameConverter : JsonConverter<Instname>
    {
        public override bool CanConvert(Type t) => t == typeof(Instname);

        public override Instname Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "A":
                    return Instname.A;
                case "BE":
                    return Instname.Be;
                case "EQ":
                    return Instname.Eq;
            }
            throw new Exception("Cannot unmarshal type Instname");
        }

        public override void Write(Utf8JsonWriter writer, Instname value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Instname.A:
                    JsonSerializer.Serialize(writer, "A", options);
                    return;
                case Instname.Be:
                    JsonSerializer.Serialize(writer, "BE", options);
                    return;
                case Instname.Eq:
                    JsonSerializer.Serialize(writer, "EQ", options);
                    return;
            }
            throw new Exception("Cannot marshal type Instname");
        }
    }
}
