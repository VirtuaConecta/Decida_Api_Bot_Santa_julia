using Decida.Sj.Applications.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System.Xml.Linq;




namespace Decida.Sj.BotApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]


    public class HealthController : Controller
    {
        private readonly IGetPlanDataByCompanyUseCase _plan;
        private readonly IGetHealthPlanDataUseCase _company;

        public HealthController(IGetPlanDataByCompanyUseCase plan, IGetHealthPlanDataUseCase company)
        {
            _plan = plan;
            _company = company;
        }

        [HttpGet]
        public async Task<IActionResult> getHealthPlan()
        {
           

            try
            {
                var(status, plan) = await _company.GetHealthPlanData();
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


        [HttpGet("getPlan/{id}")]
        public async Task<IActionResult> getPlan(int id)
        {
            Console.WriteLine($"busca lista de planos Id plano: {id}");

            try
            {
                var (status, plan) = await _plan.GetPlanDataList(id);
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