using System;
using mejorprecio.Modelo;

namespace mejorprecio.MVC.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public int Score { get; set; }
        public bool Enable { get; set; }

        public UserViewModel(string user, string mail,string role, string name, string lastname, string dni,int score, bool enable)
        {
            this.UserName = user;
            this.Email = mail;
            this.Role = role;
            this.Name = name;
            this.LastName = lastname;
            this.DNI = dni;
            this.Score = score;
            this.Enable = enable;
        }
    }
}
