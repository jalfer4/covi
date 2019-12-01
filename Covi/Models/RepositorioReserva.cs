using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioReserva : RepositorioBase, IRepositorioReserva
    {
        public RepositorioReserva(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Reserva _Reserva)
        {
            

            using (var context = new CoviContext())
            {
                var miReserva = new Reserva()
                {
                    ReservaId = _Reserva.ReservaId,
                    UsuarioId = _Reserva.UsuarioId,
                    EventoId = _Reserva.EventoId,
                    FechaAlta = _Reserva.FechaAlta,
                    EstaHabilitado = _Reserva.EstaHabilitado,
                    EstaPublicado = _Reserva.EstaPublicado,
       
    };

                context.Entry(miReserva).State = EntityState.Added;
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
                var miReserva = new Reserva()
                {
                    ReservaId = _id
                };

                context.Reserva.Remove(miReserva);
                context.SaveChanges();
            }

            return res;
        }
        public int Modificacion(Reserva _Reserva)
        {
            int res = -1;

            var miReserva = new Reserva()
            {
                ReservaId = _Reserva.ReservaId,
                UsuarioId = _Reserva.UsuarioId,
                EventoId = _Reserva.EventoId,
                FechaAlta = _Reserva.FechaAlta,
                EstaHabilitado = _Reserva.EstaHabilitado,
                EstaPublicado = _Reserva.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.Reserva.Update(miReserva);
                context.SaveChanges();
            }

            return res;
        }

        public IList<Reserva> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var resulset = context.Reserva.Include("Usuario").ToList();
                return resulset;
            }

        }

        public Reserva ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var resultSet = context.Reserva.Include("Usuario").Where(e => e.ReservaId == id).FirstOrDefault();
                return resultSet;
            }

        }
    }
}
