using Decida.Sj.Applications.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System.Xml.Linq;




namespace Decida.Sj.BotApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]


    public class MedicController : Controller
    {
        private readonly IGetMedicDataUseCase _plan;

        public MedicController(IGetMedicDataUseCase plan)
        {
            _plan = plan;
        }

        [HttpGet]
        public async Task<IActionResult> getMedic()
        {
           

            try
            {
                var(status, plan) = await _plan.GetMedicDataUseCaseList();
                // Se encontrou, retorna status 200 (OK)
                if (status)
                {
                    return Ok(new
                    {   
                        list = plan,
                        status = true
                    });
                }
                else
                {

                    Ok(new
                    {   
                        list= "nd",
                        status = false
                    });

                }
            }
            catch (Exception)
            {


            }


          return  BadRequest(new
            {
             
              list = "nd",
              status = false
          });
        }

        [HttpGet("list/{cd_especialidade}/{id_convenio}")]
        public async Task<IActionResult> getMedicByHelthPlanEnpecialty(int cd_especialidade,int id_convenio)
        {
            Console.WriteLine($"lista medicos especialida/convenio Id Especialidade: {cd_especialidade} Id Convenio: {id_convenio} ");

            try
            {
                var (status, plan) = await _plan.GetMedicDataListByHealthPlanEspecialtyUseCase(cd_especialidade, id_convenio);
                // Se encontrou, retorna status 200 (OK)
                if (status)
                {
                    return Ok(new
                    {
                        list = plan,
                        status = true
                    });
                }
                else
                {

                    Ok(new
                    {
                        list = "nd",
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
                status = false
            });
        }

    }
}