using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Models
{
    public class RepositorioTipoLocal : RepositorioBase, IRepositorioTipoLocal
    {
        public RepositorioTipoLocal(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(TipoLocal _TipoLocal)
        {
            int res = -1;

            using (var context = new CoviContext())
            {
                var miTipoLocal = new TipoLocal()
                {
                    TipoLocald = _TipoLocal.TipoLocald,
                    Nombre = _TipoLocal.Nombre,
                    EstaHabilitado = _TipoLocal.EstaHabilitado,
                    EstaPublicado = _TipoLocal.EstaPublicado,
                };

                context.Entry(miTipoLocal).State = EntityState.Added;
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
                var miTipoLocal = new TipoLocal()
                {
                    TipoLocald = _id
                };

                context.TipoLocal.Remove(miTipoLocal);
                context.SaveChanges();
            }


            return res;
        }
        public int Modificacion(TipoLocal _TipoLocal)
        {
            int res = -1;

            var miTipoLocal = new TipoLocal()
            {
                TipoLocald = _TipoLocal.TipoLocald,
                Nombre = _TipoLocal.Nombre,
                EstaHabilitado = _TipoLocal.EstaHabilitado,
                EstaPublicado = _TipoLocal.EstaPublicado,
            };

            using (var context = new CoviContext())
            {
                context.TipoLocal.Update(miTipoLocal);
                context.SaveChanges();
            }

            return res;
        }

        public IList<TipoLocal> ObtenerTodos()
        {
            using (var context = new CoviContext())
            {
                var emp = context.TipoLocal.ToList();
                return emp;
            }

        }

        public TipoLocal ObtenerPorId(int id)
        {
            using (var context = new CoviContext())
            {
                var miTipoLocal = context.TipoLocal.Where(e => e.TipoLocald == id).FirstOrDefault();
                return miTipoLocal;
            }

        }
    }
}
