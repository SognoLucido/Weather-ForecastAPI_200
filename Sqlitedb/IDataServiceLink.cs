using OpenWeatherMapLogic.JsonModelApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSqlite;

public interface IDataServiceLink
{
    Task UpdateDbvalues(CustomWeathermodel weathermodel);
    Task<CustomWeathermodel?> CityExist(string Cityinmethod);
}
