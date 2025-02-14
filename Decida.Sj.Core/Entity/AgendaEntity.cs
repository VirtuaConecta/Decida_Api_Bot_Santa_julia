using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Core.Entity
{
    public class AgendaEntity
    {
        public string Dia { get; set; }
        public string Hora { get; set; } = string.Empty;
        public string DiaSemana { get; set; } = string.Empty;
        public int Rank { get; set; }
        public int CdAgenda { get; set; }
        public string HoraInicial { get; set; } = string.Empty;
        public string HoraFinal { get; set; } = string.Empty;
        public int CdPessoaFisica { get; set; }
        public string NmPessoaFisica { get; set; } = string.Empty;
    }

}
