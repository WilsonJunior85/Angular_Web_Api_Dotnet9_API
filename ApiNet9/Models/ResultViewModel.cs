namespace ApiNet9.Models
{
    public class ResultViewModel<T>
    {
        public T? Data { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        
    }
}
