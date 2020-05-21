using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Launchpad.Util;

namespace Launchpad.Forms
{
    public partial class MissionPatchForm : Form
    {
        private void ShowMissionPatchFormTips()
        {
            var toolTip = new ToolTip {AutoPopDelay = 2000, InitialDelay = 100, ReshowDelay = 100, ShowAlways = true};
            toolTip.SetToolTip(missionPatchImageLabel, "Click to close");
        }
        private void missionPatchImageLabel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        public MissionPatchForm(string missionPatchLink, int imageWidth, int imageHeight)
        {
            InitializeComponent();
            ShowMissionPatchFormTips();
            CenterToScreen();
            missionPatchImageLabel.Image = Task.Run(() =>
                HttpUtil.StreamUrlToImageAndResize(missionPatchLink, imageWidth, imageHeight)).Result;
        }
    }
}