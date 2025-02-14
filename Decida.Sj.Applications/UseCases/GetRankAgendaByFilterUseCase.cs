using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.UseCases
{
    public class GetRankAgendaByFilterUseCase: IGetRankAgendaByFilterUseCase
    {
        private readonly IAgendaOracleRepository _agenda;

        public GetRankAgendaByFilterUseCase(IAgendaOracleRepository agenda)
        {
            _agenda = agenda;
        }

        public  async Task<(bool status, string list_agenda,string indexListJson)> Execute(int cd_convenio, int cd_especialidade,int? cd_pessoa_fisica) 
        { 
            string list_agenda = "";
            string indexListJson = "[";
            try
            {
                //buscar o nr do convenio e da especialidade


                var listAgendaRepo = await _agenda.GetAgendaByFIltersRepoAsync(cd_convenio, cd_especialidade, cd_pessoa_fisica);
            
                if (listAgendaRepo != null && listAgendaRepo.Count > 0)
                {
                    int id_agenda = 1;
                    foreach (var item in listAgendaRepo)
                    {
                        if (id_agenda > 1)
                            indexListJson = list_agenda + ",";

                    list_agenda += $"{id_agenda} - {item.Dia} ({item.DiaSemana.Trim()}) às {item.Hora} \r\n ";
                        indexListJson += $"{{'index':{id_agenda},'cd_agenda':{item.CdAgenda},'cd_pessoa_fisica':{item.CdPessoaFisica},'dia': {item.Dia},'hora':{item.Hora}}}";

                        id_agenda ++;
                    }
                    indexListJson = indexListJson+ "]";
                    string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(indexListJson));

                    // Encontrou
                    return (true, list_agenda, base64String);
                }
                else
                {
                    // Não encontrou
                    return (false, list_agenda, indexListJson);
                }

            }
            catch (Exception ex)
            {

                throw;
            }





            return (false, list_agenda, indexListJson);
        }

    }
}
