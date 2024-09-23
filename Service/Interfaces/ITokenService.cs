using Newtonsoft.Json.Linq;

namespace AuthFlow.Service.Interfaces
{
    public interface ITokenService
    {
        Task<JObject?> ExchangeCodeForTokenAsync(string code, bool update = false);
    }
}