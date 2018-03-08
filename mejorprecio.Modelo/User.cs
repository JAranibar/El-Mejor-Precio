using System;


namespace mejorprecio.Modelo
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public int Score { get; set; }
        public bool Enable { get; set; }
        public string GUID { get; set; }


        //public BadImageFormatException Image { get; set; }

        public User(string user, string pass, string mail,int role, string name, string lastname, string dni,int score, bool enable, string guid)
        {
            this.UserName = user;
            this.Password = pass;
            this.Email = mail;
            this.Role = role;
            this.Name = name;
            this.LastName = lastname;
            this.DNI = dni;
            this.Score = score;
            this.Enable = enable;
            this.GUID = guid;
        }
    }
}
