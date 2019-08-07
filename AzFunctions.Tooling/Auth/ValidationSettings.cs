using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.Auth
{
    public class ValidationSettings
    {
        public string StsHost { get; set; }

        public IEnumerable<string> ValidAudiences { get; set; }

        public string TenantDomain { get; set; }

        public string TenantId { get; set; }

        public string Policy { get; set; }
    }
}
