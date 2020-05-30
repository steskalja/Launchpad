using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Launchpad.Forms
{
    partial class AboutAppForm
    {
        // Required designer variable.
        private IContainer components = null;

        // Required designer variable. -> (disposing) true if managed resources should be disposed; otherwise, false.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.logoImageLabel = new Label();
            this.aboutAppLabel = new Label();
            this.SuspendLayout();
            //
            // logoImageLabel
            //
            this.logoImageLabel.Anchor =
                ((AnchorStyles) ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            this.logoImageLabel.Location = new Point(0, 40);
            this.logoImageLabel.Size = new Size(400, 128);
            //
            // aboutAppLabel
            //
            this.aboutAppLabel.Anchor =
                ((AnchorStyles) ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            this.aboutAppLabel.AutoEllipsis = true;
            this.aboutAppLabel.Font = new Font("Tahoma", 8F, FontStyle.Regular);
            this.aboutAppLabel.Location = new Point(12, 200);
            this.aboutAppLabel.Size = new Size(376, 200);
            this.aboutAppLabel.TextAlign = ContentAlignment.TopCenter;
            this.aboutAppLabel.Click += new EventHandler(this.aboutAppLabel_Click);
            //
            // AboutAppForm
            //
            this.components = new Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(400, 440);
            this.Controls.Add(this.logoImageLabel);
            this.Controls.Add(this.aboutAppLabel);
            this.Text = $"—{Application.ProductName}—";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ResumeLayout(false);
        }

        #endregion

        private Label logoImageLabel;
        private Label aboutAppLabel;
    }
}