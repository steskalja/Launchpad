using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Launchpad.Forms
{
    partial class InitialForm
    {
        private readonly ComponentResourceManager _resources =
            new ComponentResourceManager(typeof(MainForm));
        
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
            this.SuspendLayout();
            //
            // 
            // InitialForm
            // 
            this.BackgroundImage = (Image) _resources.GetObject("$this.spacexLogo");
            //this.Anchor = ((AnchorStyles) ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            //this.BackgroundImageLayout = ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(230, 120);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Icon = ((Icon) (_resources.GetObject("$this.appIcon")));
            this.MaximizeBox = false;
            this.MaximizeBox = false;
            this.Text = Application.ProductName;
            //this.TopMost = true;
            this.Load += new System.EventHandler(this.AppLoad);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ResumeLayout(false);
        }

        #endregion
    }
}