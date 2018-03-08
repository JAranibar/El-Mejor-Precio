using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejorprecio.Negocio;
using mejorprecio.Modelo;
using mejorprecio.MVC.Models;
using mejorprecio.ServiceExtern;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace mejorprecio.MVC
{
    [Route("Users")]

    public class UsersController : Controller
    {

        //------------------------------------------------------------------------------------------------------------------

        [HttpGet("add")]
        public IActionResult GET()
        {
            return View("RegisterUser");
        }

        // POST /Users/add
        [HttpPost("add")]
        public IActionResult Agregar (UserRegisterModel usu)
        {
            if(String.IsNullOrEmpty(usu.UserName)){
                this.ModelState.AddModelError("", "El usuario ingresado no puede ser vacio");
            }
            else if(String.IsNullOrWhiteSpace(usu.UserName)){
                this.ModelState.AddModelError("", "El usuario ingresado es incorrecto");
            }
            
            if(String.IsNullOrEmpty(usu.Password)){
                this.ModelState.AddModelError("", "La password ingresada no puede ser vacia");
            }
            else if(String.IsNullOrWhiteSpace(usu.Password)){
                this.ModelState.AddModelError("", "La password ingresada es incorrecta");
            }

            if(String.IsNullOrEmpty(usu.Email)){
                this.ModelState.AddModelError("", "El email ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(usu.Email)){
                this.ModelState.AddModelError("", "El email ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(usu.Name)){
                this.ModelState.AddModelError("", "El nombre ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(usu.Name)){
                this.ModelState.AddModelError("", "El nombre ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(usu.LastName)){
                this.ModelState.AddModelError("", "El apellido ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(usu.LastName)){
                this.ModelState.AddModelError("", "El apellido ingresado es incorrecto");
            }

            if(String.IsNullOrEmpty(usu.DNI)){
                this.ModelState.AddModelError("", "El DNI ingresado no puede ser vacio");                
            }
            else if(String.IsNullOrWhiteSpace(usu.DNI)){
                this.ModelState.AddModelError("", "El DNI ingresado es incorrecto");
            }

            if(ModelState.IsValid){
                Guid guid = Guid.NewGuid();
                string myGuid = guid.ToString();
                User usuario = new User(usu.UserName,usu.Password,usu.Email,0,usu.Name,usu.LastName,usu.DNI,100,true,myGuid);
                APIUser.UserRegister(usuario);
                try
                {
                    Correos Cr = new Correos();
                    MailMessage mnsj = Cr.ArmarCorreo(usu.Email, myGuid);
                    Cr.MandarCorreo(mnsj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return RedirectToAction("InitialRegistration", "Users");
            }
            else{
                return View("RegisterUser", usu);
            }
        }

        // GET /Users/endRegistration
        [HttpGet("endRegistration")]
        public IActionResult ENDREGISTRATION(string guid)
        {
            APIUser.EndRegistration(guid);
            return RedirectToAction("SuccessfulRegistration", "Users");
        }

        [HttpGet("SuccessfulRegistration")]
        public IActionResult SuccessfulRegistration()
        {
            return View("SuccessfulRegistration");
        }

        [HttpGet("InitialRegistration")]
        public IActionResult InitialRegistration()
        {
            return View("InitialRegistration");
        }

        //------------------------------------------------------------------------------------------------------------------

        [Authorize(Roles="Administrador")]
        [HttpGet("gestion")]
        public IActionResult GestionUsers()
        {
            var users = APIUser.GetAllUsers();
            var Usuarios = new List<UserViewModel>();
            for (int i = 0; i < users.Count; i++)
            {
                string Role="";
                if(users[i].Role==1){Role="User No Validado";}
                if(users[i].Role==2){Role="User Validado";}
                if(users[i].Role==3){Role="Moderador";}
                if(users[i].Role==4){Role="Administrador";}
                Usuarios.Add(new UserViewModel(users[i].UserName, users[i].Email, Role, users[i].Name, users[i].LastName, users[i].DNI, users[i].Score, users[i].Enable));
            }
            return View("GestionUsers", Usuarios);
        }

        [Authorize(Roles="Administrador")]
        //GET /Users/delete
        [HttpGet("delete")]
        public IActionResult DELETE(string user)
        {
            string userCambiante = this.User.FindFirstValue(ClaimTypes.Name);
            if (!userCambiante.Equals(user))
            {            
                APIUser.DisableUser(user);
            }
            return RedirectToAction("GestionUsers", "Users");
        }

        [Authorize(Roles="Administrador")]
        // GET Users/changeRole
        [HttpGet("changeRole")]
        public IActionResult ChangeRole(string userCambiado, int newRole)
        {        
            string userCambiante = this.User.FindFirstValue(ClaimTypes.Name);
            if (!userCambiado.Equals(userCambiante))
            {
                APIUser.ChangeRole(userCambiado, newRole);
            }
            return RedirectToAction("GestionUsers", "Users");
        }
        
        //------------------------------------------------------------------------------------------------------------------

        [Authorize(Roles="Moderador")]
        [HttpGet("validator")]
        public IActionResult ValidatorUsers()
        {
            var users = APIUser.GetUsersNoValidated();
            var Usuarios = new List<UserViewModel>();
            for (int i = 0; i < users.Count; i++)
            {
                string Role="User No validado";
                Usuarios.Add(new UserViewModel(users[i].UserName, users[i].Email, Role, users[i].Name, users[i].LastName, users[i].DNI, users[i].Score, users[i].Enable));
            }
            return View("ValidatorUsers", Usuarios);
        }


        [Authorize(Roles="Moderador")]
        // GET Users/changeRole
        [HttpGet("validateUser")]
        public IActionResult ChangeRole(string userCambiado)
        {
            APIUser.ChangeRole(userCambiado, 2);
            return RedirectToAction("ValidatorUsers", "Users");
        }

        //------------------------------------------------------------------------------------------------------------------

        [Authorize(Roles="Administrador, Moderador, User Validado, User No Validado")]
        // GET Users/changePass
        [HttpPost("changePass")]
        public IActionResult Post(string newPass, string repitPass)
        {
            string user = this.User.FindFirstValue(ClaimTypes.Name);
            APIUser.ChangePassword(user, newPass, repitPass);
            return RedirectToAction("SuccessfulChangePass", "Users");
        }

        [Authorize(Roles="Administrador, Moderador, User Validado, User No Validado")]
        // GET Users/changePass
        [HttpGet("changePass")]
        public IActionResult ChangePass()
        {
            return View("ChangePass");
        }

        [HttpGet("SuccessfulChangePass")]
        public IActionResult SuccessfulChangePass()
        {
            return View("SuccessfulChangePass");
        }

        //------------------------------------------------------------------------------------------------------------------

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }
            
            return View(new LoginViewModel());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if(!APIUser.CheckExistUserLoginRemoveLogic(model.Username))
            {
                this.ModelState.AddModelError("","El usuario ya existe");
            }
         /*   if(!APIUser.CheckPassLogin(model.Username,model.Password))
            {
                this.ModelState.AddModelError("","Contraseña incorrecta");
            }
            */
            if (ModelState.IsValid) 
            {
                if(APIUser.UserLogin(model.Username,model.Password))
                {
                    var usernameClaim = new Claim(ClaimTypes.Name, model.Username);
                    var roleClaim = new Claim(ClaimTypes.Role, APIUser.FindRoleUser(model.Username));
                    var identity = new ClaimsIdentity(new [] {usernameClaim, roleClaim}, "cookie");
                    var principal = new ClaimsPrincipal(identity);

                    await this.HttpContext.SignInAsync(principal);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout() {
            await this.HttpContext.SignOutAsync();
            if(Request.Cookies["Username"] != null)
           {
           }
            return RedirectToAction("Index","Home");
        }

        //------------------------------------------------------------------------------------------------------------------

    }
}
