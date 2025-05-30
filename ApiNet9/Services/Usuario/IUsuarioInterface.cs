using ApiNet9.Dto.Login;
using ApiNet9.Dto.usuario;
using ApiNet9.Models;

namespace ApiNet9.Services.Usuario
{
    public interface IUsuarioInterface
    {
        // Task<ResultViewModel> ListarUsuario(UsuariosQuery usuariosQuery);
        Task<ResultViewModel<List<UsuarioModel>>> ListarUsuario();

        Task<ResultViewModel<UsuarioModel>> BuscarUsuarioPorId(int id);

        Task<ResultViewModel<UsuarioModel>> RemoverUsuarioPorId(int id);

        Task<ResultViewModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto);

        Task<ResultViewModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto);

        Task<ResultViewModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto);
    }

}
