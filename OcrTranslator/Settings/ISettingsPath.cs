using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Settings;

public interface ISettingsPath
{
    bool SettingsFolderExists(string powertoy);

    void CreateSettingsFolder(string powertoy);

    void DeleteSettings(string powertoy = "");

    string GetSettingsPath(string powertoy, string fileName);
}
