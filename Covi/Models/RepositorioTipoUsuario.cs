using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioTipoUsuario : RepositorioBase, IRepositorioTipoUsuario
    {
        public RepositorioTipoUsuario(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(TipoUsuario pTipoUsuario)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miTipoUsuario = new TipoUsuario()
                {
                    TipoUsuarioId = pTipoUsuario.TipoUsuarioId,
                    Nombre = pTipoUsuario.Nombre,
                    EstaHabilitado = pTipoUsuario.EstaHabilitado,
                    EstaPublicado = pTipoUsuario.EstaPublicado,
                };

                context.Entry(miTipoUsuario).State = EntityState.Added;
                //context.SaveChanges();

                //context.Camara.Add(camara);
                //context.SaveChanges();

                return context.SaveChanges();
            }

            return res;
        }
        public int Baja(int id)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miTipoUsuario = new TipoUsuario()
                {
                    TipoUsuarioId = id
                };

                context.TipoUsuario.Remove(miTipoUsuario);
                context.SaveChanges();
            }


            return res;
        }
        public int Modificacion(TipoUsuario miTipoUsuario)
        {
            int res = -1;

            var miCamaraEditada = new TipoUsuario()
            {
                TipoUsuarioId = miTipoUsuario.TipoUsuarioId,
                Nombre = miTipoUsuario.Nombre,
                EstaHabilitado = miTipoUsuario.EstaHabilitado,
                EstaPublicado = miTipoUsuario.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.TipoUsuario.Update(miCamaraEditada);
                context.SaveChanges();
            }

            return res;
        }

        public IList<TipoUsuario> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var emp = context.TipoUsuario.ToList();
                return emp;
            }

        }

        public TipoUsuario ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var camara = context.TipoUsuario.Where(e => e.TipoUsuarioId == id).FirstOrDefault();
                return camara;
            }

        }
    }
}
