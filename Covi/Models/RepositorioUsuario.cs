using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Usuario _Usuario)
        {
            

            using (var context = new CoviContext())
            {
                var miUsuario = new Usuario()
                {
                    UsuarioId = _Usuario.UsuarioId,
                    TipoUsuarioId = _Usuario.TipoUsuarioId,
                    Nombre = _Usuario.Nombre,
                    Clave = _Usuario.Clave,
                    Apellido = _Usuario.Apellido,
                    ApyNomb = _Usuario.ApyNomb,
                    Dni = _Usuario.Dni,
                    Email = _Usuario.Email,
                    Telefono = _Usuario.Telefono,
                    EstaHabilitado = _Usuario.EstaHabilitado,
                    EstaPublicado = _Usuario.EstaPublicado,
       
    };

                context.Entry(miUsuario).State = EntityState.Added;
                context.SaveChanges();

                //context.Camara.Add(camara);
                //context.SaveChanges();

                return 1;
            }

            
        }
        public int Baja(int _id)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miUsuario = new Usuario()
                {
                    UsuarioId = _id
                };

                context.Usuario.Remove(miUsuario);
                context.SaveChanges();
            }

            return res;
        }
        public int Modificacion(Usuario _Usuario)
        {
            int res = -1;

            var miUsuario = new Usuario()
            {
                UsuarioId = _Usuario.UsuarioId,
                TipoUsuarioId = _Usuario.TipoUsuarioId,
                Nombre = _Usuario.Nombre,
                Clave = _Usuario.Clave,
                Apellido = _Usuario.Apellido,
                ApyNomb = _Usuario.ApyNomb,
                Dni = _Usuario.Dni,
                Email = _Usuario.Email,
                Telefono = _Usuario.Telefono,
                EstaHabilitado = _Usuario.EstaHabilitado,
                EstaPublicado = _Usuario.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.Usuario.Update(miUsuario);
                context.SaveChanges();
            }

            return res;
        }

        public IList<Usuario> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var resulset = context.Usuario.ToList();
                return resulset;
            }

        }

        public Usuario ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var resultSet = context.Usuario.Where(e => e.UsuarioId == id).FirstOrDefault();
                return resultSet;
            }

        }

        public Usuario ObtenerPorEmail(string email)
        {
             

            using (var context = new CoviContext())
            {
                var resultSet = context.Usuario.Where(e => e.Email == email).FirstOrDefault();
                return resultSet;
            }

             
        }

        public IList<Usuario> BuscarPorNombre(string nombre)
        {

            using (var context = new CoviContext())
            {
                var resultSet = context.Usuario.Where(e => e.Nombre == nombre).ToList();
                return resultSet;
            }

        }
    }
}
