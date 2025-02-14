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

    public class GetMedicDataUseCase : IGetMedicDataUseCase
    {
       
        private readonly IMedicService _Medic;

        public GetMedicDataUseCase(IMedicService Medic)
        {

            _Medic = Medic;
        }

        public async Task<(bool status,string listMedic)> GetMedicDataUseCaseList()
        {
            try
            {
                string listMedic = "";
                var plan = await _Medic.GetMedicListAsyncService();

                if (plan != null && plan.Count > 0)
                {

                    foreach (var item in plan)
                    {
                        listMedic += $"{item.id_medico} - {item.nm_pessoa_fisica}\r\n ";


                    }
                    // Encontrou
                    return (true, listMedic);
                }
                else
                {
                    // Não encontrou
                    return (false, listMedic);
                }

            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }

        }
    }
}
