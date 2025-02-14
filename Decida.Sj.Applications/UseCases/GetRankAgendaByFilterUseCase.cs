using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Core.Entity;
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

        public async Task<(bool status, string list_agenda, string indexListJson)> Execute(int cd_convenio, int cd_especialidade, int? cd_pessoa_fisica)
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

            if (cd_pessoa_fisica > 0)
            {
                var medico = await _medic.GetMedByIdRepsitory(Convert.ToInt32(cd_pessoa_fisica));
                if (medico != null)
                 cd_pessoa_fisica = medico.cd_pessoa_fisica;
            }

            string list_agenda = "";
            string indexListJson = "[";
            try
            {
             
                var listAgendaRepo = await _agenda.GetAgendaByFIltersRepoAsync(convenio.cd_convenio, especialidade.cd_especialidade, cd_pessoa_fisica);

                if (listAgendaRepo != null && listAgendaRepo.Count > 0)
                {
                    int id_agenda = 1;
                    foreach (var item in listAgendaRepo)
                    {
                        if (id_agenda > 1)
                            indexListJson +=  ",";

                        list_agenda += $"{id_agenda} - {item.Dia} ({item.DiaSemana.Trim()}) às {item.Hora} \r\n ";
                        indexListJson += $"{{\"index\":{id_agenda},\"cd_agenda\":{item.CdAgenda},\"cd_pessoa_fisica\":{item.CdPessoaFisica},\"dia\":\"{item.Dia}\",\"hora\":\"{item.Hora}\"}}";

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
