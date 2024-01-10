using BusinessLogic.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IValidator<TModel> where TModel : class
    {
        public Task<ValidationResult> ValidateForAddAsync(TModel model, bool identityCheck = true);
        public Task<ValidationResult> ValidateIdAsync(int id);
        public Task<ValidationResult> ValidateForUpdateAsync(TModel model);
    }

}
