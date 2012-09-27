namespace SourceCookifier
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.btnTvBackColor = new System.Windows.Forms.Button();
            this.cbxShowInvalidSources = new System.Windows.Forms.CheckBox();
            this.cbxCaseSensitiveExt = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.cbxShowToolTips = new System.Windows.Forms.CheckBox();
            this.cbxGoToDefByCtrlAndLmButton = new System.Windows.Forms.CheckBox();
            this.tbxFileSizeLimit = new System.Windows.Forms.TextBox();
            this.lblFileSizeLimit = new System.Windows.Forms.Label();
            this.cbxShowMenuAtBottom = new System.Windows.Forms.CheckBox();
            this.cbxFlowingMenuButtons = new System.Windows.Forms.CheckBox();
            this.lblStartupSessionMode = new System.Windows.Forms.Label();
            this.cbxStartupSessionMode = new System.Windows.Forms.ComboBox();
            this.cbxStartupShowMode = new System.Windows.Forms.ComboBox();
            this.lblStartupShowMode = new System.Windows.Forms.Label();
            this.cbxHandleCtagsWarnings = new System.Windows.Forms.CheckBox();
            this.cbxSaveRelativePaths = new System.Windows.Forms.CheckBox();
            this.lblSessionFileExt = new System.Windows.Forms.Label();
            this.tbxSessionFileExt = new System.Windows.Forms.TextBox();
            this.gbxGeneral = new System.Windows.Forms.GroupBox();
            this.cbxShowGtdToolTips = new System.Windows.Forms.CheckBox();
            this.lblMaxNumShownSymbols = new System.Windows.Forms.Label();
            this.tbxMaxNumShownSymbols = new System.Windows.Forms.TextBox();
            this.lblFileSizeLimitInc = new System.Windows.Forms.Label();
            this.tbxFileSizeLimitInc = new System.Windows.Forms.TextBox();
            this.gbxTreeview = new System.Windows.Forms.GroupBox();
            this.cbxShowPlusMinusSource = new System.Windows.Forms.CheckBox();
            this.cbxShowPlusMinusTags = new System.Windows.Forms.CheckBox();
            this.gbxToolbar = new System.Windows.Forms.GroupBox();
            this.gbxCtags = new System.Windows.Forms.GroupBox();
            this.gbxSession = new System.Windows.Forms.GroupBox();
            this.ttOptions = new System.Windows.Forms.ToolTip(this.components);
            this.gbxGeneral.SuspendLayout();
            this.gbxTreeview.SuspendLayout();
            this.gbxToolbar.SuspendLayout();
            this.gbxCtags.SuspendLayout();
            this.gbxSession.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTvBackColor
            // 
            this.btnTvBackColor.Location = new System.Drawing.Point(33, 20);
            this.btnTvBackColor.Name = "btnTvBackColor";
            this.btnTvBackColor.Size = new System.Drawing.Size(251, 21);
            this.btnTvBackColor.TabIndex = 0;
            this.btnTvBackColor.Text = "Treeview background color";
            this.ttOptions.SetToolTip(this.btnTvBackColor, "It is recommended to not use a bright treeview background\r\non Windows 7 because o" +
        "f the too bright treenode selection\r\ncolor.");
            this.btnTvBackColor.UseVisualStyleBackColor = true;
            this.btnTvBackColor.Click += new System.EventHandler(this.BtnTvBackColorClick);
            // 
            // cbxShowInvalidSources
            // 
            this.cbxShowInvalidSources.Location = new System.Drawing.Point(13, 49);
            this.cbxShowInvalidSources.Name = "cbxShowInvalidSources";
            this.cbxShowInvalidSources.Size = new System.Drawing.Size(254, 24);
            this.cbxShowInvalidSources.TabIndex = 1;
            this.cbxShowInvalidSources.Text = "Show nodes of invalid sources";
            this.ttOptions.SetToolTip(this.cbxShowInvalidSources, "Check this if you want to include unsupported\r\nfile extensions in your session fi" +
        "les.");
            this.cbxShowInvalidSources.UseVisualStyleBackColor = true;
            // 
            // cbxCaseSensitiveExt
            // 
            this.cbxCaseSensitiveExt.Location = new System.Drawing.Point(13, 49);
            this.cbxCaseSensitiveExt.Name = "cbxCaseSensitiveExt";
            this.cbxCaseSensitiveExt.Size = new System.Drawing.Size(260, 24);
            this.cbxCaseSensitiveExt.TabIndex = 2;
            this.cbxCaseSensitiveExt.Text = "Case sensitive language<>extension mapping";
            this.ttOptions.SetToolTip(this.cbxCaseSensitiveExt, "Check this if you need to assure case\r\nsensitivity of supported file extensions.");
            this.cbxCaseSensitiveExt.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(471, 344);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(146, 34);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(318, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 34);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblBackColor
            // 
            this.lblBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBackColor.Location = new System.Drawing.Point(13, 22);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(15, 17);
            this.lblBackColor.TabIndex = 6;
            this.ttOptions.SetToolTip(this.lblBackColor, "It is recommended to not use a bright treeview background\r\non Windows 7 because o" +
        "f the too bright treenode selection\r\ncolor.");
            // 
            // cbxShowToolTips
            // 
            this.cbxShowToolTips.Location = new System.Drawing.Point(13, 77);
            this.cbxShowToolTips.Name = "cbxShowToolTips";
            this.cbxShowToolTips.Size = new System.Drawing.Size(237, 24);
            this.cbxShowToolTips.TabIndex = 7;
            this.cbxShowToolTips.Text = "Show tooltips on nodes";
            this.ttOptions.SetToolTip(this.cbxShowToolTips, "Show various infos in the toolips of the tree nodes.\r\nSource node... full file pa" +
        "th\r\nTag type node (grouped view)... number of tags\r\nTag node... access type, sco" +
        "pe, signature, etc.");
            this.cbxShowToolTips.UseVisualStyleBackColor = true;
            // 
            // cbxGoToDefByCtrlAndLmButton
            // 
            this.cbxGoToDefByCtrlAndLmButton.Location = new System.Drawing.Point(13, 129);
            this.cbxGoToDefByCtrlAndLmButton.Name = "cbxGoToDefByCtrlAndLmButton";
            this.cbxGoToDefByCtrlAndLmButton.Size = new System.Drawing.Size(281, 24);
            this.cbxGoToDefByCtrlAndLmButton.TabIndex = 8;
            this.cbxGoToDefByCtrlAndLmButton.Text = "Go to definition by pressing ctrl + left mousebutton";
            this.ttOptions.SetToolTip(this.cbxGoToDefByCtrlAndLmButton, "Simply left-click on a word while pressing the CTRL-key in order to jump to its d" +
        "efinition.");
            this.cbxGoToDefByCtrlAndLmButton.UseVisualStyleBackColor = true;
            // 
            // tbxFileSizeLimit
            // 
            this.tbxFileSizeLimit.Location = new System.Drawing.Point(240, 75);
            this.tbxFileSizeLimit.Name = "tbxFileSizeLimit";
            this.tbxFileSizeLimit.Size = new System.Drawing.Size(44, 20);
            this.tbxFileSizeLimit.TabIndex = 9;
            this.tbxFileSizeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttOptions.SetToolTip(this.tbxFileSizeLimit, "Don\'t analyze source files, which have a bigger\r\nfilesize than the defined one (e" +
        ".g. \"200kb\" or\r\n\"5mb\"). Leave it empty to set no limit.");
            // 
            // lblFileSizeLimit
            // 
            this.lblFileSizeLimit.Location = new System.Drawing.Point(10, 73);
            this.lblFileSizeLimit.Name = "lblFileSizeLimit";
            this.lblFileSizeLimit.Size = new System.Drawing.Size(177, 23);
            this.lblFileSizeLimit.TabIndex = 10;
            this.lblFileSizeLimit.Text = "Size limit of analyzed source files";
            this.lblFileSizeLimit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblFileSizeLimit, "Don\'t analyze source files, which have a bigger\r\nfilesize than the defined one (e" +
        ".g. \"200kb\" or\r\n\"5mb\"). Leave it empty to set no limit.");
            // 
            // cbxShowMenuAtBottom
            // 
            this.cbxShowMenuAtBottom.Location = new System.Drawing.Point(13, 20);
            this.cbxShowMenuAtBottom.Name = "cbxShowMenuAtBottom";
            this.cbxShowMenuAtBottom.Size = new System.Drawing.Size(254, 24);
            this.cbxShowMenuAtBottom.TabIndex = 11;
            this.cbxShowMenuAtBottom.Text = "Show toolbar menu at bottom";
            this.ttOptions.SetToolTip(this.cbxShowMenuAtBottom, "Check this if you want to see the panel\'s\r\ntoolbar at the bottom of the panel.");
            this.cbxShowMenuAtBottom.UseVisualStyleBackColor = true;
            // 
            // cbxFlowingMenuButtons
            // 
            this.cbxFlowingMenuButtons.Location = new System.Drawing.Point(13, 49);
            this.cbxFlowingMenuButtons.Name = "cbxFlowingMenuButtons";
            this.cbxFlowingMenuButtons.Size = new System.Drawing.Size(259, 24);
            this.cbxFlowingMenuButtons.TabIndex = 12;
            this.cbxFlowingMenuButtons.Text = "Flowing menu buttons (no hidden overflow)";
            this.ttOptions.SetToolTip(this.cbxFlowingMenuButtons, "Check this if you need to use a very tiny\r\ntreeview panel with a small toolbar.");
            this.cbxFlowingMenuButtons.UseVisualStyleBackColor = true;
            // 
            // lblStartupSessionMode
            // 
            this.lblStartupSessionMode.Location = new System.Drawing.Point(10, 17);
            this.lblStartupSessionMode.Name = "lblStartupSessionMode";
            this.lblStartupSessionMode.Size = new System.Drawing.Size(124, 23);
            this.lblStartupSessionMode.TabIndex = 13;
            this.lblStartupSessionMode.Text = "Startup session mode";
            this.lblStartupSessionMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblStartupSessionMode, "Always use the selected session mode when Notepad++\r\nstarts. Leaving the combobox" +
        " empty means remembering\r\nthe current session mode when Notepad++ shuts down.");
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
            this.cbxStartupSessionMode.Location = new System.Drawing.Point(180, 19);
            this.cbxStartupSessionMode.Name = "cbxStartupSessionMode";
            this.cbxStartupSessionMode.Size = new System.Drawing.Size(104, 21);
            this.cbxStartupSessionMode.TabIndex = 14;
            this.ttOptions.SetToolTip(this.cbxStartupSessionMode, "Always use the selected session mode when Notepad++\r\nstarts. Leaving the combobox" +
        " empty means remembering\r\nthe current session mode when Notepad++ shuts down.");
            // 
            // cbxStartupShowMode
            // 
            this.cbxStartupShowMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStartupShowMode.FormattingEnabled = true;
            this.cbxStartupShowMode.Items.AddRange(new object[] {
            "",
            "Show",
            "Hide"});
            this.cbxStartupShowMode.Location = new System.Drawing.Point(180, 20);
            this.cbxStartupShowMode.Name = "cbxStartupShowMode";
            this.cbxStartupShowMode.Size = new System.Drawing.Size(104, 21);
            this.cbxStartupShowMode.TabIndex = 16;
            this.ttOptions.SetToolTip(this.cbxStartupShowMode, "Always show or always hide the SourceCookifier\r\npanel when Notepad++ starts. Leav" +
        "ing the combobox\r\nempty means remembering the current panel state\r\nwhen Notepad+" +
        "+ shuts down.");
            // 
            // lblStartupShowMode
            // 
            this.lblStartupShowMode.Location = new System.Drawing.Point(10, 18);
            this.lblStartupShowMode.Name = "lblStartupShowMode";
            this.lblStartupShowMode.Size = new System.Drawing.Size(124, 23);
            this.lblStartupShowMode.TabIndex = 15;
            this.lblStartupShowMode.Text = "Startup panel state";
            this.lblStartupShowMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblStartupShowMode, "Always show or always hide the SourceCookifier\r\npanel when Notepad++ starts. Leav" +
        "ing the combobox\r\nempty means remembering the current panel state\r\nwhen Notepad+" +
        "+ shuts down.");
            // 
            // cbxHandleCtagsWarnings
            // 
            this.cbxHandleCtagsWarnings.Location = new System.Drawing.Point(13, 19);
            this.cbxHandleCtagsWarnings.Name = "cbxHandleCtagsWarnings";
            this.cbxHandleCtagsWarnings.Size = new System.Drawing.Size(237, 24);
            this.cbxHandleCtagsWarnings.TabIndex = 17;
            this.cbxHandleCtagsWarnings.Text = "Handle CTags warnings as errors";
            this.ttOptions.SetToolTip(this.cbxHandleCtagsWarnings, resources.GetString("cbxHandleCtagsWarnings.ToolTip"));
            this.cbxHandleCtagsWarnings.UseVisualStyleBackColor = true;
            // 
            // cbxSaveRelativePaths
            // 
            this.cbxSaveRelativePaths.Location = new System.Drawing.Point(13, 72);
            this.cbxSaveRelativePaths.Name = "cbxSaveRelativePaths";
            this.cbxSaveRelativePaths.Size = new System.Drawing.Size(254, 24);
            this.cbxSaveRelativePaths.TabIndex = 18;
            this.cbxSaveRelativePaths.Text = "Use relative paths in session file";
            this.ttOptions.SetToolTip(this.cbxSaveRelativePaths, resources.GetString("cbxSaveRelativePaths.ToolTip"));
            this.cbxSaveRelativePaths.UseVisualStyleBackColor = true;
            // 
            // lblSessionFileExt
            // 
            this.lblSessionFileExt.Location = new System.Drawing.Point(10, 45);
            this.lblSessionFileExt.Name = "lblSessionFileExt";
            this.lblSessionFileExt.Size = new System.Drawing.Size(177, 23);
            this.lblSessionFileExt.TabIndex = 20;
            this.lblSessionFileExt.Text = "Session file extension";
            this.lblSessionFileExt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblSessionFileExt, "By default set to \"c00k!e\", but if you can\'t leave with it for some\r\nreason, than" +
        " you can define your own extension, but don\'t\r\nprefix the extension string with " +
        "a \".\"");
            // 
            // tbxSessionFileExt
            // 
            this.tbxSessionFileExt.Location = new System.Drawing.Point(225, 47);
            this.tbxSessionFileExt.Name = "tbxSessionFileExt";
            this.tbxSessionFileExt.Size = new System.Drawing.Size(59, 20);
            this.tbxSessionFileExt.TabIndex = 19;
            this.tbxSessionFileExt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttOptions.SetToolTip(this.tbxSessionFileExt, "By default set to \"c00k!e\", but if you can\'t leave with it for some\r\nreason, then" +
        " you can define your own extension, but don\'t\r\nprefix the extension string with " +
        "a \".\"");
            // 
            // gbxGeneral
            // 
            this.gbxGeneral.Controls.Add(this.cbxShowGtdToolTips);
            this.gbxGeneral.Controls.Add(this.lblMaxNumShownSymbols);
            this.gbxGeneral.Controls.Add(this.tbxMaxNumShownSymbols);
            this.gbxGeneral.Controls.Add(this.lblFileSizeLimitInc);
            this.gbxGeneral.Controls.Add(this.tbxFileSizeLimitInc);
            this.gbxGeneral.Controls.Add(this.lblStartupShowMode);
            this.gbxGeneral.Controls.Add(this.cbxStartupShowMode);
            this.gbxGeneral.Controls.Add(this.lblFileSizeLimit);
            this.gbxGeneral.Controls.Add(this.tbxFileSizeLimit);
            this.gbxGeneral.Controls.Add(this.cbxGoToDefByCtrlAndLmButton);
            this.gbxGeneral.Location = new System.Drawing.Point(12, 12);
            this.gbxGeneral.Name = "gbxGeneral";
            this.gbxGeneral.Size = new System.Drawing.Size(299, 191);
            this.gbxGeneral.TabIndex = 21;
            this.gbxGeneral.TabStop = false;
            this.gbxGeneral.Text = "General";
            // 
            // cbxShowGtdToolTips
            // 
            this.cbxShowGtdToolTips.Location = new System.Drawing.Point(13, 158);
            this.cbxShowGtdToolTips.Name = "cbxShowGtdToolTips";
            this.cbxShowGtdToolTips.Size = new System.Drawing.Size(261, 24);
            this.cbxShowGtdToolTips.TabIndex = 21;
            this.cbxShowGtdToolTips.Text = "Show tooltips in GoToDefinition contextmenu";
            this.ttOptions.SetToolTip(this.cbxShowGtdToolTips, resources.GetString("cbxShowGtdToolTips.ToolTip"));
            this.cbxShowGtdToolTips.UseVisualStyleBackColor = true;
            // 
            // lblMaxNumShownSymbols
            // 
            this.lblMaxNumShownSymbols.Location = new System.Drawing.Point(10, 45);
            this.lblMaxNumShownSymbols.Name = "lblMaxNumShownSymbols";
            this.lblMaxNumShownSymbols.Size = new System.Drawing.Size(184, 23);
            this.lblMaxNumShownSymbols.TabIndex = 20;
            this.lblMaxNumShownSymbols.Text = "Max. number of symbols to show";
            this.lblMaxNumShownSymbols.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblMaxNumShownSymbols, resources.GetString("lblMaxNumShownSymbols.ToolTip"));
            // 
            // tbxMaxNumShownSymbols
            // 
            this.tbxMaxNumShownSymbols.Location = new System.Drawing.Point(205, 47);
            this.tbxMaxNumShownSymbols.Name = "tbxMaxNumShownSymbols";
            this.tbxMaxNumShownSymbols.Size = new System.Drawing.Size(79, 20);
            this.tbxMaxNumShownSymbols.TabIndex = 19;
            this.tbxMaxNumShownSymbols.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttOptions.SetToolTip(this.tbxMaxNumShownSymbols, resources.GetString("tbxMaxNumShownSymbols.ToolTip"));
            // 
            // lblFileSizeLimitInc
            // 
            this.lblFileSizeLimitInc.Location = new System.Drawing.Point(10, 101);
            this.lblFileSizeLimitInc.Name = "lblFileSizeLimitInc";
            this.lblFileSizeLimitInc.Size = new System.Drawing.Size(177, 23);
            this.lblFileSizeLimitInc.TabIndex = 18;
            this.lblFileSizeLimitInc.Text = "Size limit of analyzed INCLUDES";
            this.lblFileSizeLimitInc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ttOptions.SetToolTip(this.lblFileSizeLimitInc, "Don\'t analyze files in the INCLUDES node,\r\nwhich have a bigger filesize than the\r" +
        "\ndefined one (e.g. \"200kb\" or \"5mb\").\r\nLeave it empty to set no limit.");
            // 
            // tbxFileSizeLimitInc
            // 
            this.tbxFileSizeLimitInc.Location = new System.Drawing.Point(240, 103);
            this.tbxFileSizeLimitInc.Name = "tbxFileSizeLimitInc";
            this.tbxFileSizeLimitInc.Size = new System.Drawing.Size(44, 20);
            this.tbxFileSizeLimitInc.TabIndex = 17;
            this.tbxFileSizeLimitInc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttOptions.SetToolTip(this.tbxFileSizeLimitInc, "Don\'t analyze files in the INCLUDES node,\r\nwhich have a bigger filesize than the\r" +
        "\ndefined one (e.g. \"200kb\" or \"5mb\").\r\nLeave it empty to set no limit.");
            // 
            // gbxTreeview
            // 
            this.gbxTreeview.Controls.Add(this.cbxShowPlusMinusSource);
            this.gbxTreeview.Controls.Add(this.cbxShowPlusMinusTags);
            this.gbxTreeview.Controls.Add(this.btnTvBackColor);
            this.gbxTreeview.Controls.Add(this.lblBackColor);
            this.gbxTreeview.Controls.Add(this.cbxShowInvalidSources);
            this.gbxTreeview.Controls.Add(this.cbxShowToolTips);
            this.gbxTreeview.Location = new System.Drawing.Point(12, 209);
            this.gbxTreeview.Name = "gbxTreeview";
            this.gbxTreeview.Size = new System.Drawing.Size(299, 169);
            this.gbxTreeview.TabIndex = 22;
            this.gbxTreeview.TabStop = false;
            this.gbxTreeview.Text = "Treeview";
            // 
            // cbxShowPlusMinusSource
            // 
            this.cbxShowPlusMinusSource.Location = new System.Drawing.Point(13, 106);
            this.cbxShowPlusMinusSource.Name = "cbxShowPlusMinusSource";
            this.cbxShowPlusMinusSource.Size = new System.Drawing.Size(257, 24);
            this.cbxShowPlusMinusSource.TabIndex = 9;
            this.cbxShowPlusMinusSource.Text = "Show plus minus controls (for source nodes)";
            this.ttOptions.SetToolTip(this.cbxShowPlusMinusSource, "Show the plus minus control in front of source nodes when they contain subnodes.");
            this.cbxShowPlusMinusSource.UseVisualStyleBackColor = true;
            // 
            // cbxShowPlusMinusTags
            // 
            this.cbxShowPlusMinusTags.Location = new System.Drawing.Point(13, 135);
            this.cbxShowPlusMinusTags.Name = "cbxShowPlusMinusTags";
            this.cbxShowPlusMinusTags.Size = new System.Drawing.Size(257, 24);
            this.cbxShowPlusMinusTags.TabIndex = 8;
            this.cbxShowPlusMinusTags.Text = "Show plus minus controls (for tag nodes)";
            this.ttOptions.SetToolTip(this.cbxShowPlusMinusTags, "Show the plus minus control in front of tag nodes when they contain subnodes.");
            this.cbxShowPlusMinusTags.UseVisualStyleBackColor = true;
            // 
            // gbxToolbar
            // 
            this.gbxToolbar.Controls.Add(this.cbxShowMenuAtBottom);
            this.gbxToolbar.Controls.Add(this.cbxFlowingMenuButtons);
            this.gbxToolbar.Location = new System.Drawing.Point(318, 121);
            this.gbxToolbar.Name = "gbxToolbar";
            this.gbxToolbar.Size = new System.Drawing.Size(299, 82);
            this.gbxToolbar.TabIndex = 22;
            this.gbxToolbar.TabStop = false;
            this.gbxToolbar.Text = "Toolbar";
            // 
            // gbxCtags
            // 
            this.gbxCtags.Controls.Add(this.cbxHandleCtagsWarnings);
            this.gbxCtags.Controls.Add(this.cbxCaseSensitiveExt);
            this.gbxCtags.Location = new System.Drawing.Point(318, 209);
            this.gbxCtags.Name = "gbxCtags";
            this.gbxCtags.Size = new System.Drawing.Size(299, 82);
            this.gbxCtags.TabIndex = 23;
            this.gbxCtags.TabStop = false;
            this.gbxCtags.Text = "CTags";
            // 
            // gbxSession
            // 
            this.gbxSession.Controls.Add(this.cbxStartupSessionMode);
            this.gbxSession.Controls.Add(this.lblStartupSessionMode);
            this.gbxSession.Controls.Add(this.tbxSessionFileExt);
            this.gbxSession.Controls.Add(this.lblSessionFileExt);
            this.gbxSession.Controls.Add(this.cbxSaveRelativePaths);
            this.gbxSession.Location = new System.Drawing.Point(318, 12);
            this.gbxSession.Name = "gbxSession";
            this.gbxSession.Size = new System.Drawing.Size(299, 103);
            this.gbxSession.TabIndex = 24;
            this.gbxSession.TabStop = false;
            this.gbxSession.Text = "Session";
            // 
            // ttOptions
            // 
            this.ttOptions.AutomaticDelay = 0;
            this.ttOptions.AutoPopDelay = 10000;
            this.ttOptions.InitialDelay = 0;
            this.ttOptions.ReshowDelay = 0;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 390);
            this.Controls.Add(this.gbxSession);
            this.Controls.Add(this.gbxCtags);
            this.Controls.Add(this.gbxToolbar);
            this.Controls.Add(this.gbxTreeview);
            this.Controls.Add(this.gbxGeneral);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.gbxGeneral.ResumeLayout(false);
            this.gbxGeneral.PerformLayout();
            this.gbxTreeview.ResumeLayout(false);
            this.gbxToolbar.ResumeLayout(false);
            this.gbxCtags.ResumeLayout(false);
            this.gbxSession.ResumeLayout(false);
            this.gbxSession.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label lblStartupShowMode;
		internal System.Windows.Forms.ComboBox cbxStartupShowMode;
		internal System.Windows.Forms.ComboBox cbxStartupSessionMode;
		private System.Windows.Forms.Label lblStartupSessionMode;
		internal System.Windows.Forms.CheckBox cbxFlowingMenuButtons;
		internal System.Windows.Forms.CheckBox cbxShowMenuAtBottom;
		internal System.Windows.Forms.TextBox tbxFileSizeLimit;
		private System.Windows.Forms.Label lblFileSizeLimit;
		internal System.Windows.Forms.CheckBox cbxGoToDefByCtrlAndLmButton;
		internal System.Windows.Forms.CheckBox cbxShowToolTips;
		internal System.Windows.Forms.CheckBox cbxCaseSensitiveExt;
		private System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblBackColor;
		private System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.CheckBox cbxShowInvalidSources;
		private System.Windows.Forms.Button btnTvBackColor;
		internal System.Windows.Forms.CheckBox cbxHandleCtagsWarnings;
		internal System.Windows.Forms.CheckBox cbxSaveRelativePaths;
		private System.Windows.Forms.Label lblSessionFileExt;
		internal System.Windows.Forms.TextBox tbxSessionFileExt;
		private System.Windows.Forms.GroupBox gbxGeneral;
		private System.Windows.Forms.GroupBox gbxTreeview;
		private System.Windows.Forms.GroupBox gbxToolbar;
		private System.Windows.Forms.GroupBox gbxCtags;
		private System.Windows.Forms.GroupBox gbxSession;
		private System.Windows.Forms.ToolTip ttOptions;
		internal System.Windows.Forms.CheckBox cbxShowPlusMinusTags;
		private System.Windows.Forms.Label lblFileSizeLimitInc;
		internal System.Windows.Forms.TextBox tbxFileSizeLimitInc;
        private System.Windows.Forms.Label lblMaxNumShownSymbols;
        internal System.Windows.Forms.TextBox tbxMaxNumShownSymbols;
        internal System.Windows.Forms.CheckBox cbxShowPlusMinusSource;
        internal System.Windows.Forms.CheckBox cbxShowGtdToolTips;
	}
}
