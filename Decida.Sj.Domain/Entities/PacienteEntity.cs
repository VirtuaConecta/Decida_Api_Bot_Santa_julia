using Decida.Sj.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Domain.Entities
{
    public class PacienteEntity
    {
        public Cpf? PacientCpf { get; set; }
        public string PacientName { get; set; }=string.Empty;
        public int PacienteId{ get; set; }
        public int PacientIdCare { get; set; }

        public string PacientCare { get; set; }
    
    }
}
