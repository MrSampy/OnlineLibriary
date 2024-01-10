using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> BookIds { get; set; }
    }
}
