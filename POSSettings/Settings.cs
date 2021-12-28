using System.Collections.Generic;

namespace Branch.POSSettings
{
    public static class Settings
    {
        private static Dictionary<string, bool> _boolSettings;
        private static Dictionary<string, bool> DbSettings
        {
            get
            {
                if (_boolSettings is null)
                    _boolSettings = Data.GetSettings();
                return _boolSettings;
            }
        }
        public static bool TaxBeforeDiscount => DbSettings[SettingKeys.TaxBeforeDiscountKey];
        public static bool IncludeItemDiscountInOrderDiscount => DbSettings[SettingKeys.IncludeItemDiscountInOrderDiscountKey];
    }
}
