using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejorprecio.Negocio;
using mejorprecio.Modelo;

namespace mejorprecio.RestAPI.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {

   // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            return this.Json(APIUser.GetAllUsers());
        }
        
        // POST /Users/add
        [HttpPost("add")]
        public string Post([FromBody]User usu)
        {
            return APIUser.UserRegister(usu);
        }

        // GET Users/login
        [HttpGet("login")]
        public bool GET(string user,string pass)
        {
            return APIUser.UserLogin(user,pass);
        }


        // DELETE /Users/delete
        [HttpDelete("delete")]
        public string DELETE(string user)
        {
            return APIUser.DisableUser(user);
        }

        // GET Users/changePass
        [HttpGet("changePass")]
        public string GET(string user, string pass, string repitPass)
        {
            return APIUser.ChangePassword(user,pass,repitPass);
        }

        // GET Users/changeRole
        [HttpGet("changeRole")]
        public string GET(string userCambiado, int newRole)
        {
            return APIUser.ChangeRole(userCambiado, newRole);
        }

    }
}
