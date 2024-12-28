using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Models;

public class NullAsyncResult : IAsyncResult
{
    public object? AsyncState => null;

    public WaitHandle AsyncWaitHandle => new NullWaitHandle();

    public bool CompletedSynchronously => true;

    public bool IsCompleted => true;
}
