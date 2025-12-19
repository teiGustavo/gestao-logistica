namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(string apelido, string senha);
}