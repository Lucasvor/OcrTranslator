using Microsoft.VisualBasic;
using OcrTranslator.Helpers.UI;
using OcrTranslator.Helpers;
using OcrTranslator.Keyboard;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using Application = System.Windows.Application;
using OcrTranslator.Settings;

namespace OcrTranslator;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application, IDisposable
{
    private KeyboardMonitor? keyboardMonitor;
    private EventMonitor? eventMonitor;
    private CancellationTokenSource NativeThreadCTS { get; set; }

    public App()
    {
        //Logger.InitializeLogger("\\TextExtractor\\Logs");

        try
        {
            string appLanguage = LanguageHelper.LoadLanguage();
            if (!string.IsNullOrEmpty(appLanguage))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(appLanguage);
            }
        }
        catch (CultureNotFoundException ex)
        {
            //Logger.LogError("CultureNotFoundException: " + ex.Message);
        }

        NativeThreadCTS = new CancellationTokenSource();

        NativeEventWaiter.WaitForEventLoop(
            "OcrTranslator",
            this.Shutdown,
            this.Dispatcher,
            NativeThreadCTS.Token);
    }
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var userSettings = new UserSettings(new Helpers.ThrottledActionInvoker());
        keyboardMonitor = new KeyboardMonitor(userSettings);
        keyboardMonitor?.Start();

    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        keyboardMonitor?.Dispose();
    }
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Dispose();
    }
}
