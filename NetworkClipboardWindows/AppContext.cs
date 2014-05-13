using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;

namespace NetworkClipboardWindows
{
    class AppContext : ApplicationContext
    {
        private NotifyIcon mNotifyIcon; 
        private DeviceConfigForm mConfigform = new DeviceConfigForm();
        public static string deviceIp;
        public static int devicePort;
        public AppContext() 
        {
            mNotifyIcon = new NotifyIcon();
            MenuItem exitItem = new MenuItem("Exit", Exit);
            MenuItem configItem = new MenuItem("Set device address...", Config);
            MenuItem sendItem = new MenuItem("Send clipboard", SendClipboard);
            mNotifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { sendItem, configItem, exitItem });
            mNotifyIcon.Icon = NetworkClipboardWindows.Properties.Resources.AppIcon;
            mNotifyIcon.Visible = true;

        }

        public void Exit(object sender, EventArgs args)
        {
            mNotifyIcon.Visible = false;
            Application.Exit();
        }

        public void Config(object sender, EventArgs args)
        {
            if (mConfigform.Visible)
            {
                mConfigform.Activate();
            }
            else 
            {
                mConfigform.Show();
            }
        }

        public void SendClipboard(object sender, EventArgs args)
        {
            if (AppContext.deviceIp != null && AppContext.deviceIp.Length > 0 && AppContext.devicePort != 0)
            {
                if (Clipboard.ContainsText())
                {
                    _SendClipboard();
                }
                else
                {
                    mNotifyIcon.BalloonTipTitle = "No text in clipboard";
                    mNotifyIcon.BalloonTipText = "There is no text in clipboard to send";
                    mNotifyIcon.ShowBalloonTip(4);
                }
            }
            else
            {
                mNotifyIcon.BalloonTipTitle = "No device address specified";
                mNotifyIcon.BalloonTipText = "Specify a device address by selecting \"Set device address\"";
                mNotifyIcon.ShowBalloonTip(4);
            }
        }

        private async void _SendClipboard()
        {
            try { 
                using (HttpClient httpClient = new HttpClient())
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("content", Clipboard.GetText());
                    FormUrlEncodedContent content = new FormUrlEncodedContent(dict);

                    HttpResponseMessage response = await httpClient.PostAsync(AppContext.deviceIp, content);
                    if (response.IsSuccessStatusCode)
                    {
                        mNotifyIcon.BalloonTipTitle = "Success!";
                        mNotifyIcon.BalloonTipText = "Clipboard sent!";
                        mNotifyIcon.ShowBalloonTip(4);
                    }
                    else
                    {
                        mNotifyIcon.BalloonTipTitle = "Error sending clipboard";
                        mNotifyIcon.BalloonTipText = "Reason: " + response.ReasonPhrase;
                        mNotifyIcon.ShowBalloonTip(4);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                mNotifyIcon.BalloonTipTitle = "Error sending clipboard";
                mNotifyIcon.BalloonTipText = "Device URL address is invalid";
                mNotifyIcon.ShowBalloonTip(4);
            }
            catch (HttpRequestException e)
            {
                mNotifyIcon.BalloonTipTitle = "Cannot connect to device";
                mNotifyIcon.BalloonTipText = "Is the service enabled on the device?";
                mNotifyIcon.ShowBalloonTip(4);
            }
            catch (Exception e)
            {
                mNotifyIcon.BalloonTipTitle = "An unexpected error happened :(";
                mNotifyIcon.BalloonTipText = "An error has ocurred while sending the clipboard. Please check for updates of this software. Exception: "+e.Message;
                mNotifyIcon.ShowBalloonTip(4);
            }
        }

    }
}
