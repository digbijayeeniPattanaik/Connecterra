using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CowDto
    {
        public int CowId { get; set; }
        public string State { get; set; }
        public string Farm { get; set; }
    }
}
