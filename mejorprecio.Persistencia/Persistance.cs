using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using mejorprecio.Modelo;

namespace mejorprecio.Persistencia
{
    public class Persistance
    {
        public string StoreProduct(Price price)
        {
            DBConnection dbConnection = new DBConnection();

            using (var connection = new SqlConnection(dbConnection.ConnString))
            {

                int validalocation = 0;
                int validaproduct = 0;
                connection.Open();

                //--------------------------MODIFICAR PRODUCTO------------------------------------------//


                var consulta = "select 1 from Products where Products.codeBar=@codebar";
                using (SqlCommand consultaproduct = new SqlCommand(consulta, connection))
                {
                    consultaproduct.Parameters.AddWithValue("@codebar", price.Product.CodeBar);
                     if(consultaproduct.ExecuteScalar() == null)
                    {
                        validaproduct= 0;
                    }
                    else{
                    var validaproduct_string = consultaproduct.ExecuteScalar().ToString().Trim();
                    validaproduct = Int32.Parse(validaproduct_string);
                    }
                }

                var consulta_locations = "select 1 from Locations where Locations.address=@Address";
                using (SqlCommand consultalocations = new SqlCommand(consulta_locations, connection))
                {
                    consultalocations.Parameters.AddWithValue("@Address", price.Location.Address);
                    if(consultalocations.ExecuteScalar() == null)
                    {
                        validalocation= 0;
                    }
                    else{
                    var validalocation_string = consultalocations.ExecuteScalar().ToString().Trim();
                    validalocation = Int32.Parse(validalocation_string);
                    }
                }

                if (validalocation == 1 && validaproduct == 1)
                {
                    this.ModifProduct(price);
                }


                if (validaproduct != 1)
                {
                    string insert_products = "insert into products (name,codebar,deleted) values (@name,@codebar,@deleted)";
                    using (SqlCommand command = new SqlCommand(insert_products, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@name", price.Product.Name);
                        command.Parameters.AddWithValue("@codebar", price.Product.CodeBar);
                        command.Parameters.AddWithValue("@deleted", 0);
                        command.ExecuteNonQuery();
                    }
                }
                if (validalocation != 1)
                {
                    string insert_location = "insert into Locations (local,address,deleted) values (@local,@address,@deleted)";
                    using (var command2 = new SqlCommand(insert_location, connection))
                    {
                        command2.CommandType = CommandType.Text;
                        command2.Parameters.AddWithValue("@local", price.Location.Local);
                        command2.Parameters.AddWithValue("@address", price.Location.Address);
                        command2.Parameters.AddWithValue("@deleted", 0);
                        command2.ExecuteNonQuery();
                    }
                }

                int location_value;
                int product_value;

                string id_location = "select Locations.idLocation from Locations where Locations.address=@naddress and deleted= 0";
                using (SqlCommand selectidloc = new SqlCommand(id_location, connection))
                {
                    selectidloc.Parameters.AddWithValue("@naddress", price.Location.Address);
                    var location_value_string = selectidloc.ExecuteScalar().ToString().Trim();
                    location_value = Int32.Parse(location_value_string);
                }

                string id_product = "select Products.idProduct from Products where Products.codeBar=@codebar and deleted= 0";
                using (SqlCommand selectidprod = new SqlCommand(id_product, connection))
                {
                    selectidprod.Parameters.AddWithValue("@codebar", price.Product.CodeBar);
                    var product_value_string = selectidprod.ExecuteScalar().ToString().Trim();
                    product_value = Int32.Parse(product_value_string);
                }

                string SqlString = "Insert Into Prices (value,value_date,idProduct,idLocation,deleted,Qualification,NameUser) Values (@value, @date_price, @id_product,@id_location,@deleted,@qualification,@NameUser)";
                using (SqlCommand command3 = new SqlCommand(SqlString, connection))
                {
                    command3.CommandType = CommandType.Text;
                    command3.Parameters.AddWithValue("@value", price.Value);
                    command3.Parameters.AddWithValue("@date_price", price.Date);
                    command3.Parameters.AddWithValue("@id_product", product_value);
                    command3.Parameters.AddWithValue("@id_location", location_value);
                    command3.Parameters.AddWithValue("@deleted", 0);
                    command3.Parameters.AddWithValue("@qualification", 0);
                    command3.Parameters.AddWithValue("@NameUser", price.NameUser);
                    command3.ExecuteNonQuery();
                }
                connection.Close();
            }

            return ("Se guardo correctamente el producto");
        }

        public void ModifProduct(Price price)
        {
            int idProduct = 0;
            int idLocation= 0;
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                string id_product2 = "select Products.idProduct from Products where codeBar =@codebar";
                using (SqlCommand selectidprod = new SqlCommand(id_product2, conn))
                {
                    selectidprod.Parameters.AddWithValue("@codebar", price.Product.CodeBar);
                    var product_value_string = selectidprod.ExecuteScalar().ToString().Trim();
                    idProduct = Int32.Parse(product_value_string);
                }
                string id_location = "select Locations.idLocation from Locations where Locations.address=@naddress";
                using (SqlCommand selectidloc = new SqlCommand(id_location, conn))
                {
                    selectidloc.Parameters.AddWithValue("@naddress", price.Location.Address);
                    var location_value_string = selectidloc.ExecuteScalar().ToString().Trim();
                    idLocation = Int32.Parse(location_value_string);
                }
                this.DeleteProduct(idProduct,idLocation);
            }
        }
        public void DeleteProduct(int idProduct, int idLocation)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var deleted2 = new SqlCommand("UPDATE Prices SET deleted = 1 WHERE idProduct = @idProduct and idLocation= @idLocation", conn);
                deleted2.Parameters.AddWithValue("@idProduct", idProduct);
                deleted2.Parameters.AddWithValue("@idLocation",idLocation);
                deleted2.ExecuteNonQuery();
            }
        }

        public void QualificationDeletePrice(int idPrice)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var deleted = new SqlCommand("UPDATE Prices SET deleted = 1 WHERE idPrice = @idPrice", conn);
                deleted.Parameters.AddWithValue("@idPrice", idPrice);
                deleted.ExecuteNonQuery();

                var qualification = new SqlCommand("UPDATE Prices SET Qualification = 1 WHERE idPrice = @idPrice", conn);
                qualification.Parameters.AddWithValue("@idPrice", idPrice);
                qualification.ExecuteNonQuery();

                //Actualizacion de Calificacion automatica
                var selectName = new SqlCommand("select Prices.NameUser from Prices where Prices.idPrice= @idPrice", conn);
                selectName.Parameters.AddWithValue("@idPrice", idPrice);
                var prices = selectName.ExecuteReader();
                while (prices.Read())
                {
                    string usuario = prices.GetString(0);
                    int Total = CountProductByUser(usuario,true)+CountProductByUser(usuario,false);
                    int Good = CountProductByUser(usuario,false);
                    int NewScore = (Good*100)/Total;
                    UserUpdateScore(usuario, NewScore);
                }
                prices.Close();
                
            }

        }

        public List<Price> SearchProductByName(string prodToSearch)
        {
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                var prods = new SqlCommand("SELECT Products.name, Products.codeBar,Prices.idPrice, Prices.value, Prices.value_date, Locations.local, Locations.address, Prices.NameUser FROM Products INNER JOIN Prices ON Prices.idProduct = Products.idProduct INNER JOIN Locations ON Prices.idLocation = Locations.idLocation WHERE Products.Name LIKE @name + '%' OR Products.CodeBar LIKE @name + '%' AND Prices.Deleted = 0", connection);
                var prodName = prods.Parameters.AddWithValue("@name", prodToSearch);
                var products = prods.ExecuteReader();
                List<Price> listProducts = new List<Price>();
                while (products.Read())
                {
                    Location loc = new Location(products["local"].ToString().Trim(), products["address"].ToString().Trim());
                    Product prod = new Product(products["name"].ToString().Trim(), products["codeBar"].ToString().Trim());
                    Price price = new Price((int)products["idPrice"], (decimal)products["value"], (DateTimeOffset)products["value_date"], prod, loc, (string)products["NameUser"]);
                    listProducts.Add(price);
                }
                connection.Close();
                return listProducts;
            }
        }


        public List<Price> GetAllProducts()
        {
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                var pri = new SqlCommand("SELECT Products.name, Products.codeBar, Prices.idPrice, Prices.value, Prices.value_date, Locations.local, Locations.address , Prices.NameUser FROM Products INNER JOIN Prices ON Prices.idProduct = Products.idProduct INNER JOIN Locations ON Prices.idLocation = Locations.idLocation WHERE Prices.Deleted = 0 and Prices.Qualification = 0", connection);
                var prices = pri.ExecuteReader();
                List<Price> listPrice = new List<Price>();
                while (prices.Read())
                {
                    Location loc = new Location(prices["local"].ToString().Trim(), prices["address"].ToString().Trim());
                    Product prod = new Product(prices["name"].ToString().Trim(), prices["codeBar"].ToString().Trim());
                    Price price = new Price((int)prices["idPrice"],(decimal)prices["value"], (DateTimeOffset)prices["value_date"], prod, loc, (string)prices["NameUser"]);
                    listPrice.Add(price);
                }
                connection.Close();
                return listPrice;
            }
        }

        //-------------------------------------------------------------------------------------------

        public string StoreUser(User user)
        {
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                string insert_user = "insert into Users values (@nameUser,@pass,@email,@role,@name,@lastName,@dni,@score,@enable,@GUID)";
                using (SqlCommand command = new SqlCommand(insert_user, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nameUser", user.UserName);
                    command.Parameters.AddWithValue("@pass", user.Password);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.Parameters.AddWithValue("@name", user.Name);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@dni", user.DNI);
                    command.Parameters.AddWithValue("@score", user.Score);
                    command.Parameters.AddWithValue("@enable", user.Enable);
                    command.Parameters.AddWithValue("@GUID", user.GUID);
                    command.ExecuteNonQuery();
                }
            }
            //query almacenar en la base de datos
            return "Operacion de registro de Usuraio: Done";
        }

        public List<User> GetAllUsers()
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var users = new SqlCommand("SELECT * FROM Users WHERE Users.Enable=1", conn);
                var user = users.ExecuteReader();
                List<User> listUsers = new List<User>();
                while (user.Read())
                {
                    if(user.GetInt32(3)>=1){
                        string usuario = user.GetString(0);
                        string mail = user.GetString(2);
                        int role = user.GetInt32(3);
                        string name = user.GetString(4);
                        string lastname = user.GetString(5);
                        string dni = user.GetString(6);
                        int score = user.GetInt32(7);
                        bool enable = user.GetBoolean(8);
                        User usu = new User(usuario, "", mail, role, name, lastname, dni, score, enable,"");
                        listUsers.Add(usu);
                    }
                }
                return listUsers;
            }
        }

        public List<User> GetUsersNoValidated()
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var users = new SqlCommand("SELECT * FROM Users WHERE Users.Enable=1 and Users.Role=1", conn);
                var user = users.ExecuteReader();
                List<User> listUsers = new List<User>();
                while (user.Read())
                {
                    string usuario = user.GetString(0);
                    string mail = user.GetString(2);
                    int role = user.GetInt32(3);
                    string name = user.GetString(4);
                    string lastname = user.GetString(5);
                    string dni = user.GetString(6);
                    int score = user.GetInt32(7);
                    bool enable = user.GetBoolean(8);
                    User usu = new User(usuario, "", mail, role, name, lastname, dni, score, enable,"");
                    listUsers.Add(usu);
                }
                return listUsers;
            }
        }

        public string DisableUser(string userMod)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                string search_user = "Update Users set Enable=@enable WHERE NameUser=@nameUser";
                using (SqlCommand command = new SqlCommand(search_user, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@enable", 0);
                    command.Parameters.AddWithValue("@nameUser", userMod);
                    command.ExecuteNonQuery();
                }
            }
            return "Cambio de estado habilitante: Done";
        }

        public string ChangeRole(string userCambiado, int newRole)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var command = new SqlCommand("Update Users set Role=@newRole WHERE NameUser=@userCambiado", conn);
                command.Parameters.AddWithValue("@userCambiado", userCambiado);
                command.Parameters.AddWithValue("@newRole", newRole);
                command.ExecuteNonQuery();
            }
            return "Operacion de cambio de rol: Done";
        }

        public string UserUpdateScore(string user, int newScore)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var updateScore = new SqlCommand("Update Users set Score=@newScore WHERE NameUser=@userUpdate", conn);
                updateScore.Parameters.AddWithValue("@userUpdate", user);
                updateScore.Parameters.AddWithValue("@newScore", newScore);
                updateScore.ExecuteNonQuery();
            }
            return "Operacion de Actualizacion: Done";
        }

        public string ChangePassword(string user, string newPassword)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var updateScore = new SqlCommand("Update Users set Password=@newPassword WHERE NameUser=@userUpdate", conn);
                updateScore.Parameters.AddWithValue("@userUpdate", user);
                updateScore.Parameters.AddWithValue("@newPassword", newPassword);
                updateScore.ExecuteNonQuery();
            }
            return "Operacion de cambio de contraseña: Done";
        }

        public bool CheckExistUser(string user)
        {
            bool salida = false;
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                string search_user = "SELECT COUNT(NameUser) FROM Users WHERE NameUser=@nameUser OR Email=@nameUser";
                using (SqlCommand command = new SqlCommand(search_user, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nameUser", user);
                    command.ExecuteNonQuery();
                    var countOut = command.ExecuteReader();
                    while (countOut.Read())
                    {
                        if (countOut.GetInt32(0) == 1)
                        {
                            salida = true;
                        }
                    }
                }
            }
            return salida;
        }

        public bool CheckExistMail(string mail)
        {
            bool salida = false;
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                string search_user = "SELECT COUNT(Email) FROM Users WHERE Email=@mail";
                using (SqlCommand command = new SqlCommand(search_user, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@mail", mail);
                    command.ExecuteNonQuery();
                    var countOut = command.ExecuteReader();
                    while (countOut.Read())
                    {
                        if (countOut.GetInt32(0) == 1)
                        {
                            salida = true;
                        }
                    }
                }
            }
            return salida;
        }

        public bool CheckPassLogin(string user, string pass)
        {
            bool salida = false;
            DBConnection dbConnection = new DBConnection();
            using (var connection = new SqlConnection(dbConnection.ConnString))
            {
                connection.Open();
                string login_user = "SELECT COUNT(NameUser) FROM Users WHERE (NameUser=@nameUser OR Email=@nameUser) AND Password=@pass AND Enable=1 AND Role>=1";
                using (SqlCommand command = new SqlCommand(login_user, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nameUser", user);
                    command.Parameters.AddWithValue("@pass", pass);
                    command.ExecuteNonQuery();
                    var countOut = command.ExecuteReader();
                    while (countOut.Read())
                    {
                        if (countOut.GetInt32(0) == 1)
                        {
                            salida = true;
                        }
                    }
                }
            }
            return salida;
        }
        
        public int CountProductByUser(string nameUser, bool qualification)
        {
            int retorno = 0;
            
            DBConnection db = new DBConnection();
            using(SqlConnection connection = new SqlConnection(db.ConnString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"

                        SELECT count(idProduct)
                        FROM Prices 
                        WHERE Prices.Qualification = @qualification AND Prices.NameUser = @nameUser";
                        
                    command.Parameters.AddWithValue("@nameUser", nameUser);
                    command.Parameters.AddWithValue("@Qualification", qualification);
                        
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetInt32(0);
                        }
                    }
                }
            }
            return retorno;
        }

        public void EndRegistration(string guid)
        {
            DBConnection dbConnection = new DBConnection();
            using (var conn = new SqlConnection(dbConnection.ConnString))
            {
                conn.Open();
                var updateScore = new SqlCommand("Update Users set Role=@newRole WHERE GUID=@guid", conn);
                updateScore.Parameters.AddWithValue("@newRole", 1);
                updateScore.Parameters.AddWithValue("@guid", guid);
                updateScore.ExecuteNonQuery();
            }
        }

        public int FindRoleUser(string userName)
        {
            int retorno = 0;
            DBConnection db = new DBConnection();
            using(SqlConnection connection = new SqlConnection(db.ConnString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"

                        SELECT Role
                        FROM Users 
                        WHERE Users.NameUser = @nameUser";
                        
                    command.Parameters.AddWithValue("@nameUser", userName);
                        
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetInt32(0);
                        }
                    }
                }
            }
            return retorno; 
        }
    }
}
