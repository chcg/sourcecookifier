
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
			this.tssTv = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiMove = new System.Windows.Forms.ToolStripMenuItem();
			this.tsBar = new System.Windows.Forms.ToolStrip();
			this.tsBtnSearch = new System.Windows.Forms.ToolStripButton();
			this.tsTbxSearch = new System.Windows.Forms.ToolStripTextBox();
			this.tsBtnFlatview = new System.Windows.Forms.ToolStripButton();
			this.tsBtnSort = new System.Windows.Forms.ToolStripButton();
			this.tsBtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.tsSddbtnFold = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiLevel1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLevel2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLevel3 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSddbtnSession = new System.Windows.Forms.ToolStripDropDownButton();
			this.cmsSession = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSessionSingle = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSessionNpp = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSessionCookie = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionSave = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSessionClear = new System.Windows.Forms.ToolStripMenuItem();
			this.tssSession3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsBtnShowIcons = new System.Windows.Forms.ToolStripButton();
			this.tsProgress = new System.Windows.Forms.ToolStrip();
			this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.cmsFlatView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiKeepTypesGrouped = new System.Windows.Forms.ToolStripMenuItem();
			this.ttTv = new System.Windows.Forms.ToolTip(this.components);
			this.cmsTv.SuspendLayout();
			this.tsBar.SuspendLayout();
			this.cmsSession.SuspendLayout();
			this.tsProgress.SuspendLayout();
			this.cmsFlatView.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvTags
			// 
			this.tvTags.AllowDrop = true;
			this.tvTags.ContextMenuStrip = this.cmsTv;
			this.tvTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvTags.HideSelection = false;
			this.tvTags.Indent = 19;
			this.tvTags.Location = new System.Drawing.Point(0, 25);
			this.tvTags.Name = "tvTags";
			this.tvTags.ShowPlusMinus = false;
			this.tvTags.ShowRootLines = false;
			this.tvTags.Size = new System.Drawing.Size(261, 449);
			this.tvTags.TabIndex = 0;
			this.tvTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseDoubleClick);
			this.tvTags.DragDrop += new System.Windows.Forms.DragEventHandler(this.TvTagsDragDrop);
			this.tvTags.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseMove);
			this.tvTags.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TvTagsMouseDown);
			this.tvTags.DragEnter += new System.Windows.Forms.DragEventHandler(this.TvTagsDragEnter);
			this.tvTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.tvTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			// 
			// cmsTv
			// 
			this.cmsTv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiShow,
									this.tsmiTrackCaret,
									this.tssTv,
									this.tsmiRemove,
									this.tsmiMove});
			this.cmsTv.Name = "cmsTv";
			this.cmsTv.ShowImageMargin = false;
			this.cmsTv.ShowItemToolTips = false;
			this.cmsTv.Size = new System.Drawing.Size(108, 98);
			this.cmsTv.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.CmsTvClosed);
			this.cmsTv.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsTvItemClicked);
			this.cmsTv.Opening += new System.ComponentModel.CancelEventHandler(this.CmsTvOpening);
			// 
			// tsmiShow
			// 
			this.tsmiShow.DropDown = this.cmsShow;
			this.tsmiShow.Name = "tsmiShow";
			this.tsmiShow.Size = new System.Drawing.Size(107, 22);
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
			this.tsmiTrackCaret.Size = new System.Drawing.Size(107, 22);
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
			// tssTv
			// 
			this.tssTv.Name = "tssTv";
			this.tssTv.Size = new System.Drawing.Size(104, 6);
			this.tssTv.Visible = false;
			// 
			// tsmiRemove
			// 
			this.tsmiRemove.Name = "tsmiRemove";
			this.tsmiRemove.Size = new System.Drawing.Size(107, 22);
			this.tsmiRemove.Text = "Remove";
			this.tsmiRemove.Visible = false;
			// 
			// tsmiMove
			// 
			this.tsmiMove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsmiMove.Name = "tsmiMove";
			this.tsmiMove.Size = new System.Drawing.Size(107, 22);
			this.tsmiMove.Visible = false;
			// 
			// tsBar
			// 
			this.tsBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsBtnSearch,
									this.tsTbxSearch,
									this.tsBtnFlatview,
									this.tsBtnSort,
									this.tsBtnRefresh,
									this.tsSddbtnFold,
									this.tsSddbtnSession,
									this.tsBtnShowIcons});
			this.tsBar.Location = new System.Drawing.Point(0, 0);
			this.tsBar.Name = "tsBar";
			this.tsBar.Size = new System.Drawing.Size(261, 25);
			this.tsBar.TabIndex = 1;
			this.tsBar.TabStop = true;
			// 
			// tsBtnSearch
			// 
			this.tsBtnSearch.CheckOnClick = true;
			this.tsBtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnSearch.Image = global::NppPluginNET.Properties.Resources.search_tags;
			this.tsBtnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsBtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnSearch.Name = "tsBtnSearch";
			this.tsBtnSearch.Size = new System.Drawing.Size(23, 22);
			this.tsBtnSearch.ToolTipText = "Tag search mode";
			this.tsBtnSearch.Click += new System.EventHandler(this.TsBtnSearchClick);
			// 
			// tsTbxSearch
			// 
			this.tsTbxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tsTbxSearch.Name = "tsTbxSearch";
			this.tsTbxSearch.Size = new System.Drawing.Size(100, 25);
			this.tsTbxSearch.ToolTipText = "Search";
			this.tsTbxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			this.tsTbxSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.tsTbxSearch.TextChanged += new System.EventHandler(this.TsTbxSearchTextChanged);
			// 
			// tsBtnFlatview
			// 
			this.tsBtnFlatview.CheckOnClick = true;
			this.tsBtnFlatview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnFlatview.Image = global::NppPluginNET.Properties.Resources.flat_view;
			this.tsBtnFlatview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsBtnFlatview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnFlatview.Name = "tsBtnFlatview";
			this.tsBtnFlatview.Size = new System.Drawing.Size(28, 22);
			this.tsBtnFlatview.ToolTipText = "Flat view";
			this.tsBtnFlatview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TsBtnFlatviewMouseUp);
			this.tsBtnFlatview.Click += new System.EventHandler(this.TsBtnFlatviewClick);
			// 
			// tsBtnSort
			// 
			this.tsBtnSort.CheckOnClick = true;
			this.tsBtnSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnSort.Image = global::NppPluginNET.Properties.Resources.alphabetical_sort;
			this.tsBtnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnSort.Name = "tsBtnSort";
			this.tsBtnSort.Size = new System.Drawing.Size(23, 22);
			this.tsBtnSort.ToolTipText = "Alphabetical sort";
			this.tsBtnSort.Click += new System.EventHandler(this.TsBtnSortClick);
			// 
			// tsBtnRefresh
			// 
			this.tsBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnRefresh.Image = global::NppPluginNET.Properties.Resources.refresh;
			this.tsBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnRefresh.Name = "tsBtnRefresh";
			this.tsBtnRefresh.Size = new System.Drawing.Size(23, 22);
			this.tsBtnRefresh.ToolTipText = "Refresh";
			this.tsBtnRefresh.Click += new System.EventHandler(this.TsBtnRefreshClick);
			// 
			// tsSddbtnFold
			// 
			this.tsSddbtnFold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsSddbtnFold.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiLevel1,
									this.tsmiLevel2,
									this.tsmiLevel3});
			this.tsSddbtnFold.Image = global::NppPluginNET.Properties.Resources.level;
			this.tsSddbtnFold.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsSddbtnFold.Name = "tsSddbtnFold";
			this.tsSddbtnFold.ShowDropDownArrow = false;
			this.tsSddbtnFold.Size = new System.Drawing.Size(20, 22);
			this.tsSddbtnFold.ToolTipText = "Fold";
			this.tsSddbtnFold.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TsSddbtnFoldDropDownItemClicked);
			this.tsSddbtnFold.DropDownClosed += new System.EventHandler(this.TsSddbtnFoldDropDownClosed);
			// 
			// tsmiLevel1
			// 
			this.tsmiLevel1.Name = "tsmiLevel1";
			this.tsmiLevel1.Size = new System.Drawing.Size(110, 22);
			this.tsmiLevel1.Text = "Level 1";
			// 
			// tsmiLevel2
			// 
			this.tsmiLevel2.Name = "tsmiLevel2";
			this.tsmiLevel2.Size = new System.Drawing.Size(110, 22);
			this.tsmiLevel2.Text = "Level 2";
			// 
			// tsmiLevel3
			// 
			this.tsmiLevel3.Name = "tsmiLevel3";
			this.tsmiLevel3.Size = new System.Drawing.Size(110, 22);
			this.tsmiLevel3.Text = "Level 3";
			// 
			// tsSddbtnSession
			// 
			this.tsSddbtnSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsSddbtnSession.DropDown = this.cmsSession;
			this.tsSddbtnSession.Image = global::NppPluginNET.Properties.Resources.session;
			this.tsSddbtnSession.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsSddbtnSession.Name = "tsSddbtnSession";
			this.tsSddbtnSession.ShowDropDownArrow = false;
			this.tsSddbtnSession.Size = new System.Drawing.Size(20, 22);
			this.tsSddbtnSession.ToolTipText = "Session";
			this.tsSddbtnSession.DropDownClosed += new System.EventHandler(this.TsSddbtnSessionDropDownClosed);
			// 
			// cmsSession
			// 
			this.cmsSession.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiSessionSingle,
									this.tssSession1,
									this.tsmiSessionNpp,
									this.tssSession2,
									this.tsmiSessionCookie,
									this.tsmiSessionLoad,
									this.tsmiSessionSave,
									this.tsmiSessionClear,
									this.tssSession3});
			this.cmsSession.Name = "cmsSession";
			this.cmsSession.OwnerItem = this.tsSddbtnSession;
			this.cmsSession.ShowCheckMargin = true;
			this.cmsSession.ShowImageMargin = false;
			this.cmsSession.Size = new System.Drawing.Size(187, 154);
			this.cmsSession.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmsSessionItemClicked);
			this.cmsSession.Opening += new System.ComponentModel.CancelEventHandler(this.CmsSessionOpening);
			// 
			// tsmiSessionSingle
			// 
			this.tsmiSessionSingle.Name = "tsmiSessionSingle";
			this.tsmiSessionSingle.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionSingle.Text = "Single file mode";
			// 
			// tssSession1
			// 
			this.tssSession1.Name = "tssSession1";
			this.tssSession1.Size = new System.Drawing.Size(183, 6);
			// 
			// tsmiSessionNpp
			// 
			this.tsmiSessionNpp.Name = "tsmiSessionNpp";
			this.tsmiSessionNpp.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionNpp.Text = "N++ session mode";
			// 
			// tssSession2
			// 
			this.tssSession2.Name = "tssSession2";
			this.tssSession2.Size = new System.Drawing.Size(183, 6);
			// 
			// tsmiSessionCookie
			// 
			this.tsmiSessionCookie.Name = "tsmiSessionCookie";
			this.tsmiSessionCookie.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionCookie.Text = "Cookie session mode";
			// 
			// tsmiSessionLoad
			// 
			this.tsmiSessionLoad.Name = "tsmiSessionLoad";
			this.tsmiSessionLoad.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionLoad.Text = "Load";
			// 
			// tsmiSessionSave
			// 
			this.tsmiSessionSave.Enabled = false;
			this.tsmiSessionSave.Name = "tsmiSessionSave";
			this.tsmiSessionSave.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionSave.Text = "Save";
			// 
			// tsmiSessionClear
			// 
			this.tsmiSessionClear.Enabled = false;
			this.tsmiSessionClear.Name = "tsmiSessionClear";
			this.tsmiSessionClear.Size = new System.Drawing.Size(186, 22);
			this.tsmiSessionClear.Text = "Clear";
			// 
			// tssSession3
			// 
			this.tssSession3.Name = "tssSession3";
			this.tssSession3.Size = new System.Drawing.Size(183, 6);
			this.tssSession3.Visible = false;
			// 
			// tsBtnShowIcons
			// 
			this.tsBtnShowIcons.CheckOnClick = true;
			this.tsBtnShowIcons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnShowIcons.Image = global::NppPluginNET.Properties.Resources.show_icon;
			this.tsBtnShowIcons.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnShowIcons.Name = "tsBtnShowIcons";
			this.tsBtnShowIcons.Size = new System.Drawing.Size(23, 20);
			this.tsBtnShowIcons.ToolTipText = "Show icons";
			this.tsBtnShowIcons.Click += new System.EventHandler(this.TsBtnShowIconsClick);
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
			// cmsFlatView
			// 
			this.cmsFlatView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsmiKeepTypesGrouped});
			this.cmsFlatView.Name = "cmsFlatView";
			this.cmsFlatView.ShowCheckMargin = true;
			this.cmsFlatView.ShowImageMargin = false;
			this.cmsFlatView.ShowItemToolTips = false;
			this.cmsFlatView.Size = new System.Drawing.Size(180, 26);
			// 
			// tsmiKeepTypesGrouped
			// 
			this.tsmiKeepTypesGrouped.CheckOnClick = true;
			this.tsmiKeepTypesGrouped.Name = "tsmiKeepTypesGrouped";
			this.tsmiKeepTypesGrouped.Size = new System.Drawing.Size(179, 22);
			this.tsmiKeepTypesGrouped.Text = "Keep types grouped";
			this.tsmiKeepTypesGrouped.Click += new System.EventHandler(this.TsmiKeepTypesGroupedClick);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 474);
			this.Controls.Add(this.tvTags);
			this.Controls.Add(this.tsBar);
			this.Controls.Add(this.tsProgress);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SourceCookifier";
			this.VisibleChanged += new System.EventHandler(this.FrmMainVisibleChanged);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalKeyPress);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalKeyDown);
			this.cmsTv.ResumeLayout(false);
			this.tsBar.ResumeLayout(false);
			this.tsBar.PerformLayout();
			this.cmsSession.ResumeLayout(false);
			this.tsProgress.ResumeLayout(false);
			this.tsProgress.PerformLayout();
			this.cmsFlatView.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripButton tsBtnSearch;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionSingle;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionNpp;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionCookie;
		private System.Windows.Forms.ToolStripSeparator tssSession3;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionClear;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionSave;
		private System.Windows.Forms.ToolStripMenuItem tsmiSessionLoad;
		private System.Windows.Forms.ToolStripSeparator tssSession2;
		private System.Windows.Forms.ToolStripSeparator tssSession1;
		private System.Windows.Forms.ContextMenuStrip cmsSession;
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
		private System.Windows.Forms.ContextMenuStrip cmsFlatView;
		private System.Windows.Forms.ToolStripMenuItem tsmiKeepTypesGrouped;
		internal System.Windows.Forms.ToolTip ttTv;
		private System.Windows.Forms.ToolStripDropDownButton tsSddbtnSession;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel1;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel3;
		private System.Windows.Forms.ToolStripMenuItem tsmiLevel2;
		private System.Windows.Forms.ToolStripDropDownButton tsSddbtnFold;
		private System.Windows.Forms.ToolStripButton tsBtnRefresh;
		private System.Windows.Forms.ToolStripButton tsBtnShowIcons;
		private System.Windows.Forms.ToolStripButton tsBtnSort;
		private System.Windows.Forms.ToolStripButton tsBtnFlatview;
		internal System.Windows.Forms.ToolStripTextBox tsTbxSearch;
		private System.Windows.Forms.ToolStrip tsBar;
		internal System.Windows.Forms.TreeView tvTags;
	}
}
