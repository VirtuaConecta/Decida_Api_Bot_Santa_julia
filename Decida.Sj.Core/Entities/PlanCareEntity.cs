using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Core.Entities
{
    public class PlanCareEntity
    {
        public int Sequence { get; set; }
        public int CdConvenio { get; set; }
        public string CdCategoria { get; set; }
        public string CdPlano { get; set; }
        public string DsPlano { get; set; }
    }
}
