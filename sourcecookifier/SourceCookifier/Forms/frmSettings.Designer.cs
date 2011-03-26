namespace NppPluginNET
{
	partial class frmSettings
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
			this.btnOK = new System.Windows.Forms.Button();
			this.lbxLanguages = new System.Windows.Forms.ListBox();
			this.gbxLanguage = new System.Windows.Forms.GroupBox();
			this.tbxNewLanguage = new System.Windows.Forms.TextBox();
			this.btnLanguageDel = new System.Windows.Forms.Button();
			this.btnLanguageAdd = new System.Windows.Forms.Button();
			this.gbxTagType = new System.Windows.Forms.GroupBox();
			this.cbxTagTypeAdd = new System.Windows.Forms.ComboBox();
			this.btnTagTypeDel = new System.Windows.Forms.Button();
			this.btnTagTypeAdd = new System.Windows.Forms.Button();
			this.btnTagTypeDown = new System.Windows.Forms.Button();
			this.btnTagTypeUp = new System.Windows.Forms.Button();
			this.lbxTagTypes = new System.Windows.Forms.ListBox();
			this.gbxExtension = new System.Windows.Forms.GroupBox();
			this.tbxNewExtension = new System.Windows.Forms.TextBox();
			this.btnExtensionDel = new System.Windows.Forms.Button();
			this.btnExtensionAdd = new System.Windows.Forms.Button();
			this.lbxExtensions = new System.Windows.Forms.ListBox();
			this.cbxDisplayReturnType = new System.Windows.Forms.CheckBox();
			this.lblDisplayReturnType = new System.Windows.Forms.Label();
			this.cbxDisplayScope = new System.Windows.Forms.CheckBox();
			this.lblDisplayScope = new System.Windows.Forms.Label();
			this.cbxDisplaySignature = new System.Windows.Forms.CheckBox();
			this.lblDisplaySignature = new System.Windows.Forms.Label();
			this.cbxDisplayAccess = new System.Windows.Forms.CheckBox();
			this.lblDisplayAccess = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.tbxDescription = new System.Windows.Forms.TextBox();
			this.cbxShow = new System.Windows.Forms.CheckBox();
			this.lblShow = new System.Windows.Forms.Label();
			this.lblIcon = new System.Windows.Forms.Label();
			this.tbxIcon = new System.Windows.Forms.TextBox();
			this.btnIcon = new System.Windows.Forms.Button();
			this.pbxIcon = new System.Windows.Forms.PictureBox();
			this.btnForeColor = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.btnAcceptChanges = new System.Windows.Forms.Button();
			this.gbxAppearance = new System.Windows.Forms.GroupBox();
			this.cbxTrackCaret = new System.Windows.Forms.CheckBox();
			this.lblTrackCaret = new System.Windows.Forms.Label();
			this.gbxRegex = new System.Windows.Forms.GroupBox();
			this.cbxRegexCaseSensitive = new System.Windows.Forms.CheckBox();
			this.lblRegexOutput = new System.Windows.Forms.Label();
			this.lblRegexInput = new System.Windows.Forms.Label();
			this.tbxRegexInput = new System.Windows.Forms.TextBox();
			this.btnRegexDel = new System.Windows.Forms.Button();
			this.btnRegexChange = new System.Windows.Forms.Button();
			this.btnRegexAdd = new System.Windows.Forms.Button();
			this.tbxRegexOutput = new System.Windows.Forms.TextBox();
			this.lbxRegex = new System.Windows.Forms.ListBox();
			this.btnTry = new System.Windows.Forms.Button();
			this.gbxDisplay = new System.Windows.Forms.GroupBox();
			this.gbxSemantics = new System.Windows.Forms.GroupBox();
			this.tbxSemanticsScopeOperator = new System.Windows.Forms.TextBox();
			this.lblSemanticsScopeOperator = new System.Windows.Forms.Label();
			this.cbxSemanticsCaseSensitive = new System.Windows.Forms.CheckBox();
			this.lblSemanticsCaseSensitive = new System.Windows.Forms.Label();
			this.gbxLanguage.SuspendLayout();
			this.gbxTagType.SuspendLayout();
			this.gbxExtension.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
			this.gbxAppearance.SuspendLayout();
			this.gbxRegex.SuspendLayout();
			this.gbxDisplay.SuspendLayout();
			this.gbxSemantics.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(525, 356);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(197, 26);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lbxLanguages
			// 
			this.lbxLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxLanguages.FormattingEnabled = true;
			this.lbxLanguages.Location = new System.Drawing.Point(6, 19);
			this.lbxLanguages.Name = "lbxLanguages";
			this.lbxLanguages.Size = new System.Drawing.Size(126, 290);
			this.lbxLanguages.Sorted = true;
			this.lbxLanguages.TabIndex = 1;
			this.lbxLanguages.SelectedIndexChanged += new System.EventHandler(this.LbxLanguagesSelectedIndexChanged);
			// 
			// gbxLanguage
			// 
			this.gbxLanguage.Controls.Add(this.tbxNewLanguage);
			this.gbxLanguage.Controls.Add(this.btnLanguageDel);
			this.gbxLanguage.Controls.Add(this.lbxLanguages);
			this.gbxLanguage.Controls.Add(this.btnLanguageAdd);
			this.gbxLanguage.Location = new System.Drawing.Point(12, 12);
			this.gbxLanguage.Name = "gbxLanguage";
			this.gbxLanguage.Size = new System.Drawing.Size(138, 370);
			this.gbxLanguage.TabIndex = 2;
			this.gbxLanguage.TabStop = false;
			this.gbxLanguage.Text = "Language";
			// 
			// tbxNewLanguage
			// 
			this.tbxNewLanguage.Location = new System.Drawing.Point(6, 343);
			this.tbxNewLanguage.Name = "tbxNewLanguage";
			this.tbxNewLanguage.Size = new System.Drawing.Size(126, 20);
			this.tbxNewLanguage.TabIndex = 21;
			this.tbxNewLanguage.TextChanged += new System.EventHandler(this.TbxNewLanguageTextChanged);
			// 
			// btnLanguageDel
			// 
			this.btnLanguageDel.Enabled = false;
			this.btnLanguageDel.Location = new System.Drawing.Point(72, 314);
			this.btnLanguageDel.Name = "btnLanguageDel";
			this.btnLanguageDel.Size = new System.Drawing.Size(60, 23);
			this.btnLanguageDel.TabIndex = 22;
			this.btnLanguageDel.Text = "Del";
			this.btnLanguageDel.UseVisualStyleBackColor = true;
			this.btnLanguageDel.Click += new System.EventHandler(this.BtnLanguageDelClick);
			// 
			// btnLanguageAdd
			// 
			this.btnLanguageAdd.Enabled = false;
			this.btnLanguageAdd.Location = new System.Drawing.Point(6, 314);
			this.btnLanguageAdd.Name = "btnLanguageAdd";
			this.btnLanguageAdd.Size = new System.Drawing.Size(60, 23);
			this.btnLanguageAdd.TabIndex = 21;
			this.btnLanguageAdd.Text = "Add";
			this.btnLanguageAdd.UseVisualStyleBackColor = true;
			this.btnLanguageAdd.Click += new System.EventHandler(this.BtnLanguageAddClick);
			// 
			// gbxTagType
			// 
			this.gbxTagType.Controls.Add(this.cbxTagTypeAdd);
			this.gbxTagType.Controls.Add(this.btnTagTypeDel);
			this.gbxTagType.Controls.Add(this.btnTagTypeAdd);
			this.gbxTagType.Controls.Add(this.btnTagTypeDown);
			this.gbxTagType.Controls.Add(this.btnTagTypeUp);
			this.gbxTagType.Controls.Add(this.lbxTagTypes);
			this.gbxTagType.Enabled = false;
			this.gbxTagType.Location = new System.Drawing.Point(300, 12);
			this.gbxTagType.Name = "gbxTagType";
			this.gbxTagType.Size = new System.Drawing.Size(138, 370);
			this.gbxTagType.TabIndex = 3;
			this.gbxTagType.TabStop = false;
			this.gbxTagType.Text = "TagType";
			// 
			// cbxTagTypeAdd
			// 
			this.cbxTagTypeAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxTagTypeAdd.FormattingEnabled = true;
			this.cbxTagTypeAdd.Location = new System.Drawing.Point(7, 342);
			this.cbxTagTypeAdd.Name = "cbxTagTypeAdd";
			this.cbxTagTypeAdd.Size = new System.Drawing.Size(49, 21);
			this.cbxTagTypeAdd.Sorted = true;
			this.cbxTagTypeAdd.TabIndex = 19;
			// 
			// btnTagTypeDel
			// 
			this.btnTagTypeDel.Enabled = false;
			this.btnTagTypeDel.Location = new System.Drawing.Point(6, 314);
			this.btnTagTypeDel.Name = "btnTagTypeDel";
			this.btnTagTypeDel.Size = new System.Drawing.Size(96, 23);
			this.btnTagTypeDel.TabIndex = 18;
			this.btnTagTypeDel.Text = "Del";
			this.btnTagTypeDel.UseVisualStyleBackColor = true;
			this.btnTagTypeDel.Click += new System.EventHandler(this.BtnTagTypeDelClick);
			// 
			// btnTagTypeAdd
			// 
			this.btnTagTypeAdd.Location = new System.Drawing.Point(62, 341);
			this.btnTagTypeAdd.Name = "btnTagTypeAdd";
			this.btnTagTypeAdd.Size = new System.Drawing.Size(70, 23);
			this.btnTagTypeAdd.TabIndex = 16;
			this.btnTagTypeAdd.Text = "Add";
			this.btnTagTypeAdd.UseVisualStyleBackColor = true;
			this.btnTagTypeAdd.Click += new System.EventHandler(this.BtnTagTypeAddClick);
			// 
			// btnTagTypeDown
			// 
			this.btnTagTypeDown.Enabled = false;
			this.btnTagTypeDown.Location = new System.Drawing.Point(108, 181);
			this.btnTagTypeDown.Name = "btnTagTypeDown";
			this.btnTagTypeDown.Size = new System.Drawing.Size(24, 156);
			this.btnTagTypeDown.TabIndex = 17;
			this.btnTagTypeDown.Text = "Down";
			this.btnTagTypeDown.UseVisualStyleBackColor = true;
			this.btnTagTypeDown.Click += new System.EventHandler(this.BtnTagTypeDownClick);
			// 
			// btnTagTypeUp
			// 
			this.btnTagTypeUp.Enabled = false;
			this.btnTagTypeUp.Location = new System.Drawing.Point(108, 19);
			this.btnTagTypeUp.Name = "btnTagTypeUp";
			this.btnTagTypeUp.Size = new System.Drawing.Size(24, 156);
			this.btnTagTypeUp.TabIndex = 16;
			this.btnTagTypeUp.Text = "Up";
			this.btnTagTypeUp.UseVisualStyleBackColor = true;
			this.btnTagTypeUp.Click += new System.EventHandler(this.BtnTagTypeUpClick);
			// 
			// lbxTagTypes
			// 
			this.lbxTagTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxTagTypes.FormattingEnabled = true;
			this.lbxTagTypes.Location = new System.Drawing.Point(6, 19);
			this.lbxTagTypes.Name = "lbxTagTypes";
			this.lbxTagTypes.Size = new System.Drawing.Size(96, 290);
			this.lbxTagTypes.TabIndex = 2;
			this.lbxTagTypes.SelectedIndexChanged += new System.EventHandler(this.LbxTagTypeSelectedIndexChanged);
			// 
			// gbxExtension
			// 
			this.gbxExtension.Controls.Add(this.tbxNewExtension);
			this.gbxExtension.Controls.Add(this.btnExtensionDel);
			this.gbxExtension.Controls.Add(this.btnExtensionAdd);
			this.gbxExtension.Controls.Add(this.lbxExtensions);
			this.gbxExtension.Enabled = false;
			this.gbxExtension.Location = new System.Drawing.Point(156, 12);
			this.gbxExtension.Name = "gbxExtension";
			this.gbxExtension.Size = new System.Drawing.Size(138, 175);
			this.gbxExtension.TabIndex = 4;
			this.gbxExtension.TabStop = false;
			this.gbxExtension.Text = "Extension";
			// 
			// tbxNewExtension
			// 
			this.tbxNewExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tbxNewExtension.Location = new System.Drawing.Point(6, 147);
			this.tbxNewExtension.Name = "tbxNewExtension";
			this.tbxNewExtension.Size = new System.Drawing.Size(126, 20);
			this.tbxNewExtension.TabIndex = 17;
			this.tbxNewExtension.TextChanged += new System.EventHandler(this.TbxNewExtensionTextChanged);
			// 
			// btnExtensionDel
			// 
			this.btnExtensionDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnExtensionDel.Enabled = false;
			this.btnExtensionDel.Location = new System.Drawing.Point(72, 118);
			this.btnExtensionDel.Name = "btnExtensionDel";
			this.btnExtensionDel.Size = new System.Drawing.Size(60, 23);
			this.btnExtensionDel.TabIndex = 20;
			this.btnExtensionDel.Text = "Del";
			this.btnExtensionDel.UseVisualStyleBackColor = true;
			this.btnExtensionDel.Click += new System.EventHandler(this.BtnExtensionDelClick);
			// 
			// btnExtensionAdd
			// 
			this.btnExtensionAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnExtensionAdd.Enabled = false;
			this.btnExtensionAdd.Location = new System.Drawing.Point(6, 118);
			this.btnExtensionAdd.Name = "btnExtensionAdd";
			this.btnExtensionAdd.Size = new System.Drawing.Size(60, 23);
			this.btnExtensionAdd.TabIndex = 19;
			this.btnExtensionAdd.Text = "Add";
			this.btnExtensionAdd.UseVisualStyleBackColor = true;
			this.btnExtensionAdd.Click += new System.EventHandler(this.BtnExtensionAddClick);
			// 
			// lbxExtensions
			// 
			this.lbxExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxExtensions.FormattingEnabled = true;
			this.lbxExtensions.Location = new System.Drawing.Point(6, 19);
			this.lbxExtensions.Name = "lbxExtensions";
			this.lbxExtensions.Size = new System.Drawing.Size(126, 95);
			this.lbxExtensions.TabIndex = 2;
			this.lbxExtensions.SelectedIndexChanged += new System.EventHandler(this.LbxExtensionsSelectedIndexChanged);
			// 
			// cbxDisplayReturnType
			// 
			this.cbxDisplayReturnType.Location = new System.Drawing.Point(117, 39);
			this.cbxDisplayReturnType.Name = "cbxDisplayReturnType";
			this.cbxDisplayReturnType.Size = new System.Drawing.Size(15, 24);
			this.cbxDisplayReturnType.TabIndex = 28;
			this.cbxDisplayReturnType.UseVisualStyleBackColor = true;
			// 
			// lblDisplayReturnType
			// 
			this.lblDisplayReturnType.AutoSize = true;
			this.lblDisplayReturnType.Location = new System.Drawing.Point(6, 45);
			this.lblDisplayReturnType.Name = "lblDisplayReturnType";
			this.lblDisplayReturnType.Size = new System.Drawing.Size(62, 13);
			this.lblDisplayReturnType.TabIndex = 27;
			this.lblDisplayReturnType.Text = "Return type";
			// 
			// cbxDisplayScope
			// 
			this.cbxDisplayScope.Location = new System.Drawing.Point(117, 61);
			this.cbxDisplayScope.Name = "cbxDisplayScope";
			this.cbxDisplayScope.Size = new System.Drawing.Size(15, 24);
			this.cbxDisplayScope.TabIndex = 26;
			this.cbxDisplayScope.UseVisualStyleBackColor = true;
			this.cbxDisplayScope.CheckedChanged += new System.EventHandler(this.DisplayExtendedChanged);
			// 
			// lblDisplayScope
			// 
			this.lblDisplayScope.AutoSize = true;
			this.lblDisplayScope.Location = new System.Drawing.Point(6, 66);
			this.lblDisplayScope.Name = "lblDisplayScope";
			this.lblDisplayScope.Size = new System.Drawing.Size(38, 13);
			this.lblDisplayScope.TabIndex = 25;
			this.lblDisplayScope.Text = "Scope";
			// 
			// cbxDisplaySignature
			// 
			this.cbxDisplaySignature.Location = new System.Drawing.Point(117, 83);
			this.cbxDisplaySignature.Name = "cbxDisplaySignature";
			this.cbxDisplaySignature.Size = new System.Drawing.Size(15, 24);
			this.cbxDisplaySignature.TabIndex = 24;
			this.cbxDisplaySignature.UseVisualStyleBackColor = true;
			this.cbxDisplaySignature.CheckedChanged += new System.EventHandler(this.DisplayExtendedChanged);
			// 
			// lblDisplaySignature
			// 
			this.lblDisplaySignature.AutoSize = true;
			this.lblDisplaySignature.Location = new System.Drawing.Point(6, 88);
			this.lblDisplaySignature.Name = "lblDisplaySignature";
			this.lblDisplaySignature.Size = new System.Drawing.Size(52, 13);
			this.lblDisplaySignature.TabIndex = 23;
			this.lblDisplaySignature.Text = "Signature";
			// 
			// cbxDisplayAccess
			// 
			this.cbxDisplayAccess.Location = new System.Drawing.Point(117, 17);
			this.cbxDisplayAccess.Name = "cbxDisplayAccess";
			this.cbxDisplayAccess.Size = new System.Drawing.Size(15, 24);
			this.cbxDisplayAccess.TabIndex = 22;
			this.cbxDisplayAccess.UseVisualStyleBackColor = true;
			this.cbxDisplayAccess.CheckedChanged += new System.EventHandler(this.DisplayExtendedChanged);
			// 
			// lblDisplayAccess
			// 
			this.lblDisplayAccess.AutoSize = true;
			this.lblDisplayAccess.Location = new System.Drawing.Point(6, 22);
			this.lblDisplayAccess.Name = "lblDisplayAccess";
			this.lblDisplayAccess.Size = new System.Drawing.Size(42, 13);
			this.lblDisplayAccess.TabIndex = 21;
			this.lblDisplayAccess.Text = "Access";
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(6, 21);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(61, 19);
			this.lblDescription.TabIndex = 6;
			this.lblDescription.Text = "Description";
			// 
			// tbxDescription
			// 
			this.tbxDescription.Location = new System.Drawing.Point(74, 18);
			this.tbxDescription.Name = "tbxDescription";
			this.tbxDescription.Size = new System.Drawing.Size(279, 20);
			this.tbxDescription.TabIndex = 8;
			this.tbxDescription.TextChanged += new System.EventHandler(this.TagTypeAppearanceChanged);
			// 
			// cbxShow
			// 
			this.cbxShow.Location = new System.Drawing.Point(74, 72);
			this.cbxShow.Name = "cbxShow";
			this.cbxShow.Size = new System.Drawing.Size(15, 24);
			this.cbxShow.TabIndex = 9;
			this.cbxShow.UseVisualStyleBackColor = true;
			this.cbxShow.CheckedChanged += new System.EventHandler(this.TagTypeAppearanceChanged);
			// 
			// lblShow
			// 
			this.lblShow.Location = new System.Drawing.Point(6, 77);
			this.lblShow.Name = "lblShow";
			this.lblShow.Size = new System.Drawing.Size(61, 19);
			this.lblShow.TabIndex = 10;
			this.lblShow.Text = "Show";
			// 
			// lblIcon
			// 
			this.lblIcon.Location = new System.Drawing.Point(6, 49);
			this.lblIcon.Name = "lblIcon";
			this.lblIcon.Size = new System.Drawing.Size(61, 19);
			this.lblIcon.TabIndex = 11;
			this.lblIcon.Text = "Icon";
			// 
			// tbxIcon
			// 
			this.tbxIcon.Location = new System.Drawing.Point(98, 46);
			this.tbxIcon.Name = "tbxIcon";
			this.tbxIcon.Size = new System.Drawing.Size(222, 20);
			this.tbxIcon.TabIndex = 12;
			this.tbxIcon.TextChanged += new System.EventHandler(this.TagTypeAppearanceChanged);
			// 
			// btnIcon
			// 
			this.btnIcon.Location = new System.Drawing.Point(326, 44);
			this.btnIcon.Name = "btnIcon";
			this.btnIcon.Size = new System.Drawing.Size(27, 23);
			this.btnIcon.TabIndex = 13;
			this.btnIcon.Text = "...";
			this.btnIcon.UseVisualStyleBackColor = true;
			this.btnIcon.Click += new System.EventHandler(this.BtnIconClick);
			// 
			// pbxIcon
			// 
			this.pbxIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbxIcon.Location = new System.Drawing.Point(74, 46);
			this.pbxIcon.Name = "pbxIcon";
			this.pbxIcon.Size = new System.Drawing.Size(20, 20);
			this.pbxIcon.TabIndex = 14;
			this.pbxIcon.TabStop = false;
			// 
			// btnForeColor
			// 
			this.btnForeColor.Location = new System.Drawing.Point(98, 72);
			this.btnForeColor.Name = "btnForeColor";
			this.btnForeColor.Size = new System.Drawing.Size(157, 23);
			this.btnForeColor.TabIndex = 15;
			this.btnForeColor.Text = "ForeColor";
			this.btnForeColor.UseVisualStyleBackColor = true;
			this.btnForeColor.Click += new System.EventHandler(this.BtnForeColorClick);
			// 
			// button3
			// 
			this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button3.Location = new System.Drawing.Point(728, 356);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 26);
			this.button3.TabIndex = 16;
			this.button3.Text = "Cancel";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// btnAcceptChanges
			// 
			this.btnAcceptChanges.Enabled = false;
			this.btnAcceptChanges.Location = new System.Drawing.Point(6, 103);
			this.btnAcceptChanges.Name = "btnAcceptChanges";
			this.btnAcceptChanges.Size = new System.Drawing.Size(347, 23);
			this.btnAcceptChanges.TabIndex = 17;
			this.btnAcceptChanges.Text = "Accept changes";
			this.btnAcceptChanges.UseVisualStyleBackColor = true;
			this.btnAcceptChanges.Click += new System.EventHandler(this.BtnAcceptChangesClick);
			// 
			// gbxAppearance
			// 
			this.gbxAppearance.Controls.Add(this.cbxTrackCaret);
			this.gbxAppearance.Controls.Add(this.lblTrackCaret);
			this.gbxAppearance.Controls.Add(this.lblDescription);
			this.gbxAppearance.Controls.Add(this.btnAcceptChanges);
			this.gbxAppearance.Controls.Add(this.tbxDescription);
			this.gbxAppearance.Controls.Add(this.cbxShow);
			this.gbxAppearance.Controls.Add(this.btnForeColor);
			this.gbxAppearance.Controls.Add(this.lblShow);
			this.gbxAppearance.Controls.Add(this.pbxIcon);
			this.gbxAppearance.Controls.Add(this.lblIcon);
			this.gbxAppearance.Controls.Add(this.btnIcon);
			this.gbxAppearance.Controls.Add(this.tbxIcon);
			this.gbxAppearance.Enabled = false;
			this.gbxAppearance.Location = new System.Drawing.Point(444, 12);
			this.gbxAppearance.Name = "gbxAppearance";
			this.gbxAppearance.Size = new System.Drawing.Size(359, 132);
			this.gbxAppearance.TabIndex = 18;
			this.gbxAppearance.TabStop = false;
			this.gbxAppearance.Text = "Appearance";
			// 
			// cbxTrackCaret
			// 
			this.cbxTrackCaret.Location = new System.Drawing.Point(338, 72);
			this.cbxTrackCaret.Name = "cbxTrackCaret";
			this.cbxTrackCaret.Size = new System.Drawing.Size(15, 24);
			this.cbxTrackCaret.TabIndex = 19;
			this.cbxTrackCaret.UseVisualStyleBackColor = true;
			this.cbxTrackCaret.CheckedChanged += new System.EventHandler(this.TagTypeAppearanceChanged);
			// 
			// lblTrackCaret
			// 
			this.lblTrackCaret.Location = new System.Drawing.Point(261, 77);
			this.lblTrackCaret.Name = "lblTrackCaret";
			this.lblTrackCaret.Size = new System.Drawing.Size(64, 19);
			this.lblTrackCaret.TabIndex = 18;
			this.lblTrackCaret.Text = "Track caret";
			// 
			// gbxRegex
			// 
			this.gbxRegex.Controls.Add(this.cbxRegexCaseSensitive);
			this.gbxRegex.Controls.Add(this.lblRegexOutput);
			this.gbxRegex.Controls.Add(this.lblRegexInput);
			this.gbxRegex.Controls.Add(this.tbxRegexInput);
			this.gbxRegex.Controls.Add(this.btnRegexDel);
			this.gbxRegex.Controls.Add(this.btnRegexChange);
			this.gbxRegex.Controls.Add(this.btnRegexAdd);
			this.gbxRegex.Controls.Add(this.tbxRegexOutput);
			this.gbxRegex.Controls.Add(this.lbxRegex);
			this.gbxRegex.Enabled = false;
			this.gbxRegex.Location = new System.Drawing.Point(444, 150);
			this.gbxRegex.Name = "gbxRegex";
			this.gbxRegex.Size = new System.Drawing.Size(359, 200);
			this.gbxRegex.TabIndex = 19;
			this.gbxRegex.TabStop = false;
			this.gbxRegex.Text = "RegularExpression";
			// 
			// cbxRegexCaseSensitive
			// 
			this.cbxRegexCaseSensitive.Location = new System.Drawing.Point(256, 146);
			this.cbxRegexCaseSensitive.Name = "cbxRegexCaseSensitive";
			this.cbxRegexCaseSensitive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbxRegexCaseSensitive.Size = new System.Drawing.Size(98, 24);
			this.cbxRegexCaseSensitive.TabIndex = 8;
			this.cbxRegexCaseSensitive.Text = "Case sensitive";
			this.cbxRegexCaseSensitive.UseVisualStyleBackColor = true;
			this.cbxRegexCaseSensitive.CheckedChanged += new System.EventHandler(this.CbxRegexCaseSensitiveCheckedChanged);
			// 
			// lblRegexOutput
			// 
			this.lblRegexOutput.Location = new System.Drawing.Point(6, 147);
			this.lblRegexOutput.Name = "lblRegexOutput";
			this.lblRegexOutput.Size = new System.Drawing.Size(41, 19);
			this.lblRegexOutput.TabIndex = 7;
			this.lblRegexOutput.Text = "Output";
			this.lblRegexOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRegexInput
			// 
			this.lblRegexInput.Location = new System.Drawing.Point(6, 121);
			this.lblRegexInput.Name = "lblRegexInput";
			this.lblRegexInput.Size = new System.Drawing.Size(41, 19);
			this.lblRegexInput.TabIndex = 6;
			this.lblRegexInput.Text = "Input";
			this.lblRegexInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbxRegexInput
			// 
			this.tbxRegexInput.Location = new System.Drawing.Point(49, 120);
			this.tbxRegexInput.Name = "tbxRegexInput";
			this.tbxRegexInput.Size = new System.Drawing.Size(304, 20);
			this.tbxRegexInput.TabIndex = 5;
			this.tbxRegexInput.TextChanged += new System.EventHandler(this.TbxRegexTextChanged);
			// 
			// btnRegexDel
			// 
			this.btnRegexDel.Enabled = false;
			this.btnRegexDel.Location = new System.Drawing.Point(124, 172);
			this.btnRegexDel.Name = "btnRegexDel";
			this.btnRegexDel.Size = new System.Drawing.Size(111, 23);
			this.btnRegexDel.TabIndex = 4;
			this.btnRegexDel.Text = "Del";
			this.btnRegexDel.UseVisualStyleBackColor = true;
			this.btnRegexDel.Click += new System.EventHandler(this.BtnRegexDelClick);
			// 
			// btnRegexChange
			// 
			this.btnRegexChange.Enabled = false;
			this.btnRegexChange.Location = new System.Drawing.Point(241, 172);
			this.btnRegexChange.Name = "btnRegexChange";
			this.btnRegexChange.Size = new System.Drawing.Size(112, 23);
			this.btnRegexChange.TabIndex = 3;
			this.btnRegexChange.Text = "Accept changes";
			this.btnRegexChange.UseVisualStyleBackColor = true;
			this.btnRegexChange.Click += new System.EventHandler(this.BtnRegexChangeClick);
			// 
			// btnRegexAdd
			// 
			this.btnRegexAdd.Location = new System.Drawing.Point(6, 172);
			this.btnRegexAdd.Name = "btnRegexAdd";
			this.btnRegexAdd.Size = new System.Drawing.Size(112, 23);
			this.btnRegexAdd.TabIndex = 2;
			this.btnRegexAdd.Text = "Add";
			this.btnRegexAdd.UseVisualStyleBackColor = true;
			this.btnRegexAdd.Click += new System.EventHandler(this.BtnRegexAddClick);
			// 
			// tbxRegexOutput
			// 
			this.tbxRegexOutput.Location = new System.Drawing.Point(49, 146);
			this.tbxRegexOutput.Name = "tbxRegexOutput";
			this.tbxRegexOutput.Size = new System.Drawing.Size(204, 20);
			this.tbxRegexOutput.TabIndex = 1;
			this.tbxRegexOutput.TextChanged += new System.EventHandler(this.TbxRegexTextChanged);
			// 
			// lbxRegex
			// 
			this.lbxRegex.FormattingEnabled = true;
			this.lbxRegex.Location = new System.Drawing.Point(6, 19);
			this.lbxRegex.Name = "lbxRegex";
			this.lbxRegex.Size = new System.Drawing.Size(347, 95);
			this.lbxRegex.TabIndex = 0;
			this.lbxRegex.SelectedIndexChanged += new System.EventHandler(this.LbxRegexSelectedIndexChanged);
			// 
			// btnTry
			// 
			this.btnTry.Location = new System.Drawing.Point(444, 356);
			this.btnTry.Name = "btnTry";
			this.btnTry.Size = new System.Drawing.Size(75, 26);
			this.btnTry.TabIndex = 20;
			this.btnTry.Text = "Try";
			this.btnTry.UseVisualStyleBackColor = true;
			this.btnTry.Click += new System.EventHandler(this.BtnTryClick);
			// 
			// gbxDisplay
			// 
			this.gbxDisplay.Controls.Add(this.cbxDisplayReturnType);
			this.gbxDisplay.Controls.Add(this.lblDisplayAccess);
			this.gbxDisplay.Controls.Add(this.lblDisplayReturnType);
			this.gbxDisplay.Controls.Add(this.cbxDisplayAccess);
			this.gbxDisplay.Controls.Add(this.cbxDisplayScope);
			this.gbxDisplay.Controls.Add(this.lblDisplaySignature);
			this.gbxDisplay.Controls.Add(this.lblDisplayScope);
			this.gbxDisplay.Controls.Add(this.cbxDisplaySignature);
			this.gbxDisplay.Enabled = false;
			this.gbxDisplay.Location = new System.Drawing.Point(156, 272);
			this.gbxDisplay.Name = "gbxDisplay";
			this.gbxDisplay.Size = new System.Drawing.Size(138, 111);
			this.gbxDisplay.TabIndex = 29;
			this.gbxDisplay.TabStop = false;
			this.gbxDisplay.Text = "Display";
			// 
			// gbxSemantics
			// 
			this.gbxSemantics.Controls.Add(this.tbxSemanticsScopeOperator);
			this.gbxSemantics.Controls.Add(this.lblSemanticsScopeOperator);
			this.gbxSemantics.Controls.Add(this.cbxSemanticsCaseSensitive);
			this.gbxSemantics.Controls.Add(this.lblSemanticsCaseSensitive);
			this.gbxSemantics.Enabled = false;
			this.gbxSemantics.Location = new System.Drawing.Point(156, 193);
			this.gbxSemantics.Name = "gbxSemantics";
			this.gbxSemantics.Size = new System.Drawing.Size(138, 73);
			this.gbxSemantics.TabIndex = 30;
			this.gbxSemantics.TabStop = false;
			this.gbxSemantics.Text = "Semantics";
			// 
			// tbxSemanticsScopeOperator
			// 
			this.tbxSemanticsScopeOperator.Location = new System.Drawing.Point(108, 43);
			this.tbxSemanticsScopeOperator.MaxLength = 2;
			this.tbxSemanticsScopeOperator.Name = "tbxSemanticsScopeOperator";
			this.tbxSemanticsScopeOperator.Size = new System.Drawing.Size(22, 20);
			this.tbxSemanticsScopeOperator.TabIndex = 25;
			this.tbxSemanticsScopeOperator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbxSemanticsScopeOperator.TextChanged += new System.EventHandler(this.SemanticsChanged);
			// 
			// lblSemanticsScopeOperator
			// 
			this.lblSemanticsScopeOperator.AutoSize = true;
			this.lblSemanticsScopeOperator.Location = new System.Drawing.Point(6, 47);
			this.lblSemanticsScopeOperator.Name = "lblSemanticsScopeOperator";
			this.lblSemanticsScopeOperator.Size = new System.Drawing.Size(80, 13);
			this.lblSemanticsScopeOperator.TabIndex = 24;
			this.lblSemanticsScopeOperator.Text = "Scope operator";
			// 
			// cbxSemanticsCaseSensitive
			// 
			this.cbxSemanticsCaseSensitive.Location = new System.Drawing.Point(117, 17);
			this.cbxSemanticsCaseSensitive.Name = "cbxSemanticsCaseSensitive";
			this.cbxSemanticsCaseSensitive.Size = new System.Drawing.Size(15, 24);
			this.cbxSemanticsCaseSensitive.TabIndex = 23;
			this.cbxSemanticsCaseSensitive.UseVisualStyleBackColor = true;
			this.cbxSemanticsCaseSensitive.CheckedChanged += new System.EventHandler(this.SemanticsChanged);
			// 
			// lblSemanticsCaseSensitive
			// 
			this.lblSemanticsCaseSensitive.AutoSize = true;
			this.lblSemanticsCaseSensitive.Location = new System.Drawing.Point(6, 23);
			this.lblSemanticsCaseSensitive.Name = "lblSemanticsCaseSensitive";
			this.lblSemanticsCaseSensitive.Size = new System.Drawing.Size(75, 13);
			this.lblSemanticsCaseSensitive.TabIndex = 22;
			this.lblSemanticsCaseSensitive.Text = "Case sensitive";
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(816, 395);
			this.Controls.Add(this.gbxSemantics);
			this.Controls.Add(this.gbxDisplay);
			this.Controls.Add(this.btnTry);
			this.Controls.Add(this.gbxRegex);
			this.Controls.Add(this.gbxAppearance);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.gbxExtension);
			this.Controls.Add(this.gbxTagType);
			this.Controls.Add(this.gbxLanguage);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Language settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSettingsFormClosing);
			this.gbxLanguage.ResumeLayout(false);
			this.gbxLanguage.PerformLayout();
			this.gbxTagType.ResumeLayout(false);
			this.gbxExtension.ResumeLayout(false);
			this.gbxExtension.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
			this.gbxAppearance.ResumeLayout(false);
			this.gbxAppearance.PerformLayout();
			this.gbxRegex.ResumeLayout(false);
			this.gbxRegex.PerformLayout();
			this.gbxDisplay.ResumeLayout(false);
			this.gbxDisplay.PerformLayout();
			this.gbxSemantics.ResumeLayout(false);
			this.gbxSemantics.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label lblSemanticsScopeOperator;
		private System.Windows.Forms.TextBox tbxSemanticsScopeOperator;
		private System.Windows.Forms.Label lblSemanticsCaseSensitive;
		private System.Windows.Forms.CheckBox cbxSemanticsCaseSensitive;
		private System.Windows.Forms.GroupBox gbxSemantics;
		private System.Windows.Forms.GroupBox gbxDisplay;
		private System.Windows.Forms.Label lblDisplayReturnType;
		private System.Windows.Forms.CheckBox cbxDisplayReturnType;
		private System.Windows.Forms.Label lblDisplayAccess;
		private System.Windows.Forms.CheckBox cbxDisplayAccess;
		private System.Windows.Forms.Label lblDisplaySignature;
		private System.Windows.Forms.CheckBox cbxDisplaySignature;
		private System.Windows.Forms.Label lblDisplayScope;
		private System.Windows.Forms.CheckBox cbxDisplayScope;
		private System.Windows.Forms.TextBox tbxRegexOutput;
		private System.Windows.Forms.CheckBox cbxRegexCaseSensitive;
		private System.Windows.Forms.Label lblRegexOutput;
		private System.Windows.Forms.Label lblRegexInput;
		private System.Windows.Forms.TextBox tbxRegexInput;
		private System.Windows.Forms.Label lblTrackCaret;
		private System.Windows.Forms.CheckBox cbxTrackCaret;
		private System.Windows.Forms.Button btnTry;
		private System.Windows.Forms.Button btnLanguageAdd;
		private System.Windows.Forms.Button btnLanguageDel;
		private System.Windows.Forms.TextBox tbxNewLanguage;
		private System.Windows.Forms.ComboBox cbxTagTypeAdd;
		private System.Windows.Forms.GroupBox gbxRegex;
		private System.Windows.Forms.GroupBox gbxExtension;
		private System.Windows.Forms.Button btnRegexDel;
		private System.Windows.Forms.ListBox lbxRegex;
		private System.Windows.Forms.Button btnRegexAdd;
		private System.Windows.Forms.Button btnRegexChange;
		private System.Windows.Forms.GroupBox gbxAppearance;
		private System.Windows.Forms.Button btnAcceptChanges;
		private System.Windows.Forms.TextBox tbxNewExtension;
		private System.Windows.Forms.Button btnExtensionDel;
		private System.Windows.Forms.Button btnExtensionAdd;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button btnTagTypeUp;
		private System.Windows.Forms.Button btnTagTypeDown;
		private System.Windows.Forms.Button btnTagTypeAdd;
		private System.Windows.Forms.Button btnTagTypeDel;
		private System.Windows.Forms.Button btnForeColor;
		private System.Windows.Forms.PictureBox pbxIcon;
		private System.Windows.Forms.Button btnIcon;
		private System.Windows.Forms.TextBox tbxIcon;
		private System.Windows.Forms.Label lblIcon;
		private System.Windows.Forms.Label lblShow;
		private System.Windows.Forms.ListBox lbxTagTypes;
		private System.Windows.Forms.CheckBox cbxShow;
		private System.Windows.Forms.TextBox tbxDescription;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.ListBox lbxExtensions;
		private System.Windows.Forms.GroupBox gbxTagType;
		private System.Windows.Forms.GroupBox gbxLanguage;
		private System.Windows.Forms.ListBox lbxLanguages;
		private System.Windows.Forms.Button btnOK;
	}
}
