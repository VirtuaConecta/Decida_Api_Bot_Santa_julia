using Decida.Sj.Applications.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;




namespace Decida.Sj.BotApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]


    public class PacientController : Controller
    {
        private readonly IGetPacientDataUseCase _pacient;

        public PacientController(IGetPacientDataUseCase pacient)
        {
            _pacient = pacient;
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> getCpf(string cpf)
        {
            var (status, pacient) = await _pacient.GetPacientData(cpf);

            try
            {

                // Se encontrou, retorna status 200 (OK)
                if (status)
                {
                    return Ok(new
                    {   id= pacient.PacienteId,
                        name= pacient.PacientName,
                        convenio=pacient.PacientCare,
                        id_convenio = pacient.PacientIndexCare,
                        status = true
                    });
                }
                else
                {

                    Ok(new
                    {   id=0,
                        name= "nd",
                        convenio= "nd",
                        id_convenio=0,
                        status = false
                    });

                }
            }
            catch (Exception)
            {


            }


          return  BadRequest(new
            {
              id = 0,
              name = "nd",
              convenio = "nd",
              id_convenio = 0,
              status = false
          });
        }

    }
}