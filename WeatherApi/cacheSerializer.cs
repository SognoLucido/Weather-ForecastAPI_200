using Microsoft.Extensions.Caching.Hybrid;
using Shared.MeteoData.Models.Dto;
using System.Buffers;
using System.Text.Json;

namespace WeatherApi
{
    public  class GeoinfoSerializer : IHybridCacheSerializer<GeoinfoplusProvider>
    {


        public GeoinfoplusProvider Deserialize(ReadOnlySequence<byte> source)
        {
           
            var jsonBytes = source.ToArray();

            var test = JsonSerializer.Deserialize(jsonBytes, GeoinfoResposte.Default.GeoinfoplusProvider);


            return test ;
        }

        public void Serialize(GeoinfoplusProvider value, IBufferWriter<byte> target)
        {
            using var writer = new Utf8JsonWriter(target);

            JsonSerializer.Serialize(writer, value, GeoinfoResposte.Default.GeoinfoplusProvider);


            writer.Flush();
        }
    }


    public class ForecastSerializer : IHybridCacheSerializer<ForecastDto>
    {
        public ForecastDto Deserialize(ReadOnlySequence<byte> source)
        {
            var jsonBytes = source.ToArray();

            var test = JsonSerializer.Deserialize(jsonBytes, ForecastDtoSGmodel.Default.ForecastDto);


            return test;
        }

        public void Serialize(ForecastDto value, IBufferWriter<byte> target)
        {
            using var writer = new Utf8JsonWriter(target);

            JsonSerializer.Serialize(writer, value, ForecastDtoSGmodel.Default.ForecastDto);


            writer.Flush();
        }
    }


}
