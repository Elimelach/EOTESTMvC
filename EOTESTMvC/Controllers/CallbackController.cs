using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intuit.Ipp.OAuth2PlatformClient;
using System.Security.Claims;
using System.Web;
using System.Configuration;
using System.Net;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using EOTESTMvC.Models;

namespace EOTESTMvC.Controllers
{
    public class CallbackController : Controller
    {
        public CallbackController(EoContext eocontext)
        {
            Context = eocontext;
        }

        public EoContext Context { get; }

        public async Task<IActionResult> IndexAsync([FromQuery]string code="", [FromQuery]string realmId="")
        {
            await GetAuthTokensAsync(code, realmId);
            return RedirectToAction("Index","Home");
        }
        private async System.Threading.Tasks.Task GetAuthTokensAsync(string code, string realmId)
        {
            if (!String.IsNullOrEmpty( code) )
            {
                Tokens tokens = new Tokens
                {
                    RelmId = realmId,
                    AuthorizationCode = code
                };
                var tokenResponse = await HomeController.auth2Client.GetBearerTokenAsync(code);
                if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                {
                    tokens.AccessToken = tokenResponse.AccessToken;
                    tokens.AccessToken_exp = DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn);
                    tokens.RefreshToken = tokenResponse.RefreshToken;
                    tokens.RefreshToken_exp = DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn);
                    //var t= Context.Tokens.FirstOrDefault();
                    //if (t!= null)
                    //{
                    //    Context.Tokens.Remove(t); 
                    //}
                    //Context.Tokens.Add(tokens);
                    //Context.SaveChanges();
                    //return;
                    IOMethods.WriteTokon(tokens);
                }


            }

           // Request.GetOwinContext().Authentication.SignOut("TempState");

           // var claims = new List<Claim>();

           // if (HttpContext.Session.GetString("realmId") != null)
           // {
           //     claims.Add(new Claim("realmId", HttpContext.Session.GetString("realmId").ToString()));
           // }

           // if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
           // {
           //     claims.Add(new Claim("access_token", tokenResponse.AccessToken));
           //     claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn)).ToString()));
           // }

           // if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
           // {
           //     claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
           //     claims.Add(new Claim("refresh_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn)).ToString()));
           // }

           // var id = new ClaimsIdentity(claims, "Cookies");
           //// Request.GetOwinContext().Authentication.SignIn(id);
        }
    }
}


//namespace MvcCodeFlowClientManual.Controllers
//{
//    public class AppController : Controller
//    {
        


//        /// <summary>
//        /// Use the Index page of App controller to get all endpoints from discovery url
//        /// </summary>
//        //public ActionResult Index()
//        //{
//        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//        //    Session.Clear();
//        //    Session.Abandon();
//        //    Request.GetOwinContext().Authentication.SignOut("Cookies");
//        //    return View();
//        //}

//        /// <summary>
//        /// Start Auth flow
//        /// </summary>
//        //public ActionResult InitiateAuth(string submitButton)
//        //{
//        //    switch (submitButton)
//        //    {
//        //        case "Connect to QuickBooks":
//        //            List<OidcScopes> scopes = new List<OidcScopes>();
//        //            scopes.Add(OidcScopes.Accounting);
//        //            string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
//        //            return Redirect(authorizeUrl);
//        //        default:
//        //            return (View());
//        //    }
//        //}

//        /// <summary>
//        /// QBO API Request
//        /// </summary>
       

       

//        /// <summary>
//        /// Action that takes redirection from Callback URL
//        /// </summary>
//        //public ActionResult Tokens()
//        //{
//        //    return View("Tokens");
//        //}
//    }
//}

//namespace MvcCodeFlowClientManual.Controllers
//{
//    public class CallbackController : Controller
//    {
//        /// <summary>
//        /// Code and realmid/company id recieved on Index page after redirect is complete from Authorization url
//        /// </summary>
//        public async Task<ActionResult> Index()
//        {
//            //Sync the state info and update if it is not the same
//            var state = Request.QueryString["state"];
//            if (state.Equals(AppController.auth2Client.CSRFToken, StringComparison.Ordinal))
//            {
//                ViewBag.State = state + " (valid)";
//            }
//            else
//            {
//                ViewBag.State = state + " (invalid)";
//            }

//            string code = Request.QueryString["code"] ?? "none";
//            string realmId = Request.QueryString["realmId"] ?? "none";
//            await GetAuthTokensAsync(code, realmId);

//            ViewBag.Error = Request.QueryString["error"] ?? "none";

//            return RedirectToAction("Tokens", "App");
//        }

//        /// <summary>
//        /// Exchange Auth code with Auth Access and Refresh tokens and add them to Claim list
//        /// </summary>
       
//    }
//}