using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class Evento
    {
        public Evento()
        {
            Reserva = new HashSet<Reserva>();
        }

        public int EventoId { get; set; }
        public int LocalId { get; set; }
        public int TipoEventoId { get; set; }
        public DateTime FechaAlta { get; set; }
        public string NombreArtista { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual TipoEvento TipoEventoNavigation { get; set; }
        public virtual Local LocalNavigation { get; set; }
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
