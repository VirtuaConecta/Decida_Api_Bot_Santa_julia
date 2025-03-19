using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Model;
using Decida.Sj.Applications.Services;
using Decida.Sj.Core.Entities;
using FastMapper;
using FastMapper.NetCore;
using System.ComponentModel.Design;

namespace Decida.Sj.Applications.UseCases
{
    public class InsertNewAgendaToPacientUseCase : IInsertNewAgendaToPacientUseCase
    {
        private readonly UtilitiesService _utilitiesService;
        private readonly IHealthPlanMysqlRepository _healthService;
        private readonly IAgendaOracleRepository _oracleRepository;

        public InsertNewAgendaToPacientUseCase(UtilitiesService utilitiesService, IHealthPlanMysqlRepository healthService, IAgendaOracleRepository oracleRepository)
        {
            _utilitiesService = utilitiesService;
            _healthService = healthService;
            _oracleRepository = oracleRepository;
        }


        public async Task<(bool status, string message)> Execute(RequestAgendaDTO request)
        {
            try
            {
                var validaVencCarteira = _utilitiesService.ValidarData(request.vencimentoCarteira);
                if(validaVencCarteira)
                {
                    var (status, agendaConfirmada) = _utilitiesService.DecodeAgendaoConfirmaHash(request.indexAgenda, request.hash);

                    var company = await _healthService.GetHelthPlanByIDRepoAsync(request.idConvenio);
                    if (company == null)
                        return (false, "Convenio não validado!");



                    var plan = (await _healthService.GetPlanListByCompanyRepoAsync(company.id_convenio)).Where(x=>x.Sequence== request.indexPlan).FirstOrDefault();
                    if (plan == null)
                        return (false, "Plano não validado!");

                    if (status)
                    {

                        AgendaConsultaAgendarEntity agenda = new AgendaConsultaAgendarEntity();
                        agenda.CdUsuarioConvenio = request.nrCarteira;
                        agenda.DtValidadeCarteira = Convert.ToDateTime(request.vencimentoCarteira);
                        agenda.cdPessoaFisicaPaciente = request.cdPessoaFisica;
                        agenda.CdPlano = plan.CdPlano;
                        agenda.NrSequencia= agendaConfirmada.NrSequencia;
                        agenda.Dia = agendaConfirmada.Dia;
                        agenda.Hora = agendaConfirmada.Hora;
                        agenda.CdUsuarioConvenio=request.nrCarteira;
                        agenda.CdCategoria =plan.CdCategoria;
                        agenda.CdAgenda = agendaConfirmada.CdAgenda;
                        agenda.CdConvenio=company.cd_convenio;

                        var respAtualiza = await _oracleRepository.UpdateAppointmentRepoAsync(agenda);

                        if (respAtualiza == "OK")
                            return (true, "Agenda atualizada com sucesso!");

                        return (false,"Ocorreu um erro ao atualizar a agenda!");
                    } 
                }
                else
                {
                    return (false, "Data carteira invalida");
                }
                return (false, "erro ao decodificar hash");
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }

        }




    }
}
