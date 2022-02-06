using Intuit.Ipp.OAuth2PlatformClient;

namespace EOTESTMvC.Models
{
    public interface IIntuitFunc
    {
        OAuth2Client Auth2Client { get; }
    }
}