using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace WindowsToastNotification
{
    public class ToastNotification
    {
        private readonly string APP_ID;
        private readonly XmlDocument toastXml;
        private readonly XmlNodeList stringElements;
        private readonly ToastNotifier toastNotifier;
        public ToastNotification(string iconPath, string applicationName)
        {
            toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode("Title"));
            stringElements[1].AppendChild(toastXml.CreateTextNode("Text"));

            if (!string.IsNullOrEmpty(iconPath))
            {
                String imagePath = "file:///" + Path.GetFullPath(iconPath);
                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
            }

            //toast.Activated += ToastActivated;
            //toast.Dismissed += ToastDismissed;
            //toast.Failed += ToastFailed;

            APP_ID = applicationName;
            toastNotifier = ToastNotificationManager.CreateToastNotifier(APP_ID);

            TryCreateShortcut();
        }

        public void Show(string notificationTitle, string notificationText)
        {
            stringElements[0].FirstChild.InnerText = notificationTitle;
            stringElements[1].FirstChild.InnerText = notificationText;

            toastNotifier.Show(new Windows.UI.Notifications.ToastNotification(toastXml));
        }

        private bool TryCreateShortcut()
        {
            String shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\Microsoft\\Windows\\Start Menu\\Programs\\{APP_ID}.lnk";
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(shortcutPath);
                return true;
            }
            return false;
        }
        private void InstallShortcut(String shortcutPath)
        {
            // Find the path to the current executable
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe
            ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
            ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property
            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(APP_ID))
            {
                ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
                ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk
            IPersistFile newShortcutSave = (IPersistFile)newShortcut;

            ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }
        private void ToastActivated(Windows.UI.Notifications.ToastNotification sender, object e)
        {
            //Dispatcher.Invoke(() =>
            //{
            //    Activate();
            //    Output.Text = "The user activated the toast.";
            //});
        }

        private void ToastDismissed(Windows.UI.Notifications.ToastNotification sender, ToastDismissedEventArgs e)
        {
            String outputText = "";
            switch (e.Reason)
            {
                case ToastDismissalReason.ApplicationHidden:
                    outputText = "The app hid the toast using ToastNotifier.Hide";
                    break;
                case ToastDismissalReason.UserCanceled:
                    outputText = "The user dismissed the toast";
                    break;
                case ToastDismissalReason.TimedOut:
                    outputText = "The toast has timed out";
                    break;
            }

            //Dispatcher.Invoke(() =>
            //{
            //    Output.Text = outputText;
            //});
        }

        private void ToastFailed(Windows.UI.Notifications.ToastNotification sender, ToastFailedEventArgs e)
        {
            //Dispatcher.Invoke(() =>
            //{
            //    Output.Text = "The toast encountered an error.";
            //});
        }
    }
}
