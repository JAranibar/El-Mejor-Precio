using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejorprecio.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using mejorprecio.Negocio;

namespace mejorprecio.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var products = APIProduct.GetAllProducts();
            var prods = new List<PriceViewModel>();
            for (int i = 0; i < products.Count; i++)
            {            
                var price = new PriceViewModel(products[i].IdPrice,products[i].Value, products[i].Product, products[i].Location);
                price.Date = products[i].Date;
                prods.Add(price);
            }
            return View("Index", prods);
        }

        [HttpPost]
        public IActionResult Index(string prodToSearch)
        {
            var pcontroller = new ProductsController();
            return pcontroller.SearchProd(prodToSearch);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Usuario loggeado: " + this.User.Identity.Name;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
