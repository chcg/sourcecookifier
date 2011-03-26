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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblBackColor = new System.Windows.Forms.Label();
			this.cbxShowToolTips = new System.Windows.Forms.CheckBox();
			this.cbxShowGtdToolTips = new System.Windows.Forms.CheckBox();
			this.tbxFileSizeLimit = new System.Windows.Forms.TextBox();
			this.lblFileSizeLimit = new System.Windows.Forms.Label();
			this.cbxShowMenuAtBottom = new System.Windows.Forms.CheckBox();
			this.cbxFlowingMenuButtons = new System.Windows.Forms.CheckBox();
			this.lblStartupSessionMode = new System.Windows.Forms.Label();
			this.cbxStartupSessionMode = new System.Windows.Forms.ComboBox();
			this.cbxStartupShowMode = new System.Windows.Forms.ComboBox();
			this.lblStartupShowMode = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnTvBackColor
			// 
			this.btnTvBackColor.Location = new System.Drawing.Point(31, 13);
			this.btnTvBackColor.Name = "btnTvBackColor";
			this.btnTvBackColor.Size = new System.Drawing.Size(239, 21);
			this.btnTvBackColor.TabIndex = 0;
			this.btnTvBackColor.Text = "Treeview background color";
			this.btnTvBackColor.UseVisualStyleBackColor = true;
			this.btnTvBackColor.Click += new System.EventHandler(this.BtnTvBackColorClick);
			// 
			// cbxShowInvalidSources
			// 
			this.cbxShowInvalidSources.Location = new System.Drawing.Point(12, 160);
			this.cbxShowInvalidSources.Name = "cbxShowInvalidSources";
			this.cbxShowInvalidSources.Size = new System.Drawing.Size(254, 24);
			this.cbxShowInvalidSources.TabIndex = 1;
			this.cbxShowInvalidSources.Text = "Show invalid sources in treeview";
			this.cbxShowInvalidSources.UseVisualStyleBackColor = true;
			// 
			// cbxCaseSensitiveExt
			// 
			this.cbxCaseSensitiveExt.Location = new System.Drawing.Point(12, 252);
			this.cbxCaseSensitiveExt.Name = "cbxCaseSensitiveExt";
			this.cbxCaseSensitiveExt.Size = new System.Drawing.Size(280, 24);
			this.cbxCaseSensitiveExt.TabIndex = 2;
			this.cbxCaseSensitiveExt.Text = "Case sensitive language<>extension mapping";
			this.cbxCaseSensitiveExt.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(93, 323);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(177, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(11, 323);
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
			this.cbxShowToolTips.Location = new System.Drawing.Point(12, 190);
			this.cbxShowToolTips.Name = "cbxShowToolTips";
			this.cbxShowToolTips.Size = new System.Drawing.Size(254, 24);
			this.cbxShowToolTips.TabIndex = 7;
			this.cbxShowToolTips.Text = "Show tooltips in treeview";
			this.cbxShowToolTips.UseVisualStyleBackColor = true;
			// 
			// cbxShowGtdToolTips
			// 
			this.cbxShowGtdToolTips.Location = new System.Drawing.Point(12, 220);
			this.cbxShowGtdToolTips.Name = "cbxShowGtdToolTips";
			this.cbxShowGtdToolTips.Size = new System.Drawing.Size(254, 24);
			this.cbxShowGtdToolTips.TabIndex = 8;
			this.cbxShowGtdToolTips.Text = "Show tooltips in GoToDefinition contextmenu";
			this.cbxShowGtdToolTips.UseVisualStyleBackColor = true;
			// 
			// tbxFileSizeLimit
			// 
			this.tbxFileSizeLimit.Location = new System.Drawing.Point(209, 283);
			this.tbxFileSizeLimit.Name = "tbxFileSizeLimit";
			this.tbxFileSizeLimit.Size = new System.Drawing.Size(61, 20);
			this.tbxFileSizeLimit.TabIndex = 9;
			// 
			// lblFileSizeLimit
			// 
			this.lblFileSizeLimit.Location = new System.Drawing.Point(9, 281);
			this.lblFileSizeLimit.Name = "lblFileSizeLimit";
			this.lblFileSizeLimit.Size = new System.Drawing.Size(194, 23);
			this.lblFileSizeLimit.TabIndex = 10;
			this.lblFileSizeLimit.Text = "Filesize limit (e.g. 200kb or 5mb)";
			this.lblFileSizeLimit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxShowMenuAtBottom
			// 
			this.cbxShowMenuAtBottom.Location = new System.Drawing.Point(12, 40);
			this.cbxShowMenuAtBottom.Name = "cbxShowMenuAtBottom";
			this.cbxShowMenuAtBottom.Size = new System.Drawing.Size(280, 24);
			this.cbxShowMenuAtBottom.TabIndex = 11;
			this.cbxShowMenuAtBottom.Text = "Show toolbar menu at bottom";
			this.cbxShowMenuAtBottom.UseVisualStyleBackColor = true;
			// 
			// cbxFlowingMenuButtons
			// 
			this.cbxFlowingMenuButtons.Location = new System.Drawing.Point(12, 70);
			this.cbxFlowingMenuButtons.Name = "cbxFlowingMenuButtons";
			this.cbxFlowingMenuButtons.Size = new System.Drawing.Size(254, 24);
			this.cbxFlowingMenuButtons.TabIndex = 12;
			this.cbxFlowingMenuButtons.Text = "Flowing menu buttons (no hidden overflow)";
			this.cbxFlowingMenuButtons.UseVisualStyleBackColor = true;
			// 
			// lblStartupSessionMode
			// 
			this.lblStartupSessionMode.Location = new System.Drawing.Point(9, 130);
			this.lblStartupSessionMode.Name = "lblStartupSessionMode";
			this.lblStartupSessionMode.Size = new System.Drawing.Size(124, 23);
			this.lblStartupSessionMode.TabIndex = 13;
			this.lblStartupSessionMode.Text = "Startup session mode";
			this.lblStartupSessionMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxStartupSessionMode
			// 
			this.cbxStartupSessionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxStartupSessionMode.FormattingEnabled = true;
			this.cbxStartupSessionMode.Items.AddRange(new object[] {
									"",
									"Single file",
									"N++ session",
									"Cookie session"});
			this.cbxStartupSessionMode.Location = new System.Drawing.Point(166, 132);
			this.cbxStartupSessionMode.Name = "cbxStartupSessionMode";
			this.cbxStartupSessionMode.Size = new System.Drawing.Size(104, 21);
			this.cbxStartupSessionMode.TabIndex = 14;
			// 
			// cbxStartupShowMode
			// 
			this.cbxStartupShowMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxStartupShowMode.FormattingEnabled = true;
			this.cbxStartupShowMode.Items.AddRange(new object[] {
									"",
									"Show",
									"Hide"});
			this.cbxStartupShowMode.Location = new System.Drawing.Point(166, 102);
			this.cbxStartupShowMode.Name = "cbxStartupShowMode";
			this.cbxStartupShowMode.Size = new System.Drawing.Size(104, 21);
			this.cbxStartupShowMode.TabIndex = 16;
			// 
			// lblStartupShowMode
			// 
			this.lblStartupShowMode.Location = new System.Drawing.Point(9, 100);
			this.lblStartupShowMode.Name = "lblStartupShowMode";
			this.lblStartupShowMode.Size = new System.Drawing.Size(124, 23);
			this.lblStartupShowMode.TabIndex = 15;
			this.lblStartupShowMode.Text = "Startup show mode";
			this.lblStartupShowMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// frmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 365);
			this.Controls.Add(this.cbxStartupShowMode);
			this.Controls.Add(this.lblStartupShowMode);
			this.Controls.Add(this.cbxStartupSessionMode);
			this.Controls.Add(this.lblStartupSessionMode);
			this.Controls.Add(this.cbxFlowingMenuButtons);
			this.Controls.Add(this.cbxShowMenuAtBottom);
			this.Controls.Add(this.lblFileSizeLimit);
			this.Controls.Add(this.tbxFileSizeLimit);
			this.Controls.Add(this.cbxShowGtdToolTips);
			this.Controls.Add(this.cbxShowToolTips);
			this.Controls.Add(this.lblBackColor);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
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
			this.PerformLayout();
		}
		private System.Windows.Forms.Label lblStartupShowMode;
		internal System.Windows.Forms.ComboBox cbxStartupShowMode;
		internal System.Windows.Forms.ComboBox cbxStartupSessionMode;
		private System.Windows.Forms.Label lblStartupSessionMode;
		internal System.Windows.Forms.CheckBox cbxFlowingMenuButtons;
		internal System.Windows.Forms.CheckBox cbxShowMenuAtBottom;
		internal System.Windows.Forms.TextBox tbxFileSizeLimit;
		private System.Windows.Forms.Label lblFileSizeLimit;
		internal System.Windows.Forms.CheckBox cbxShowGtdToolTips;
		internal System.Windows.Forms.CheckBox cbxShowToolTips;
		internal System.Windows.Forms.CheckBox cbxCaseSensitiveExt;
		private System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblBackColor;
		private System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.CheckBox cbxShowInvalidSources;
		private System.Windows.Forms.Button btnTvBackColor;
	}
}
