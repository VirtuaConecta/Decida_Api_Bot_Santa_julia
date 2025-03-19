using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Services
{
    public interface IMedicService
    {
        Task<List<MedicEntity>> GetMedicListAsyncService();
        Task<List<MedicEntity>> GetMedicListByHealthPlanEspecialtyAsyncService(int cd_especialida, int cd_convenio);
    }
}
