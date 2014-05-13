using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace NetworkClipboardWindows
{
    public partial class DeviceConfigForm : Form
    {
        public DeviceConfigForm()
        {
            InitializeComponent();
            this.ipBox.Text = NetworkClipboardWindows.AppContext.deviceIp;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (ValidIp() && ValidPort())
            {
                NetworkClipboardWindows.AppContext.deviceIp = this.ipBox.Text;
                NetworkClipboardWindows.AppContext.devicePort = Int32.Parse(this.portBox.Text);
            }
            else
            {
                this.ipBox.Text = NetworkClipboardWindows.AppContext.deviceIp;
                this.portBox.Text = string.Format("{0}", NetworkClipboardWindows.AppContext.devicePort);
            }
            this.Hide();
        }

        private bool ValidPort()
        {
            try
            {
                int portnumber = Int32.Parse(this.portBox.Text);
                if (portnumber >= 80 && portnumber <= 65535)
                {
                    return true;
                }
                else
                {
                    ShowInvalidPortMessage();
                    return false;
                }

            }
            catch (Exception)
            {
                ShowInvalidPortMessage();
                return false;
            }
        }

        private void ShowInvalidPortMessage()
        {
            MessageBox.Show("Please specify a valid port number. It's usually between 80 and 65535");
        }

        private void ShowInvalidIpMessage()
        {
            MessageBox.Show("Please specify a valid IP address. Its a number of the form XXX.XXX.XXX.XXX");
        }

        private bool ValidIp()
        {
            Regex pattern = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            if (pattern.IsMatch(this.ipBox.Text))
            {
                return true;
            }
            else
            {
                ShowInvalidIpMessage();
                return false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
