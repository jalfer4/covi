using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioLocal : RepositorioBase, IRepositorioLocal
    {
        public RepositorioLocal(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Local _Local)
        {
            

            using (var context = new CoviContext())
            {
                var miLocal = new Local()
                {
                    LocalId = _Local.LocalId,
                    UsuarioId = _Local.UsuarioId,
                    TipoLocalId = _Local.TipoLocalId,
                    Nombre = _Local.Nombre,
                    Capacidad = _Local.Capacidad,
                    Direccion = _Local.Direccion,
                    Telefono =_Local.Telefono, 
                    EstaHabilitado = _Local.EstaHabilitado,
                    EstaPublicado = _Local.EstaPublicado,
       
    };

                context.Entry(miLocal).State = EntityState.Added;
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
                var miLocal = new Local()
                {
                    LocalId = _id
                };

                context.Local.Remove(miLocal);
                context.SaveChanges();
            }

            return res;
        }
        public int Modificacion(Local _Local)
        {
            int res = -1;

            var miLocal = new Local()
            {
                LocalId = _Local.LocalId,
                UsuarioId = _Local.UsuarioId,
                TipoLocalId = _Local.TipoLocalId,
                Nombre = _Local.Nombre,
                Capacidad = _Local.Capacidad,
                Direccion = _Local.Direccion,
                Telefono = _Local.Telefono,
                EstaHabilitado = _Local.EstaHabilitado,
                EstaPublicado = _Local.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.Local.Update(miLocal);
                context.SaveChanges();
            }

            return res;
        }

        public IList<Local> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var resulset = context.Local.Include("Usuario").ToList();
                return resulset;
            }

        }

        public Local ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var resultSet = context.Local.Include("Usuario").Where(e => e.LocalId == id).FirstOrDefault();
                return resultSet;
            }

        }
    }
}
