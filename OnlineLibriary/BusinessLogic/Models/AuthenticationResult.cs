using BusinessLogic.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class AuthenticationResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public UserDTO User { get; set; }
    }
}
