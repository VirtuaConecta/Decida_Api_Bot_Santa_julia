using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.UseCases
{
    public  interface IGetRankAgendaByFilterUseCase
    {

        Task<(bool status, string list_agenda, string indexListJson)> Execute(int cd_convenio, int cd_especialidade, int? cd_pessoa_fisica);
    }
}
