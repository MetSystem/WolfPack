using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Manager.Commands
{
    public class RestartConsoleCommand : ISystemCommand
    {       
        public const int WM_CLOSE = 0x10;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private readonly RestartConsoleInstruction _instruction;

        public RestartConsoleCommand(RestartConsoleInstruction instruction)
        {
            _instruction = instruction;
        }

        public void Execute()
        {
            try
            {
                Logger.Info("Locating wolfpack console, process id = {0}...", _instruction.ProcessId);
                var console = Process.GetProcessById(_instruction.ProcessId);

                Logger.Info("Process located...sending close message");
                SendMessage(console.MainWindowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                Logger.Info("Close message sent, waiting for process to exit");

                CloseApplication(console);
                StartApplication();
            }
            catch (Exception e)
            {
                Logger.Error(Logger.Event.During("RestartConsoleCommand.Execute").Encountered(e));
                throw;
            }
        }

        private void StartApplication()
        {
            Logger.Info("Starting wolfpack...");
            Process.Start(new ProcessStartInfo("wolfpack.agent.exe")
                              {
                                  CreateNoWindow = true
                              });
        }

        private static void CloseApplication(Process console)
        {
            var i = 0;

            while (!console.HasExited && i++ < 20)
            {
                Thread.Sleep(1000);
            }

            if (!console.HasExited)
                throw new InvalidOperationException(string.Format("Unable to close wolfpack process :("));
        }
    }
}