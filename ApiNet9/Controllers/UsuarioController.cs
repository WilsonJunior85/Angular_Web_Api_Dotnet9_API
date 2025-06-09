using ApiNet9.Data;
using ApiNet9.Dto.usuario;
using ApiNet9.Models;
using ApiNet9.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ApiNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;
        public AppDbContext _context;

        public UsuarioController(IUsuarioInterface usuarioInterface, AppDbContext context )
        {
            _usuarioInterface = usuarioInterface;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult> ListarUsuarios()
        {
            var usuarios = await _usuarioInterface.ListarUsuario();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarUsuariosPorId(int id)
        {
            var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);
            return Ok(usuario);
        }

        [HttpPut]
        public async Task<ActionResult> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {
            var usuario = await _usuarioInterface.EditarUsuario(usuarioEdicaoDto);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoverUsuarios(int id)
        {
            var usuario = await _usuarioInterface.RemoverUsuarioPorId(id);
            return Ok(usuario);
        }

    }
}
