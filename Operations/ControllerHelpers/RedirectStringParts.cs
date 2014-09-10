using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.ControllerHelpers
{
    public class RedirectStringParts
    {
        public string BaseUrl { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string RouteParam { get; set; }
    }
}