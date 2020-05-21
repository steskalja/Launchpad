using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Launchpad.Util;
using Oddity.API.Models.Launch;

namespace Launchpad.Forms
{
    public partial class MissionDataForm : Form
    {
        private readonly ComponentResourceManager _resources =
            new ComponentResourceManager(typeof(MainForm));

        private readonly LaunchInfo _missionData;
        private int _imageNumber;
        private readonly int _imagesInMedia;
        private Image _image;

        private void ShowRocketImage(string rocketObject)
        {
            rocketImageLabel.Image = ImageUtil.ResizeImageAndKeepRatio((Image) _resources.GetObject(rocketObject),102, 395);
        }

        private void ShowRocketData()
        {
            rocketNameLabel.Text = _missionData.Rocket.RocketName;
            launchDateLabel.Text = _missionData.LaunchDateLocal.ToString();
            launchSiteNameLabel.Text = _missionData.LaunchSite.SiteLongName;

            switch (_missionData.Rocket.RocketName)
            {
                case "Falcon Heavy" when _missionData.Rocket.SecondStage.Payloads[0].PayloadType == "Satellite":
                    ShowRocketImage("$this.falconHeavyPayloadImage");
                    break;
                case "Falcon 9" when _missionData.Rocket.SecondStage.Payloads[0].PayloadType == "Satellite":
                    ShowRocketImage("$this.falcon9PayloadImage");
                    break;
                case "Falcon 9" when _missionData.Rocket.SecondStage.Payloads[0].PayloadType == "Crew Dragon":
                    ShowRocketImage("$this.falcon9CrewDragonImage");
                    break;
                case "Falcon 9" when _missionData.Rocket.SecondStage.Payloads[0].PayloadType.Contains("Dragon 1."):
                    ShowRocketImage("$this.falcon9DragonImage");
                    break;
            }

            //
            
            var firstStage = _missionData.Rocket.FirstStage.Cores;
            var coresData = new List<string>();
            var coreNumber = 1;
            foreach (var core in firstStage)
            {
                coresData.Add($" --- Core: {coreNumber} --- ");
                coresData.Add(core.CoreSerial != null ? $"Core serial: {core.CoreSerial}" : "Core serial: —");
                coresData.Add(core.Flight != null ? $"Flight: {core.Flight}" : "Flight: —");
                coresData.Add(core.Block != null ? $"Block: {core.Block}" : "Block: —");
                coresData.Add(core.Reused != null ? $"Reused: {core.Reused}" : "Reused: —");
                coresData.Add(core.LandSuccess == null ? "Land success: —" : $"Land success: {core.LandSuccess}");
                coresData.Add(core.LandingType != null ? $"Landing type: {core.LandingType}" : "Landing type: —");
                coresData.Add(core.LandingVehicle != null ? $"Landing vehicle: {core.LandingVehicle}" : "Landing vehicle: —");
                coresData.Add(string.Empty);
                coreNumber++;
            }

            coresData.RemoveAt(coresData.Count - 1);
            firstStageTextBox.Text = string.Join(Environment.NewLine, coresData);
            
            //

            var secondStage = _missionData.Rocket.SecondStage.Payloads;
            var payloadsData = new List<string>();
            var block = _missionData.Rocket.SecondStage.Block;
            payloadsData.Add(block != null ? $"Block: {block}" : "Block: —");
            payloadsData.Add(string.Empty);
            var payloadNumber = 1;
            foreach (var payload in secondStage)
            {
                payloadsData.Add($" --- Payload: {payloadNumber} --- ");
                payloadsData.Add(payload.PayloadId != null ? $"Name: {payload.PayloadId}" : "Name: —");
                payloadsData.Add(payload.Customers != null ? $"Customer(s): {string.Join(", ", payload.Customers)}" : "Customer(s): —");
                payloadsData.Add(payload.Nationality != null ? $"Nationality: {payload.Nationality}" : "Nationality: —");
                payloadsData.Add(payload.Manufacturer != null ? $"Manufacturer: {payload.Manufacturer}" : "Manufacturer: —");
                payloadsData.Add(payload.PayloadType != null ? $"Type: {payload.PayloadType}" : "Type: —");
                payloadsData.Add(payload.Orbit != null ? $"Orbit: {payload.Orbit}" : "Orbit: —");
                payloadsData.Add(string.Empty);
                payloadNumber++;
            }

            payloadsData.RemoveAt(payloadsData.Count - 1);
            secondStageTextBox.Text = string.Join(Environment.NewLine, payloadsData);
        }

