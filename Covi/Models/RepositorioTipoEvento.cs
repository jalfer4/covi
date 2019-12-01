using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioTipoEvento : RepositorioBase, IRepositorioTipoEvento
    {
        public RepositorioTipoEvento(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(TipoEvento _TipoEvento)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miTipoUsuario = new TipoUsuario()
                {
                    TipoUsuarioId = _TipoEvento.TipoEventoId,
                    Nombre = _TipoEvento.Nombre,
                    EstaHabilitado = _TipoEvento.EstaHabilitado,
                    EstaPublicado = _TipoEvento.EstaPublicado,
                };

                context.Entry(miTipoUsuario).State = EntityState.Added;
                //context.SaveChanges();

                //context.Camara.Add(camara);
                //context.SaveChanges();

                return context.SaveChanges();
            }

            return res;
        }
        public int Baja(int _id)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miTipoEvento = new TipoEvento()
                {
                    TipoEventoId = _id
                };

                context.TipoEvento.Remove(miTipoEvento);
                context.SaveChanges();
            }


            return res;
        }
        public int Modificacion(TipoEvento _TipoEvento)
        {
            int res = -1;

            var miTipoEvento = new TipoEvento()
            {
                TipoEventoId = _TipoEvento.TipoEventoId,
                Nombre = _TipoEvento.Nombre,
                EstaHabilitado = _TipoEvento.EstaHabilitado,
                EstaPublicado = _TipoEvento.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.TipoEvento.Update(miTipoEvento);
                context.SaveChanges();
            }

            return res;
        }

        public IList<TipoEvento> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var emp = context.TipoEvento.ToList();
                return emp;
            }

        }

        public TipoEvento ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var miTipoEventos = context.TipoEvento.Where(e => e.TipoEventoId == id).FirstOrDefault();
                return miTipoEventos;
            }

        }
    }
}
