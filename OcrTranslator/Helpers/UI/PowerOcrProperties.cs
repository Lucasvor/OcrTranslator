using OcrTranslator.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcrTranslator.Helpers.UI;

public class PowerOcrProperties
{
    [CmdConfigureIgnore]
    public HotkeySettings DefaultActivationShortcut => new HotkeySettings(true, false, false, true, 0x54); // Win+Shift+T

    public PowerOcrProperties()
    {
        ActivationShortcut = DefaultActivationShortcut;
        PreferredLanguage = string.Empty;
    }

    public HotkeySettings ActivationShortcut { get; set; }

    public string PreferredLanguage { get; set; }

    public override string ToString()
        => JsonSerializer.Serialize(this);
}
