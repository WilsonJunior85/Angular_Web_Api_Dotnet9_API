using ApiNet9.Data;
using ApiNet9.Dto.Login;
using ApiNet9.Dto.usuario;
using ApiNet9.Models;
using ApiNet9.Services.Auditoria;
using ApiNet9.Services.Senha;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace ApiNet9.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        private readonly IMapper _mapper;
        private readonly IAuditoriaInterface _auditoriaInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(AppDbContext context, ISenhaInterface senhaInterface , IMapper mapper, IAuditoriaInterface auditoriaInterface, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _mapper = mapper;
            _auditoriaInterface = auditoriaInterface;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<ResultViewModel<UsuarioModel>> BuscarUsuarioPorId(int id)
        {
            ResultViewModel<UsuarioModel> result = new ResultViewModel<UsuarioModel>();

            try
            {
                var usuarios = await _context.Usuarios.FindAsync(id);

                if(usuarios == null)
                {
                    result.Mensagem = "Usuário não localizado";
                    return result;
                }

                result.Data = usuarios;
                result.Mensagem = "Usuário localizado";
                return result;

            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;
            }
        }


        public async Task<ResultViewModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {
            ResultViewModel<UsuarioModel> result = new ResultViewModel<UsuarioModel>();

            try
            {
                UsuarioModel usuarioBanco = await _context.Usuarios.FindAsync(usuarioEdicaoDto.Id);

                if(usuarioBanco == null)
                {
                    result.Mensagem = "Usuário não localizado!";
                    return result;
                }

                //Dados antes de salvar o usuário
                var dadosAntes = JsonConvert.SerializeObject(usuarioBanco);



                usuarioBanco.Nome = usuarioEdicaoDto.Nome;
                usuarioBanco.Sobrenome = usuarioEdicaoDto.Sobrenome;
                usuarioBanco.Email = usuarioEdicaoDto.Email;
                usuarioBanco.Usuario = usuarioEdicaoDto.Usuario;
                usuarioBanco.DataAlteracao = DateTime.Now;

                _context.Update(usuarioBanco);
                await _context.SaveChangesAsync();

                result.Mensagem = "Usuário editado com sucesso!";
                result.Data = usuarioBanco;

                //Depois ja com os dados novos
                var dadosDepois = JsonConvert.SerializeObject(usuarioBanco);

                //Pegar o usuario id
                var usuarioId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                await _auditoriaInterface.RegistrarAuditoriaAsync("Atualização", usuarioId, $"Antes: {dadosAntes}, Depois {dadosDepois}");

                return result;

            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;


            }
        }


        public async Task<ResultViewModel<List<UsuarioModel>>> ListarUsuario()
        {
            ResultViewModel<List<UsuarioModel>> result = new ResultViewModel<List<UsuarioModel>>();

            try
            {
                //Criando variável usuarios, entrando dentro do banco, dentro da tabela Usuários e transformando em lista.
                var usuarios =  await _context.Usuarios.ToListAsync();

                if (usuarios.Count() == 0)
                {
                    result.Mensagem = "Nenhum usuário encontrado";
                    return result;
                }
                
                
                    //Jogo dentro dos meus dados as informações do usuário.
                    result.Data = usuarios;
                    result.Mensagem = "Usuários localizados com sucesso";
                    return result;
                
                  
            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;

            }
        }

        public async Task<ResultViewModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResultViewModel<UsuarioModel> result = new ResultViewModel<UsuarioModel>();

            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(userBanco => userBanco.Email == usuarioLoginDto.Email);

                if(usuario == null)
                {
                    result.Mensagem = "Credenciais inválidas!";
                    return result;
                }

                if (!_senhaInterface.VerificaSenhaHash(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt)) 
                {
                    result.Mensagem = "Credenciais inválidas!";
                    return result;
                }

                var token = _senhaInterface.CriarToken(usuario);

                usuario.Token = token;

                //Gravar no banco o Token
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                result.Data = usuario;
                result.Mensagem = "Usuário logado com sucesso!";

                return result;
            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;
            }
        }






        public async Task<ResultViewModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            ResultViewModel<UsuarioModel> result = new ResultViewModel<UsuarioModel>();

            try
            {
                if (!VerificaSeExisteEmailUsuarioRepetidos(usuarioCriacaoDto))
                {
                    result.Mensagem = "Usuário já cadastrado!";
                    return result;
                }

                //Criação da senha Hash
                _senhaInterface.CriarSenhaHash(usuarioCriacaoDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                
                UsuarioModel usuario = _mapper.Map<UsuarioModel>(usuarioCriacaoDto);
                usuario.SenhaHash = senhaHash;
                usuario.SenhaSalt = senhaSalt;
                usuario.DataCriacao = DateTime.Now;
               // usuario.DataAlteracao = DateTime.Now;

                //Criando, entrando dentro do banco e adicionando o usuario
                _context.Add(usuario);
                //Salvando os dados dentro do banco
                await _context.SaveChangesAsync();

                result.Mensagem = "Usuário cadastrado com sucesso!";
                result.Data = usuario;

                return result;

            }
           catch (Exception ex) 
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;
            }
        }



        public async Task<ResultViewModel<UsuarioModel>> RemoverUsuarioPorId(int id)
        {
            ResultViewModel<UsuarioModel> result = new ResultViewModel<UsuarioModel>();

            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if(usuario == null)
                {
                    result.Mensagem = "Nenhum usuário encontrado";
                    return result;
                }

               // result.Data = usuario;
                _context.Remove(usuario);
                await _context.SaveChangesAsync();

                result.Mensagem = $"Usuário {usuario.Nome} removido com sucesso!";
                result.Data = usuario;





                //Depois ja com os dados novos
                var dadosAntes = JsonConvert.SerializeObject(usuario);

                //Pegar o usuario id
                var usuarioId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                await _auditoriaInterface.RegistrarAuditoriaAsync("Remoção", usuarioId, $"Antes: {dadosAntes}");






                return result;


            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Status = false;
                return result;

            }
        }



        private bool VerificaSeExisteEmailUsuarioRepetidos(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(item => item.Email == usuarioCriacaoDto.Email || item.Usuario == usuarioCriacaoDto.Usuario);

            if( usuario != null)
            {
                return false;
            }

            return true;
        } 
    }
}
