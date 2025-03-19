 
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Model;
using Decida.Sj.Applications.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace Decida.Sj.BotApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly IGetRankAgendaByFilterUseCase _rank;
        private readonly UtilitiesService _utilitiesService;
        private readonly IInsertNewAgendaToPacientUseCase _insertNewAgendaToPacientUseCase;

        public AgendaController(IGetRankAgendaByFilterUseCase rank, UtilitiesService utilitiesService, IInsertNewAgendaToPacientUseCase insertNewAgendaToPacientUseCase)
        {
            _rank = rank;
            _utilitiesService = utilitiesService;
            _insertNewAgendaToPacientUseCase = insertNewAgendaToPacientUseCase;
        }


        [HttpGet("{convenio}/{especialidade}/{cd_pessoa_fisica?}")]
        public async Task<IActionResult> BuscarAgenda(int convenio, int especialidade, int? cd_pessoa_fisica)
        {

            try
            {
                Console.WriteLine($"Id Convenio: {convenio} Id Especialidade: {especialidade} Cd_pessoa_fisica: {cd_pessoa_fisica} ");


                var (status, agendas,hash) = await _rank.Execute(convenio, especialidade, cd_pessoa_fisica);

                Console.WriteLine($"status: {status} agendas: {agendas}"); 
             
                if (status && !string.IsNullOrEmpty(agendas))
                {
                    return Ok(new
                    {
                        list = agendas,
                        hash=hash,
                        status = true
                    });
                }
                else
                {

                    Ok(new
                    {
                        list = "nd",
                        hash = "nd",
                        status = false
                    });

                }
            }
            catch (Exception)
            {


            }


            return BadRequest(new
            {

                list = "nd",
                hash = "nd",
                status = false
            });
        }


        [HttpGet("validaHash/{id}/{hash}")]
        public async Task<IActionResult> ValidaEscolha(int id, string hash)
        {

            try
            {
                Console.WriteLine($"Id: {id} hash: {hash} ");


                var (status, agenda) = _utilitiesService.DecodeAgendaHash(id,hash);

                Console.WriteLine($"status: {status} agendas: {agenda}");

                if (status && !string.IsNullOrEmpty(agenda))
                {
                    return Ok(new
                    {
                        escolha = agenda,
                        
                        status = true
                    });
                }
                else
                {

                    Ok(new
                    {
                        escolha = "nd",
                       
                        status = false
                    });

                }
            }
            catch (Exception)
            {


            }


            return BadRequest(new
            {

                escolha = "nd",
               
                status = false
            });
        }

 
        [HttpPost("criaAgendamento")]
         public async Task<IActionResult> PostAgenda([FromBody] dynamic agenda )
        {
            try
            {
                var agendaStr = agenda.ToString();

                Console.WriteLine(agendaStr);

                RequestAgendaDTO dataAgenda = JsonConvert.DeserializeObject<RequestAgendaDTO> (agendaStr);


                var (status, message) = await _insertNewAgendaToPacientUseCase.Execute(dataAgenda);
                if (status && !string.IsNullOrEmpty(message))
                {
                    return Ok(new
                    {
                        mensagem = message,

                        status = true
                    });
                }
                else
                {
                    return BadRequest(new
                    {

                        mensagem = message,

                        status = false
                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {

                    mensagem = ex.Message,

                    status = false
                });
            }

        }

 
 





    }
}
