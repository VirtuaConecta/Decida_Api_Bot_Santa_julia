using Decida.Sj.Applications.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System.Xml.Linq;




namespace Decida.Sj.BotApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]


    public class MedicalSpecialtyController : Controller
    {
        private readonly IGetMedicalSpecialtyDataUseCase _plan;

        public MedicalSpecialtyController(IGetMedicalSpecialtyDataUseCase plan)
        {
            _plan = plan;
        }

        [HttpGet("{id_convenio}")]
        public async Task<IActionResult> getMedicalSpecialty(int? id_convenio)
        {
           

            try
            {
                var(status, plan) = await _plan.GetMedicalSpecialtyDataUseCaseList(id_convenio);
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

    }
}