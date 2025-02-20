using Decida.Sj.Applications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.UseCases
{
    public interface IInsertNewAgendaToPacientUseCase
    {
        Task<(bool status, string message)> Execute(RequestAgendaDTO request);
    }
}
