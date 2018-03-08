using System;
using System.Text;

namespace mejorprecio.Modelo
{
    public class Price
    {
        public int IdPrice { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset Date { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public string NameUser { get; set; }

        public Price(decimal value, DateTimeOffset date, Product product, Location location, string nameUser)
        {
            Value = value;
            Date = date;
            Product = product;
            Location = location;
            NameUser = nameUser;
        }
        public Price(int idprice, decimal value, DateTimeOffset date, Product product, Location location, string nameUser)
        {
            IdPrice = idprice;
            Value = value;
            Date = date;
            Product = product;
            Location = location;
            NameUser = nameUser;
        }

    }
}
