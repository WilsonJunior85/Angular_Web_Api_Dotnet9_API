using ApiNet9.Services.Auditoria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaInterface _auditoriaInterface;

        public AuditoriaController(IAuditoriaInterface auditoriaInterface)
        {
            _auditoriaInterface = auditoriaInterface;
        }




        [HttpGet("Auditorias")]
        public async Task<IActionResult> BuscarAuditorias()
        {

            var auditorias = await _auditoriaInterface.BuscarAuditorias();
            return Ok(auditorias);

        }
    }
}
