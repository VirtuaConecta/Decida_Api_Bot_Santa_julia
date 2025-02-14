using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.UseCases
{
    public interface IGetPacientDataUseCase
    {
        Task<(bool status, PacienteEntity pacient)> GetPacientData(string cpf);
    }
}
