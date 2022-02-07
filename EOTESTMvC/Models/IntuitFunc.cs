using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Options;

namespace EOTESTMvC.Models
{
    public class IntuitFunc : IIntuitFunc
    {
        private readonly IntuitSettings _intitSett;

        public OAuth2Client Auth2Client { get; }

        public IntuitFunc(IOptions<IntuitSettings> sett)
        {
            _intitSett = sett.Value;
            Auth2Client = new OAuth2Client(
            _intitSett.Clientid,
            _intitSett.ClientSecret,
            _intitSett.RedirectUrl,
            _intitSett.AppEnvironment);
        }
    }
}
