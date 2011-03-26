
namespace NppPluginNET
{
	public partial class frmMain
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
			this.tvTags = new System.Windows.Forms.TreeView();
			this.cmsTv = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiShow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsShow = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiTrackCaret = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTrackCaret = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiDisplay = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsDisplay = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiAccess = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiReturnType = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiScope = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSignature = new System.Windows.Forms.ToolStripMenuItem();
			this.tssTv = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiMove = new System.Windows.Forms.ToolStripMenuItem();
			this.tsBar = new System.Windows.Forms.ToolStrip();
			this.tsBtnSearchMode = new System.Windows.Forms.ToolStripButton();
			this.tsTbxSearchFilter = new System.Windows.Forms.ToolStripTextBox();
			this.tsBtnClearSearchFilter = new System.Windows.Forms.ToolStripButton();
			this.tsDdbtnSession = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiSingleFileMode = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiNppSessionMode = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiCookieSessionMode = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionSave = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionClear = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsDdbtnView = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiFlatView = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFlatGroupedView = new System.Windows.Forms.ToolStripMenuItem();
			this.tssView1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiGroupedView = new System.Windows.Forms.ToolStripMenuItem();
			this.tssView2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClassViewSingle = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClassViewSession = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiView3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiShowIcons = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAlphabeticalSort = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFold = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLevel1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLevel2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLevel3 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsBtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.tsDdbtnSettings = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiLanguageSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSettings1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.tsProgress = new System.Windows.Forms.ToolStrip();
			this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.ttTv = new System.Windows.Forms.ToolTip(this.components);
			this.cmsTv.SuspendLayout();
			this.cmsDisplay.SuspendLayout();
			this.tsBar.SuspendLayout();
			this.tsProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvTags
			// 
			this.tvTags.AllowDrop = true;
			this.tvTags.ContextMenuStrip = this.cmsTv;
			this.tvTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvTags.HideSelection = false;
			this.tvTags.Indent = 19;
			this.tvTags.Location = new System.Drawing.Point(0, 26);
			this.tvTags.Name = "tvTags";
			this.tvTags.ShowPlusMinus = false;
			this.tvTags.ShowRootLines = false;
			this.tvTags.Size = new System.Drawing.Size(243, 448);
			this.tvTags.TabIndex = 0;
			this.tvTags.DragDrop += new System.Windows.Forms.DragEventHandler(this.TvTagsDragDrop);
			this.tvTags.DragEnter += new System.Windows.Forms.DragEventHandler(this.TvTagsDragEnter);
			this.tvTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			this.tvTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.tvTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseDoubleClick);
			this.tvTags.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseDown);
			this.tvTags.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseMove);
			this.tvTags.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseUp);
			// 
			// cmsTv
			// 
			this.cmsTv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiShow,
									this.tsmiTrackCaret,
									this.tsmiDisplay,
									this.tssTv,
									this.tsmiRemove,
									this.tsmiMove});
			this.cmsTv.Name = "cmsTv";
			this.cmsTv.ShowImageMargin = false;
			this.cmsTv.ShowItemToolTips = false;
			this.cmsTv.Size = new System.Drawing.Size(147, 120);
			this.cmsTv.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.CmsTvClosed);
			this.cmsTv.Opening += new System.ComponentModel.CancelEventHandler(this.CmsTvOpening);
			this.cmsTv.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsTvItemClicked);
			// 
			// tsmiShow
			// 
			this.tsmiShow.DropDown = this.cmsShow;
			this.tsmiShow.Name = "tsmiShow";
			this.tsmiShow.Size = new System.Drawing.Size(146, 22);
			this.tsmiShow.Text = "Show";
			// 
			// cmsShow
			// 
			this.cmsShow.Name = "cmsShow";
			this.cmsShow.OwnerItem = this.tsmiShow;
			this.cmsShow.ShowCheckMargin = true;
			this.cmsShow.Size = new System.Drawing.Size(83, 4);
			this.cmsShow.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsTvItemClicked);
			// 
			// tsmiTrackCaret
			// 
			this.tsmiTrackCaret.DropDown = this.cmsTrackCaret;
			this.tsmiTrackCaret.Name = "tsmiTrackCaret";
			this.tsmiTrackCaret.Size = new System.Drawing.Size(146, 22);
			this.tsmiTrackCaret.Text = "Track caret";
			// 
			// cmsTrackCaret
			// 
			this.cmsTrackCaret.Name = "cmsShow";
			this.cmsTrackCaret.OwnerItem = this.tsmiTrackCaret;
			this.cmsTrackCaret.ShowCheckMargin = true;
			this.cmsTrackCaret.Size = new System.Drawing.Size(83, 4);
			this.cmsTrackCaret.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsTvItemClicked);
			// 
			// tsmiDisplay
			// 
			this.tsmiDisplay.DropDown = this.cmsDisplay;
			this.tsmiDisplay.Name = "tsmiDisplay";
			this.tsmiDisplay.Size = new System.Drawing.Size(146, 22);
			this.tsmiDisplay.Text = "Display (extended)";
			// 
			// cmsDisplay
			// 
			this.cmsDisplay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiAccess,
									this.tsmiReturnType,
									this.tsmiScope,
									this.tsmiSignature});
			this.cmsDisplay.Name = "cmsShow";
			this.cmsDisplay.OwnerItem = this.tsmiDisplay;
			this.cmsDisplay.ShowCheckMargin = true;
			this.cmsDisplay.ShowImageMargin = false;
			this.cmsDisplay.Size = new System.Drawing.Size(133, 92);
			this.cmsDisplay.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsTvItemClicked);
			// 
			// tsmiAccess
			// 
			this.tsmiAccess.Name = "tsmiAccess";
			this.tsmiAccess.Size = new System.Drawing.Size(132, 22);
			this.tsmiAccess.Text = "Access";
			// 
			// tsmiReturnType
			// 
			this.tsmiReturnType.Name = "tsmiReturnType";
			this.tsmiReturnType.Size = new System.Drawing.Size(132, 22);
			this.tsmiReturnType.Text = "Returntype";
			// 
			// tsmiScope
			// 
			this.tsmiScope.Name = "tsmiScope";
			this.tsmiScope.Size = new System.Drawing.Size(132, 22);
			this.tsmiScope.Text = "Scope";
			// 
			// tsmiSignature
			// 
			this.tsmiSignature.Name = "tsmiSignature";
			this.tsmiSignature.Size = new System.Drawing.Size(132, 22);
			this.tsmiSignature.Text = "Signature";
			// 
			// tssTv
			// 
			this.tssTv.Name = "tssTv";
			this.tssTv.Size = new System.Drawing.Size(143, 6);
			this.tssTv.Visible = false;
			// 
			// tsmiRemove
			// 
			this.tsmiRemove.Name = "tsmiRemove";
			this.tsmiRemove.Size = new System.Drawing.Size(146, 22);
			this.tsmiRemove.Text = "Remove";
			this.tsmiRemove.Visible = false;
			// 
			// tsmiMove
			// 
			this.tsmiMove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsmiMove.Name = "tsmiMove";
			this.tsmiMove.Size = new System.Drawing.Size(146, 22);
			this.tsmiMove.Visible = false;
			// 
			// tsBar
			// 
			this.tsBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsBtnSearchMode,
									this.tsTbxSearchFilter,
									this.tsBtnClearSearchFilter,
									this.tsDdbtnSession,
									this.tsDdbtnView,
									this.tsBtnRefresh,
									this.tsDdbtnSettings});
			this.tsBar.Location = new System.Drawing.Point(0, 0);
			this.tsBar.Name = "tsBar";
			this.tsBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsBar.Size = new System.Drawing.Size(243, 26);
			this.tsBar.TabIndex = 1;
			this.tsBar.TabStop = true;
			// 
			// tsBtnSearchMode
			// 
			this.tsBtnSearchMode.CheckOnClick = true;
			this.tsBtnSearchMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnSearchMode.Image = global::NppPluginNET.Properties.Resources.search_tags;
			this.tsBtnSearchMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsBtnSearchMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnSearchMode.Name = "tsBtnSearchMode";
			this.tsBtnSearchMode.Size = new System.Drawing.Size(23, 23);
			this.tsBtnSearchMode.ToolTipText = "Tag search mode";
			this.tsBtnSearchMode.Click += new System.EventHandler(this.TsBtnSearchModeClick);
			// 
			// tsTbxSearchFilter
			// 
			this.tsTbxSearchFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tsTbxSearchFilter.Name = "tsTbxSearchFilter";
			this.tsTbxSearchFilter.Size = new System.Drawing.Size(100, 26);
			this.tsTbxSearchFilter.ToolTipText = "Search";
			this.tsTbxSearchFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			this.tsTbxSearchFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.tsTbxSearchFilter.TextChanged += new System.EventHandler(this.TsTbxSearchFilterTextChanged);
			// 
			// tsBtnClearSearchFilter
			// 
			this.tsBtnClearSearchFilter.AutoSize = false;
			this.tsBtnClearSearchFilter.BackColor = System.Drawing.SystemColors.Window;
			this.tsBtnClearSearchFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnClearSearchFilter.Image = global::NppPluginNET.Properties.Resources.cancel;
			this.tsBtnClearSearchFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsBtnClearSearchFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnClearSearchFilter.Name = "tsBtnClearSearchFilter";
			this.tsBtnClearSearchFilter.Size = new System.Drawing.Size(12, 23);
			this.tsBtnClearSearchFilter.ToolTipText = "Clear search filter";
			this.tsBtnClearSearchFilter.Click += new System.EventHandler(this.TsBtnClearSearchFilterClick);
			this.tsBtnClearSearchFilter.Paint += new System.Windows.Forms.PaintEventHandler(this.TsBtnClearSearchFilterPaint);
			// 
			// tsDdbtnSession
			// 
			this.tsDdbtnSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsDdbtnSession.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiSingleFileMode,
									this.tssSession1,
									this.tsmiNppSessionMode,
									this.tssSession2,
									this.tsmiCookieSessionMode,
									this.tsmiSessionLoad,
									this.tsmiSessionSave,
									this.tsmiSessionClear,
									this.tssSession3});
			this.tsDdbtnSession.Image = global::NppPluginNET.Properties.Resources.session_mode_none;
			this.tsDdbtnSession.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsDdbtnSession.Name = "tsDdbtnSession";
			this.tsDdbtnSession.ShowDropDownArrow = false;
			this.tsDdbtnSession.Size = new System.Drawing.Size(20, 23);
			this.tsDdbtnSession.ToolTipText = "Session";
			this.tsDdbtnSession.DropDownClosed += new System.EventHandler(this.TsDdbtnSessionDropDownClosed);
			this.tsDdbtnSession.DropDownOpening += new System.EventHandler(this.TsDdbtnSessionDropDownOpening);
			this.tsDdbtnSession.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TsDdbtnSessionDropDownItemClicked);
			// 
			// tsmiSingleFileMode
			// 
			this.tsmiSingleFileMode.Image = global::NppPluginNET.Properties.Resources.session_mode_none;
			this.tsmiSingleFileMode.Name = "tsmiSingleFileMode";
			this.tsmiSingleFileMode.Size = new System.Drawing.Size(186, 22);
			this.tsmiSingleFileMode.Text = "Single file mode";
			// 
			// tssSession1
			// 
			this.tssSession1.Name = "tssSession1";
			this.tssSession1.Size = new System.Drawing.Size(183, 6);
			// 
			// tsmiNppSessionMode
			// 
			this.tsmiNppSessionMode.Image = global::NppPluginNET.Properties.Resources.session_mode_npp;
			this.tsmiNppSessionMode.Name = "tsmiNppSessionMode";
			this.tsmiNppSessionMode.Size = new System.Drawing.Size(186, 22);
			this.tsmiNppSessionMode.Text = "N++ session mode";
			// 
			// tssSession2
			// 
			this.tssSession2.Name = "tssSession2";
			this.tssSession2.Size = new System.Drawing.Size(183, 6);
			// 
			// tsmiCookieSessionMode
			// 
			this.tsmiCookieSessionMode.Image = global::NppPluginNET.Properties.Resources.session_mode_sc;
			this.tsmiCookieSessionMode.Name = "tsmiCookieSessionMode";
			this.tsmiCookieSessionMode.Size = new System.Drawing.Size(186, 22);
			this.tsmiCookieSessionMode.Text = "Cookie session mode";
			// 
			// tsmiSessionLoad
			// 
			this.tsmiSessionLoad.Image = global::NppPluginNET.Properties.Resources.session_load;
			this.tsmiSessionLoad.Name = "tsmiSessionLoad";
			this.tsmiSessionLoad.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionLoad.Text = "Load";
			// 
			// tsmiSessionSave
			// 
			this.tsmiSessionSave.Enabled = false;
			this.tsmiSessionSave.Image = global::NppPluginNET.Properties.Resources.session_save;
			this.tsmiSessionSave.Name = "tsmiSessionSave";
			this.tsmiSessionSave.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionSave.Text = "Save";
			// 
			// tsmiSessionClear
			// 
			this.tsmiSessionClear.Enabled = false;
			this.tsmiSessionClear.Image = global::NppPluginNET.Properties.Resources.session_clear;
			this.tsmiSessionClear.Name = "tsmiSessionClear";
			this.tsmiSessionClear.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionClear.Text = "Clear";
			// 
			// tssSession3
			// 
			this.tssSession3.Name = "tssSession3";
			this.tssSession3.Size = new System.Drawing.Size(183, 6);
			// 
			// tsDdbtnView
			// 
			this.tsDdbtnView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsDdbtnView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiFlatView,
									this.tsmiFlatGroupedView,
									this.tssView1,
									this.tsmiGroupedView,
									this.tssView2,
									this.tsmiClassViewSingle,
									this.tsmiClassViewSession,
									this.tsmiView3,
									this.tsmiShowIcons,
									this.tsmiAlphabeticalSort,
									this.tsmiFold});
			this.tsDdbtnView.Image = global::NppPluginNET.Properties.Resources.flat_view;
			this.tsDdbtnView.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsDdbtnView.Name = "tsDdbtnView";
			this.tsDdbtnView.ShowDropDownArrow = false;
			this.tsDdbtnView.Size = new System.Drawing.Size(20, 23);
			this.tsDdbtnView.ToolTipText = "View";
			this.tsDdbtnView.DropDownClosed += new System.EventHandler(this.TsDdbtnViewDropDownClosed);
			// 
			// tsmiFlatView
			// 
			this.tsmiFlatView.Image = global::NppPluginNET.Properties.Resources.flat_view;
			this.tsmiFlatView.Name = "tsmiFlatView";
			this.tsmiFlatView.Size = new System.Drawing.Size(177, 22);
			this.tsmiFlatView.Text = "Flat view";
			this.tsmiFlatView.Click += new System.EventHandler(this.TsmiFlatViewClick);
			// 
			// tsmiFlatGroupedView
			// 
			this.tsmiFlatGroupedView.Image = global::NppPluginNET.Properties.Resources.flat_grouped_view;
			this.tsmiFlatGroupedView.Name = "tsmiFlatGroupedView";
			this.tsmiFlatGroupedView.Size = new System.Drawing.Size(177, 22);
			this.tsmiFlatGroupedView.Text = "Flat grouped view";
			this.tsmiFlatGroupedView.Click += new System.EventHandler(this.TsmiFlatGroupedViewClick);
			// 
			// tssView1
			// 
			this.tssView1.Name = "tssView1";
			this.tssView1.Size = new System.Drawing.Size(174, 6);
			// 
			// tsmiGroupedView
			// 
			this.tsmiGroupedView.Image = global::NppPluginNET.Properties.Resources.grouped_view;
			this.tsmiGroupedView.Name = "tsmiGroupedView";
			this.tsmiGroupedView.Size = new System.Drawing.Size(177, 22);
			this.tsmiGroupedView.Text = "Grouped view";
			this.tsmiGroupedView.Click += new System.EventHandler(this.TsmiGroupedViewClick);
			// 
			// tssView2
			// 
			this.tssView2.Name = "tssView2";
			this.tssView2.Size = new System.Drawing.Size(174, 6);
			// 
			// tsmiClassViewSingle
			// 
			this.tsmiClassViewSingle.Image = global::NppPluginNET.Properties.Resources.class_view_single;
			this.tsmiClassViewSingle.Name = "tsmiClassViewSingle";
			this.tsmiClassViewSingle.Size = new System.Drawing.Size(177, 22);
			this.tsmiClassViewSingle.Text = "Class view (single)";
			this.tsmiClassViewSingle.Click += new System.EventHandler(this.TsmiClassViewSingleClick);
			// 
			// tsmiClassViewSession
			// 
			this.tsmiClassViewSession.Enabled = false;
			this.tsmiClassViewSession.Image = global::NppPluginNET.Properties.Resources.class_view_session;
			this.tsmiClassViewSession.Name = "tsmiClassViewSession";
			this.tsmiClassViewSession.Size = new System.Drawing.Size(177, 22);
			this.tsmiClassViewSession.Text = "Class view (session)";
			this.tsmiClassViewSession.Click += new System.EventHandler(this.TsmiClassViewSessionClick);
			// 
			// tsmiView3
			// 
			this.tsmiView3.Name = "tsmiView3";
			this.tsmiView3.Size = new System.Drawing.Size(174, 6);
			// 
			// tsmiShowIcons
			// 
			this.tsmiShowIcons.CheckOnClick = true;
			this.tsmiShowIcons.Image = global::NppPluginNET.Properties.Resources.show_icon;
			this.tsmiShowIcons.Name = "tsmiShowIcons";
			this.tsmiShowIcons.Size = new System.Drawing.Size(177, 22);
			this.tsmiShowIcons.Text = "Show icons";
			this.tsmiShowIcons.Click += new System.EventHandler(this.TsmiShowIconsClick);
			// 
			// tsmiAlphabeticalSort
			// 
			this.tsmiAlphabeticalSort.CheckOnClick = true;
			this.tsmiAlphabeticalSort.Image = global::NppPluginNET.Properties.Resources.alphabetical_sort;
			this.tsmiAlphabeticalSort.Name = "tsmiAlphabeticalSort";
			this.tsmiAlphabeticalSort.Size = new System.Drawing.Size(177, 22);
			this.tsmiAlphabeticalSort.Text = "Alphabetical sort";
			this.tsmiAlphabeticalSort.Click += new System.EventHandler(this.TsmiAlphabeticalSortClick);
			// 
			// tsmiFold
			// 
			this.tsmiFold.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiLevel1,
									this.tsmiLevel2,
									this.tsmiLevel3});
			this.tsmiFold.Image = global::NppPluginNET.Properties.Resources.level;
			this.tsmiFold.Name = "tsmiFold";
			this.tsmiFold.Size = new System.Drawing.Size(177, 22);
			this.tsmiFold.Text = "Fold";
			// 
			// tsmiLevel1
			// 
			this.tsmiLevel1.Name = "tsmiLevel1";
			this.tsmiLevel1.Size = new System.Drawing.Size(113, 22);
			this.tsmiLevel1.Text = "Level 1";
			this.tsmiLevel1.Click += new System.EventHandler(this.TsmiLevel1Click);
			// 
			// tsmiLevel2
			// 
			this.tsmiLevel2.Name = "tsmiLevel2";
			this.tsmiLevel2.Size = new System.Drawing.Size(113, 22);
			this.tsmiLevel2.Text = "Level 2";
			this.tsmiLevel2.Click += new System.EventHandler(this.TsmiLevel2Click);
			// 
			// tsmiLevel3
			// 
			this.tsmiLevel3.Name = "tsmiLevel3";
			this.tsmiLevel3.Size = new System.Drawing.Size(113, 22);
			this.tsmiLevel3.Text = "Level N";
			this.tsmiLevel3.Click += new System.EventHandler(this.TsmiLevel3Click);
			// 
			// tsBtnRefresh
			// 
			this.tsBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnRefresh.Image = global::NppPluginNET.Properties.Resources.refresh;
			this.tsBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnRefresh.Name = "tsBtnRefresh";
			this.tsBtnRefresh.Size = new System.Drawing.Size(23, 23);
			this.tsBtnRefresh.ToolTipText = "Refresh";
			this.tsBtnRefresh.Click += new System.EventHandler(this.TsBtnRefreshClick);
			// 
			// tsDdbtnSettings
			// 
			this.tsDdbtnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsDdbtnSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiLanguageSettings,
									this.tsmiOptions,
									this.tssSettings1,
									this.tsmiHelp});
			this.tsDdbtnSettings.Image = global::NppPluginNET.Properties.Resources.settings;
			this.tsDdbtnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsDdbtnSettings.Name = "tsDdbtnSettings";
			this.tsDdbtnSettings.ShowDropDownArrow = false;
			this.tsDdbtnSettings.Size = new System.Drawing.Size(20, 23);
			this.tsDdbtnSettings.ToolTipText = "Settings";
			this.tsDdbtnSettings.DropDownClosed += new System.EventHandler(this.TsDdbtnSettingsDropDownClosed);
			// 
			// tsmiLanguageSettings
			// 
			this.tsmiLanguageSettings.Image = global::NppPluginNET.Properties.Resources.language_settings;
			this.tsmiLanguageSettings.Name = "tsmiLanguageSettings";
			this.tsmiLanguageSettings.Size = new System.Drawing.Size(170, 22);
			this.tsmiLanguageSettings.Text = "Language settings";
			this.tsmiLanguageSettings.Click += new System.EventHandler(this.TsmiLanguageSettingsClick);
			// 
			// tsmiOptions
			// 
			this.tsmiOptions.Image = global::NppPluginNET.Properties.Resources.options;
			this.tsmiOptions.Name = "tsmiOptions";
			this.tsmiOptions.Size = new System.Drawing.Size(170, 22);
			this.tsmiOptions.Text = "Options";
			this.tsmiOptions.Click += new System.EventHandler(this.TsmiOptionsClick);
			// 
			// tssSettings1
			// 
			this.tssSettings1.Name = "tssSettings1";
			this.tssSettings1.Size = new System.Drawing.Size(167, 6);
			// 
			// tsmiHelp
			// 
			this.tsmiHelp.Image = global::NppPluginNET.Properties.Resources.help_1;
			this.tsmiHelp.Name = "tsmiHelp";
			this.tsmiHelp.Size = new System.Drawing.Size(170, 22);
			this.tsmiHelp.Text = "Help";
			this.tsmiHelp.Click += new System.EventHandler(this.TsmiHelpClick);
			// 
			// tsProgress
			// 
			this.tsProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tsProgress.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tspbProgress});
			this.tsProgress.Location = new System.Drawing.Point(0, 449);
			this.tsProgress.Name = "tsProgress";
			this.tsProgress.Size = new System.Drawing.Size(261, 25);
			this.tsProgress.TabIndex = 1;
			this.tsProgress.Text = "toolStrip1";
			this.tsProgress.Visible = false;
			// 
			// tspbProgress
			// 
			this.tspbProgress.AutoSize = false;
			this.tspbProgress.Name = "tspbProgress";
			this.tspbProgress.Size = new System.Drawing.Size(100, 22);
			this.tspbProgress.Step = 1;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(243, 474);
			this.Controls.Add(this.tvTags);
			this.Controls.Add(this.tsBar);
			this.Controls.Add(this.tsProgress);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SourceCookifier";
			this.VisibleChanged += new System.EventHandler(this.FrmMainVisibleChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.cmsTv.ResumeLayout(false);
			this.cmsDisplay.ResumeLayout(false);
			this.tsBar.ResumeLayout(false);
			this.tsBar.PerformLayout();
			this.tsProgress.ResumeLayout(false);
			this.tsProgress.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem tsmiClassViewSession;
		private System.Windows.Forms.ToolStripMenuItem tsmiClassViewSingle;
		private System.Windows.Forms.ToolStripSeparator tsmiView3;
		private System.Windows.Forms.ToolStripMenuItem tsmiFlatGroupedView;
		private System.Windows.Forms.ToolStripSeparator tssSettings1;
		private System.Windows.Forms.ToolStripSeparator tssSession3;
		private System.Windows.Forms.ToolStripButton tsBtnSearchMode;
		private System.Windows.Forms.ToolStripMenuItem tsmiSingleFileMode;
		private System.Windows.Forms.ToolStripMenuItem tsmiNppSessionMode;
		private System.Windows.Forms.ToolStripMenuItem tsmiCookieSessionMode;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionLoad;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionSave;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionClear;
		private System.Windows.Forms.ToolStripMenuItem tsmiFlatView;
		private System.Windows.Forms.ToolStripMenuItem tsmiGroupedView;
		private System.Windows.Forms.ToolStripMenuItem tsmiAlphabeticalSort;
		private System.Windows.Forms.ToolStripMenuItem tsmiFold;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel1;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel2;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel3;
		private System.Windows.Forms.ToolStripMenuItem tsmiLanguageSettings;
		private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
		private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
		internal System.Windows.Forms.ToolStripTextBox tsTbxSearchFilter;
		private System.Windows.Forms.ToolStripSeparator tssView2;
		private System.Windows.Forms.ToolStripSeparator tssView1;
		private System.Windows.Forms.ToolStripSeparator tssSession2;
		private System.Windows.Forms.ToolStripSeparator tssSession1;
		private System.Windows.Forms.ToolStripDropDownButton tsDdbtnView;
		private System.Windows.Forms.ToolStripDropDownButton tsDdbtnSession;
		private System.Windows.Forms.ToolStripMenuItem tsmiShowIcons;
		private System.Windows.Forms.ToolStripDropDownButton tsDdbtnSettings;
		private System.Windows.Forms.ToolStripMenuItem tsmiSignature;
		private System.Windows.Forms.ToolStripMenuItem tsmiScope;
		private System.Windows.Forms.ToolStripMenuItem tsmiAccess;
		private System.Windows.Forms.ToolStripMenuItem tsmiReturnType;
		private System.Windows.Forms.ContextMenuStrip cmsDisplay;
		private System.Windows.Forms.ToolStripMenuItem tsmiDisplay;
		private System.Windows.Forms.ToolStripMenuItem tsmiRemove;
		private System.Windows.Forms.ToolStripSeparator tssTv;
		private System.Windows.Forms.ContextMenuStrip cmsTv;
		private System.Windows.Forms.ContextMenuStrip cmsShow;
		private System.Windows.Forms.ToolStripMenuItem tsmiMove;
		private System.Windows.Forms.ToolStripProgressBar tspbProgress;
		private System.Windows.Forms.ToolStrip tsProgress;
		private System.Windows.Forms.ContextMenuStrip cmsTrackCaret;
		private System.Windows.Forms.ToolStripMenuItem tsmiTrackCaret;
		private System.Windows.Forms.ToolStripMenuItem tsmiShow;
		internal System.Windows.Forms.ToolTip ttTv;
		public System.Windows.Forms.ToolStripButton tsBtnRefresh;
		internal System.Windows.Forms.ToolStrip tsBar;
		internal System.Windows.Forms.TreeView tvTags;
		public System.Windows.Forms.ToolStripButton tsBtnClearSearchFilter;
	}
}
