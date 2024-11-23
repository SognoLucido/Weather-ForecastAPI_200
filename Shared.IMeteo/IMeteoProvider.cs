using Shared.MeteoData.Models;

namespace Shared.IMeteo
{
   
    public interface IMeteoProvider
    {
        Task<string> ez();
        Task<List<GeoinfoModel>?> GeoinfoModel(string City,string? key);
    }
}
