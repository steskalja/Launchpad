using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Launchpad.Util;

namespace Launchpad.Forms
{
    public partial class MissionPatchForm : Form
    {
        private readonly ComponentResourceManager _resources =
            new ComponentResourceManager(typeof(MainForm));
        private readonly Image _image;
        private readonly string _missionName;
        
        private void ShowMissionPatchFormTips()
        {
            var toolTip = new ToolTip {AutoPopDelay = 2000, InitialDelay = 100, ReshowDelay = 100, ShowAlways = true};
            toolTip.SetToolTip(missionPatchImageLabel, "Click to close");
        }
        private void missionPatchImageLabel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void saveMissionPatchImageButton_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                DefaultExt = "png",
                Filter = "PNG|*.png",
                FileName = Regex.Replace($"{_missionName}-Mission Patch", @"\s+", "_")
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                new Bitmap(_image).Save(saveDialog.FileName, ImageFormat.Png);
            }
        }
        
        public MissionPatchForm(string missionName, string missionPatchLink, int imageWidth, int imageHeight)
        {
            InitializeComponent();
            ShowMissionPatchFormTips();
            CenterToScreen();
            Controls.SetChildIndex(saveMissionPatchImageButton, 0);
            _image = Task.Run(() =>
                HttpUtil.StreamUrlToImage(missionPatchLink)).Result;
            missionPatchImageLabel.Image = ImageUtil.ResizeImageAndKeepRatio(_image, imageWidth, imageHeight);
            _missionName = missionName;
        }
    }
}