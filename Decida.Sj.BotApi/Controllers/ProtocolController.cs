using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decida.Sj.BotApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProtocolController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetProtocol()
        {
            string generatedProtocol = GenerateProtocol();
            return Ok(new
            {
                protocol= generatedProtocol,
                status = true
            });
        }
 

    private string GenerateProtocol()
    {
            // Obtém a data no formato YYYYMMDD
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

            // Gera uma sequência numérica única de 5 dígitos
            string uniquePart = new Random().Next(10000, 99999).ToString();

            // Combina no formato desejado
            return $"{datePart}S{uniquePart}";
        }

    }
}
