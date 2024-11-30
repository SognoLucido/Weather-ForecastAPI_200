using Microsoft.Extensions.Caching.Hybrid;
using Shared.MeteoData.Models.Dto;
using System.Buffers;
using System.Text.Json;

namespace WeatherApi
{
    public class JsonCachedeserializer : IHybridCacheSerializer<GeoinfoplusProvider>
    {


        public GeoinfoplusProvider Deserialize(ReadOnlySequence<byte> source)
        {
           
            var jsonBytes = source.ToArray();

            var test = JsonSerializer.Deserialize(jsonBytes, GeoinfoResposte.Default.GeoinfoplusProvider);
            //throw new NotImplementedException();

            return test ;
        }

        public void Serialize(GeoinfoplusProvider value, IBufferWriter<byte> target)
        {
            using var writer = new Utf8JsonWriter(target);

            JsonSerializer.Serialize(writer, value, GeoinfoResposte.Default.GeoinfoplusProvider);
            //throw new NotImplementedException();

            writer.Flush();
        }
    }
}
