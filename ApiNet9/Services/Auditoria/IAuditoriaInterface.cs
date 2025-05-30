using ApiNet9.Models;

namespace ApiNet9.Services.Auditoria
{
    public interface IAuditoriaInterface
    {
        Task RegistrarAuditoriaAsync(string acao, string usuarioId, string dadosAlterados);
        Task<List<AuditoriaModel>> BuscarAuditorias();
    }
}
