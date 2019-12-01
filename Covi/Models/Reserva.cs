using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class Reserva
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
