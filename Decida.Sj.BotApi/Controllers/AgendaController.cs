using Decida.Sj.Applications.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decida.Sj.BotApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly IGetRankAgendaByFilterUseCase _rank;

        public AgendaController(IGetRankAgendaByFilterUseCase rank)
        {
            _rank = rank;
        }


        [HttpGet("{convenio}/{especialidade}/{cd_pessoa_fisica?}")]
        public async Task<IActionResult> BuscarAgenda(int convenio, int especialidade, int? cd_pessoa_fisica)
        {

            try
            {
                Console.WriteLine($"Id Convenio: {convenio} Id Especialidade: {especialidade} Cd_pessoa_fisica: {cd_pessoa_fisica} ");


                var (status, agendas,hash) = await _rank.Execute(convenio, especialidade, cd_pessoa_fisica);

                Console.WriteLine($"status: {status} agendas: {agendas}"); 
             
                if (status)
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
    }
}
