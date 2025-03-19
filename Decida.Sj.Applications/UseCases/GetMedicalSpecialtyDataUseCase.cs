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

    public class GetMedicalSpecialtyDataUseCase : IGetMedicalSpecialtyDataUseCase
    {
       
        private readonly IMedicalSpecialtyServices _MedicalSpec;
        private readonly IHealthServices _healthServices;

        public GetMedicalSpecialtyDataUseCase(IMedicalSpecialtyServices MedicalSpec, IHealthServices healthServices)
        {

            _MedicalSpec = MedicalSpec;
            _healthServices = healthServices;
        }

        public async Task<(bool status,string listMedicalSpecialty)> GetMedicalSpecialtyDataUseCaseList(int? id_convenio)
        {
            try
            {

                var companies =  (await _healthServices.GetHealthPlanListAsyncService()).Where(x=>x.id_convenio== id_convenio).FirstOrDefault();

                string listMedicalSpecialty = "";
                var plan = await _MedicalSpec.GetHMedicalSpecialtyListAsyncService(companies.cd_convenio);

                if (plan != null && plan.Count > 0)
                {

                    foreach (var item in plan)
                    {
                        listMedicalSpecialty += $"{item.CdEspecialidade} - {item.DsEspecialidade}\r\n ";


                    }
                    // Encontrou
                    return (true, listMedicalSpecialty);
                }
                else
                {
                    // Não encontrou
                    return (false, listMedicalSpecialty);
                }

            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }

        }
    }
}
