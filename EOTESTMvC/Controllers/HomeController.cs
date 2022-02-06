using EOTESTMvC.Models;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Controllers
{
   
    public class HomeController : Controller
    {
       

        public HomeController(IIntuitFunc intuitFunc)
        {
            Auth2Client = intuitFunc.Auth2Client;
        }
        
        public OAuth2Client Auth2Client { get; }

        public IActionResult Index()
        {
            ViewBag.isAuth = IOMethods.IsValid100Token().ToString();
            return View();
        }
        public IActionResult CancelAuth()
        {
            IOMethods.DeleteAuth();
            return RedirectToAction("Index");
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
            string authorizeUrl = Auth2Client.GetAuthorizationURL(scopes);
            return Redirect(authorizeUrl);
        }
        public IActionResult Reload()
        {
            HttpContext.Session.Remove("data");
            return RedirectToAction("Load");
        }

        public async Task<ActionResult> ApiCallServiceAsync()
        {
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
                    var refr = await Auth2Client.RefreshTokenAsync(token.RefreshToken);
                    token.AccessToken = refr.AccessToken;
                    token.AccessToken_exp = DateTime.Now.AddSeconds(refr.AccessTokenExpiresIn);
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
            }
        }


    }
}
