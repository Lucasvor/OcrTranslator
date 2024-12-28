using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Helpers;
public interface IThrottledActionInvoker
{
    void ScheduleAction(Action action, int milliseconds);
}