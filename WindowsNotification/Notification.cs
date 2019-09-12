using System.Drawing;
using System.Windows.Forms;

namespace WindowsNotification
{
    public class Notification
    {
        private readonly Bitmap bitmapIcon;
        private readonly NotifyIcon notification;
        public Notification(string iconPath)
        {
            notification = new NotifyIcon
            {
                Visible = true
            };

            if (!string.IsNullOrWhiteSpace(iconPath))
            {
                bitmapIcon = new Bitmap(iconPath);
                notification.Icon = Icon.FromHandle(bitmapIcon.GetHicon());
            }
        }

        public void Show(string notificationTitle,string notificationText,int displayTime)
        {
            notification.BalloonTipTitle = notificationTitle;
            notification.BalloonTipText = notificationText;
            notification.ShowBalloonTip(displayTime);
        }

        /// <summary>
        /// Display a notification with default display time equal to 5 seconds
        /// </summary>
        /// <param name="notificationTitle"></param>
        /// <param name="notificationText"></param>
        public void Show(string notificationTitle, string notificationText)
        {
            Show(notificationTitle, notificationText, 5000);
        }
    }
}
