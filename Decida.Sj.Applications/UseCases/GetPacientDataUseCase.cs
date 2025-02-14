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

    public class GetPacientDataUseCase : IGetPacientDataUseCase
    {
        private readonly IPacientsServices _pacient;

        public GetPacientDataUseCase(IPacientsServices pacient)
        {
            _pacient = pacient;
        }

        public async Task<(bool status,PacienteEntity pacient)> GetPacientData(string cpf)
        {
            var pacient = await _pacient.GetPacientByCpfService(cpf);
            
            if (pacient != null && pacient.PacienteId>0)
            {
                // Encontrou
                return (true, pacient);
            }
            else
            {
                // Não encontrou
                return (false, pacient);
            }
        }
    }
}
