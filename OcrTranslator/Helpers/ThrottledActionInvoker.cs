using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OcrTranslator.Helpers;

[Export(typeof(IThrottledActionInvoker))]
public sealed class ThrottledActionInvoker : IThrottledActionInvoker
{
    private Lock _invokerLock = new Lock();
    private Action? _actionToRun;

    private DispatcherTimer _timer;

    public ThrottledActionInvoker()
    {
        _timer = new DispatcherTimer();
        _timer.Tick += Timer_Tick;
    }

    public void ScheduleAction(Action action, int milliseconds)
    {
        lock (_invokerLock)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }

            _actionToRun = action;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);

            _timer.Start();
        }
    }

    private void Timer_Tick(object? sender, EventArgs? e)
    {
        lock (_invokerLock)
        {
            _timer.Stop();
            _actionToRun?.Invoke();
        }
    }
}

