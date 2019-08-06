using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.Auth
{
    public enum TokenValidationStatus
    {
        Error,
        Expired,
        NoToken,
        Success
    }
}
