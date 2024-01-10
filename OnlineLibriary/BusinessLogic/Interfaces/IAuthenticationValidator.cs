using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAuthenticationValidator
    {
        public Task<ValidationResult> ValidateForSignInAsync(UserCredentials userCredentials);
    }
}
