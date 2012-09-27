using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using NppPluginNET;

namespace SourceCookifier
{
    public partial class frmMain : Form
    {
        #region " Main "
        public frmMain()
        {
            Main.TRACE("-START-");
            InitializeComponent();

            Settings.LoadSettings(tvTags);
            cmsShow.ImageList = Settings.IconList;
            cmsTrackCaret.ImageList = Settings.IconList;
            
            #if !FIND_IN_SESSION
            tsBtnSearchMode.Visible = false;
            #endif
            // Toolbar buttons
            if (Settings.Configs.SearchInSession)
            {
                tsBtnSearchMode.Checked = true;
                tsBtnSearchMode.Image = Properties.Resources.search_session;
                tsBtnSearchMode.ToolTipText = "String search mode";
            }

            if (!string.IsNullOrEmpty(Settings.Configs.StartupSessionMode))
            {
                switch (Settings.Configs.StartupSessionMode)
                {
                    case "Single file": Settings.Configs.SessionMode = Settings.SessionMode.None; break;
                    case "N++ session": Settings.Configs.SessionMode = Settings.SessionMode.Npp; break;
                    case "Cookie session": Settings.Configs.SessionMode = Settings.SessionMode.Cookie; break;
                    default: Settings.Configs.StartupSessionMode = ""; break;
                }
            }
            switch (Settings.Configs.SessionMode)
            {
                case Settings.SessionMode.None:
                    tsmiSingleFileMode.Checked = true;
                    tsDdbtnSession.Image = Properties.Resources.session_mode_none;
                    break;
                case Settings.SessionMode.Npp:
                    tsmiNppSessionMode.Checked = true;
                    tsDdbtnSession.Image = Properties.Resources.session_mode_npp;
                    break;
                case Settings.SessionMode.Cookie:
                    tsmiCookieSessionMode.Checked = true;
                    tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                    tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
                    break;
                default: break;
            }
            
            switch (Settings.Configs.ViewMode)
            {
                case Settings.TreeViewMode.Flat:
                    tsmiFlatView.Checked = true;
                    tsDdbtnView.Image = Properties.Resources.flat_view;
                    break;
                case Settings.TreeViewMode.FlatGrouped:
                    tsmiFlatGroupedView.Checked = true;
                    tsDdbtnView.Image = Properties.Resources.flat_grouped_view;
                    break;
                case Settings.TreeViewMode.Grouped:
                    tsmiGroupedView.Checked = true;
                    tsDdbtnView.Image = Properties.Resources.grouped_view;
                    break;
                case Settings.TreeViewMode.ClassSingle:
                    tsmiClassViewSingle.Checked = true;
                    tsDdbtnView.Image = Properties.Resources.class_view_single;
                    break;
                case Settings.TreeViewMode.ClassSession:
                    tsmiClassViewSession.Checked = true;
                    tsDdbtnView.Image = Properties.Resources.class_view_session;
                    break;
                default: break;
            }
            tsmiShowIcons.Checked = Settings.Configs.ShowIcons;
            cmsShow.ShowImageMargin = Settings.Configs.ShowIcons;
            cmsTrackCaret.ShowImageMargin = Settings.Configs.ShowIcons;
            tsmiAlphabeticalSort.Checked = Settings.Configs.SortTags;
            switch (Settings.Configs.FoldLevel)
            {
                case 1: tsmiLevel1.Checked = true; break;
                case 2: tsmiLevel2.Checked = true; break;
                default: tsmiLevel3.Checked = true; break;
            }
            tsmiLevel3.Enabled = !Settings.Configs.ViewMode.ToString().StartsWith("Flat");
            
            // Options
            if (Settings.Configs.BackColor == 0x13333337)
            {
                Settings.Configs.BackColor = Color.FromKnownColor(KnownColor.Window).ToArgb();
            }
            tvTags.BackColor = Color.FromArgb(Settings.Configs.BackColor);
            tvTags.ShowRootLines = Settings.Configs.ShowPlusMinusSource;
            tvTags.ShowPlusMinus = Settings.Configs.ShowPlusMinusTags;
            tsBar.Dock = Settings.Configs.ShowMenuAtBottom ? DockStyle.Bottom : DockStyle.Top;
            tsBar.LayoutStyle = Settings.Configs.FlowingMenuButtons ?
                ToolStripLayoutStyle.Flow : ToolStripLayoutStyle.HorizontalStackWithOverflow;
            ttTv.Active = Settings.Configs.ShowToolTips;
            
            tvTags.TreeViewNodeSorter = new TagNodeSorter();
            
            if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
                DoAllOpenedDocuments();
            
            Main.TRACE("-END-");
        }
        void FrmMainVisibleChanged(object sender, EventArgs e)
        {
            Main.TRACE("-START-");
            try
            {
                if (Visible)
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                        PluginBase._funcItems.Items[Main.idFrmMain]._cmdID, 1);
                    TreeNodeSelection selNode = GetTreeNodeSelection(tvTags.SelectedNode);
                    if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                        DoCurrentSciBufferTags();
                    else if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
                        DoAllOpenedDocuments();
                    SetTreeNodeSelection(selNode, Settings.Configs.FoldLevel);
                    tsBtnClearSearchFilter.Height = tsBtnRefresh.Height;
                }
                else
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                        PluginBase._funcItems.Items[Main.idFrmMain]._cmdID, 0);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
            Main.TRACE("-END-");
        }
        public void FrmMainFormClosing()
        {
            Main.TRACE("-START-");
            WarningIfSessionDataChanged();
            Main.TRACE("-END-");
        }
        #endregion
        
        #region " Treeview "
        internal bool droppedWithModKey = false;
        void TvTagsDragEnter(object sender, DragEventArgs e)
        {
            Main.TRACE("");
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        void TvTagsDragDrop(object sender, DragEventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (files.Length > 0)//Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                    {
                        if (Settings.Configs.SessionMode != Settings.SessionMode.Cookie)
                        {
                            Source.invisibleSourceNodes.Clear();
                            Source.invisibleIncludeFileNodes.Clear();
                            tvTags.Nodes.Clear();
                            tsmiCookieSessionMode.Checked = true;
                            Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
                            tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                            tsmiSingleFileMode.Checked = tsmiNppSessionMode.Checked = false;
                            tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
                        }
                        droppedWithModKey = (Control.ModifierKeys != Keys.None);
                        DoTags(files);
                        droppedWithModKey = false;
                        Focus();
                    }
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }

        TreeNode lastHoveredNode = null;
        void TvTagsMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Main.TRACE("");
            try
            {
                if ((tvTags.SelectedNode != null)
                    && ((tvTags.SelectedNode is Tag) || (tvTags.SelectedNode is IncludeFile))
                    && (tvTags.SelectedNode.Nodes.Count == 0))
                    ShowSelectedTagInNotepadPP();
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TvTagsMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Middle))
                {
                    TreeNode node = tvTags.GetNodeAt(e.X, e.Y);
                    if (node != null)
                    {
                        if (tvTags.SelectedNode != node)
                            tvTags.SelectedNode = node;
                    }
                }
            }
            catch { }
        }
        void TvTagsMouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {
                    if ((tvTags.SelectedNode != null) && (e.Button == MouseButtons.Middle))
                    {
                        Main.TRACE("");
                        ShowSelectedTagInNotepadPP();
                    }
                }
            }
            catch { }
        }
        void TvTagsMouseMove(object sender, MouseEventArgs e)
        {
            try {
                TreeNode node = tvTags.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    if (node != lastHoveredNode)
                    {
                        if (node.Text != node.ToolTipText)
                            ttTv.Show(node.ToolTipText, tvTags, node.Bounds.X, node.Bounds.Y - 15, 3000);
                        else ttTv.Show("", tvTags);
                    }
                }
                else ttTv.Show("", tvTags);
                lastHoveredNode = node;
            } catch { }
        }

        void CmsTvOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.TRACE("-START-");
            try
            {
                e.Cancel = true;
                cmsShow.Items.Clear();
                cmsTrackCaret.Items.Clear();
                if (tvTags.SelectedNode != null)
                {
                    tsmiShow.Visible = false;
                    tsmiTrackCaret.Visible = false;
                    tsmiDisplay.Visible = false;
                    tssTv.Visible = false;
                    tssTv2.Visible = false;
                    tsmiRemove.Visible = false;
                    tsmiMove.Visible = false;
                 
                    Source source = tvTags.SelectedNode.Tag as Source;
                    Tag tag = tvTags.SelectedNode as Tag;
                    if ((source == null) && (tag == null)
                        && (Settings.Configs.ViewMode == Settings.TreeViewMode.Grouped)
                        && (tvTags.SelectedNode.Parent != null))
                        source = tvTags.SelectedNode.Parent.Tag as Source;
                    if ((source != null) || (tag != null))
                    {
                        string name = (source != null) ? source.Language : tag.Language;
                        if (!string.IsNullOrEmpty(name))
                        {
                            foreach (string identifier in Settings.Languages[name].TagTypes.Keys)
                            {
                                ToolStripMenuItem _tsmiShow = new ToolStripMenuItem(
                                    Settings.Languages[name].TagTypes[identifier].Description);
                                ToolStripMenuItem _tsmiTrackCaret = new ToolStripMenuItem(
                                    Settings.Languages[name].TagTypes[identifier].Description);
                                _tsmiShow.ImageIndex = _tsmiTrackCaret.ImageIndex =
                                    Settings.Languages[name].TagTypes[identifier].ImageListIndex;
                                _tsmiShow.ForeColor = _tsmiTrackCaret.ForeColor =
                                    Color.FromArgb(Settings.Languages[name].TagTypes[identifier].ForeColor);
                                _tsmiShow.Tag = _tsmiTrackCaret.Tag =
                                    identifier;
                                _tsmiShow.Checked = Settings.Languages[name].TagTypes[identifier].Show;
                                _tsmiTrackCaret.Checked = Settings.Languages[name].TagTypes[identifier].TrackCaret;
                                cmsShow.Items.Add(_tsmiShow);
                                cmsTrackCaret.Items.Add(_tsmiTrackCaret);
                            }
                            cmsTv.Tag = name;
                        
                            tsmiAccess.Checked = Settings.Languages[name].DisplayAccess;
                            tsmiReturnType.Checked = Settings.Languages[name].DisplayReturnType;
                            tsmiScope.Checked = Settings.Languages[name].DisplayScope;
                            tsmiSignature.Checked = Settings.Languages[name].DisplaySignature;
                            
                            tsmiShow.Visible = true;
                            tsmiTrackCaret.Visible = true;
                            tsmiDisplay.Visible = true;
                            tssTv.Visible = true;
                        }

                        if ((source != null) && (Settings.Configs.SessionMode == Settings.SessionMode.Cookie))
                        {
                            tssTv.Visible = string.IsNullOrEmpty(source.Error);
                            tssTv2.Visible = true;
                            tsmiRemove.Visible = true;
                            tsmiMove.Visible = true;
                            tsmiMove.Text = "Move to INCLUDES";
                            tsmiMove.ForeColor = Color.OliveDrab;
                        }
                        
                        e.Cancel = ((source != null)
                                    && !string.IsNullOrEmpty(source.Error)
                                    && (Settings.Configs.SessionMode != Settings.SessionMode.Cookie));
                    }
                    else if (tvTags.SelectedNode is IncludeFile)
                    {
                        tssTv2.Visible = true;
                        tsmiRemove.Visible = true;
                        tsmiMove.Visible = true;
                        tsmiMove.Text = "Move to SOURCES";
                        tsmiMove.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        e.Cancel = false;
                    }
                    else if (tvTags.SelectedNode.Text == Source.INCLUDES)
                    {
                        tssTv2.Visible = true;
                        tsmiRemove.Visible = true;
                        e.Cancel = false;
                    }
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
            Main.TRACE("-END-");
        }
        void CmsTvItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Main.TRACE("-START-");
            try
            {
                if (sender == cmsShow)
                {
                    ToolStripMenuItem tsmi = e.ClickedItem as ToolStripMenuItem;
                    string identifier = (string)tsmi.Tag;
                    Settings.Languages[(string)cmsTv.Tag].TagTypes[identifier].Show = !tsmi.Checked;
                    RefreshTreeView(Source.RefreshType.Tags, true, 3);
                }
                else if (sender == cmsTrackCaret)
                {
                    ToolStripMenuItem tsmi = e.ClickedItem as ToolStripMenuItem;
                    string identifier = (string)tsmi.Tag;
                    Settings.Languages[(string)cmsTv.Tag].TagTypes[identifier].TrackCaret = !tsmi.Checked;
                }
                else if (sender == cmsDisplay)
                {
                    if (e.ClickedItem == tsmiAccess)
                        Settings.Languages[(string)cmsTv.Tag].DisplayAccess = !tsmiAccess.Checked;
                    else if (e.ClickedItem == tsmiReturnType)
                        Settings.Languages[(string)cmsTv.Tag].DisplayReturnType = !tsmiReturnType.Checked;
                    else if (e.ClickedItem == tsmiScope)
                        Settings.Languages[(string)cmsTv.Tag].DisplayScope = !tsmiScope.Checked;
                    else if (e.ClickedItem == tsmiSignature)
                        Settings.Languages[(string)cmsTv.Tag].DisplaySignature = !tsmiSignature.Checked;
                    RefreshTreeView(Source.RefreshType.Tags, true, 3);
                }
                else if (e.ClickedItem == tsmiCollapseAll)
                {
                    tvTags.CollapseAll();
                }
                else if (e.ClickedItem == tsmiExpandAll)
                {
                    tvTags.ExpandAll();
                }
                else if (e.ClickedItem.Text == "Remove")
                {
                    tvTags.SelectedNode.Remove();
                    if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                        Source.RemoveEmptyIncludesRootNode(tvTags);
                }
                else if (e.ClickedItem.Text == "Move to INCLUDES")
                {
                    Source source = tvTags.SelectedNode.Tag as Source;
                    Source.AddIncludeFile(tvTags, source.PathUnquoted, tsTbxSearchFilter.Text);
                    tvTags.SelectedNode.Remove();
                }
                else if (e.ClickedItem.Text == "Move to SOURCES")
                {
                    TreeNode selNode = tvTags.SelectedNode;
                    Source source = new Source(tvTags, (tvTags.SelectedNode as IncludeFile).SourceFile, tsTbxSearchFilter.Text);
                    if (source.TagTypes != null)
                    {
                        source.DrawTreeView(tvTags, IsSearchingTags() ? tsTbxSearchFilter.Text : "", Source.RefreshType.None);
                        if (Settings.Configs.ViewMode.ToString().StartsWith("Class"))
                            tvTags.Sort();
                        selNode.Remove();
                        Source.RemoveEmptyIncludesRootNode(tvTags);
                        FoldBySetLevel();
                    }
                }
                Main.TRACE(string.Format("Finished with '{0}'", e.ClickedItem.Text));
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
            Main.TRACE("-END-");
        }
        void CmsTvClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            Main.TRACE("");
            try
            {
                // Ugly moments of life.. occasional redrawing issue of treenode!
                TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
                SetTreeNodeSelection(selectedNode, 3);
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        #endregion
        
        #region " Toolbar "
        void TsTbxSearchFilterTextChanged(object sender, EventArgs e)
        {
            Main.TRACE(tsBtnSearchMode.Text);
            try
            {
                if (!tsBtnSearchMode.Checked)
                {
                    if (tsTbxSearchFilter.Text == "")
                    {
                        RefreshTreeView(Source.RefreshType.None, true, Settings.Configs.FoldLevel);
                    }
                    else
                    {
                        if (tsTbxSearchFilter.Text != tsTbxSearchFilter.Text.ToUpper())
                            RefreshTreeView(Source.RefreshType.None, true, 3);
                    }
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsBtnClearSearchFilterClick(object sender, EventArgs e)
        {
            tsTbxSearchFilter.Clear();
        }
        void TsBtnClearSearchFilterPaint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromKnownColor(KnownColor.WindowFrame), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, tsBtnClearSearchFilter.Width - 1, tsBtnClearSearchFilter.Height - 1);
            }
        }

        void TsDdbtnSessionDropDownOpening(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                for (int i = tsDdbtnSession.DropDownItems.Count; i > 9; i--)
                    tsDdbtnSession.DropDownItems.RemoveAt(i - 1);
                for (int i = Settings.Configs.CookieHistoryList.Count; i > 0; i--)
                {
                    string file = Settings.Configs.CookieHistoryList[i - 1];
                    if (!File.Exists(file))
                    {
                        Settings.Configs.CookieHistoryList.RemoveAt(i - 1);
                    }
                    else
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem();
                        tsmi.ToolTipText = file;
                        tsmi.Text = Path.GetFileNameWithoutExtension(file);
                        tsDdbtnSession.DropDownItems.Insert(9, tsmi);
                    }
                }
                tssSession3.Visible = (tsDdbtnSession.DropDownItems.Count > 9);
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsDdbtnSessionDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                Main.TRACE(e.ClickedItem.Text);
                if (e.ClickedItem.Text == tsmiSingleFileMode.Text)
                {
                    if (!tsmiSingleFileMode.Checked)
                    {
                        WarningIfSessionDataChanged();
                        Source.invisibleSourceNodes.Clear();
                        tvTags.Nodes.Clear();
                        tsmiSingleFileMode.Checked = true;
                        Settings.Configs.SessionMode = Settings.SessionMode.None;
                        tsDdbtnSession.Image = Properties.Resources.session_mode_none;
                        tsmiNppSessionMode.Checked = tsmiCookieSessionMode.Checked = false;
                        tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = false;
                        DoCurrentSciBufferTags();
                        Settings.SessionFilePath = "";
                        Settings.SessionChanged = false;
                    }
                    tsmiSingleFileMode.Checked = true;
                }
                else if (e.ClickedItem.Text == tsmiNppSessionMode.Text)
                {
                    if (!tsmiNppSessionMode.Checked)
                    {
                        WarningIfSessionDataChanged();
                        Source.invisibleSourceNodes.Clear();
                        tvTags.Nodes.Clear();
                        tsmiNppSessionMode.Checked = true;
                        Settings.Configs.SessionMode = Settings.SessionMode.Npp;
                        tsDdbtnSession.Image = Properties.Resources.session_mode_npp;
                        tsmiSingleFileMode.Checked = tsmiCookieSessionMode.Checked = false;
                        tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = false;
                        DoAllOpenedDocuments();
                        Settings.SessionFilePath = "";
                        Settings.SessionChanged = false;
                    }
                }
                else if (e.ClickedItem.Text == tsmiCookieSessionMode.Text)
                {
                    if (!tsmiCookieSessionMode.Checked)
                    {
                        Source.invisibleSourceNodes.Clear();
                        Source.invisibleIncludeFileNodes.Clear();
                        tvTags.Nodes.Clear();
                        tsmiCookieSessionMode.Checked = true;
                        Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
                        tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                        tsmiSingleFileMode.Checked = tsmiNppSessionMode.Checked = false;
                        tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
                    }
                }
                else if (e.ClickedItem.Text == tsmiSessionLoad.Text)
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.DefaultExt = "c00k!e";
                        ofd.Filter = string.Format("Cookie session file (*.{0})|*.{1}",
                            Settings.Configs.SessionFileExt, Settings.Configs.SessionFileExt);
                        ofd.Title = "Load SourceCookifier session file";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            tsmiCookieSessionMode.Checked = true;
                            Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
                            tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                            tsmiSingleFileMode.Checked = tsmiNppSessionMode.Checked = false;
                            tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
                            DoTags(new string[] {ofd.FileName});
                            if (Settings.Configs.CookieHistoryList.Contains(ofd.FileName))
                                Settings.Configs.CookieHistoryList.Remove(ofd.FileName);
                            Settings.Configs.CookieHistoryList.Insert(0, ofd.FileName);
                        }
                    }
                }
                else if (e.ClickedItem.Text == tsmiSessionSave.Text)
                {
                    if ((Source.invisibleSourceNodes.Count == 0) && (tvTags.Nodes.Count == 0)
                        && (Source.invisibleIncludeFileNodes.Count == 0))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        return;
                    }
                    string oldSearchText = tsTbxSearchFilter.Text;
                    tsTbxSearchFilter.Clear();
                    Settings.SaveSession(tvTags);
                    tsTbxSearchFilter.Text = oldSearchText;
                }
                else if (e.ClickedItem.Text == tsmiSessionClear.Text)
                {
                    if ((Source.invisibleSourceNodes.Count == 0) && (tvTags.Nodes.Count == 0)
                        && (Source.invisibleIncludeFileNodes.Count == 0))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        return;
                    }
                    WarningIfSessionDataChanged();
                    tsTbxSearchFilter.Clear();
                    Source.invisibleSourceNodes.Clear();
                    Source.invisibleIncludeFileNodes.Clear();
                    tvTags.Nodes.Clear();
                    Settings.SessionFilePath = "";
                    Settings.SessionChanged = false;
                }
                else
                {
                    string file = e.ClickedItem.ToolTipText;
                    if (!string.IsNullOrEmpty(file))
                    {
                        DoTags(new string[] {file});
                        Settings.Configs.CookieHistoryList.Remove(file);
                        Settings.Configs.CookieHistoryList.Insert(0, file);
                    }
                }
                UpdateTbCaption();
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsDdbtnSessionDropDownClosed(object sender, EventArgs e)
        {
            Main.TRACE("");
            // Very, very ugly, but else occasionally eats global shortcuts for N++ -
            // e.g. CTRL+S pastes control char into document instead saving document
            tsDdbtnSession.DropDown.Close();
        }

        void TsmiFlatViewClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (!tsmiFlatView.Checked)
                {
                    tsDdbtnView.Image = Properties.Resources.flat_view;
                    Settings.Configs.ViewMode = Settings.TreeViewMode.Flat;
                    tsmiFlatView.Checked = true;
                    tsmiFlatGroupedView.Checked = false;
                    tsmiGroupedView.Checked = false;
                    tsmiClassViewSingle.Checked = false;
                    bool lastWasGlobal = tsmiClassViewSession.Checked;
                    tsmiClassViewSession.Checked = false;
                    tsmiLevel3.Enabled = false;
                    RefreshTreeView(lastWasGlobal ? Source.RefreshType.Tags : Source.RefreshType.None, true, -1);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiFlatGroupedViewClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (!tsmiFlatGroupedView.Checked)
                {
                    tsDdbtnView.Image = Properties.Resources.flat_grouped_view;
                    Settings.Configs.ViewMode = Settings.TreeViewMode.FlatGrouped;
                    tsmiFlatView.Checked = false;
                    tsmiFlatGroupedView.Checked = true;
                    tsmiGroupedView.Checked = false;
                    tsmiClassViewSingle.Checked = false;
                    bool lastWasGlobal = tsmiClassViewSession.Checked;
                    tsmiClassViewSession.Checked = false;
                    tsmiLevel3.Enabled = false;
                    RefreshTreeView(lastWasGlobal ? Source.RefreshType.Tags : Source.RefreshType.None, true, -1);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiGroupedViewClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (!tsmiGroupedView.Checked)
                {
                    tsDdbtnView.Image = Properties.Resources.grouped_view;
                    Settings.Configs.ViewMode = Settings.TreeViewMode.Grouped;
                    tsmiFlatView.Checked = false;
                    tsmiFlatGroupedView.Checked = false;
                    tsmiGroupedView.Checked = true;
                    tsmiClassViewSingle.Checked = false;
                    bool lastWasGlobal = tsmiClassViewSession.Checked;
                    tsmiClassViewSession.Checked = false;
                    tsmiLevel3.Enabled = true;
                    RefreshTreeView(lastWasGlobal ? Source.RefreshType.Tags : Source.RefreshType.None, true, -1);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiClassViewSingleClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (!tsmiClassViewSingle.Checked)
                {
                    tsDdbtnView.Image = Properties.Resources.class_view_single;
                    Settings.Configs.ViewMode = Settings.TreeViewMode.ClassSingle;
                    tsmiFlatView.Checked = false;
                    tsmiFlatGroupedView.Checked = false;
                    tsmiGroupedView.Checked = false;
                    tsmiClassViewSingle.Checked = true;
                    bool lastWasGlobal = tsmiClassViewSession.Checked;
                    tsmiClassViewSession.Checked = false;
                    tsmiLevel3.Enabled = true;
                    RefreshTreeView(lastWasGlobal ? Source.RefreshType.Tags : Source.RefreshType.None, true, -1);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiClassViewSessionClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                if (!tsmiClassViewSession.Checked)
                {
                    tsDdbtnView.Image = Properties.Resources.class_view_session;
                    Settings.Configs.ViewMode = Settings.TreeViewMode.ClassSession;
                    tsmiFlatView.Checked = false;
                    tsmiFlatGroupedView.Checked = false;
                    tsmiGroupedView.Checked = false;
                    tsmiClassViewSingle.Checked = false;
                    tsmiClassViewSession.Checked = true;
                    tsmiLevel3.Enabled = true;
                    RefreshTreeView(Source.RefreshType.Tags, true, -1);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiAlphabeticalSortClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                Settings.Configs.SortTags = tsmiAlphabeticalSort.Checked;
                RefreshTreeView(Source.RefreshType.None, true, -1);
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiLevel1Click(object sender, EventArgs e)
        {
            LevelClicked(sender as ToolStripMenuItem);
        }
        void TsmiLevel2Click(object sender, EventArgs e)
        {
            LevelClicked(sender as ToolStripMenuItem);
        }
        void TsmiLevel3Click(object sender, EventArgs e)
        {
            LevelClicked(sender as ToolStripMenuItem);
        }
        void TsDdbtnViewDropDownClosed(object sender, EventArgs e)
        {
            Main.TRACE("");
            // Very, very ugly, but else occasionally eats global shortcuts for N++ -
            // e.g. CTRL+S pastes control char into document instead saving document
            tsDdbtnView.DropDown.Close();
        }
        
        bool TsBtnRefreshClicked = false;
        void TsBtnRefreshClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                TsBtnRefreshClicked = true;
                RefreshTreeView(Source.RefreshType.Tags, true, 3);
                TsBtnRefreshClicked = false;
                UpdateTbCaption();
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }

        void TsmiShowIconsClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            try
            {
                Settings.Configs.ShowIcons = tsmiShowIcons.Checked;
                cmsShow.ShowImageMargin = Settings.Configs.ShowIcons;
                cmsTrackCaret.ShowImageMargin = Settings.Configs.ShowIcons;
                if (!Settings.TagTypeIconsLoaded) Settings.LoadTagTypeIcons(tvTags);
                RefreshTreeView(Source.RefreshType.None, true, 3);
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TsmiLanguageSettingsClick(object sender, EventArgs e)
        {
            Main.ShowSettings();
        }
        void TsmiOptionsClick(object sender, EventArgs e)
        {
            Main.ShowOptions();
        }
        void TsmiHelpClick(object sender, EventArgs e)
        {
            Main.ShowHelp();
        }
        void TsDdbtnSettingsDropDownClosed(object sender, EventArgs e)
        {
            Main.TRACE("");
            // Very, very ugly, but else occasionally eats global shortcuts for N++ -
            // e.g. CTRL+S pastes control char into document instead saving document
            tsDdbtnSettings.DropDown.Close();
        }

        void SessionModeChanged(object sender, EventArgs e)
        {
            Main.TRACE("");
            tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = tsmiCookieSessionMode.Checked;
        }
        void LevelClicked(ToolStripMenuItem tsmi)
        {
            try
            {
                Main.TRACE(tsmi.Text);
                bool checkedBefore = tsmi.Checked;
                foreach (ToolStripMenuItem _tsmi in tsmiFold.DropDownItems)
                    _tsmi.Checked = false;
                tsmi.Checked = true;
                
                if (!checkedBefore)
                {
                    if (tsmi == tsmiLevel1) Settings.Configs.FoldLevel = 1;
                    else if (tsmi == tsmiLevel2) Settings.Configs.FoldLevel = 2;
                    else Settings.Configs.FoldLevel = 3;
                    FoldBySetLevel();
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void FoldBySetLevel()
        {
            if (IsSearchingTags() || (Settings.Configs.FoldLevel > 2))
            {
                TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
                tvTags.ExpandAll();
                if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                    && (tvTags.Nodes.Count > 1) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
                    tvTags.Nodes[tvTags.Nodes.Count - 1].Collapse();
                SetTreeNodeSelection(selectedNode, 3);
            }
            else if (Settings.Configs.FoldLevel == 1)
            {
                TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
                tvTags.CollapseAll();
                SetTreeNodeSelection(selectedNode, 1);
            }
            else if (Settings.Configs.FoldLevel == 2)
            {
                TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
                tvTags.CollapseAll();
                if (tvTags.Nodes != null) foreach (TreeNode node in tvTags.Nodes) node.Expand();
                if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                    && (tvTags.Nodes.Count > 1) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
                    tvTags.Nodes[tvTags.Nodes.Count - 1].Collapse();
                SetTreeNodeSelection(selectedNode, 2);
            }
            Main.TRACE(string.Format("New folding level={0}", Settings.Configs.FoldLevel));
        }
        void WarningIfSessionDataChanged()
        {
            if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                && (Settings.SessionFilePath != "") && (Settings.SessionChanged))
            {
                if (MessageBox.Show("The currently loaded session data has been changed.\nDo you want to save it now?",
                    "SourceCookifier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string oldSearchText = tsTbxSearchFilter.Text;
                    tsTbxSearchFilter.Clear();
                    Settings.SaveSession(tvTags);
                    tsTbxSearchFilter.Text = oldSearchText;
                    UpdateTbCaption();
                }
            }
        }
        public void UpdateTbCaption()
        {
            Main.TRACE("-START-");
            try
            {
                if (Main._ptrNppTbData == IntPtr.Zero) return;
                string caption = "SourceCookifier - ";
                Color clrBar = Color.FromKnownColor(KnownColor.Control);
                if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                {
                    caption += "[Single file]";
                }
                else if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
                {
                    caption += "[N++ session]";
                    clrBar = Color.FromArgb(clrBar.A, Math.Max(0, clrBar.R - 20), clrBar.G, Math.Max(0, clrBar.B - 20));
                }
                else
                {
                    string curSessionName = Path.GetFileNameWithoutExtension(Settings.SessionFilePath);
                    // Caution: The max string length of a N++ plugin title is 32!
                    if (curSessionName.Length > 10)
                        curSessionName = curSessionName.Substring(0, 7) + "..";
                    caption += "\"" + curSessionName + "\"";
                    if (Settings.SessionChanged) caption += " *";
                    clrBar = Color.FromArgb(clrBar.A, Math.Max(0, clrBar.R - 30), Math.Max(0, clrBar.G - 15), clrBar.B);
                }
                tsBar.BackColor = clrBar;
                int pCaption = Marshal.ReadInt32(Main._ptrNppTbData, 4);
                byte[] newCaption = new System.Text.UnicodeEncoding().GetBytes(caption);
                int p = 0;
                for (; p < newCaption.Length; p++)
                    Marshal.WriteByte((IntPtr)(pCaption + p), newCaption[p]);
                Marshal.WriteInt16((IntPtr)(pCaption + p), 0);
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMUPDATEDISPINFO, 0, Handle);
                Main.TRACE(string.Format("New caption={0}", caption));
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
            Main.TRACE("-END-");
        }
        #endregion
        
        #region " Keyboard "
        void GlobalKeyDown(object sender, KeyEventArgs e)
        {
            Main.TRACE(string.Format("KeyCode={0}", e.KeyCode));
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (tsTbxSearchFilter.Text == "")
                    {
                        if (sender == tsTbxSearchFilter)
                        {
                            // HACK: Never send window messages from sub controls directly to N++ windows
                            tvTags.Focus();
                            SendKeys.SendWait("{ESC}");
                        }
                        else
                        {
                            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
                        }
                    }
                    else tsTbxSearchFilter.Clear();
                }
                else if (e.KeyCode == Keys.Return)
                {
                    if (sender == tsTbxSearchFilter)
                    {
                        if (tsTbxSearchFilter.Text != "")
                        {
                            if (!tsBtnSearchMode.Checked)
                                RefreshTreeView(Source.RefreshType.None, true, 3);
                            else
                                Main.SearchInSession(tsTbxSearchFilter.Text);
                        }
                    }
                    else if (sender == tvTags)
                    {
                        ShowSelectedTagInNotepadPP();
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (sender == tvTags && (Settings.Configs.SessionMode == Settings.SessionMode.Cookie))
                    {
                        TreeNode selectedNode = tvTags.SelectedNode;
                        if ((selectedNode != null) && 
                            (((selectedNode.Tag != null) && (selectedNode.Tag is Source)) ||
                             (selectedNode.Name == Source.INCLUDES) || (selectedNode is IncludeFile)))
                        {
                            tvTags.SelectedNode.Remove();
                            if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                                Source.RemoveEmptyIncludesRootNode(tvTags);
                        }
                    }
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (sender == tvTags) tsTbxSearchFilter.Focus();
                    else tvTags.Focus();
                }
                else if (e.KeyCode == Keys.C)
                {
                    if (e.Control && !e.Alt && !e.Shift)
                    {
                        if (tvTags.Focused && (tvTags.SelectedNode != null))
                        {
                            if (tvTags.SelectedNode is Tag)
                                Clipboard.SetText((tvTags.SelectedNode as Tag).TagName);
                            else
                                Clipboard.SetText(tvTags.SelectedNode.Text);
                            suppressNextBeep = true;
                        }
                    }
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        bool suppressNextBeep = false;
        void GlobalKeyPress(object sender, KeyPressEventArgs e)
        {
            // Supress beep sounds
            if (((e.KeyChar == '\r') && (sender == tvTags))
                || (e.KeyChar == '\t')
                || (e.KeyChar == (char)0x1B)
                || suppressNextBeep)
            {
                e.Handled = true;
                suppressNextBeep = false;
            }
        }
        #endregion
        
        #region " Tags "
        public bool SavingDocNow = false;
        public void DoCurrentSciBufferTags()
        {
            Main.TRACE("-START-");
            int bufID = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
            StringBuilder path = new StringBuilder(Win32.MAX_PATH);
            if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufID, path) != -1)
            {
                tvTags.Nodes.Clear();
                if (File.Exists(path.ToString()))
                {
                    DoTags(new string[] { path.ToString() });
                }
            }
            Main.TRACE("-END-");
        }
        public void DoAllOpenedDocuments()
        {
            Main.TRACE("-START-");
            int nbFile = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETNBOPENFILES, 0, 0);
            using (ClikeStringArray cStrArray = new ClikeStringArray(nbFile, Win32.MAX_PATH))
            {
                if (Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETOPENFILENAMES, cStrArray.NativePointer, nbFile) != IntPtr.Zero)
                {
                    List<string> lstNewSources = new List<string>();
                    foreach (string s in cStrArray.ManagedStringsUnicode)
                        lstNewSources.Add(s);
                    for (int i = tvTags.Nodes.Count; i > 0; i--)
                    {
                        Source source = tvTags.Nodes[i - 1].Tag as Source;
                        if ((source != null) && (cStrArray.ManagedStringsUnicode.Contains(source.PathUnquoted)))
                            lstNewSources.Remove(source.PathUnquoted);
                        else
                            tvTags.Nodes.RemoveAt(i - 1);
                    }
                    DoTags(lstNewSources.ToArray());
                }
            }
            Main.TRACE("-END-");
        }
        public void DoTags(string[] files)
        {
            Main.TRACE("-START-");
            List<string> flattenedFiles = FlattenDirectories(files);
            
            for (int i = flattenedFiles.Count - 1; i >= 0; i--)
                if (!File.Exists(flattenedFiles[i])) flattenedFiles.RemoveAt(i);
            
            foreach (string file in flattenedFiles)
            {
                if (Path.GetExtension(flattenedFiles[0]) == ("." + Settings.Configs.SessionFileExt))
                {
                    tsmiCookieSessionMode.Checked = true;
                    Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
                    tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                    tsmiSingleFileMode.Checked = tsmiNppSessionMode.Checked = false;
                    tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
                    Settings.LoadSession(tvTags, flattenedFiles[0]);
                    UpdateTbCaption();
                    RefreshTreeView(Source.RefreshType.None, false, 0);
                    Focus();
                    Main.TRACE("Found .c00k!e file.. finished");
                    return;
                }
            }
            
            if ((flattenedFiles.Count > 1) && (Settings.Configs.SessionMode == Settings.SessionMode.Cookie))
            {
                Main.TRACE(string.Format("Starting with {0} file(s) in cookie mode", flattenedFiles.Count));
                List<string> lstSupportedExt = new List<string>();
                List<string> lstUnsupportedExt = new List<string>();
                string ext = "", lang = "", scopeOperator = "";
                foreach (string file in flattenedFiles)
                {
                    try
                    {
                        Source.GetLanguage(file, ref lang, ref ext, ref scopeOperator);
                        if (string.IsNullOrEmpty(ext)) ext = Path.GetFileName(file) + ".";
                        if (!lstSupportedExt.Contains(ext)) lstSupportedExt.Add(ext);
                    }
                    catch
                    {
                        if (Settings.Configs.ShowInvalidSources)
                        {
                            ext = Path.GetExtension(file);
                            if (string.IsNullOrEmpty(ext)) ext = Path.GetFileName(file) + ".";
                            if (!lstUnsupportedExt.Contains(ext)) lstUnsupportedExt.Add(ext);
                        }
                    }
                }
                if ((lstSupportedExt.Count + lstUnsupportedExt.Count) > 1)
                {
                    Main.TRACE("Found multiple extensions.. preparing import dialog");
                    frmChooseExtensions frmChooseExtensions = new frmChooseExtensions();
                    frmChooseExtensions.TopMost = TopMost;
                    lstSupportedExt.Sort();
                    foreach (string _ext in lstSupportedExt)
                        frmChooseExtensions.lbExtensions.Items.Add(_ext, true);
                    lstUnsupportedExt.Sort();
                    foreach (string _ext in lstUnsupportedExt)
                        frmChooseExtensions.lbExtensions.Items.Add(_ext, false);
                    int newHeight = frmChooseExtensions.lbExtensions.Items.Count * frmChooseExtensions.lbExtensions.ItemHeight + 20;
                    int maxHeight = Screen.PrimaryScreen.Bounds.Height - 50;
                    if (newHeight > frmChooseExtensions.lbExtensions.Height)
                    {
                        newHeight = frmChooseExtensions.Height + newHeight - frmChooseExtensions.lbExtensions.Height;
                        if (newHeight < maxHeight)
                            frmChooseExtensions.Height = newHeight;
                        else
                            frmChooseExtensions.Height = maxHeight;
                    }
                    if (frmChooseExtensions.ShowDialog() != DialogResult.OK) return;

                    List<string> lstExt = new List<string>();
                    for (int i = 0; i < frmChooseExtensions.lbExtensions.Items.Count; i++)
                        if (frmChooseExtensions.lbExtensions.GetItemChecked(i))
                            lstExt.Add(frmChooseExtensions.lbExtensions.Items[i].ToString());
                    
                    List<string> newFlattenedFiles = new List<string>();
                    foreach (string file in flattenedFiles)
                    {
                        try
                        {
                            Source.GetLanguage(file, ref lang, ref ext, ref scopeOperator);
                            if (string.IsNullOrEmpty(ext)) ext = Path.GetFileName(file) + ".";
                        }
                        catch
                        {
                            ext = Path.GetExtension(file);
                            if (string.IsNullOrEmpty(ext)) ext = Path.GetFileName(file) + ".";
                        }
                        if (lstExt.Contains(ext)) newFlattenedFiles.Add(file);
                    }
                    flattenedFiles = newFlattenedFiles;
                }
            }
            
            bool searching = IsSearchingTags();
            bool progressBar = !searching && (flattenedFiles.Count > 1);

            Main.TRACE(string.Format("Tagging {0} file(s).. searching={1}.. progressBar={2}.. droppedWithModKey={3}.. SavingDocNow={4}",
                flattenedFiles.Count, searching, progressBar, droppedWithModKey, SavingDocNow));
            if (progressBar) ProgressBarShow(flattenedFiles.Count);
            bool sortAgain = false;
            tvTags.BeginUpdate();
            foreach (string file in flattenedFiles)
            {
                if (progressBar) ProgressBarUpdate();
                if (droppedWithModKey)
                {
                    Source.AddIncludeFile(tvTags, file, IsSearchingTags() ? tsTbxSearchFilter.Text : "");
                    foreach (TreeNode sourceNode in tvTags.Nodes)
                    {
                        if ((sourceNode.Tag is Source) &&
                            ((sourceNode.Tag as Source).PathUnquoted == file))
                            { sourceNode.Remove(); break; }
                    }
                    for (int i = Source.invisibleSourceNodes.Count; i > 0; i--)
                    {
                        if ((Source.invisibleSourceNodes[i - 1].Tag as Source).PathUnquoted == file)
                            { Source.invisibleSourceNodes[i - 1].Remove(); break; }
                    }
                }
                else
                {
                    Source.RefreshType refreshType = Source.RefreshType.None;
                    Source source = GetSourceByFilepath(file);
                    if (source == null)
                    {
                        IncludeFile incFile = GetIncludeFileByFilepath(file);
                        if (incFile != null)
                        {
                            if (SavingDocNow)
                            {
                                incFile.RefreshTags();
                                Source.MoveFilteredIncludeFile(incFile, Source.GetIncludesRootNode(tvTags),
                                    IsSearchingTags() ? tsTbxSearchFilter.Text : "");
                            }
                            else
                            {
                                source = new Source(tvTags, file, tsTbxSearchFilter.Text);
                                incFile.Remove();
                                Source.RemoveEmptyIncludesRootNode(tvTags);
                            }
                        }
                        else if (!SavingDocNow)
                        {
                            source = new Source(tvTags, file, tsTbxSearchFilter.Text);
                            if (source.TagTypes == null)
                                source = null;
                            for (int i = Source.invisibleIncludeFileNodes.Count; i > 0; i--)
                            {
                                if (Source.invisibleIncludeFileNodes[i - 1].Name == file)
                                { Source.invisibleIncludeFileNodes[i - 1].Remove(); break; }
                            }
                        }
                        else if (Settings.Configs.SessionMode != Settings.SessionMode.Cookie)
                        {
                            TreeNode sourceNode = null;
                            if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                            {
                                if (tvTags.Nodes.Count > 0)
                                    sourceNode = tvTags.Nodes[0];
                                else if (Source.invisibleSourceNodes.Count > 0)
                                    sourceNode = Source.invisibleSourceNodes[0];
                            }
                            else
                            {
                                bool _found = false;
                                List<string> lstOpenFiles = new List<string>();
                                int nbFile = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETNBOPENFILES, 0, 0);
                                using (ClikeStringArray cStrArray = new ClikeStringArray(nbFile, Win32.MAX_PATH))
                                {
                                    if (Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETOPENFILENAMES, cStrArray.NativePointer, nbFile) != IntPtr.Zero)
                                        foreach (string _file in cStrArray.ManagedStringsUnicode)
                                            if (File.Exists(_file)) lstOpenFiles.Add(_file);
                                }
                                foreach (TreeNode _node in tvTags.Nodes)
                                    if (!lstOpenFiles.Contains(((Source)_node.Tag).PathUnquoted))
                                    {
                                        sourceNode = _node;
                                        sourceNode.Remove();
                                        _found = true;
                                        break;
                                    }
                                if (!_found)
                                    foreach (TreeNode _node in Source.invisibleSourceNodes)
                                        if (!lstOpenFiles.Contains(((Source)_node.Tag).PathUnquoted))
                                        {
                                            sourceNode = _node;
                                            break;
                                        }
                            }
                            source = new Source(tvTags, file, tsTbxSearchFilter.Text);
                            if (sourceNode != null)
                            {
                                sourceNode.Tag = source;
                            }
                        }
                    }
                    else refreshType = Source.RefreshType.TagTypes;
                    if (source != null)
                    {
                        if (!(!string.IsNullOrEmpty(source.Error) && !Settings.Configs.ShowInvalidSources))
                        {
                            if (Settings.Configs.ViewMode == Settings.TreeViewMode.ClassSession)
                                foreach (TreeNode node in tvTags.Nodes)
                                    RemoveGlobalClassViewTags(node, file);

                            bool shown = source.DrawTreeView(tvTags, searching ? tsTbxSearchFilter.Text : "", refreshType);
                            if (Settings.Configs.ViewMode.ToString().StartsWith("Class"))
                                sortAgain = true;
                            if (searching)
                            {
                                if (SavingDocNow)
                                {
                                    if (shown)
                                    {
                                        foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
                                            if (sourceNode.Tag == source)
                                            {
                                                tvTags.Nodes.Add(sourceNode);
                                                Source.invisibleSourceNodes.Remove(sourceNode);
                                                break;
                                            }
                                    }
                                    else
                                    {
                                        HideFilteredSourceNodes();
                                    }
                                }
                            }
                            FoldBySetLevel();
                        }
                    }
                }
            }
            tvTags.EndUpdate();
            if (sortAgain) tvTags.Sort();
            SelectTagNodeByCurrentLine(true);
            Debug.Assert(GetAllFileNames() != null);
            
            if (progressBar) ProgressBarHide();
            
            UpdateTbCaption();
            Main.TRACE("-END-");
        }
        public void RefreshTreeView(Source.RefreshType refreshTags, bool expandSources, int maxlevel)
        {
            Main.TRACE("-START-");
            
            if (!Settings.Initialized) return;
            bool searching = IsSearchingTags();
            
            if (Settings.Configs.ShowIcons)
                tvTags.ImageList = Settings.IconList;
            else
                tvTags.ImageList = null;

            TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
            RestoreFilteredSourceNodes();
            RestoreFilteredIncludeFileNodes();
            tvTags.BeginUpdate();

            int numSources = 0;
            foreach (TreeNode sourceNode in tvTags.Nodes)
            {
                if (sourceNode.Name == Source.INCLUDES)
                    foreach (IncludeFile incFile in sourceNode.Nodes) numSources++;
                else numSources++;
            }
            bool progressBar = !searching && (numSources > 1) && (refreshTags != Source.RefreshType.None);

            if (progressBar) ProgressBarShow(numSources);
            bool sortAgain = false;
            for (int i = 0; i < tvTags.Nodes.Count; i++ )
            {
                TreeNode sourceNode = tvTags.Nodes[i];
                if (sourceNode.Tag is Source)
                {
                    Source source = sourceNode.Tag as Source;
                    //if (!string.IsNullOrEmpty(source.Error)) continue;
                    if (progressBar) ProgressBarUpdate();
                    source.DrawTreeView(tvTags, searching ? tsTbxSearchFilter.Text : "", refreshTags);
                    if (Settings.Configs.ViewMode.ToString().StartsWith("Class"))
                        sortAgain = true;
                }
                else if (TsBtnRefreshClicked
                         && (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                         && (sourceNode.Name == Source.INCLUDES))
                {
                    foreach (IncludeFile incFile in sourceNode.Nodes)
                    {
                        if (progressBar) ProgressBarUpdate();
                        incFile.RefreshTags();
                    }
                }
            }

            tvTags.EndUpdate();
            if (sortAgain) tvTags.Sort();
            HideFilteredIncludeFileNodes();
            HideFilteredSourceNodes();
            FoldBySetLevel();
            
            SetTreeNodeSelection(selectedNode, maxlevel);
            
            if (progressBar) ProgressBarHide();

            if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie) &&
                (refreshTags != Source.RefreshType.None))
                Settings.SessionChanged = true;
            Main.TRACE("-END-");
        }
        public void ResetTreeView(bool extensionsChanged)
        {
            Main.TRACE("-START-");
            if (extensionsChanged)
            {
                if ((Settings.Configs.SessionMode == Settings.SessionMode.None) && (tvTags.Nodes.Count == 0))
                    DoCurrentSciBufferTags();
                else
                    RefreshTreeView(Source.RefreshType.Source, true, 3);
            }
            else
            {
                RefreshTreeView(Source.RefreshType.TagTypes, true, 3);
            }
            Refresh();
            Main.TRACE("-END-");
        }
        public TreeNode GetSourceNodeByFilepath(string filepath)
        {
            foreach (TreeNode sourceNode in tvTags.Nodes)
            {
                if ((sourceNode.Tag != null) && (sourceNode.Tag is Source))
                {
                    Source source = sourceNode.Tag as Source;
                    if (source.PathUnquoted == filepath) return sourceNode;
                }
            }
            return null;
        }			
        public Source GetSourceByFilepath(string filepath)
        {
            foreach (TreeNode sourceNode in tvTags.Nodes)
            {
                if ((sourceNode.Tag != null) && (sourceNode.Tag is Source))
                {
                    Source source = sourceNode.Tag as Source;
                    if (source.PathUnquoted == filepath) return source;
                }
            }
            foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
            {
                Source source = (Source)sourceNode.Tag;
                if (source.PathUnquoted == filepath) return source;
            }
            return null;
        }			
        public IncludeFile GetIncludeFileByFilepath(string filepath)
        {
            if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
            {
                if ((tvTags.Nodes.Count > 0) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
                {
                    foreach (IncludeFile incFile in tvTags.Nodes[tvTags.Nodes.Count - 1].Nodes)
                    {
                        if (incFile.Name == filepath) return incFile;
                    }
                }
                foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes)
                {
                    if (incFile.Name == filepath) return incFile;
                }
            }
            return null;
        }			
        public List<string> GetAllFileNames()
        {
            List<string> lstFiles = new List<string>();

            foreach (TreeNode rootNode in tvTags.Nodes)
            {
                if (rootNode.Tag is Source)
                {
                    {
                        Debug.Assert(!lstFiles.Contains(((Source)rootNode.Tag).PathUnquoted));
                        lstFiles.Add(((Source)rootNode.Tag).PathUnquoted);
                    }
                }
                else if (rootNode.Name == Source.INCLUDES)
                {
                    foreach (IncludeFile incFile in rootNode.Nodes)
                    {
                        Debug.Assert(!lstFiles.Contains(incFile.Name));
                        lstFiles.Add(incFile.Name);
                    }
                }
            }
            foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
            {
                Debug.Assert(!lstFiles.Contains(((Source)sourceNode.Tag).PathUnquoted));
                lstFiles.Add(((Source)sourceNode.Tag).PathUnquoted);
            }
            foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes)
            {
                Debug.Assert(!lstFiles.Contains(incFile.Name));
                lstFiles.Add(incFile.Name);
            }
            
            return lstFiles;
        }
        List<string> FlattenDirectories(string[] files)
        {
            List<string> ret = new List<string>();
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    foreach (string subfile in Directory.GetFiles(file, "*.*", SearchOption.AllDirectories))
                    {
                        ret.Add(subfile);
                    }
                }
                else
                {
                    ret.Add(file);
                }
            }
            return ret;
        }
        public void RemoveGlobalClassViewTags(TreeNode node, string path)
        {
            foreach (TreeNode n in node.Nodes)
                RemoveGlobalClassViewTags(n, path);
            if ((node.Nodes.Count == 0) && (node is Tag) && ((node as Tag).SourceFile == path))
                node.Remove();
        }
        #endregion
        
        #region " Filter "
        bool IsSearchingTags()
        {
            return ((tsTbxSearchFilter.Text != "") && !tsBtnSearchMode.Checked);
        }
        void HideFilteredSourceNodes()
        {
            Main.TRACE(string.Format("Hiding {0} source nodes", Source.invisibleSourceNodes.Count));
            foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
            {
                if (sourceNode.TreeView != null) sourceNode.Remove();
            }
        }
        void RestoreFilteredSourceNodes()
        {
            if (Source.invisibleSourceNodes.Count > 0)
            {
                Main.TRACE(string.Format("Restoring {0} source nodes", Source.invisibleSourceNodes.Count));
                foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
                    tvTags.Nodes.Add(sourceNode);
                Source.invisibleSourceNodes.Clear();
            }
        }
        void HideFilteredIncludeFileNodes()
        {
            if (IsSearchingTags())
            {
                if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                {
                    if ((tvTags.Nodes.Count > 0) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
                    {
                        foreach (IncludeFile incFile in tvTags.Nodes[Source.INCLUDES].Nodes)
                        {
                            if (incFile.QuickTags != null)
                            {
                                bool found = false;
                                foreach (QuickTag tag in incFile.QuickTags)
                                    if (tag.Text.IndexOf(tsTbxSearchFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                                        { found = true; break; }
                                if (!found) Source.invisibleIncludeFileNodes.Add(incFile);
                            }
                            else
                            {
                                // Invalid source node
                                Source.invisibleIncludeFileNodes.Add(incFile);
                            }
                        }
                        Main.TRACE(string.Format("Hiding {0} include file nodes", Source.invisibleIncludeFileNodes.Count));
                        foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes) incFile.Remove();
                        if (tvTags.Nodes[Source.INCLUDES].Nodes.Count == 0)
                            tvTags.Nodes[Source.INCLUDES].Remove();
                        else tvTags.Nodes[Source.INCLUDES].Expand();
                    }
                }
            }
        }
        void RestoreFilteredIncludeFileNodes()
        {
            if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
            {
                if (Source.invisibleIncludeFileNodes.Count > 0)
                {
                    Main.TRACE(string.Format("Restoring {0} filtered include file nodes", Source.invisibleIncludeFileNodes.Count));
                    TreeNode includesNode = Source.GetIncludesRootNode(tvTags);
                    foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes)
                        includesNode.Nodes.Add(incFile);
                    includesNode.ToolTipText = includesNode.Nodes.Count.ToString();
                    Source.invisibleIncludeFileNodes.Clear();
                }
            }
        }
        #endregion
        
        #region " Selection "
        public struct TreeNodeSelection
        {
            public string KeyPath;
            public TreeNode Node;
        }
        public TreeNodeSelection GetTreeNodeSelection(TreeNode node)
        {
            TreeNodeSelection sel = new TreeNodeSelection();
            sel.Node = node;
            if (node != null)
            {
                for (sel.KeyPath = node.Name; node.Parent != null; node = node.Parent)
                {
                    sel.KeyPath = node.Parent.Name + (char)0xD8 + sel.KeyPath;
                }
            }
            return sel;
        }
        public void SetTreeNodeSelection(TreeNodeSelection selection, int maxlevel)
        {
            Main.TRACE("");
            if (selection.KeyPath != null)
            {
                if (maxlevel == -1)
                    tvTags.SelectedNode = selection.Node;
                else
                    tvTags.SelectedNode = null;
                
                if (tvTags.SelectedNode == null)
                {
                    TreeNode selectedNode = null;
                    TreeNodeCollection childNodeCollection = tvTags.Nodes;
                    int level = 1;
                    foreach (string key in selection.KeyPath.Split((char)0xD8))
                    {
                        if (!childNodeCollection.ContainsKey(key)) break;
                        if (level++ > maxlevel) break;
                        selectedNode = childNodeCollection[key];
                        childNodeCollection = selectedNode.Nodes;
                    }
                    if (selectedNode != null) 
                    {
                        tvTags.SelectedNode = selectedNode;
                        tvTags.SelectedNode.EnsureVisible();
                    }
                }
                else
                {
                    tvTags.SelectedNode.EnsureVisible();
                }
            }
            Refresh();
        }
        
        int currentLine = -1;
        public bool SkipSelectTagNodeByCurrentLine = false;
        public void SelectTagNodeByCurrentLine(bool ignoreOldLine)
        {
            Main.TRACE("-START-");

            IntPtr curSci = PluginBase.GetCurrentScintilla();
            int pos = (int)Win32.SendMessage(curSci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int line = (int)Win32.SendMessage(curSci, SciMsg.SCI_LINEFROMPOSITION, pos, 0);
            if (!ignoreOldLine && (line == currentLine)) return;
            currentLine = line;
            line++;
            
            if (SkipSelectTagNodeByCurrentLine)
            {
                SkipSelectTagNodeByCurrentLine = false;
                return;
            }
            
            Tag selTag = null;
            if (Settings.Configs.ViewMode != Settings.TreeViewMode.ClassSession)
            {
                TreeNode sourceNode = null;
                if (Settings.Configs.SessionMode != Settings.SessionMode.None)
                {
                    int bufID = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
                    StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
                    if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufID, sbPath) != -1)
                    {
                        string path = sbPath.ToString();
                        if (File.Exists(path))
                        {
                            foreach (TreeNode rootNode in tvTags.Nodes)
                            {
                                Source _source = rootNode.Tag as Source;
                                if (_source != null)
                                {
                                    if (_source.PathUnquoted == path)
                                    {
                                        sourceNode = rootNode;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (sourceNode == null) return;
                }
                else
                {
                    if (tvTags.Nodes.Count == 0) return;
                    sourceNode = tvTags.Nodes[0];
                }
                Source source = sourceNode.Tag as Source;
                List<string> lstIdentifiers = GetTrackedTagTypes(source);
                
                if (lstIdentifiers.Count > 0)
                {
                    switch (Settings.Configs.ViewMode)
                    {
                        case Settings.TreeViewMode.Flat:
                        case Settings.TreeViewMode.FlatGrouped:
                            foreach (Tag tag in sourceNode.Nodes)
                            {
                                if (lstIdentifiers.Contains(tag.Identifier))
                                {
                                    if (tag.Line > line) continue;
                                    if ((selTag == null) || (selTag.Line < tag.Line)) selTag = tag;
                                }
                            }
                            break;
                        case Settings.TreeViewMode.Grouped:
                            foreach (string identifier in lstIdentifiers)
                            {
                                TreeNode tagTypeNode = sourceNode.Nodes[identifier];
                                if (tagTypeNode == null) continue;
                                foreach (Tag tag in tagTypeNode.Nodes)
                                {
                                    if (tag.Line > line) continue;
                                    if ((selTag == null) || (selTag.Line < tag.Line)) selTag = tag;
                                }
                            }
                            break;
                        case Settings.TreeViewMode.ClassSingle:
                            foreach	(TreeNode subNode in sourceNode.Nodes)
                            {
                                SelectSubTagNodeByCurrentLine(subNode, line, ref selTag, lstIdentifiers, null);
                            }
                            break;
                        default: break;
                    }
                }
            }
            else
            {
                int bufID = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
                StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
                if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufID, sbPath) != -1)
                {
                    string path = sbPath.ToString();
                    if (File.Exists(path))
                    {
                        Source source = GetSourceByFilepath(path);
                        List<string> lstIdentifiers = GetTrackedTagTypes(source);
                        foreach	(TreeNode node in tvTags.Nodes)
                        {
                            SelectSubTagNodeByCurrentLine(node, line, ref selTag, lstIdentifiers, path);
                        }
                    }
                }
            }
            
            if (selTag != null)
            {
                tvTags.SelectedNode = selTag;
                selTag.EnsureVisible();
                Main.TRACE(string.Format("Selected tag '{0}'", selTag.Text));
            }
                
            Main.TRACE("-END-");
        }
        void SelectSubTagNodeByCurrentLine(TreeNode node, int line, ref Tag selTag, List<string> lstIdentifiers, string path)
        {
            Tag tag = node as Tag;
            if ((tag != null)
                && ((path == null) || (tag.SourceFile == path))
                && lstIdentifiers.Contains(tag.Identifier)
                && (tag.Line <= line)
                && ((selTag == null) || (selTag.Line < tag.Line)))
            {
                selTag = tag;
            }
            foreach	(TreeNode subNode in node.Nodes)
            {
                SelectSubTagNodeByCurrentLine(subNode, line, ref selTag, lstIdentifiers, path);
            }
        }
        List<string> GetTrackedTagTypes(Source source)
        {
            List<string> lstIdentifiers = new List<string>();
            if ((source != null) && string.IsNullOrEmpty(source.Error))
            {
                string language = source.Language;
                foreach (string tagtype in Settings.Languages[language].TagTypes.Keys)
                {
                    if (Settings.Languages[language].TagTypes[tagtype].Show &&
                        Settings.Languages[language].TagTypes[tagtype].TrackCaret)
                    {
                        //if (source.TagTypes[tagtype].Tags.Count > 0)
                            lstIdentifiers.Add(tagtype);
                    }
                }
            }
            return lstIdentifiers;
        }

        void ShowSelectedTagInNotepadPP()
        {
            Main.TRACE("-START-");
            string filepath = "";
            int line = 1;
            if (tvTags.SelectedNode != null)
            {
                if (tvTags.SelectedNode is Tag)
                {
                    filepath = (tvTags.SelectedNode as Tag).SourceFile;
                    line = (tvTags.SelectedNode as Tag).Line;
                }
                else if (tvTags.SelectedNode.Tag is TagType)
                {
                    filepath = (tvTags.SelectedNode.Parent.Tag as Source).PathUnquoted;
                }
                else if (tvTags.SelectedNode.Tag is Source)
                {
                    filepath = (tvTags.SelectedNode.Tag as Source).PathUnquoted;
                }
                else if (tvTags.SelectedNode is IncludeFile)
                {
                    filepath = tvTags.SelectedNode.Name;
                }
                else return;
            }
            if (string.IsNullOrEmpty(filepath)) throw new Exception("No sourcefilepath found!");

            Main.PushJump(filepath, line);
            Main.ShowLineInNotepadPP(filepath, line, false);
            Main.TRACE("-END-");
        }
        #endregion
        
        #region " Progressbar "
        void ProgressBarShow(int numSources)
        {
            Main.TRACE("");
            tsProgress.Visible = true;
            tspbProgress.Width = tvTags.ClientSize.Width;//tvTags.Width - 7;
            tspbProgress.Value = 0;
            tspbProgress.Maximum = numSources;
        }
        void ProgressBarUpdate()
        {
            Main.TRACE("-START-");
            tspbProgress.PerformStep();
            tsProgress.Refresh();
            Application.DoEvents();
            Main.TRACE("-END-");
        }
        void ProgressBarHide()
        {
            Main.TRACE("");
            tsProgress.Visible = false;
        }
        #endregion
        
        #region " Search in session "
        // Following functionality only works with my modded Notepad++ version,
        // because the official Notepad++ doesn't expose a function for plug-ins,
        // which searches a given list of files for a text string...
        void TsBtnSearchModeClick(object sender, EventArgs e)
        {
            Main.TRACE("");
            Settings.Configs.SearchInSession = tsBtnSearchMode.Checked;
            if (tsBtnSearchMode.Checked)
            {
                tsBtnSearchMode.Image = Properties.Resources.search_session;
                tsBtnSearchMode.ToolTipText = "String search mode";
            }
            else
            {
                tsBtnSearchMode.Image = Properties.Resources.search_tags;
                tsBtnSearchMode.ToolTipText = "Tag search mode";
            }
            if (tsTbxSearchFilter.Text != "")
                RefreshTreeView(Source.RefreshType.None, true, -1);
        }
        #endregion
    }
}
