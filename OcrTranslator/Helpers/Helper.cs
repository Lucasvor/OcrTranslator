using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayoutMap = OcrTranslator.Helpers.LayoutMapManaged;

namespace OcrTranslator.Helpers
{
    public static class Helper
    {
        public static string GetKeyName(uint key)
        {
            return LayoutMap.GetKeyName(key);
        }
        public static uint GetKeyValue(string key)
        {
            return LayoutMap.GetKeyValue(key);
        }
    }
}
