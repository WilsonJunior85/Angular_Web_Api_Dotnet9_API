using System.ComponentModel.DataAnnotations;

namespace ApiNet9.Dto.usuario
{
    public class UsuarioEdicaoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o Usuário")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Digite o Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o Sobrenome")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Digite o Email")]
        public string Email { get; set; }

       // public string Token { get; set; }

       // public DateTime DataCriacao { get; set; }

        //public DateTime DataAlteracao { get; set; }

        
    }
}
