using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Models
{
    public class LogViewModel
    {
        [Required(ErrorMessage = "Please enter Email.")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
