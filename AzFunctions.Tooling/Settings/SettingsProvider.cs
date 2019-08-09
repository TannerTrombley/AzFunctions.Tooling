using System;


namespace AzFunctions.Tooling.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetRequiredSetting(string key)
        {
            var setting = GetSetting(key);
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new InvalidSettingsException($"The setting foe key: {key} does not exist");
            }

            return setting;
        }

        public string GetSetting(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }

        public bool TryGetSetting(string key, out string value)
        {
            value = null;
            try
            {
                value = System.Environment.GetEnvironmentVariable(key);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
