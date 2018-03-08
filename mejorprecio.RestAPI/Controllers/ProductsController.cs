using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejorprecio.Negocio;
using mejorprecio.Modelo;

namespace mejorprecio.RestAPI.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        // GET Products
        [HttpGet]
        public IActionResult Get()
        {
            return this.Json(APIProduct.GetAllProducts());
        }

        // GET Products/buscar
        [HttpGet("buscar")]
        public IActionResult Get(string prodToSearch)
        {
            var product = APIProduct.SearchProductByName(prodToSearch);
            return this.Json(product);
        }

        // POST 
        [HttpPost]
        public IActionResult Post([FromBody]Price price)
        {
            APIProduct.CreateProduct(price);
            return this.StatusCode(201);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int idPrice)
        {
            APIProduct.QualificationDeletePrice(idPrice);
            return this.StatusCode(201);
        }
        
    }
}
