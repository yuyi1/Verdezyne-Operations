﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations.ControllerHelpers
{
    public interface IRedirectStringBuilderFactory
    {
        RedirectStringBuilder Create(RedirectStringParts routeParts);
    }
}
