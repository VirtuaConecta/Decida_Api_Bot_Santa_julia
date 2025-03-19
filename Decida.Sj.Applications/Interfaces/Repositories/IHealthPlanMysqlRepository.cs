using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Repositories
{
    public interface IHealthPlanMysqlRepository
    {
        Task<List<HealthPlanEntity>> GetHelthPlanListRepoAsync();
        Task<HealthPlanEntity> GetHelthPlanByIDRepoAsync(int id);

        Task<List<PlanCareEntity>> GetPlanListByCompanyRepoAsync(int id_convenio);
    }
}
