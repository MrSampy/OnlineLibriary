using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class PaginationModel
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public override string ToString()
        {
            return $"{PageNumber}:{PageSize}";
        }
    }
}
