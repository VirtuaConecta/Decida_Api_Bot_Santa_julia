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

    public class GetHealthPlanDataUseCase : IGetHealthPlanDataUseCase
    {
       
        private readonly IHealthServices _healthPlan;

        public GetHealthPlanDataUseCase(IHealthServices healthPlan)
        {
      
            _healthPlan = healthPlan;
        }

        public async Task<(bool status,string listHealth)> GetHealthPlanData()
        {
            try
            {
                string listHealth = "";
                var plan = await _healthPlan.GetHealthPlanListAsyncService();

                if (plan != null && plan.Count > 0)
                {

                    foreach (var item in plan)
                    {
                        listHealth += $"{item.id_convenio} - {item.ds_convenio}\r\n ";


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
