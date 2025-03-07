using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using UniGetUI.Core.Tools;
using UniGetUI.Core.SettingsEngine;
using UniGetUI.Core.Data;
using System.Diagnostics;
using UniGetUI.Pages.DialogPages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UniGetUI.Pages.SettingsPages.GeneralPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Backup : Page, ISettingsPage
    {


        public Backup()
        {
            this.InitializeComponent();

            EnablePackageBackupUI(Settings.Get("EnablePackageBackup"));
            ResetBackupDirectory.Content = CoreTools.Translate("Reset");
            OpenBackupDirectory.Content = CoreTools.Translate("Open");
        }

        public bool CanGoBack => true;

        public string ShortTitle => CoreTools.Translate("Backup");

        public event EventHandler? RestartRequired;
        public event EventHandler<Type>? NavigationRequested;

        public void ShowRestartBanner(object sender, EventArgs e)
            => RestartRequired?.Invoke(this, e);

        private void ChangeBackupDirectory_Click(object sender, EventArgs e)
        {
            ExternalLibraries.Pickers.FolderPicker openPicker = new(MainApp.Instance.MainWindow.GetWindowHandle());
            string folder = openPicker.Show();
            if (folder != string.Empty)
            {
                Settings.SetValue("ChangeBackupOutputDirectory", folder);
                BackupDirectoryLabel.Text = folder;
                ResetBackupDirectory.IsEnabled = true;
            }
        }

        public void EnablePackageBackupUI(bool enabled)
        {
            EnableBackupTimestampingCheckBox.IsEnabled = enabled;
            ChangeBackupFileNameTextBox.IsEnabled = enabled;
            ChangeBackupDirectory.IsEnabled = enabled;
            BackupNowButton.IsEnabled = enabled;

            if (enabled)
            {
                if (!Settings.Get("ChangeBackupOutputDirectory"))
                {
                    BackupDirectoryLabel.Text = CoreData.UniGetUI_DefaultBackupDirectory;
                    ResetBackupDirectory.IsEnabled = false;
                }
                else
                {
                    BackupDirectoryLabel.Text = Settings.GetValue("ChangeBackupOutputDirectory");
                    ResetBackupDirectory.IsEnabled = true;
                }
            }
        }

        private void ResetBackupPath_Click(object sender, RoutedEventArgs e)
        {
            BackupDirectoryLabel.Text = CoreData.UniGetUI_DefaultBackupDirectory;
            Settings.Set("ChangeBackupOutputDirectory", false);
            ResetBackupDirectory.IsEnabled = false;
        }

        private void OpenBackupPath_Click(object sender, RoutedEventArgs e)
        {
            string directory = Settings.GetValue("ChangeBackupOutputDirectory");
            if (directory == "")
            {
                directory = CoreData.UniGetUI_DefaultBackupDirectory;
            }

            directory = directory.Replace("/", "\\");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Process.Start("explorer.exe", directory);
        }

        private async void DoBackup_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowLoadingDialog(CoreTools.Translate("Performing backup, please wait..."));
            await MainApp.Instance.MainWindow.NavigationPage.InstalledPage.BackupPackages();
            DialogHelper.HideLoadingDialog();
        }
    }
}
