using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Core.Entities;
using Decida.Sj.Core.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Infrastructure.Services
{
    public class HealthServices : IHealthServices
    {

        private readonly IHealthPlanMysqlRepository _plan;

        public HealthServices(IHealthPlanMysqlRepository plan)
        {

            _plan = plan;
        }
        public async Task<List<HealthPlanEntity>> GetHealthPlanListAsyncService()
        {
            var HeatlthPlanRepoReturn = new List<HealthPlanEntity>();

            try
            {
                HeatlthPlanRepoReturn = await _plan.GetHelthPlanList();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return HeatlthPlanRepoReturn;
        }

        public async Task<List<PlanCareEntity>> GetPlanCareListByCompanyAsyncService(int cd_convenio)
        {
            var PlanRepoReturn = new List<PlanCareEntity>();

            try
            {
                PlanRepoReturn = await _plan.GetPlanListByCompany(cd_convenio);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return PlanRepoReturn;
        }

      
    }
}
