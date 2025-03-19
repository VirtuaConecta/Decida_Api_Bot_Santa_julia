using Decida.Sj.Core.Entities;
using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Repositories
{
    public interface IAgendaOracleRepository
    {
        Task<List<AgendaEntity>?> GetAgendaByFIltersRepoAsync(int id_convenio, int id_especialidade, int? cd_pessoa_fisica);
        Task<string> UpdateAppointmentRepoAsync(AgendaConsultaAgendarEntity agenda);
    }
}
