using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace WebApplication1.Controllers
{
    public class Alumno
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="El campo es obligatorio")]
        public string NombreApellido { get; set; }

        [Range(18,75,ErrorMessage ="La edad va entre 18 y 75")]
        public int Edad { get; set; }
       
        public bool ActivoTrabajo { get { return Edad > 18 && Edad < 75; } }

    }

    public class AlumnosController : ApiController
    {
        private static List<Alumno> ListaCompleta = new List<Alumno>()
        {
            new Alumno(){ Id = 1, NombreApellido ="Maxi Lovera", Edad = 35 },
            new Alumno(){ Id = 2, NombreApellido ="Gonza Perez", Edad = 29},
            new Alumno(){ Id = 3, NombreApellido ="Pepe Argento", Edad = 67 },
            new Alumno(){ Id = 4, NombreApellido ="Leo Scaloni", Edad = 55 },
            new Alumno(){ Id = 5, NombreApellido ="El chavo", Edad = 14 },
            new Alumno(){ Id = 6, NombreApellido ="Tito Peñalba", Edad = 80 },
            new Alumno(){ Id = 7, NombreApellido ="Indio Liber Vespa", Edad = 93 },
        };
        
        public IHttpActionResult Get(string nombre = null, int edadMaxima = 0, bool activo = true)
        {
            var listaFiltrada = ListaCompleta;

            if (!string.IsNullOrEmpty(nombre))
                listaFiltrada = listaFiltrada.Where(x => x.NombreApellido.Contains(nombre)).ToList();

            if (edadMaxima > 0)
                listaFiltrada = listaFiltrada.Where(x => x.Edad <= edadMaxima).ToList();

            return Ok(listaFiltrada);
        }

        public IHttpActionResult Get(int id)
        {
            var elemento = ListaCompleta.FirstOrDefault(x=>x.Id == id);
            
            if (elemento == null)
                return NotFound();

            return Ok(elemento);

        }

        //Validacion nombre y apellido es obligatorio
        public IHttpActionResult Post(Alumno alumno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Random random = new Random();
            alumno.Id = random.Next(1000, 2000);

            ListaCompleta.Add(alumno);

            return Created("", alumno);
        }

        public IHttpActionResult Put(int id, Alumno alumno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alumnoGuardado = ListaCompleta.FirstOrDefault(x => x.Id == id);

            alumnoGuardado.NombreApellido = alumno.NombreApellido;
            alumnoGuardado.Edad = alumno.Edad;

            return Ok(alumnoGuardado);
        }

        public IHttpActionResult Delete(int id)
        {
            var alumnoGuardado = ListaCompleta.FirstOrDefault(x => x.Id == id);
            ListaCompleta = ListaCompleta.Where(x => x.Id != id).ToList();

            return Ok(alumnoGuardado);
        }
    }
}
