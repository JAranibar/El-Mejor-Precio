using System;
using System.Configuration;

namespace mejorprecio.Persistencia
{
    public class DBConnection
    {
        public string ConnString { get; set; }

        public DBConnection()
        {
            // ConnString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            ConnString = ConfigurationManager.ConnectionStrings["mejorprecio7"].ConnectionString;
        }
    }
}