using System.Windows.Forms;

namespace Launchpad.Forms
{
    public partial class MissionDetailsForm : Form
    {
        public MissionDetailsForm(string content)
        {
            InitializeComponent();
            textBox.Text = content;
            CenterToScreen();
        }
    }
}