using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Messages { get; set; }
    }
}
