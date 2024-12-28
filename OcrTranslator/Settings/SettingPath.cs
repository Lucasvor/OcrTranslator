using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Settings;

public class SettingPath : ISettingsPath
{
    private const string DefaultFileName = "settings.json";
    private readonly IDirectory _directory;

    private readonly IPath _path;
    public SettingPath(IDirectory directory, IPath path)
    {
        _directory = directory ?? throw new ArgumentNullException(nameof(directory));
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public bool SettingsFolderExists(string powertoy)
    {
        return _directory.Exists(System.IO.Path.Combine($"OcrTranslator\\{powertoy}"));
    }

    public void CreateSettingsFolder(string powertoy)
    {
        _directory.CreateDirectory(System.IO.Path.Combine($"OcrTranslator\\{powertoy}"));
    }

    public void DeleteSettings(string powertoy = "")
    {
        _directory.Delete(System.IO.Path.Combine($"OcrTranslator\\{powertoy}"));
    }

    /// <summary>
    /// Get path to the json settings file.
    /// </summary>
    /// <returns>string path.</returns>
    public string GetSettingsPath(string powertoy, string fileName = DefaultFileName)
    {
        if (string.IsNullOrWhiteSpace(powertoy))
        {
            return _path.Combine(
                $"OcrTranslator\\{fileName}");
        }

        return _path.Combine(
            $"OcrTranslator\\{powertoy}\\{fileName}");
    }
}
