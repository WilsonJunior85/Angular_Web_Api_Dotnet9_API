using ApiNet9.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
//using Microsoft.IdentityModel.Tokens;

namespace ApiNet9.Services.Senha
{
    public class SenhaService : ISenhaInterface
    {
        private readonly IConfiguration _config;

        public SenhaService(IConfiguration config)
        {
            _config = config;
        }



        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                //Chave aleatória
                senhaSalt = hmac.Key;
                //Pegando os bytes da senha e transformando em Hash
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }




        public string CriarToken(UsuarioModel usuario)
        {
            //Criando algumas Clains
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Email", usuario.Email),
                new Claim("Username", usuario.Usuario),
                new Claim("UserId", usuario.Id.ToString()),
            };

            //Chave
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            //Criando as credencias
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Alguma informações sobre o Token
            var token = new JwtSecurityToken(
                
                claims: claims,
                expires: DateTime.Now.AddDays(1),  // Vai ter 24 horas de validade
                signingCredentials: cred    // As credencias
                );


            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }





        public bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt)) 
            { 
                //Criptografando
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computedHash.SequenceEqual(senhaHash);
            }

        }
    }
}
