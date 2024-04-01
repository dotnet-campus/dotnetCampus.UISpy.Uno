using dotnetCampus.Ipc.Pipes.PipeConnectors;

namespace UnoSpySnoopDebugger.Communications;

class UnoSpySnoopDebuggerIpcClientPipeConnector : IIpcClientPipeConnector
{
    public async Task<IpcClientNamedPipeConnectResult> ConnectNamedPipeAsync(IpcClientPipeConnectionContext ipcClientPipeConnectionContext)
    {
        try
        {
            Console.WriteLine($"[Ipc] Connect {ipcClientPipeConnectionContext.PeerName}");
            // With special timeout
            await ipcClientPipeConnectionContext.NamedPipeClientStream.ConnectAsync(TimeSpan.FromSeconds(1),
                ipcClientPipeConnectionContext.CancellationToken);
        }
        catch (TimeoutException e)
        {
            Console.WriteLine($"[Ipc] Connect TimeoutException {ipcClientPipeConnectionContext.PeerName}");
            return new IpcClientNamedPipeConnectResult(false, e.Message);
        }

        return new IpcClientNamedPipeConnectResult(true);
    }
}
