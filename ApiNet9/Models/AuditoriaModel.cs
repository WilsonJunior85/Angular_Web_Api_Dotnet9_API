namespace ApiNet9.Models
{
    public class AuditoriaModel
    {
        public int Id { get; set; }  

        public string Acao { get; set; }  // Foi alteração? foi remoção?

        public DateTime Data { get; set; } = DateTime.Now;    //Quando isso aconteceu?

        public string UsuarioId { get; set; }  //Quem fez essa alteração?

        public string DadosAlterados { get; set; }  //Dados Salvos em forma de json

    }
}