        private void ShowMediaData(int imageNumber)
        {
            if (_imagesInMedia != 0)
            {
                ShowMediaControls();
                imageNumberLabel.Text = $"{imageNumber + 1}/{_imagesInMedia}";
                _image = Task.Run(() =>
                    HttpUtil.StreamUrlToImage(_missionData.Links.FlickrImages[imageNumber])).Result;
                mediaImageLabel.Image = ImageUtil.ResizeImageAndKeepRatio(_image, 418, 405);
            }
            else
            {
                MessageBox.Show("Images are unavailable!",
                    "—Launchpad—", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                rocketDetailsButton_Click(null, null);
                rocketDetailsButton.Select();
            }
        }

        private void ShowRocketControls()
        {
            rocketNameHeaderLabel.Visible = true;
            rocketNameLabel.Visible = true;
            firstStageHeaderLabel.Visible = true;
            firstStageTextBox.Visible = true;
            secondStageHeaderLabel.Visible = true;
            secondStageTextBox.Visible = true;
            launchDateHeaderLabel.Visible = true;
            launchDateLabel.Visible = true;
            launchSiteHeaderLabel.Visible = true;
            launchSiteNameLabel.Visible = true;
            rocketImageLabel.Visible = true;
        }
        
        private void HideRocketControls()
        {
            rocketNameHeaderLabel.Visible = false;
            rocketNameLabel.Visible = false;
            firstStageHeaderLabel.Visible = false;
            firstStageTextBox.Visible = false;
            secondStageHeaderLabel.Visible = false;
            secondStageTextBox.Visible = false;
            launchDateHeaderLabel.Visible = false;
            launchDateLabel.Visible = false;
            launchSiteHeaderLabel.Visible = false;
            launchSiteNameLabel.Visible = false;
            rocketImageLabel.Visible = false;
        }

        private void ShowMediaControls()
        {
            mediaImageLabel.Visible = true;
            nextImageButton.Visible = true;
            previousImageButton.Visible = true;
            imageNumberLabel.Visible = true;
            saveImageButton.Visible = true;
        }
        
        private void HideMediaControls()
        {
            mediaImageLabel.Visible = false;
            nextImageButton.Visible = false;
            previousImageButton.Visible = false;
            imageNumberLabel.Visible = false;
            saveImageButton.Visible = false;
        }
        
        //

        private void rocketDetailsButton_Click(object sender, EventArgs e)
        {
            HideMediaControls();
            ShowRocketControls();
            ShowRocketData();
        }

        private void missionMediaButton_Click(object sender, EventArgs e)
        {
            HideRocketControls();
            ShowMediaData(_imageNumber);
        }

        private void nextImageButton_Click(object sender, EventArgs e)
        {
            if (_imageNumber == (_imagesInMedia - 1))
            {
                previousImageButton.Focus();
            }
            else
            {
                ShowMediaData(++_imageNumber);
            }
        }

        private void previousImageButton_Click(object sender, EventArgs e)
        {
            if (_imageNumber == 0)
            {
                nextImageButton.Focus();
            }
            else
            {
                ShowMediaData(--_imageNumber);
            }
        }

        private void saveImageButton_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                DefaultExt = "jpg",
                Filter = "JPEG|*.jpg",
                FileName = $"{_missionData.MissionName}-{_imageNumber + 1}",
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                new Bitmap(_image).Save(saveDialog.FileName, ImageFormat.Jpeg);
            }
        }
        
        //
        
        public MissionDataForm(LaunchInfo launchInfo)
        {
            _missionData = launchInfo;
            _imagesInMedia = _missionData.Links.FlickrImages.Count;
            InitializeComponent();
            CenterToScreen();
            rocketDetailsButton.Select();
            HideMediaControls();
            ShowRocketData();
        }
    }
}