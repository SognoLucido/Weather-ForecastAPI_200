using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSqlite.Model
{
    public class ReturnTableDatacitymodel
    {
        public int id {  get; set; }

        public string data_time { get; set; }
        public string simple_description { get; set; }
        public string detailed_description { get; set; }

        public int temp_C { get; set; }
        public int temp_F { get; set; }
        public int temp_K { get; set; }

    }
}
