using Microsoft.VisualBasic;
using OcrTranslator.Helpers;
using OcrTranslator.Helpers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Keyboard;

internal sealed class EventMonitor
{
    public EventMonitor(System.Windows.Threading.Dispatcher dispatcher, System.Threading.CancellationToken exitToken)
    {
        NativeEventWaiter.WaitForEventLoop("OcrTranslator", StartOCRSession, dispatcher, exitToken);
    }

    public void StartOCRSession()
    {
        if (!WindowUtilities.IsOCROverlayCreated())
        {
            WindowUtilities.LaunchOCROverlayOnEveryScreen();
        }
    }
}
