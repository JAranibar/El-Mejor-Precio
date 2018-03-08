using System;
using mejorprecio.Modelo;

namespace mejorprecio.MVC.Models
{
    public class PriceViewModel
    {

        public int IdPrice { get; set; }
        public Decimal Value { get; set; }
        public DateTimeOffset Date { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public string NameUser { get; set; }

        public PriceViewModel()
        {

        }

        public PriceViewModel(int id, Decimal value, Product product, Location location)
        {
            IdPrice = id;
            Value = value;
            Product = product;
            Location = location;
        }


        public PriceViewModel(int id, Decimal value, Product product, Location location, string user)
        {
            IdPrice = id;
            Value = value;
            Product = product;
            Location = location;
            NameUser = user;
        }

    }
}