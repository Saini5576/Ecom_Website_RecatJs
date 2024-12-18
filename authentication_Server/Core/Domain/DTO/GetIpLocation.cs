using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class GetIpLocation
    {
        public string city { get; set; }
        public string citylatlong { get; set; }
        public string country { get; set; }
    }
}
