using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Operations.ControllerHelpers
{
    public class RedirectStringBuilderFactory : IRedirectStringBuilderFactory
    {
        public RedirectStringBuilder Create(RedirectStringParts routeParts)
        {
            return new RedirectStringBuilder(routeParts);
        }
    }
}