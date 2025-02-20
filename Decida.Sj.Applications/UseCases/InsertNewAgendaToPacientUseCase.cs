using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Model;
using Decida.Sj.Applications.Services;
using Decida.Sj.Core.Entities;
using FastMapper;
using FastMapper.NetCore;

namespace Decida.Sj.Applications.UseCases
{
    public class InsertNewAgendaToPacientUseCase : IInsertNewAgendaToPacientUseCase
    {
        private readonly UtilitiesService _utilitiesService;

        public InsertNewAgendaToPacientUseCase(UtilitiesService utilitiesService)
        {
            _utilitiesService = utilitiesService;
        }


        public async Task<(bool status, string message)> Execute(RequestAgendaDTO request)
        {
            try
            {
                var validaVencCarteira = _utilitiesService.ValidarData(request.vencimentoCarteira);
                if(validaVencCarteira)
                {var (status, agendaConfirmada) = _utilitiesService.DecodeAgendaoConfirmaHash(request.idAgenda, request.hash);

                    if (status)
                    {

                        AgendaConsultaAgendarEntity agenda = new AgendaConsultaAgendarEntity();
                        agenda.CdUsuarioConvenio = request.nrCarteira;
                        agenda.DtValidadeCarteira = Convert.ToDateTime(request.vencimentoCarteira);
                    


                    return (true, "sucesso");
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
