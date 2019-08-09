using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.Settings
{
    public interface ISettingsProvider
    {
        string GetSetting(string key);

        bool TryGetSetting(string key, out string value);

        string GetRequiredSetting(string key);
    }
}
