using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using dotnetCampus.Ipc.Context;
using dotnetCampus.Ipc.Exceptions;
using dotnetCampus.Ipc.IpcRouteds.DirectRouteds;
using dotnetCampus.Ipc.Pipes;
using dotnetCampus.Ipc.Threading;
using Microsoft.UI.Xaml.Data;
using UnoSpySnoopDebugger.Communications;
using UnoSpySnoopDebugger.IpcCommunicationContext;
using UnoSpySnoopDebugger.Models;
using UnoSpySnoopDebugger.View;

namespace UnoSpySnoopDebugger;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();

        var currentProcess = Process.GetCurrentProcess();
        var name = $"UnoSpySnoopDebugger_{currentProcess.ProcessName}_{currentProcess.Id}";
        var ipcProvider = new IpcProvider(name, new IpcConfiguration()
        {
            IpcTaskScheduling = IpcTaskScheduling.LocalOneByOne,
            IpcClientPipeConnector = new UnoSpySnoopDebuggerIpcClientPipeConnector(),
        });
        var jsonIpcDirectRoutedProvider = new JsonIpcDirectRoutedProvider(ipcProvider);
        IpcProvider = jsonIpcDirectRoutedProvider;

        _ = RefreshProcessInfoList();

#if HAS_UNO
        UnoSpySnoop.SpySnoop.StartSpyUI(SnoopRootGrid);
#endif
    }

    public ObservableCollection<CandidateDebugProcessInfo> ProcessInfoList { get; } =
        new ObservableCollection<CandidateDebugProcessInfo>();

    private async Task RefreshProcessInfoList()
    {
        ProcessInfoList.Clear();

        var processes = Process.GetProcesses().ToList();

#if DEBUG
        var currentProcess = Process.GetCurrentProcess();
        var otherInstance =
            processes.FirstOrDefault(p => p.Id != currentProcess.Id && p.ProcessName == currentProcess.ProcessName);
        if (otherInstance != null)
        {
            processes.Remove(otherInstance);
            processes.Insert(0, otherInstance);
        }

#if !MACCATALYST
        Window.Current.Title = $"UnoSpySnoopDebugger - PID:{currentProcess.Id}";
#endif

#endif

        HashSet<string> ignoreProcessNameHashSet;
        if (OperatingSystem.IsLinux())
        {
            ignoreProcessNameHashSet = new HashSet<string>()
            {
                "(sd-pam)",
                "ModemManager",
                "NetworkManager",
                "Xorg",
                "accounts_daemon",
                "acpi_thermal_pm",
                "agent",
                "at-spi-bus-launcher",
                "ata_sff",
                "bamfdaemon",
                "bash",
                "cron",
                "cryptd",
                "cupsd",
                "dbus-daemon",
                "kwin_x11",
                "kwin_no_scale",
                "sh",
                "ssh",
                "sshd",
                "su",
                "sudo",
                "systemd",
            };
        }
        else if (OperatingSystem.IsWindows())
        {
            ignoreProcessNameHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "cmd",
                "conhost",
                "csrss",
                "ctfmon",
                "dllhost",
                "dwm",
                "explorer",
                "GameBar",
                "OpenConsole",
                "RuntimeBroker",
                "SearchHost",
                "SearchIndexer",
                "svchost",
                "Taskmgr",
            };
        }
        else
        {
            ignoreProcessNameHashSet = new HashSet<string>();
        }


        await Parallel.ForEachAsync(processes.Where(t => !CanIgnore(t)), async (process, _) =>
        {
            await PeekProcess(process);
            process.Dispose();
        });
        return;

        bool CanIgnore(Process process)
        {
            if (ignoreProcessNameHashSet.Contains(process.ProcessName))
            {
                return true;
            }

            if (OperatingSystem.IsLinux())
            {
                if (Regex.IsMatch(process.ProcessName, @"cpuhp/\d+"))
                {
                    return true;
                }

                if (Regex.IsMatch(process.ProcessName, @"idle_inject/\d+"))
                {
                    return true;
                }

                if (Regex.IsMatch(process.ProcessName, @"ksoftirqd/\d+"))
                {
                    return true;
                }

                if (Regex.IsMatch(process.ProcessName, @"kworker/\d+\:\d"))
                {
                    return true;
                }
            }

            return false;
        }
    }

    private async Task PeekProcess(Process process)
    {
        var peerName = DebugIpcPeerNameGenerator.GetPeerNameFromProcess(process);

        try
        {
            JsonIpcDirectRoutedClientProxy client =
                await IpcProvider.GetAndConnectClientAsync(peerName);
            var response = await client.GetResponseAsync<HelloResponse>(RoutedPathList.Hello);
            if (response is null)
            {
                return;
            }

            if (response.SnoopVersionText != VersionInfo.VersionText)
            {
                return;
            }

            var info = new CandidateDebugProcessInfo()
            {
                Client = client,
                CommandLine = response.CommandLine,
                ProcessId = response.ProcessId.ToString(),
                ProcessName = response.ProcessName,
            };

            DispatcherQueue.TryEnqueue(() => { ProcessInfoList.Add(info); });
        }
        catch (IpcClientPipeConnectionException e)
        {
            // Connection Fail
            Console.WriteLine($"Connection Fail {peerName}");
        }
        catch (PlatformNotSupportedException e)
        {
            Console.WriteLine($"PlatformNotSupportedException {peerName}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public JsonIpcDirectRoutedProvider IpcProvider { get; set; }

    private async void StartDebugButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ProcessInfoListView.SelectedItem is CandidateDebugProcessInfo info)
        {
            if (info.Client is null)
            {
                return;
            }

#if !MACCATALYST
            Window.Current.Title = $"UnoSpySnoopDebugger - Debugging {info.ProcessName} PID:{info.ProcessId}";
#endif

            var snoopUserControl = new SnoopUserControl(info.Client);
            await snoopUserControl.StartAsync();

            RootGrid.Children.Clear();
            RootGrid.RowDefinitions.Clear();
            RootGrid.Children.Add(snoopUserControl);
        }
    }

    private void RefreshProcessInfoListButton_OnClick(object sender, RoutedEventArgs e)
    {
        _ = RefreshProcessInfoList();
    }
}

public class NotNullToIsEnableConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        return value is not null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
