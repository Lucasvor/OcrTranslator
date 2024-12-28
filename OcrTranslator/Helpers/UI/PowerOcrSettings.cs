using OcrTranslator.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OcrTranslator.Helpers.UI;

public class PowerOcrSettings : ISettingsConfig
{
    public const string ModuleName = "OcrTranslator";

    // Gets or sets name of the powertoy module.
    [JsonPropertyName("name")]
    public string Name { get; set; }

    // Gets or sets the powertoys version.
    [JsonPropertyName("version")]
    public string Version { get; set; }

    // converts the current to a json string.
    public virtual string ToJsonString()
    {
        // By default JsonSerializer will only serialize the properties in the base class. This can be avoided by passing the object type (more details at https://stackoverflow.com/a/62498888)
        return JsonSerializer.Serialize(this, GetType());
    }

    public override int GetHashCode()
    {
        return ToJsonString().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        var settings = obj as PowerOcrSettings;
        return settings?.ToJsonString() == ToJsonString();
    }

    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    [JsonPropertyName("properties")]
    public PowerOcrProperties Properties { get; set; }

    public PowerOcrSettings()
    {
        Properties = new PowerOcrProperties();
        Version = "1";
        Name = ModuleName;
    }

    public virtual void Save(ISettingsUtils settingsUtils)
    {
        // Save settings to file
        var options = _serializerOptions;

        ArgumentNullException.ThrowIfNull(settingsUtils);

        settingsUtils.SaveSettings(JsonSerializer.Serialize(this, options), ModuleName);
    }

    public string GetModuleName()
        => Name;

    // This can be utilized in the future if the settings.json file is to be modified/deleted.
    public bool UpgradeSettingsConfiguration()
        => false;
}
