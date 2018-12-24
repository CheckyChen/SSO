using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    public class LogonResponseBody
    {
        public string result { get; set; }
        public string data { get; set; }
        public string userPermission { get; set; }
    }
}
