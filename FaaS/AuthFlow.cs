using System.Net;
using AuthFlow.Service.Interfaces;
using AuthFlow.Service.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace AuthFlow
{
    public class AuthFlow(
        IAuthService authService,
        IOptions<AuthServiceOptions> options)
    {
        private readonly string _redirectUriPostRegister = options.Value.PostRegisterRedirectUri;
        private readonly string _redirectUriPostUpdate = options.Value.PostUpdateRedirectUri;
        private readonly string _redirectUriUnauthorized = options.Value.UnauthorizedRedirectUri;
        
        [Function(nameof(PostRegister))]
        public async Task<HttpResponseData> PostRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "post-register")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();
            var code = req.Query["code"];
            
            if (code == null)
            {
                return RedirectResponse(response, _redirectUriUnauthorized);
            }

            var success = await authService.VerifyAndProcess(code);
            
            return success ? RedirectResponse(response, _redirectUriPostRegister) : RedirectResponse(response, _redirectUriUnauthorized);
        }

        [Function(nameof(PostUpdate))]
        public async Task<HttpResponseData> PostUpdate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "post-update")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();
            var code = req.Query["code"];
            
            if (code == null)
            {
                return RedirectResponse(response, _redirectUriUnauthorized);
            }

            var success = await authService.VerifyAndProcess(code, true);
            return success ? RedirectResponse(response, _redirectUriPostUpdate) : RedirectResponse(response, _redirectUriUnauthorized);
        }
        
        private static HttpResponseData RedirectResponse(HttpResponseData response, string redirectUri)
        {
            response.StatusCode = System.Net.HttpStatusCode.Redirect;
            response.Headers.Add("Location", redirectUri);
            return response;
        }
    }
}