using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Models
{
    public class Tokens
    {
        [Key]
        public int TokenKey { get; set; }
        public string RelmId { get; set; }
        public string AuthorizationCode { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshToken_exp{ get; set; }
        public DateTime AccessToken_exp { get; set; }
    }
}
