using System;
using System.Collections.Generic;
using mejorprecio.Modelo;
using mejorprecio.Persistencia;

namespace mejorprecio.Negocio
{
    public static class APIProduct
    {
        public static string CreateProduct(Price prc)
        {
            Product prod = new Product(prc.Product.Name, prc.Product.CodeBar);
            var location = new Location(prc.Location.Local, prc.Location.Address);
            var price = new Price(prc.IdPrice,prc.Value, prc.Date, prod, location,prc.NameUser);
            Persistance persist = new Persistance();
            return persist.StoreProduct(price);
        }

        // public static void DeleteProduct(int idProduct,int idLocation)
        // {
        //     Persistance persistSvc = new Persistance();
        //     persistSvc.DeleteProduct(idProduct,idLocation);
        // }

        public static List<Price> SearchProductByName(string prodToSearch)
        {
            Persistance persSvc = new Persistance();
            var products = persSvc.SearchProductByName(prodToSearch);
            return products;
        }

        public static List<Price> GetAllProducts()
        {
            Persistance persSvc = new Persistance();
            var products = persSvc.GetAllProducts();
            return products;
        }

        public static void QualificationDeletePrice(int idPrice)
        {
            Persistance persistSvc = new Persistance();
            persistSvc.QualificationDeletePrice(idPrice);
        }

    }
}
