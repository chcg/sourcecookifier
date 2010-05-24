namespace NppPluginNET
{
	partial class frmOptions
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
			this.btnTvBackColor = new System.Windows.Forms.Button();
			this.cbxShowInvalidSources = new System.Windows.Forms.CheckBox();
			this.cbxCaseSensitiveExt = new System.Windows.Forms.CheckBox();
			this.cbxUseCtagsFile = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblBackColor = new System.Windows.Forms.Label();
			this.cbxShowToolTips = new System.Windows.Forms.CheckBox();
			this.cbxShowGtdToolTips = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnTvBackColor
			// 
			this.btnTvBackColor.Location = new System.Drawing.Point(31, 13);
			this.btnTvBackColor.Name = "btnTvBackColor";
			this.btnTvBackColor.Size = new System.Drawing.Size(248, 21);
			this.btnTvBackColor.TabIndex = 0;
			this.btnTvBackColor.Text = "Treeview background color";
			this.btnTvBackColor.UseVisualStyleBackColor = true;
			this.btnTvBackColor.Click += new System.EventHandler(this.BtnTvBackColorClick);
			// 
			// cbxShowInvalidSources
			// 
			this.cbxShowInvalidSources.Location = new System.Drawing.Point(12, 101);
			this.cbxShowInvalidSources.Name = "cbxShowInvalidSources";
			this.cbxShowInvalidSources.Size = new System.Drawing.Size(254, 24);
			this.cbxShowInvalidSources.TabIndex = 1;
			this.cbxShowInvalidSources.Text = "Show invalid sources in treeview";
			this.cbxShowInvalidSources.UseVisualStyleBackColor = true;
			// 
			// cbxCaseSensitiveExt
			// 
			this.cbxCaseSensitiveExt.Location = new System.Drawing.Point(12, 43);
			this.cbxCaseSensitiveExt.Name = "cbxCaseSensitiveExt";
			this.cbxCaseSensitiveExt.Size = new System.Drawing.Size(280, 24);
			this.cbxCaseSensitiveExt.TabIndex = 2;
			this.cbxCaseSensitiveExt.Text = "Case sensitive language<>extension mapping";
			this.cbxCaseSensitiveExt.UseVisualStyleBackColor = true;
			// 
			// cbxUseCtagsFile
			// 
			this.cbxUseCtagsFile.Location = new System.Drawing.Point(12, 72);
			this.cbxUseCtagsFile.Name = "cbxUseCtagsFile";
			this.cbxUseCtagsFile.Size = new System.Drawing.Size(254, 24);
			this.cbxUseCtagsFile.TabIndex = 3;
			this.cbxUseCtagsFile.Text = "Use \"ctags\" file instead of StandardOut";
			this.cbxUseCtagsFile.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(204, 202);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(11, 202);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblBackColor
			// 
			this.lblBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblBackColor.Location = new System.Drawing.Point(11, 15);
			this.lblBackColor.Name = "lblBackColor";
			this.lblBackColor.Size = new System.Drawing.Size(15, 17);
			this.lblBackColor.TabIndex = 6;
			// 
			// cbxShowToolTips
			// 
			this.cbxShowToolTips.Location = new System.Drawing.Point(12, 131);
			this.cbxShowToolTips.Name = "cbxShowToolTips";
			this.cbxShowToolTips.Size = new System.Drawing.Size(254, 24);
			this.cbxShowToolTips.TabIndex = 7;
			this.cbxShowToolTips.Text = "Show tooltips in treeview";
			this.cbxShowToolTips.UseVisualStyleBackColor = true;
			// 
			// cbxShowGtdToolTips
			// 
			this.cbxShowGtdToolTips.Location = new System.Drawing.Point(12, 161);
			this.cbxShowGtdToolTips.Name = "cbxShowGtdToolTips";
			this.cbxShowGtdToolTips.Size = new System.Drawing.Size(254, 24);
			this.cbxShowGtdToolTips.TabIndex = 8;
			this.cbxShowGtdToolTips.Text = "Show tooltips in GoToDefinition contextmenu";
			this.cbxShowGtdToolTips.UseVisualStyleBackColor = true;
			// 
			// frmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(291, 244);
			this.Controls.Add(this.cbxShowGtdToolTips);
			this.Controls.Add(this.cbxShowToolTips);
			this.Controls.Add(this.lblBackColor);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbxUseCtagsFile);
			this.Controls.Add(this.cbxCaseSensitiveExt);
			this.Controls.Add(this.cbxShowInvalidSources);
			this.Controls.Add(this.btnTvBackColor);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Options";
			this.ResumeLayout(false);
		}
		internal System.Windows.Forms.CheckBox cbxShowGtdToolTips;
		internal System.Windows.Forms.CheckBox cbxShowToolTips;
		internal System.Windows.Forms.CheckBox cbxCaseSensitiveExt;
		internal System.Windows.Forms.CheckBox cbxUseCtagsFile;
		private System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblBackColor;
		private System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.CheckBox cbxShowInvalidSources;
		private System.Windows.Forms.Button btnTvBackColor;
	}
}
