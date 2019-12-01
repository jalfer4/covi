using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioEvento : RepositorioBase, IRepositorioEvento
    {
        public RepositorioEvento(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Evento _Evento)
        {
            

            using (var context = new CoviContext())
            {
                var miEvento = new Evento()
                {
                    EventoId = _Evento.EventoId,
                    LocalId = _Evento.LocalId,
                    TipoEventoId = _Evento.TipoEventoId, 
                    FechaAlta = _Evento.FechaAlta,
                    EstaHabilitado = _Evento.EstaHabilitado,
                    EstaPublicado = _Evento.EstaPublicado,
       
    };

                context.Entry(miEvento).State = EntityState.Added;
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
                var mievento = new Evento()
                {
                    LocalId = _id
                };

                context.Evento.Remove(mievento);
                context.SaveChanges();
            }

            return res;
        }
        public int Modificacion(Evento _Evento)
        {
            int res = -1;

            var miEvento = new Evento()
            {
                EventoId = _Evento.EventoId,
                LocalId = _Evento.LocalId,
                TipoEventoId = _Evento.TipoEventoId, 
                FechaAlta = _Evento.FechaAlta,
                EstaHabilitado = _Evento.EstaHabilitado,
                EstaPublicado = _Evento.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.Evento.Update(miEvento);
                context.SaveChanges();
            }

            return res;
        }

        public IList<Evento> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var resulset = context.Evento.Include("Usuario").ToList();
                return resulset;
            }

        }

        public Evento ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var resultSet = context.Evento.Include("Usuario").Where(e => e.EventoId == id).FirstOrDefault();
                return resultSet;
            }

        }
    }
}
