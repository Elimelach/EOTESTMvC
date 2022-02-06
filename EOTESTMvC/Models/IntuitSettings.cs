using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Models
{
    public class IntuitSettings
    {
        public string Clientid { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public string Environment { get; set; }
    }
}
