using Decida.Sj.Core.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Model
{
    public class PacientDTO
    {
        public Cpf? PacientCpf { get; set; }
        public string PacientName { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public int PacientIdCare { get; set; }
        public int PacientIndexCare { get; set; }
        public string PacientCare { get; set; }
    }
}
