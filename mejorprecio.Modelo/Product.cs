using System;
using System.Text;

namespace mejorprecio.Modelo
{
    public class Product
    {


        public string Name { get; set; }
        public string Description { get; set; }
        public string CodeBar { get; set; }


        public Product()
        {

        }
        
        public Product(string name, string codeBar)
        {
            Name = name;
            CodeBar = codeBar;
        }
    }
}
