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
        Task<List<HealthPlanEntity>> GetHelthPlanList();
        Task<HealthPlanEntity> GetHelthPlanByID(int id);

        Task<List<PlanCareEntity>> GetPlanListByCompany(int id_convenio);
    }
}
