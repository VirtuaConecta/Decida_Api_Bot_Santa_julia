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

        public GetMedicalSpecialtyDataUseCase(IMedicalSpecialtyServices MedicalSpec)
        {

            _MedicalSpec = MedicalSpec;
        }

        public async Task<(bool status,string listMedicalSpecialty)> GetMedicalSpecialtyDataUseCaseList()
        {
            try
            {
                string listMedicalSpecialty = "";
                var plan = await _MedicalSpec.GetHMedicalSpecialtyListAsyncService();

                if (plan != null && plan.Count > 0)
                {

                    foreach (var item in plan)
                    {
                        listMedicalSpecialty += $"{item.id_especialidade} - {item.ds_especialidade}\r\n ";


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
