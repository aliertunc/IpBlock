using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpBlock.Model
{
    public class IPDetail
    {
        public string IPAddress { get; set; }
        public DateTime Time { get; set; }
        public int Count { get; set; }
    }
}
