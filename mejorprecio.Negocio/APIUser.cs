using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using mejorprecio.Modelo;
using mejorprecio.Persistencia;

namespace mejorprecio.Negocio
{
    public static class APIUser
    {
        public static string UserRegister(User usu)
        {
           if(ValidatorUser(usu.UserName)&&ValidatorPass(usu.Password)&&ValidatorMail(usu.Email)&&CheckExistUser(usu.UserName)==false&&CheckExistMail(usu.Email)==false)
            {
                string passEncrypt = Encrypt.Encriptar(usu.Password);
                User user = new User (usu.UserName,passEncrypt,usu.Email,usu.Role,usu.Name,usu.LastName,usu.DNI,usu.Score,usu.Enable,usu.GUID);
                Persistance persist = new Persistance();
                return persist.StoreUser(user);
            }
            return "algo salio mal!!";
        }

        public static bool UserLogin(string user, string pass)
        {
            string passEncrypt = Encrypt.Encriptar(pass);
            if((CheckExistUserLoginRemoveLogic(user)||CheckExistMail(user))&&CheckPassLogin(user,passEncrypt))
            {            
                return true;
            }
            return false;
        }

        public static List<User> GetAllUsers()
        {
            Persistance persist = new Persistance();
            return persist.GetAllUsers();
        }

        public static List<User> GetUsersNoValidated()
        {
            Persistance persist = new Persistance();
            return persist.GetUsersNoValidated();
        }

        public static string DisableUser(string userMod)
        {
            if(CheckExistUserLoginRemoveLogic(userMod))
            {                    
                Persistance persist = new Persistance();
                return persist.DisableUser(userMod);
            }
            return "algo salio mal!!";
        }

        public static string ChangeRole(string userCambiado, int newRole)
        {
            Persistance persist = new Persistance();
            return persist.ChangeRole(userCambiado, newRole);
        }

        public static string UserUpdateScore(string user, int total, int revomeLogic)
        {
            int newScore = (revomeLogic*100)/total;
            Persistance persist = new Persistance();
            return persist.UserUpdateScore(user, newScore);
        }

        public static string ChangePassword(string user, string newPassword, string newPassRepeat)
        {
            if(ValidatorPass(newPassword))
            {
                if(!newPassword.Equals(newPassRepeat))
                {
                    throw new Exception("Error, verifique que coincidan las password."); 
                }    
            }
            Persistance persist = new Persistance();
            
            string passEncrypt = Encrypt.Encriptar(newPassword);
            return persist.ChangePassword(user, passEncrypt);
        }

        //-----------------------------------------------------------------------------------------------------

        private static bool ValidatorUser(string user)
        {
            string formato = "^[A-Za-z0-9][A-Za-z0-9_]{3,9}$";
            Regex reg = new Regex(formato);
            if(reg.IsMatch(user)) 
            {
                return true;
            }
            else 
            { 
                throw new Exception("Usuario debe contener de 4 a 10 caracteres (Az9_)!!!"); 
            }
        }

        private static bool ValidatorPass(string pass)
        {
            //Debe contener al menos un numero una letra Mayuscula y otra minuscula.
            //^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$
            string formato = "^([a-zA-Z0-9]{8,16})$";
            Regex reg = new Regex(formato);
            if(reg.IsMatch(pass)) 
            {
                return true;
            }
            else 
            { 
                throw new Exception("Contraseña debe contener de 8 a 16 caracteres!!!"); 
            }
        }

        private static bool ValidatorMail(string mail)
        {
            string formato = "^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            Regex reg = new Regex(formato);
            if(reg.IsMatch(mail)) 
            {
                return true;
            }
            else 
            { 
                throw new Exception("Mail invalido!!!"); 
            }
        }
        
        public static bool CheckExistUser(string user)
        {
            Persistance persist = new Persistance();
            if(persist.CheckExistUser(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        private static bool CheckExistMail(string mail)
        {
            Persistance persist = new Persistance();
            if(persist.CheckExistMail(mail))
            {
                throw new Exception("El mail ingresado ya existe");
            }
            else
            {
                return false;
            }
        }

        public static bool CheckExistUserLoginRemoveLogic(string user)
        {
            Persistance persist = new Persistance();
            if(persist.CheckExistUser(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckPassLogin(string user,string pass)
        {
            Persistance persist = new Persistance();
            if(persist.CheckPassLogin(user,pass))
            {
                return true;
            }
            else
            {
                return false;
                //throw new Exception("La contraseña ingresada es incorrecta / fue eliminado del sistema / no termino de registrarse");
            }
        }     

        public static int CountProductByUser(string user, bool qualification)
        {
            Persistance persist = new Persistance();
            return persist.CountProductByUser(user, qualification);
        }

        public static void EndRegistration (string guid)
        {
            Persistance persist = new Persistance();
            persist.EndRegistration(guid);
        }

        public static string FindRoleUser(string userName)
        {
            Persistance persistance = new Persistance();
            int roleInt = persistance.FindRoleUser(userName);
            string retorno = "";
            switch (roleInt)
            {
                case 1:
                    retorno = "User No Validado";
                    break;
                case 2:
                    retorno = "User Validado";
                    break;
                case 3:
                    retorno = "Moderador";
                    break;
                case 4:
                    retorno = "Administrador";
                    break;
            }
            return retorno;
        }
    }
}