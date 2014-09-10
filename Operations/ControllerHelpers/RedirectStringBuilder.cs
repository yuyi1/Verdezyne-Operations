using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace Operations.ControllerHelpers
{
    public class RedirectStringBuilder : IRedirectStringBuilder
    {
        private RedirectStringParts _routeParts;

        public RedirectStringBuilder(RedirectStringParts routeParts)
        {
            _routeParts = routeParts;
        }

        public string GetRedirectString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_routeParts.BaseUrl);
            if (!_routeParts.BaseUrl.EndsWith("/"))
            {
                sb.Append("/");
            }
            sb.Append(_routeParts.Controller);
            if (!string.IsNullOrEmpty(_routeParts.Method))
            {
                sb.Append("/");
                sb.Append(_routeParts.Method);
            }
            if (!string.IsNullOrEmpty(_routeParts.RouteParam))
            {
                sb.Append("/");
                sb.Append(_routeParts.RouteParam);
            }

            return sb.ToString();
        }
    }
}