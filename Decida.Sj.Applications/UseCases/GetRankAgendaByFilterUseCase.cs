using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Model;
using Decida.Sj.Core.Entities;
using FastMapper.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.UseCases
{
    public class GetRankAgendaByFilterUseCase : IGetRankAgendaByFilterUseCase
    {
        private readonly IAgendaOracleRepository _agenda;
        private readonly IHealthPlanMysqlRepository _healthPlan;
        private readonly IMSpecialtyMysqlRepository _mSpecialty;
        private readonly IMedicMysqlRepository _medic;

        public GetRankAgendaByFilterUseCase(IAgendaOracleRepository agenda,
            IHealthPlanMysqlRepository HealthPlan, IMSpecialtyMysqlRepository mSpecialty, IMedicMysqlRepository medic)
        {
            _agenda = agenda;
            _healthPlan = HealthPlan;
            _mSpecialty = mSpecialty;
            _medic = medic;
        }

        public async Task<(bool status, string list_agenda, string indexListJson)> Execute(int cd_convenio, int cd_especialidade, int? cd_pessoa_fisica_medico)
        {

            if (cd_convenio == 0)
                return (false, "", "");

            if (cd_especialidade == 0)
                return (false, "", "");

            var convenio = await _healthPlan.GetHelthPlanByID(cd_convenio);
            if (convenio == null)
                return (false, "Convênio não encontrado.", "");

            var especialidade = await _mSpecialty.GetMedSpecByIdRepsitory(cd_especialidade);
            if (especialidade == null)
                return (false, "Especialidade não encontrada.", "");

            if (cd_pessoa_fisica_medico > 0)
            {
                var medico = await _medic.GetMedByIdRepsitory(Convert.ToInt32(cd_pessoa_fisica_medico));
                if (medico != null)
                    cd_pessoa_fisica_medico = medico.cd_pessoa_fisica;
            }

            string list_agenda = "";
            string indexListJson = "[";
            try
            {
             
                var listAgendaRepo = await _agenda.GetAgendaByFIltersRepoAsync(convenio.cd_convenio, especialidade.cd_especialidade, cd_pessoa_fisica_medico);

                if (listAgendaRepo != null && listAgendaRepo.Count > 0)
                {
                    List<AgendaDTO> agenda = TypeAdapter.Adapt< List<AgendaEntity>,List<AgendaDTO>>(listAgendaRepo);


                    int id_agenda = 1;

                    foreach (var item in agenda)
                    {
                        if (id_agenda > 1)
                            indexListJson +=  ",";

                        list_agenda += $"{id_agenda} - {item.Dia} ({item.DiaSemana.Trim()}) às {item.Hora} \r\n ";
                        indexListJson += $"{{\"index\":{id_agenda},\"nr_sequencia\":{item.NrSequencia},\"cd_agenda\":{item.CdAgenda},\"cd_pessoa_fisica_medico\":{item.CdPessoaFisica},\"nm_pessoa_fisica_medico\":\"{item.NmPessoaFisica}\",\"dia\":\"{item.Dia}\",\"hora\":\"{item.Hora}\",\"DiaSemana\":\"{item.DiaSemana}\",\"ds_especialidade\":\"{item.DsEspecialidade}\" ,\"ds_convenio\":\"{convenio.cd_convenio}\"}}";
                        
                        id_agenda++;
                    }
                    indexListJson += "]";
                    string base64String = "";
                    try
                    {
                        base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(indexListJson));
                    }
                    catch (Exception)
                    {
                        return (false, "Erro ao codificar JSON.", "");
                    }


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

        }

    }
}
