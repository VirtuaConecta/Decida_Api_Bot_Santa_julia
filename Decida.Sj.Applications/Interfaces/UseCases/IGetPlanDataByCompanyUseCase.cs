using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.UseCases
{
    public interface IGetPlanDataByCompanyUseCase
    {
        Task<(bool status, string listHealth)> GetPlanDataList(int cd_convenio);
    }
}
