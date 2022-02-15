using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escuela_de_Magia.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Hogwarts;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ESTUDIANTE>()
               .HasIndex(c => c.Id).IsUnique(true);
            modelBuilder.Entity<ESTUDIANTE>()
               .Property(c => c.Edad)
               .HasColumnType("numeric")
               .HasPrecision(2,0);
            modelBuilder.Entity<ESTUDIANTE>()
              .Property(c => c.Identificacion)
              .HasColumnType("numeric")
              .HasPrecision(10, 0);

            modelBuilder.Entity<CASA>()
              .HasIndex(c => c.Id).IsUnique(true);

        }


        /*              Inicio de los modelos de la Tablas de la Base de Datos DbSet<>*/
        public DbSet<ESTUDIANTE> Estudiante { get; set; }
        public DbSet<CASA> Casa { get; set; }


        /*                                  Fin de Seccion de los DbSet<>*/

        /*Funcion para el Manejo de la respuesta del Salvar cambios en el DbContext*/
        public async Task<string> GuardarDatos()
        {
            try
            {
                await SaveChangesAsync();
                return ""; //si retorna vacio todo salió correcto, caso contrario, retorna error.
            }
            catch (Exception e)
            {
                // Guarda en el Log el error, para llevar registro de errores de la base
                //Database.Log?.Invoke(e.Message);
                return "Error al guardar. " + (e.Message + "\n") + e.InnerException.ToString();
            }
        }
    }
}
