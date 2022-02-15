using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Escuela_de_Magia.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;

namespace Escuela_de_Magia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        //Clase crada para darle estilo a la infomacion de Datos de Estudiante
        public class DATOSEstudiante
        {
            public int Id { get; set; } = 0;

            [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Solo Caracteres")]
            [MaxLength(20), Required()]
            public string Nombre { get; set; } = "";

            [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Solo Caracteres")]
            [MaxLength(20), Required()]
            public string Apellido { get; set; } = "";

            [Required()]
            public int Identificacion { get; set; } = 0;

            [Required()]
            public int Edad { get; set; } = 0;

            [Required()]
            public int Id_Casa { get; set; } = 0;
        }//cierre de public class DATOSEstudiante

        /*Constructor*/
        public EstudiantesController(AppDbContext context)
        {
            _context = context;

            //Inicializa las Casas Posibles de la Escuela
            if (_context.Casa.Count() == 0)
            {
                var Escuelas = new List<CASA>();
                Escuelas.Add(new CASA { Nombre = "Gryffindor" });
                Escuelas.Add(new CASA { Nombre = "Hufflepuff" });
                Escuelas.Add(new CASA { Nombre = "Ravenclaw" });
                Escuelas.Add(new CASA { Nombre = "Slytherin" });

                _context.Casa.AddRange(Escuelas);
                var response = context.SaveChanges();
            }
        }//cierre  public EstudiantesController(AppDbContext context)

        // GET: api/Estudiantes
        /*Metodo que devuelve todos los estudiantes con su casa asignada que se encuentren en la base de datos*/
        [HttpGet("ConsultarALL")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult> GetAllEstudiantes()
        {
            //Busqueda y relacion entre los Datos del Estudiante y su Casa asignada
            var students = await _context.Estudiante.Join(_context.Casa, post => post.CasaId, meta => meta.Id,
              (p, m) => new
              {
                  Id = p.Id,
                  Nombre = p.Nombre,
                  Apellido = p.Apellido,
                  Identificacion = p.Identificacion,
                  Edad = p.Edad,
                  Nombre_Casa = m.Nombre,
              }).ToListAsync();

            //Verifica si existen estudiantes en la base de datos y devuelve la informacion
            if (students.Count() > 0)
                return Ok(students);

            return BadRequest("No Hay Estudiantes Ingresados");
        }//cierre de  public async Task<ActionResult> GetAllEstudiantes()

        // GET: api/Estudiantes/5
        /*Metodo Consulta los Datos del Estudiante segun su Id*/
        [HttpGet("Consulta/{id}")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult> GetESTUDIANTE(int id)
        {
            //Busqueda y relacion entre los Datos del Estudiante y su Casa asignada por el Id recibido por parametro
            var eSTUDIANTE = await _context.Estudiante.Join(_context.Casa, post => post.CasaId, meta => meta.Id,
              (p, m) => new
              {
                  Id = p.Id,
                  Nombre = p.Nombre,
                  Apellido = p.Apellido,
                  Identificacion = p.Identificacion,
                  Edad = p.Edad,
                  Nombre_Casa = m.Nombre,
              }).Where(post => post.Id == id).FirstOrDefaultAsync();

            //Verifica si existe un estudiante segun su id recibido por parametro
            if (eSTUDIANTE == null)
                return NotFound();

            return Ok(eSTUDIANTE);
        }//cierre de  public async Task<ActionResult> GetESTUDIANTE(int id)

        /*Metodo que devuelve las opciones de las Casas Adminitidas*/
        [HttpGet("Casas")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult<IEnumerable<CASA>>> GetCasa()
        {
            //Inicializa las Casas Posibles de la Escuela
            if (_context.Casa.Count() == 0)
            {
                var Escuelas = new List<CASA>();
                Escuelas.Add(new CASA { Nombre = "Gryffindor" });
                Escuelas.Add(new CASA { Nombre = "Hufflepuff" });
                Escuelas.Add(new CASA { Nombre = "Ravenclaw" });
                Escuelas.Add(new CASA { Nombre = "Slytherin" });

                _context.Casa.AddRange(Escuelas);
                var response = _context.SaveChanges();
            }
            return await _context.Casa.ToListAsync();
        }//cierre  public async Task<ActionResult<IEnumerable<CASA>>> GetCasa()

        // POST: api/Estudiantes
        /*Metodo que inlcuye los datos del estudiante recibido por parametro a la base de datos*/
        [HttpPost("Enviar")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult> PostESTUDIANTE(DATOSEstudiante eSTUDIANTE)
        {
            try
            {
                //Verifica la casa recibida en el parametro
                var Home = await _context.Casa.Where(x => x.Id == eSTUDIANTE.Id_Casa).FirstOrDefaultAsync();

                if (Home == null)
                    return BadRequest("El ID de Casa Incorrecto. (Solo 4 posibles opciones: 1:Gryffindor, 2:Hufflepuff, 3:Ravenclaw, 4:Slytherin)");

                //Verifica que los datos del nombre y el apellido solo sean caracteres
                if (!eSTUDIANTE.Nombre.All(char.IsLetter) || !eSTUDIANTE.Apellido.All(char.IsLetter))
                    return BadRequest("Los Campos Nombre y Apellido Solo pueden tener Caracteres. Sin espacios en Blanco");

                //actualiza los datos recibido y construye la estructura de Estudiante
                ESTUDIANTE newStudent = new ESTUDIANTE()
                {
                    Nombre = eSTUDIANTE.Nombre,
                    Apellido = eSTUDIANTE.Apellido,
                    Identificacion = eSTUDIANTE.Identificacion,
                    Edad = eSTUDIANTE.Edad,
                    CasaId = eSTUDIANTE.Id_Casa,
                    Casa = Home,
                };
                _context.Estudiante.Add(newStudent);
                await _context.SaveChangesAsync();//Guarda en Base Datos

                return CreatedAtAction("GetESTUDIANTE", new { id = newStudent.Id }, newStudent);
            }
            catch (Exception e)//Verifica si da error al guardar los datos en base datos
            {
                return BadRequest(e.InnerException.Message);
            }
        }//cierre de  public async Task<ActionResult> PostESTUDIANTE(DATOSEstudiante eSTUDIANTE)

        // PUT: api/Estudiantes/5
        /*Metodo que actualiza los datos del estudiante recibido por parametro a la base de datos*/
        [HttpPut("Actualizar/{id}")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> PutESTUDIANTE(int id, DATOSEstudiante eSTUDIANTE)
        {
            //Compara que el id del estudiante a actualizar sean los mismos enviados por parametro
            if (id != eSTUDIANTE.Id)
                return BadRequest("El Id a modificar No Coincide con los Datos del Estudiante Ingresado");

            //Verifica el Id de la casa recibida en el parametro sea validos
            var Home = await _context.Casa.Where(x => x.Id == eSTUDIANTE.Id_Casa).FirstOrDefaultAsync();

            if (Home == null)
                return BadRequest("El ID de Casa Incorrecto. (Solo 4 posibles opciones: 1:Gryffindor, 2:Hufflepuff, 3:Ravenclaw, 4:Slytherin)");

            //Verifica que los datos del nombre y el apellido solo sean caracteres
            if (!eSTUDIANTE.Nombre.All(char.IsLetter) || !eSTUDIANTE.Apellido.All(char.IsLetter))
                return BadRequest("Los Campos Nombre y Apellido Solo pueden tener Caracteres. Sin espacios en Blanco");

            //actualiza los datos recibido y construye la estructura de Estudiante
            ESTUDIANTE newStudent = new ESTUDIANTE()
            {
                Id = eSTUDIANTE.Id,
                Nombre = eSTUDIANTE.Nombre,
                Apellido = eSTUDIANTE.Apellido,
                Identificacion = eSTUDIANTE.Identificacion,
                Edad = eSTUDIANTE.Edad,
                CasaId = eSTUDIANTE.Id_Casa,
                Casa = Home,
            };

            //Actualiza los datos de Estudiante
            _context.Entry(newStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();//Guarda en la base de Datos
            }
            catch (DbUpdateConcurrencyException)
            {
                //Verifica si hay error de concurrencia si existe el estudiante o fue previamente eliminado
                if (!ESTUDIANTEExists(id))
                {
                    return NotFound("Id del Estudiante No Existe");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)//Verifica errores al momento de actualizar en base de Datos
            {
                return BadRequest(e.InnerException.Message);
            }

            return NoContent();
        }//cierre de  public async Task<IActionResult> PutESTUDIANTE(int id, DATOSEstudiante eSTUDIANTE)


        // DELETE: api/Estudiantes/5
        /*Metodo que recibe un id del estudiante y lo elimina de la base de datos*/
        [HttpDelete("Eliminar/{id}")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult<ESTUDIANTE>> DeleteESTUDIANTE(int id)
        {
            try
            {
                //Busca el estudiante por Id
                var eSTUDIANTE = await _context.Estudiante.FindAsync(id);

                //Si no existe el estudiante por el Id devuelve error
                if (eSTUDIANTE == null)
                    return NotFound();

                //Elimina el estudiante de la base de datos
                _context.Estudiante.Remove(eSTUDIANTE);
                await _context.SaveChangesAsync();//Guarda en Base de Datos

                return eSTUDIANTE;
            }
            catch (Exception e)//Verifica si hay error al guardar los datos den Base de Datos
            {
                return BadRequest(e.InnerException.Message);
            }
        }//cierre de  public async Task<ActionResult<ESTUDIANTE>> DeleteESTUDIANTE(int id)

        /*Metodo que verifica si existe el estudiante por Id*/
        private bool ESTUDIANTEExists(int id)
        {
            return _context.Estudiante.Any(e => e.Id == id);
        }

    }
}
