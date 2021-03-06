﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleScreenCapture
{
    class Program
    {
        private const int DefaultScreenIndex = 0;
        private const SaveMode DefaultSaveMode = SaveMode.Clipboard;
        private static readonly string DefaultDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private const string DefaultFileName = "screenshot.png";
        private static readonly ImageFormat DefaultImageFormat = ImageFormat.Png;

        private enum SaveMode
        {
            Clipboard,
            File
        }

        static void Main(string[] args)
        {
            var arguments = ParseCommandLineArguments(args);

            int screenIndex;
            if (!arguments.ContainsKey("screen-index")) screenIndex = DefaultScreenIndex;
            else if (!(int.TryParse(arguments["screen-index"], out screenIndex)) ||
                     !(screenIndex < Screen.AllScreens.Length))
            {
                string message = "Invalid Command-Line Argument (screen-index)\n";
                message += "Example : --screen-index=0";
                MessageBox.Show(message);
                return;
            }

            SaveMode saveMode;
            if (!arguments.ContainsKey("save-mode")) saveMode = DefaultSaveMode;
            else if (!Enum.TryParse(arguments["save-mode"], out saveMode))
            {
                string message = "Invalid Command-Line Argument (save-mode)\n";
                message += "0 : Save to Clipboard, 1 : Save as File\n";
                message += "Example 1 (Clipboard) : --save-mode=0\n";
                message += "Example 2 (File) : --save-mode=1";
                MessageBox.Show(message);
                return;
            }

            string dirPath = DefaultDirPath;
            string fileName = DefaultFileName;
            var imageFormat = DefaultImageFormat;

            var bounds = Screen.AllScreens[screenIndex].Bounds;
            try
            {
                switch (saveMode)
                {
                    case SaveMode.Clipboard:
                        ScreenShot.SaveToClipboard(bounds.Left, bounds.Top, bounds.Width, bounds.Height);
                        break;
                    case SaveMode.File:
                        ScreenShot.SaveAsFile(bounds.Left, bounds.Top, bounds.Width, bounds.Height, dirPath, fileName, imageFormat);
                        break;
                    default:
                        MessageBox.Show($"Unsupported Save Mode ({saveMode})");
                        return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            if (saveMode == SaveMode.Clipboard)
            {
                var targetProcess = GetProcessLockingClipboard();
                if (targetProcess.Id != 0)
                {
                    string message = $"The clipboard is currently locked due to the following process.\n";
                    message += $"Process ID : {targetProcess.Id}\n";
                    message += $"Process Name : {targetProcess.ProcessName}";
                    MessageBox.Show(message);
                    return;
                }
                /*
                else if (// TODO : Data Availability Check)
                {
                    string message = $"Clipboard processing has failed due to an unknown process.\n";
                    message += "System restart is recommended if this problem consists.";
                    MessageBox.Show(message);
                    return;
                }
                */
            }
        }

        private static Dictionary<string, string> ParseCommandLineArguments(string[] args)
        {
            var arguments = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    string[] contents = args[i].Substring(2).Split('=');
                    arguments.Add(contents[0], contents[1]);
                }
            }

            return arguments;
        }
        private static Process GetProcessLockingClipboard()
        {
            int processId;
            WinAPI.GetWindowThreadProcessId(WinAPI.GetOpenClipboardWindow(), out processId);

            return Process.GetProcessById(processId);
        }
    }
}
