using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Launchpad.Forms
{
    partial class MissionDetailsForm
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
            this.textBox = new TextBox();
            this.SuspendLayout();
            //
            // textBox
            //
            this.textBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Size = new System.Drawing.Size(400, 250);
            this.textBox.Multiline = true;
            this.textBox.ScrollBars = ScrollBars.Vertical;
            this.textBox.ReadOnly = true;
            this.textBox.BorderStyle = BorderStyle.None;
            //
            // MissionDetailsForm
            //
            this.components = new Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.textBox);
            this.Text = $"—{Application.ProductName}—";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Select();
            this.ResumeLayout(false);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        #endregion

        private TextBox textBox;
    }
}