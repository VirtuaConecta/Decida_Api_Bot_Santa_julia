using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Model;
using Decida.Sj.Core.Entities;
using FastMapper.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.UseCases
{

    public class GetPacientDataUseCase : IGetPacientDataUseCase
    {
        private readonly IPacientsServices _pacient;
        private readonly IHealthServices _healthServices;

        public GetPacientDataUseCase(IPacientsServices pacient,IHealthServices healthServices)
        {
            _pacient = pacient;
            _healthServices = healthServices;
        }

        public async Task<(bool status, PacientDTO pacient)> GetPacientData(string cpf)
        {
            var pacient = await _pacient.GetPacientByCpfService(cpf);
            
            if (pacient != null && pacient.PacienteId>0)
            {
                var convenioLocal = (await _healthServices.GetHealthPlanListAsyncService()).Where(x => x.cd_convenio == pacient.PacientIdCare).FirstOrDefault();

                PacientDTO pacientResponse = TypeAdapter.Adapt<PacienteEntity, PacientDTO>(pacient);
                pacientResponse.PacientIndexCare = convenioLocal.id_convenio;
                // Encontrou 
                return (true, pacientResponse);
            }
            else
            {
                // Não encontrou
                return (false, new PacientDTO());
            }
        }
    }
}
