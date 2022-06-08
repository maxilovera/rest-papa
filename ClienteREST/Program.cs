using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClienteREST
{
    class AlumnoCliente
    {
        public int Codigo { get; set; }
        public string NombreYApellido { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //Mapear Newtonsoft la clase del servicio REST a una clase mia 
            var url = "http://localhost:50112/api/alumnos";

            // Create a request for the URL.
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);

                //Instalar newtonsoft

                var respuesta = JsonConvert.Deserialize<dynamic>(responseFromServer);

                List<AlumnoCliente> lista = new List<AlumnoCliente>();

                foreach (var item in respuesta)
                {
                    lista.Add(new AlumnoCliente()
                    {
                        Codigo = item.Id,
                        NombreYApellido = item.Nombre
                    });
                }
            }

            // Close the response.
            response.Close();

            Console.Read();
        }
    }
}
