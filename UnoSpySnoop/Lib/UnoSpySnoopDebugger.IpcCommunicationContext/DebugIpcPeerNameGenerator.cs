using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSpySnoopDebugger.IpcCommunicationContext;

public static class DebugIpcPeerNameGenerator
{
    public static string GetPeerNameFromProcess(Process process)
    {
        return $"UnoSpySnoop_{process.ProcessName.Replace('/', '_')}_{process.Id}";
    }
}
