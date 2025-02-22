﻿using AvalonEditB;
using HandyControl.Controls;
using HandyControl.Data;
using SharpVectors.Dom;
using SkEditorPlus.Managers;
using SkEditorPlus.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TabControl = System.Windows.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace SkEditorPlus
{
    public class SkEditor : SkEditorAPI
    {
        private readonly string[] args;
        private string startupFile = null;
        private MainWindow mainWindow;

        public event EventHandler WindowOpen;

        public SkEditor(string[] args)
        {
            this.args = args;
        }

        public void Run()
        {
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    startupFile = args[0];
                }
            }

            mainWindow = new MainWindow(this);
            WindowOpen?.Invoke(mainWindow, EventArgs.Empty);
            mainWindow.Show();
        }

        /// <returns>Main window</returns>
        public MainWindow GetMainWindow()
        {
            return mainWindow;
        }

        /// <returns>Startup file</returns>
        public string GetStartupFile()
        {
            return startupFile;
        }


        /// <returns>App's main menu</returns>
        public Menu GetMenu()
        {
            return GetMainWindow().GetMenu();
        }

        /// <returns>True if file is currently opened</returns>
        public bool IsFileOpen()
        {
            return GetMainWindow().GetFileManager().GetTextEditor() != null;
        }

        /// <returns>True if provided TabItem is file</returns>
        public bool IsFile(TabItem tabItem)
        {
            return tabItem.Content is TextEditor;
        }


        /// <returns>Current opened text editor if exists, otherwise null</returns>
        public TextEditor GetTextEditor()
        {
            return GetFileManager().GetTextEditor();
        }

        /// <returns>App's tabcontrol</returns>
        public HandyControl.Controls.TabControl GetTabControl()
        {
            return GetMainWindow().tabControl;
        }

        public FormattingWindow GetQuickEditsWindow()
        {
            return FormattingWindow.instance;
        }

        /// <returns>FileManager instance</returns>
        public FileManager GetFileManager()
        {
            return GetMainWindow().GetFileManager();
        }

        /// <summary>
        /// Shows MessageBox
        /// </summary>
        /// <param name="title">Title of messagebox</param>
        /// <param name="text">Text of messagebox</param>
        public void ShowMessage(string text, string title)
        {
            HandyControl.Controls.MessageBox.Show(text, title);
        }

        /// <summary>
        /// Shows MessageBox
        /// </summary>
        /// <param name="title">Title of messagebox</param>
        /// <param name="text">Text of messagebox</param>
        public void ShowError(string text, string title)
        {
            HandyControl.Controls.MessageBox.Error(text, title);
        }

        /// <summary>
        /// Opens provided URL in default browser
        /// </summary>
        public void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }


        public TabControl GetSideTabControl()
        {
            return GetMainWindow().leftTabControl;
        }

        public string GetVersion()
        {
            return MainWindow.Version;
        }

        public Dispatcher GetDispatcher()
        {
            return GetMainWindow().Dispatcher;
        }


        public bool IsAddonInstalled(string name) => AddonManager.addons.Any(addon => addon.Name.Equals(name));

        public int GetApiVersion() => 0;
    }
}
