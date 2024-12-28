using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcrTranslator.Settings;

public class SettingsUtils : ISettingsUtils
{
    public const string DefaultFileName = "settings.json";
    private const string DefaultModuleName = "";
    private readonly IFile _file;
    private readonly ISettingsPath _settingsPath;

    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        MaxDepth = 0,
        IncludeFields = true,
    };

    public SettingsUtils()
            : this(new FileSystem())
    {
    }

    public SettingsUtils(IFileSystem fileSystem)
        : this(fileSystem?.File, new SettingPath(fileSystem?.Directory, fileSystem?.Path))
    {
    }
    public SettingsUtils(IFile file, ISettingsPath settingPath)
    {
        _file = file ?? throw new ArgumentNullException(nameof(file));
        _settingsPath = settingPath;
    }
    public void DeleteSettings(string powertoy = "")
    {
        throw new NotImplementedException();
    }

    public T GetSettings<T>(string powertoy = "", string fileName = "settings.json") where T : ISettingsConfig, new()
    {
        if (!SettingsExists(powertoy, fileName))
        {
            throw new FileNotFoundException();
        }

        // Given the file already exists, to deserialize the file and read its content.
        T deserializedSettings = GetFile<T>(powertoy, fileName);

        // If the file needs to be modified, to save the new configurations accordingly.
        if (deserializedSettings.UpgradeSettingsConfiguration())
        {
            SaveSettings(deserializedSettings.ToJsonString(), powertoy, fileName);
        }

        return deserializedSettings;
    }
    private T GetFile<T>(string powertoyFolderName = DefaultModuleName, string fileName = DefaultFileName)
    {
        // Adding Trim('\0') to overcome possible NTFS file corruption.
        // Look at issue https://github.com/microsoft/PowerToys/issues/6413 you'll see the file has a large sum of \0 to fill up a 4096 byte buffer for writing to disk
        // This, while not totally ideal, does work around the problem by trimming the end.
        // The file itself did write the content correctly but something is off with the actual end of the file, hence the 0x00 bug
        var jsonSettingsString = _file.ReadAllText(_settingsPath.GetSettingsPath(powertoyFolderName, fileName)).Trim('\0');

        var options = _serializerOptions;
        return JsonSerializer.Deserialize<T>(jsonSettingsString, options);
    }
    public string GetSettingsFilePath(string powertoy = "", string fileName = "settings.json")
    {
        throw new NotImplementedException();
    }

    public T GetSettingsOrDefault<T>(string powertoy = "", string fileName = "settings.json") where T : ISettingsConfig, new()
    {
        try
        {
            return GetSettings<T>(powertoy, fileName);
        }

        // Catch json deserialization exceptions when the file is corrupt and has an invalid json.
        // If there are any deserialization issues like in https://github.com/microsoft/PowerToys/issues/7500, log the error and create a new settings.json file.
        // This is different from the case where we have trailing zeros following a valid json file, which we have handled by trimming the trailing zeros.
        catch (JsonException ex)
        {
            //Logger.LogError($"Exception encountered while loading {powertoy} settings.", ex);
        }
        catch (FileNotFoundException)
        {
            //Logger.LogInfo($"Settings file {fileName} for {powertoy} was not found.");
        }

        // If the settings file does not exist or if the file is corrupt, to create a new object with default parameters and save it to a newly created settings file.
        T newSettingsItem = new T();
        SaveSettings(newSettingsItem.ToJsonString(), powertoy, fileName);
        return newSettingsItem;
    }

    public T GetSettingsOrDefault<T, T2>(string powertoy = "", string fileName = "settings.json", Func<object, object> settingsUpgrader = null)
        where T : ISettingsConfig, new()
        where T2 : ISettingsConfig, new()
    {
        throw new NotImplementedException();
    }

    public void SaveSettings(string jsonSettings, string powertoy = "", string fileName = "settings.json")
    {
        try
        {
            if (jsonSettings != null)
            {
                if (!_settingsPath.SettingsFolderExists(powertoy))
                {
                    _settingsPath.CreateSettingsFolder(powertoy);
                }

                _file.WriteAllText(_settingsPath.GetSettingsPath(powertoy, fileName), jsonSettings);
            }
        }
        catch (Exception e)
        {
            //Logger.LogError($"Exception encountered while saving {powertoy} settings.", e);
#if DEBUG
            if (e is ArgumentException || e is ArgumentNullException || e is PathTooLongException)
            {
                throw;
            }
#endif
        }
    }

    public bool SettingsExists(string powertoy = "", string fileName = "settings.json")
    {
        var settingsPath = _settingsPath.GetSettingsPath(powertoy, fileName);
        return _file.Exists(settingsPath);
    }
}
