using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class TipoEvento
    {
        public TipoEvento()
        {
            Evento = new HashSet<Evento>();
        }

        public int TipoEventoId { get; set; }
        public string Nombre { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
    }
}
