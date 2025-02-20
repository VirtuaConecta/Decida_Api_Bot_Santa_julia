using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.UseCases
{

    public class GetPlanDataByCompanyUseCase : IGetPlanDataByCompanyUseCase
    {
       
        private readonly IHealthServices _healthPlan;

        public GetPlanDataByCompanyUseCase(IHealthServices healthPlan)
        {
      
            _healthPlan = healthPlan;
        }

        public async Task<(bool status,string listHealth)> GetPlanDataList(int cd_convenio)
        {
            try
            {
                string listHealth = "";
                var plan = await _healthPlan.GetPlanCareListByCompanyAsyncService(cd_convenio);

                if (plan != null && plan.Count > 0)
                {

                    foreach (var item in plan)
                    {
                        listHealth += $"{item.Sequence} - {item.DsPlano}\r\n ";


                    }
                    // Encontrou
                    return (true, listHealth);
                }
                else
                {
                    // Não encontrou
                    return (false, listHealth);
                }

            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }

        }
    }
}
