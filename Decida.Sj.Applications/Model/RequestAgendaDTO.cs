using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Model
{
    public class RequestAgendaDTO
    {
        public string hash { get; set; }=string.Empty;
        public int idAgenda { get; set; }
        public string nrCarteira { get; set; } = string.Empty;
        public  string vencimentoCarteira { get; set; } = string.Empty;
    }
}

