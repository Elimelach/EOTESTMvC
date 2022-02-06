using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Intuit.Ipp.OAuth2PlatformClient;
using EOTESTMvC.Models;

namespace EOTESTMvC.Controllers
{
    public class CallbackController : Controller
    {
        public CallbackController(IIntuitFunc intuitFunc)
        {
            Auth2Client = intuitFunc.Auth2Client;
        }

        public OAuth2Client Auth2Client { get; }

        public async Task<IActionResult> IndexAsync([FromQuery]string code="", [FromQuery]string realmId="")
        {
            await GetAuthTokensAsync(code, realmId);
            return RedirectToAction("Index","Home");
        }
        private async Task GetAuthTokensAsync(string code, string realmId)
        {
            if (!string.IsNullOrEmpty( code) )
            {
                Tokens tokens = new Tokens
                {
                    RelmId = realmId,
                    AuthorizationCode = code
                };
                var tokenResponse = await Auth2Client.GetBearerTokenAsync(code);
                if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                {
                    tokens.AccessToken = tokenResponse.AccessToken;
                    tokens.AccessToken_exp = DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn);
                    tokens.RefreshToken = tokenResponse.RefreshToken;
                    tokens.RefreshToken_exp = DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn);
                    IOMethods.WriteTokon(tokens);
                }
            }
        }
    }
}


