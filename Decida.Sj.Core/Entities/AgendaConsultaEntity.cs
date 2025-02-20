using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Core.Entities
{
    public class AgendaConsultaAgendarEntity:AgendaEntity
    {

        public int CdConvenio { get; set; }
        public string CdUsuarioConvenio { get; set; }
        public int CdPlano { get; set; }
        public DateTime DtValidadeCarteira { get; set; }

        public string CdCategoria { get; set; }
    }
 
}
