using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Launchpad.Util;

namespace Launchpad.Forms
{
    public partial class AboutAppForm : Form
    {
        private readonly ComponentResourceManager _resources =
            new ComponentResourceManager(typeof(MainForm));

        private void aboutAppLabel_Click(object sender, EventArgs e)
        {
            new MissionDetailsForm(aboutAppLabel.Text).ShowDialog();
        }
        
        public AboutAppForm()
        {
            InitializeComponent();
            CenterToScreen();
            logoImageLabel.Image = ImageUtil.ResizeImageAndKeepRatio((Image) _resources.GetObject("$this.appLogo"), 128, 128);
            aboutAppLabel.Text = $"{Application.ProductName} {Application.ProductVersion.Remove(3)}\r\n\r\n" +
                                 $"Made with love for all by skyffx\r\n" +
                                 $"(Wojciech Piekielniak, wojciech.piekielniak@protonmail.com)\r\n\r\n\r\n" +
                                 $"This amazing app is based on:\r\n" +
                                 $"SpaceX-API — github.com/r-spacex/SpaceX-API\r\n" +
                                 $"Oddity — github.com/Tearth/Oddity\r\n" +
                                 $"App icon from flaticon.com\r\n\r\n" +
                                 $"SpaceX is the rightful owner of presented data.\r\n\r\n\r\n" +
                                 $"—2020—";
        }
    }
}