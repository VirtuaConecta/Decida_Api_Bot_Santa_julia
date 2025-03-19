using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.UseCases
{
    public interface IGetMedicDataUseCase
    {
        Task<(bool status, string listMedic)> GetMedicDataUseCaseList();
        Task<(bool status, string listMedic)> GetMedicDataListByHealthPlanEspecialtyUseCase(int cd_espcialidade, int id_convenio);
    }
}
