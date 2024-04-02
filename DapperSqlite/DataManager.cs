using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DapperSqlite;

public class DataManager : IDataServiceLink
{
    private readonly IConfiguration _config;

    public DataManager(IConfiguration config) 
    {
        _config = config;
    }

    public void hello()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CityExist()  //TODO
    {
        using (var connection = new SQLiteConnection(_config.GetConnectionString("Sqlite")))
        { 
        
        
        }
            return true;
    }


}
