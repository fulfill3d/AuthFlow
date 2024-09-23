using Microsoft.Azure.Functions.Worker.Http;

namespace AuthFlow.Service.Interfaces
{
    public interface IAuthService
    {
        Task<bool> VerifyAndProcess(string code, bool update = false);
    }
}