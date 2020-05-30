using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using Launchpad.Util;
using Oddity;
using Oddity.API.Models.Launch;

namespace Launchpad.Forms
{
    public partial class MainForm : Form
    {
        private readonly OddityCore _oddity = new OddityCore();
        private readonly ComponentResourceManager _resources =
            new ComponentResourceManager(typeof(MainForm));

        private readonly int _currentFlightNumber;
        private string _missionPatchLink;
        private readonly List<LaunchInfo> _missionsData;
        private int _indexOfMission;
        private string _missionName;

        //

        private void ShowMainFormTips()
        {
            var toolTip = new ToolTip {AutoPopDelay = 2000, InitialDelay = 100, ReshowDelay = 100, ShowAlways = true};
            toolTip.SetToolTip(previousMissionButton, "Go to previous mission");
            toolTip.SetToolTip(currentMissionButton, "Display information about current mission");
            toolTip.SetToolTip(nextMissionButton, "Go to next mission");
            toolTip.SetToolTip(missionPatchImageLabel, "Click to enlarge");
            toolTip.SetToolTip(missionDetailsLabel, "Click to show");
        }
        
        private void EnableControls()
        {
            previousMissionButton.Enabled = true;
            currentMissionButton.Enabled = true;
            nextMissionButton.Enabled = true;
            missionNameLabel.Focus();
        }

        private void DisableControls()
        {
            previousMissionButton.Enabled = false;
            currentMissionButton.Enabled = false;
            nextMissionButton.Enabled = false;
            missionNameLabel.Focus();
        }
        
        private void ShowMissionDetailsForm(string content)
        {
            new MissionDetailsForm(content).ShowDialog();
        }
        
        //

        private int GetCurrentMissionFlightNumber()
        {
            return Convert.ToInt32(_oddity.Launches.GetNext().Execute().FlightNumber);
        }
        
        private void MissionData(int missionNumber)
        {
            DisableControls();
            //
            _missionPatchLink = _missionsData[missionNumber].Links.MissionPatch;
            var missionPatchImage = _missionsData[missionNumber].Links.MissionPatchSmall;
            _missionName = _missionsData[missionNumber].MissionName;
            var missionDetails = _missionsData[missionNumber].Details;
            var vehiclesStatus = _missionsData[missionNumber].Upcoming;
            var launchStatus = _missionsData[missionNumber].LaunchSuccess;

            if (string.IsNullOrWhiteSpace(missionPatchImage))
            {
                missionPatchImageLabel.Image = (Image) _resources.GetObject("$this.spacexLogo");
            }
            else
            {
                missionPatchImageLabel.Image = Task.Run(() => HttpUtil.StreamUrlToImageAndResize(missionPatchImage, 256, 256)).Result;
            }

            missionNameLabel.Text = string.IsNullOrWhiteSpace(_missionName) ? "— No mission name —" : _missionName;

            missionDetailsLabel.Text =
                string.IsNullOrWhiteSpace(missionDetails) ? "— No mission details —" : missionDetails;

            vehicleStatusLabel.Text = vehiclesStatus == true
                ? $"{_missionsData[missionNumber].Rocket.RocketName} will be launched from {_missionsData[missionNumber].LaunchSite.SiteName}"
                : $"{_missionsData[missionNumber].Rocket.RocketName} launched from {_missionsData[missionNumber].LaunchSite.SiteName}";

            switch (launchStatus)
            {
                case true:
                    missionStatusLabel.ForeColor = Color.Green;
                    missionStatusLabel.Text = "SUCCESSFUL";
                    break;
                case false:
                    missionStatusLabel.ForeColor = Color.Red;
                    missionStatusLabel.Text = "FAILED";
                    break;
                default:
                    missionStatusLabel.ForeColor = Color.Blue;
                    missionStatusLabel.Text = $"Will be launched in {_missionsData[missionNumber].LaunchDateLocal}";
                    break;
            }            
            //
            EnableControls();
        }
        
        //

        private void previousMissionButton_Click(object sender, EventArgs e)
        {
            if (_indexOfMission == 0)
            {
                currentMissionButton.Select();
            }
            else
            {
                MissionData(--_indexOfMission);
            }
        }

        private void currentMissionButton_Click(object sender, EventArgs e)
        {
            _indexOfMission = _currentFlightNumber;
            MissionData(_indexOfMission);
        }

        private void nextMissionButton_Click(object sender, EventArgs e)
        {
            if (_indexOfMission == (_missionsData.Count - 1))
            {
                MessageBox.Show("Next mission has not been planned, yet ;)", "—Launchpad—",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentMissionButton.Select();
            }
            else
            {
                MissionData(++_indexOfMission);
            }
        }

        private void missionPatchImageLabel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_missionPatchLink))
            {
                missionPatchImageLabel.Enabled = false;
                new MissionPatchForm(_missionName, _missionPatchLink, 512, 512).ShowDialog();
                missionPatchImageLabel.Enabled = true;
            }
            else
            {
                MessageBox.Show("Mission patch is not available to enlarge!", "—Launchpad—",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void missionDetailsLabel_Click(object sender, EventArgs e)
        {
            ShowMissionDetailsForm(missionDetailsLabel.Text);
        }

        private void missionDataButton_Click(object sender, EventArgs e)
        {
            missionDataButton.Enabled = false;
            new MissionDataForm(_missionsData[_indexOfMission]).ShowDialog();
            missionDataButton.Enabled = true;
            missionDataButton.Select();
        }
        
        private void aboutLaunchpadButton_Click(object sender, EventArgs e)
        {
            new AboutAppForm().ShowDialog();
        }

        //
        
        public MainForm(List<LaunchInfo> launchInfo)
        {
            InitializeComponent();
            CenterToScreen();
            ShowMainFormTips();
            //
            _currentFlightNumber = GetCurrentMissionFlightNumber();
            _missionsData = launchInfo;
            _indexOfMission = _missionsData.FindIndex(mission
                => mission.FlightNumber != null && mission.FlightNumber.Value.ToString()
                    .Contains(_currentFlightNumber.ToString()));
            _currentFlightNumber = _indexOfMission;
            MissionData(_indexOfMission);
        }
    }
}