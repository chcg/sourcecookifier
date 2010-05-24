
namespace NppPluginNET
{
	partial class frmHelp
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtbHelp = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtbHelp
			// 
			this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbHelp.Location = new System.Drawing.Point(0, 0);
			this.rtbHelp.Name = "rtbHelp";
			this.rtbHelp.ReadOnly = true;
			this.rtbHelp.Size = new System.Drawing.Size(605, 397);
			this.rtbHelp.TabIndex = 0;
			this.rtbHelp.Text = "";
			this.rtbHelp.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RtbHelpLinkClicked);
			// 
			// frmHelp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(605, 397);
			this.Controls.Add(this.rtbHelp);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmHelp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Help";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.RichTextBox rtbHelp;
	}
}
