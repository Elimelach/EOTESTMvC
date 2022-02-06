using EOTESTMvC.Models;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EOTESTMvC.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        //public static string clientid = ConfigurationManager.AppSettings["clientid"];
        //public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        //public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        //public static string environment = ConfigurationManager.AppSettings["appEnvironment"];

        public HomeController(EoContext eoContext)
        {
            Context = eoContext;
        }
        public static OAuth2Client auth2Client = new OAuth2Client(
            "ABi5RL5Esr0HttVY1ZNQx0Ef5Jdfs1iYzizuIBhiCCOJuA0YVZ",
            "9rBPj6745Ekh0drSxg9YXsl28XBmFw6541fHjL4z",
            "https://localhost:5001/callback",
            "sandbox");

        public EoContext Context { get; }

        public IActionResult Index()
        {
            ViewBag.isAuth = IOMethods.IsValid100Token().ToString();
            return View();
        }
        public async Task<IActionResult> Load()
        {
            var data = HttpContext.Session.GetObject<CompanyInfo>("data");
            if (data == null)
            {
                return await ApiCallServiceAsync();
            }
            string output = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return View((object)output);
        }
        public ActionResult InitiateAuth()
        {

            List<OidcScopes> scopes = new List<OidcScopes>
            {
                OidcScopes.Accounting
            };
            string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
            return Redirect(authorizeUrl);
        }
        public IActionResult Reload()
        {
            HttpContext.Session.Remove("data");
            return RedirectToAction("Load");
        }

        public async Task<ActionResult> ApiCallServiceAsync()
        {
            //Tokens token = Context.Tokens.FirstOrDefault();
            Tokens token = IOMethods.ReadCodeFile();
            if (token?.AccessToken != null )
            {
                string realmId = token.RelmId;
                var date = DateTime.Now;

                string accses;
                if (token.AccessToken_exp > date)
                {
                    accses = token.AccessToken;

                }
                else if (token.RefreshToken_exp > date)
                {
                    var refr = await auth2Client.RefreshTokenAsync(token.RefreshToken);
                    token.AccessToken = refr.AccessToken;
                    token.AccessToken_exp = DateTime.Now.AddSeconds(refr.AccessTokenExpiresIn);
                    //Context.Tokens.Update(token);
                    //Context.SaveChanges();
                    IOMethods.WriteTokon(token);
                    accses = token.AccessToken;
                }
                else
                {
                    return InitiateAuth();
                }
                try
                {
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(accses);

                    // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    serviceContext.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/";
                    // Create a QuickBooks QueryService using ServiceContext
                    QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                    CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();
                    HttpContext.Session.SetObject("data", companyInfo);
                    return RedirectToAction("Load");
                }
                catch (Exception ex)
                {
                    return View("ApiCallService", (object)("QBO API call Failed!" + " Error message: " + ex.Message));
                }
            }
            else
            {
                return InitiateAuth();
                //return View("ApiCallService", (object)("QBO API call Failed!"));

            }




            ////string output = JsonConvert.SerializeObject(companyInfo, new JsonSerializerSettings
            ////{
            ////    NullValueHandling = NullValueHandling.Ignore
            ////});
            //return View("ApiCallService", (object)output);
        }


    }
}
