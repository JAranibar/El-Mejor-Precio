using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejorprecio.Negocio;
using mejorprecio.Modelo;
using mejorprecio.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace mejorprecio.MVC.Controllers
{
    [Route("Products")]

    public class ProductsController : Controller
    {

        //------------------------------------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador, Moderador, User Validado")]
        [HttpGet("add")]
        public IActionResult RegisterProduct()
        {
            return View("RegisterProduct");
        }

        [Authorize(Roles = "Administrador, Moderador, User Validado")]
        // POST Products/add
        [HttpPost("add")]
        public IActionResult Post([FromForm]PriceViewModel price)
        {
            if(String.IsNullOrEmpty(price.Value.ToString())){
                this.ModelState.AddModelError("", "El valor ingresado no puede ser vacio");
            }
            else if(String.IsNullOrWhiteSpace(price.Value.ToString())){
                this.ModelState.AddModelError("", "El valor ingresado es incorrecto");
            }
            
            if(String.IsNullOrEmpty(price.Product.Name)){
                this.ModelState.AddModelError("", "El producto ingresado no puede ser vacio");
            }
            else if(String.IsNullOrWhiteSpace(price.Product.Name)){
                this.ModelState.AddModelError("", "El producto ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(price.Product.CodeBar)){
                this.ModelState.AddModelError("", "El codigo de barras ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(price.Product.CodeBar)){
                this.ModelState.AddModelError("", "El codigo de barras ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(price.Location.Local)){
                this.ModelState.AddModelError("", "El local ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(price.Location.Local.ToString())){
                this.ModelState.AddModelError("", "El local ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(price.Location.Address)){
                this.ModelState.AddModelError("", "La direccion ingresada no puede ser vacia");                
            }
            else if(String.IsNullOrWhiteSpace(price.Location.Address)){
                this.ModelState.AddModelError("", "La direccion ingresada es incorrecta");
            }

            if (ModelState.IsValid)
            {
                var fecha = DateTimeOffset.Now;
                string user = this.User.FindFirstValue(ClaimTypes.Name);
                var prc = new Price(price.Value, fecha, price.Product, price.Location, user);
                APIProduct.CreateProduct(prc);
            }
            return View("RegisterProduct", price);
        }

        //------------------------------------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador, Moderador")]
        // GET Products/gestion
        [HttpGet("gestion")]
        public IActionResult GestionProducts()
        {
            var products = APIProduct.GetAllProducts();
            var prods = new List<PriceViewModel>();
            for (int i = 0; i < products.Count; i++)
            {
                var price = new PriceViewModel(products[i].IdPrice, products[i].Value, products[i].Product, products[i].Location, products[i].NameUser);
                price.Date = products[i].Date;
                prods.Add(price);
            }
            return View("GestionProducts", prods);
        }

        [Authorize(Roles = "Administrador, Moderador")]
        // GET Products/delete
        [HttpGet("delete")]
        public IActionResult DELETE(int idPrice)
        {
            APIProduct.QualificationDeletePrice(idPrice);
            return RedirectToAction("GestionProducts", "Products");
        }

        //------------------------------------------------------------------------------------------------------------------

        // GET Products/buscar
        [HttpGet("buscar")]
        public IActionResult SearchProd(string prodToSearch)
        {
            var products = APIProduct.SearchProductByName(prodToSearch);
            var prods = new List<PriceViewModel>();
            for (int i = 0; i < products.Count; i++)
            {
                prods.Add(new PriceViewModel(products[i].IdPrice, products[i].Value, products[i].Product, products[i].Location));
            }

            if (prods == null)
            {
                return NoContent();
            }
            return View("Product", prods);
        }

        //------------------------------------------------------------------------------------------------------------------

    }
}
