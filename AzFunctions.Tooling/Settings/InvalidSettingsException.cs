using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.Settings
{
    public class InvalidSettingsException : Exception
    {
        public InvalidSettingsException(string message) : base(message)
        { }
    }
}
