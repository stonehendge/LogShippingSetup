using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogShippingSetup
{
    class Config
    {
        

    }

    public class LSDB
    {
        public string database_name;
        public string primary_server;
        public string secondary_server;
        public string src_share_path;
        public string dest_share_path;
        //public string mdf_path;
        //public string ldf_path;
    }
}
