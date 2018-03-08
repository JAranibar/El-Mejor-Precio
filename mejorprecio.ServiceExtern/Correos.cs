using System;
using System.Net.Mail;

namespace mejorprecio.ServiceExtern
{
    public class Correos
    {
        // El código de la clase es:
        /*
        * Cliente SMTP
        * Gmail:  smtp.gmail.com  puerto:587
        * Hotmail: smtp.liva.com  puerto:25
        */
        SmtpClient server = new SmtpClient("smtp.gmail.com", 587);

        public Correos()
        {
            /*
            * Autenticacion en el Servidor
            * Utilizaremos nuestra cuenta de correo
            *
            * Direccion de Correo (Gmail o Hotmail)
            * y Contrasena correspondiente
            */
            server.Credentials = new System.Net.NetworkCredential("mejor.precio.7@gmail.com", "mejorprecio7");
            server.EnableSsl = true;
        }

        public MailMessage ArmarCorreo(string Mail, string GUID){
            MailMessage mnsj = new MailMessage();
            
            mnsj.Subject = "Bienvenido a Defensoria del Pueblo!!!";
        
            //Quien lo envia (de).
            mnsj.From = new MailAddress("mejor.precio.7@gmail.com", "Proyect Lagash");
        
            //Para quien (a).
            mnsj.To.Add(new MailAddress(Mail));

            /* Si deseamos Adjuntar algún archivo*/
            //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
        
            mnsj.Body = " Para finalizar exitosamente la registracion, por favor ingrese al siguiente link: \n\n"+"http://localhost:5000/users/endRegistration/?guid="+GUID;
            /*Retorna el mensaje Armado*/
            return mnsj;
        }

        public void MandarCorreo(MailMessage mensaje)
        {
            server.Send(mensaje);
        }

    }

}
