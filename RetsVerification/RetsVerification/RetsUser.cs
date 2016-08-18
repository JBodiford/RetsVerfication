using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RetsVerification
{
    //json object parsing

    public class RootObject
    {
        public RetsUser[] RetsUser { get; set; }
    }
    public class RetsUser
    {
        public int TenantID { get; set; }
        public int BoardID { get; set; }
        public string RetsAssociatedUser { get; set; }
        public string RetsLoginURL { get; set; }
        public string RetsUserID { get; set; }
        public string RetsPassword { get; set; }
        public string RetsUserAgent { get; set; }
        public string RetsUserAgentPassword { get; set; }
        public double RetsVersion { get; set; }
        public string QueryString { get; set; }
    }
}
