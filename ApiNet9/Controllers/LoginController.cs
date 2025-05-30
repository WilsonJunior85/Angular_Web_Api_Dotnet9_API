using ApiNet9.Dto.Login;
using ApiNet9.Dto.usuario;
using ApiNet9.Services.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;

        public LoginController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }

        [HttpPost("Register")]
        public async  Task<IActionResult> RegistrarUsuario (UsuarioCriacaoDto usuarioCriacaoDto)
        {
            var usuario = await _usuarioInterface.RegistrarUsuario(usuarioCriacaoDto);
            return Ok(usuario);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = await _usuarioInterface.Login(usuarioLoginDto);
            return Ok(usuario);
        }
    }
}
