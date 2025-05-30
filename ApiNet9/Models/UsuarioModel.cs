using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNet9.Models
{
    
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public byte[] SenhaHash { get; set; }  //Criptografia da Senha
        public byte[] SenhaSalt { get; set; }  //Chave utilizada para descriptografar a senha
    }
}
