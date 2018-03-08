using System;
using System.Text;

namespace mejorprecio.Modelo
{
    public class Location
    {

        public string Local { get; set; }

        public string Address { get; set; }

        public Location()
        {

        }
        public Location(string local, string address)
        {
            Local = local;
            Address = address;
        }

    }
}
