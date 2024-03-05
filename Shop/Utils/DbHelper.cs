using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using WinformTable.Ext;
using System.Reflection;

namespace Shop.Utils
{
    public class DbHelper
    {
        private readonly IConfiguration _configuration;
        
        public DbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public T ConnDb<T>(Func<SqlConnection, T> func)
        {
            var connectionString = _configuration.GetConnectionString("Database");
            using (var sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();
                return func(sqlconn);
            }
        }
        public void ConnDb(Action<SqlConnection> act)
        {
            ConnDb(act.ToFunc());
        }
    }
}
