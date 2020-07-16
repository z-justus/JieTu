using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace ViewModel.Base
{
    public class Utils
    {

        public static int SecondOfDay(DateTime dateTime) => dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;

        private static readonly string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string StartupValue = "Tools";
        public static void SetStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            key.SetValue(StartupValue, Process.GetCurrentProcess().MainModule.FileName + " -a 1");
        }
        public static bool HasStartupKey()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            return key.GetValue(StartupValue) != null;
        }
        public static void CancelStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            key.DeleteValue(StartupValue);
        }
        public static bool IsAutoRun()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            return key.GetValue(StartupValue) != null;
        }
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

    }
}
