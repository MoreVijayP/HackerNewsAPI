using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace HackerNewsAPI
{
    public class StoryItem
    {
        public int id {  get; set; }
        public string by { get; set; }
        public int descendants { get; set; }
        public List<int> kids { get; set; }
        public int score { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime time { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
    //public class UnixTimestampConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(DateTime);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Integer)
    //        {
    //            long unixTimestamp = (long)reader.Value;
    //            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
    //            return dateTime;
    //        }

    //        throw new JsonSerializationException("Unexpected token type. Expected Integer.");
    //    }
    //}
}
