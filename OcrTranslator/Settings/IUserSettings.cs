using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Settings;

public interface IUserSettings
{
    SettingItem<string> ActivationShortcut { get; }

    SettingItem<string> PreferredLanguage { get; }

    void SendSettingsTelemetry();
}
