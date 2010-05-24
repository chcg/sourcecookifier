#region " Imports "
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace NppPluginNET
{
	public partial class frmMain : Form
	{
		PluginBase pluginBase;
		public frmMain(PluginBase pluginbase)
		{
			PluginBase.TRACE("-START-");
			pluginBase = pluginbase;
			InitializeComponent();

			Settings.LoadSettings(tvTags);
			cmsShow.ImageList = Settings.IconList;
			cmsTrackCaret.ImageList = Settings.IconList;
			
			#if !FIND_IN_SESSION
			tsBtnSearch.Visible = false;
			#endif
			// Toolbar buttons
			if (Settings.Configs.SearchInSession)
			{
				tsBtnSearch.Checked = true;
				tsBtnSearch.Image = Properties.Resources.search_session;
				tsBtnSearch.ToolTipText = "String search mode";
			}
			tsBtnFlatview.Checked = Settings.Configs.FlatView;
			tsmiKeepTypesGrouped.Checked = Settings.Configs.KeepTypesGrouped;
			tsBtnSort.Checked = Settings.Configs.SortTags;
			tsBtnShowIcons.Checked = Settings.Configs.ShowIcons;
			cmsShow.ShowImageMargin = Settings.Configs.ShowIcons;
			cmsTrackCaret.ShowImageMargin = Settings.Configs.ShowIcons;
			switch (Settings.Configs.FoldLevel)
			{
				case 1: tsmiLevel1.Checked = true; break;
				case 2: tsmiLevel2.Checked = true; break;
				default: tsmiLevel3.Checked = true; break;
			}
			tsmiLevel3.Enabled = !Settings.Configs.FlatView;
			switch (Settings.Configs.SessionMode)
			{
				case Settings.SessionMode.None:
					tsmiSessionSingle.Checked = true;
					tsSddbtnSession.Image = Properties.Resources.session;
					break;
				case Settings.SessionMode.Npp:
					tsmiSessionNpp.Checked = true;
					tsSddbtnSession.Image = Properties.Resources.session_npp;
					break;
				case Settings.SessionMode.Cookie:
					tsmiSessionCookie.Checked = true;
					tsSddbtnSession.Image = Properties.Resources.session_sc;
					tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
					break;
			}
			
			if (Settings.Configs.BackColor == 0x13333337)
			{
				Settings.Configs.BackColor = Color.FromKnownColor(KnownColor.Window).ToArgb();
			}
			tvTags.BackColor = Color.FromArgb(Settings.Configs.BackColor);

			ttTv.Active = Settings.Configs.ShowToolTips;
			
			tvTags.TreeViewNodeSorter = new TagNodeSorter();
			
			if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
				DoAllOpenedDocuments();
			PluginBase.TRACE("-END-");
		}
		public void FrmMainFormClosing()
		{
			PluginBase.TRACE("-START-");
			WarningIfSessionDataChanged();
			Settings.SaveSettings();
			PluginBase.TRACE("-END-");
		}
		void FrmMainVisibleChanged(object sender, EventArgs e)
		{
			PluginBase.TRACE("-START-");
			try
			{
				if (Visible)
				{
		            Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
					    pluginBase._funcItems.Items[pluginBase.idFrmMain]._cmdID, 1);
	                TreeNodeSelection selNode = GetTreeNodeSelection(tvTags.SelectedNode);
					if (Settings.Configs.SessionMode == Settings.SessionMode.None) DoCurrentSciBufferTags();
					SetTreeNodeSelection(selNode, Settings.Configs.FoldLevel);
				}
				else
				{
					Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
				    	pluginBase._funcItems.Items[pluginBase.idFrmMain]._cmdID, 0);
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
			PluginBase.TRACE("-END-");
		}
		
		void TvTagsDragEnter(object sender, DragEventArgs e)
		{
			PluginBase.TRACE("");
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
		}
		bool droppedWithModKey = false;
		void TvTagsDragDrop(object sender, DragEventArgs e)
		{
			PluginBase.TRACE("");
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
					        tvTags.Nodes.Clear();
							tsmiSessionCookie.Checked = true;
							Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
							tsSddbtnSession.Image = Properties.Resources.session_sc;
							tsmiSessionSingle.Checked = tsmiSessionNpp.Checked = false;
							tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
	            		}
	            		droppedWithModKey = (Control.ModifierKeys != Keys.None);
		            	DoTags(files);
		            	droppedWithModKey = false;
		            	Focus();
	            	}
	            }
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TvTagsMouseDoubleClick(object sender, MouseEventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				if ((tvTags.SelectedNode != null) &&
				    ((tvTags.SelectedNode is Tag) || (tvTags.SelectedNode is IncludeFile)))
					ShowSelectedTagInNotepadPP();
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TvTagsMouseDown(object sender, MouseEventArgs e)
		{
			try {
				if ( e.Button == MouseButtons.Right)
				{
					TreeNode node = tvTags.GetNodeAt(e.X, e.Y);
					if ((node != null) && (tvTags.SelectedNode != node))
					    tvTags.SelectedNode = node;
				}
			} catch { }
		}
		TreeNode lastHoveredNode = null;
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
		
		void TsTbxSearchTextChanged(object sender, EventArgs e)
		{
			PluginBase.TRACE(tsBtnSearch.Text);
			try
			{
				if (!tsBtnSearch.Checked)
				{
					if (tsTbxSearch.Text == "")
						RefreshTreeView(Source.RefreshType.None, true, Settings.Configs.FoldLevel);
					else
						RefreshTreeView(Source.RefreshType.None, true, 3);
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TsBtnFlatviewClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				Settings.Configs.FlatView = tsBtnFlatview.Checked;
				tsmiLevel3.Enabled = !Settings.Configs.FlatView;
				RefreshTreeView(Source.RefreshType.None, true, -1);
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TsBtnSortClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				Settings.Configs.SortTags = tsBtnSort.Checked;
				RefreshTreeView(Source.RefreshType.None, true, -1);
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		bool BtnRefreshClicked = false;
		void TsBtnRefreshClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				if (tsTbxSearch.Text != "") tsTbxSearch.Clear();
				BtnRefreshClicked = true;
				RefreshTreeView(Source.RefreshType.Tags, true, 3);
				BtnRefreshClicked = false;
				UpdateTbCaption();
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TsSddbtnFoldDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			PluginBase.TRACE(e.ClickedItem.Text);
			try
			{
				ToolStripMenuItem tsmi = (ToolStripMenuItem)e.ClickedItem;
				bool checkedBefore = tsmi.Checked;
				foreach (ToolStripMenuItem _tsmi in tsSddbtnFold.DropDownItems) _tsmi.Checked = false;
				tsmi.Checked = true;
				
				if (!checkedBefore)
				{
					if (tsmi == tsmiLevel1) Settings.Configs.FoldLevel = 1;
					else if (tsmi == tsmiLevel2) Settings.Configs.FoldLevel = 2;
					else Settings.Configs.FoldLevel = 3;
					if (tsTbxSearch.Text == "") FoldBySetLevel();
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TsSddbtnFoldDropDownClosed(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			// Ugly moments of life.. occasionally eats shortcuts for N++
			tsSddbtnFold.DropDown.Close();
		}
		void TsSddbtnSessionDropDownClosed(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			// Ugly moments of life.. occasionally eats shortcuts for N++
			tsSddbtnSession.DropDown.Close();
		}
		void CmsSessionOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				for (int i = cmsSession.Items.Count; i > 9; i--)
					cmsSession.Items.RemoveAt(i - 1);
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
						cmsSession.Items.Insert(9, tsmi);
					}
				}
				tssSession3.Visible = (cmsSession.Items.Count > 9);
				e.Cancel = false;
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void CmsSessionItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				PluginBase.TRACE(e.ClickedItem.Text);
				if (e.ClickedItem == tsmiSessionSingle)
				{
					if (!tsmiSessionSingle.Checked)
					{
						WarningIfSessionDataChanged();
				        Source.invisibleSourceNodes.Clear();
				        tvTags.Nodes.Clear();
						tsmiSessionSingle.Checked = true;
						Settings.Configs.SessionMode = Settings.SessionMode.None;
						tsSddbtnSession.Image = Properties.Resources.session;
						tsmiSessionNpp.Checked = tsmiSessionCookie.Checked = false;
						tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = false;
						DoCurrentSciBufferTags();
						Settings.SessionFilePath = "";
						Settings.SessionChanged = false;
					}
				}
				else if (e.ClickedItem == tsmiSessionNpp)
				{
					if (!tsmiSessionNpp.Checked)
					{
						WarningIfSessionDataChanged();
				        Source.invisibleSourceNodes.Clear();
				        tvTags.Nodes.Clear();
						tsmiSessionNpp.Checked = true;
						Settings.Configs.SessionMode = Settings.SessionMode.Npp;
						tsSddbtnSession.Image = Properties.Resources.session_npp;
						tsmiSessionSingle.Checked = tsmiSessionCookie.Checked = false;
						tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = false;
			            DoAllOpenedDocuments();
						Settings.SessionFilePath = "";
						Settings.SessionChanged = false;
					}
				}
				else if (e.ClickedItem == tsmiSessionCookie)
				{
					if (!tsmiSessionCookie.Checked)
					{
				        Source.invisibleSourceNodes.Clear();
				        tvTags.Nodes.Clear();
						tsmiSessionCookie.Checked = true;
						Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
						tsSddbtnSession.Image = Properties.Resources.session_sc;
						tsmiSessionSingle.Checked = tsmiSessionNpp.Checked = false;
						tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
					}
				}
				else if (e.ClickedItem == tsmiSessionLoad)
				{
					using (OpenFileDialog ofd = new OpenFileDialog())
					{
						ofd.DefaultExt = "c00k!e";
						ofd.Filter = "Cookie session file (*.c00k!e)|*.c00k!e";
						ofd.Title = "Load SourceCookifier session file";
			            if (ofd.ShowDialog() == DialogResult.OK)
			            {
			            	tsmiSessionCookie.Checked = true;
			            	Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
			            	tsSddbtnSession.Image = Properties.Resources.session_sc;
			            	tsmiSessionSingle.Checked = tsmiSessionNpp.Checked = false;
			            	tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
			            	DoTags(new string[] {ofd.FileName});
			            	if (Settings.Configs.CookieHistoryList.Contains(ofd.FileName))
			            		Settings.Configs.CookieHistoryList.Remove(ofd.FileName);
			            	Settings.Configs.CookieHistoryList.Insert(0, ofd.FileName);
			            }
					}
				}
				else if (e.ClickedItem == tsmiSessionSave)
				{
					if ((Source.invisibleSourceNodes.Count == 0) && (tvTags.Nodes.Count == 0)
					    && (Source.invisibleIncludeFileNodes.Count == 0))
					{
						System.Media.SystemSounds.Hand.Play();
						return;
					}
	    			if (tsTbxSearch.Text != "") tsTbxSearch.Clear();
	    			Settings.SaveSession(tvTags);
				}
				else if (e.ClickedItem == tsmiSessionClear)
				{
					if ((Source.invisibleSourceNodes.Count == 0) && (tvTags.Nodes.Count == 0)
					    && (Source.invisibleIncludeFileNodes.Count == 0))
					{
						System.Media.SystemSounds.Hand.Play();
						return;
					}
					WarningIfSessionDataChanged();
					if (tsTbxSearch.Text != "") tsTbxSearch.Clear();
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
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void WarningIfSessionDataChanged()
		{
			if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
			    && (Settings.SessionFilePath != "") && (Settings.SessionChanged))
			{
				if (MessageBox.Show("The currently loaded session data has been changed.\nDo you want to save it now?",
	                "SourceCookifier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
	    			if (tsTbxSearch.Text != "") tsTbxSearch.Clear();
	    			Settings.SaveSession(tvTags);
	    			UpdateTbCaption();
				}
			}
		}
		void TsmiSessionModeCheckedChanged(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = tsmiSessionCookie.Checked;
		}
		void TsBtnShowIconsClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				Settings.Configs.ShowIcons = tsBtnShowIcons.Checked;
				cmsShow.ShowImageMargin = Settings.Configs.ShowIcons;
				cmsTrackCaret.ShowImageMargin = Settings.Configs.ShowIcons;
				if (!Settings.TagTypeIconsLoaded) Settings.LoadTagTypeIcons(tvTags);
				RefreshTreeView(Source.RefreshType.None, true, 3);
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		
		void GlobalKeyDown(object sender, KeyEventArgs e)
		{
			PluginBase.TRACE(string.Format("KeyCode={0}", e.KeyCode));
			try
			{
				if (e.KeyCode == Keys.Escape)
				{
					if (tsTbxSearch.Text == "")
					{
						if (sender == tsTbxSearch)
						{
							// HACK: Never send window messages from sub controls directly to N++ windows
							tvTags.Focus();
							SendKeys.SendWait("{ESC}");
						}
						else
						{
							Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
						}
					}
					else tsTbxSearch.Clear();
				}
				else if (e.KeyCode == Keys.Return)
				{
					if (sender == tsTbxSearch)
					{
						if (tsBtnSearch.Checked && (tsTbxSearch.Text != ""))
						{
							pluginBase.SearchInSession(tsTbxSearch.Text);
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
					if (sender == tvTags) tsTbxSearch.Focus();
					else tvTags.Focus();
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void GlobalKeyPress(object sender, KeyPressEventArgs e)
		{
			// Supress beep sounds
			if (((e.KeyChar == '\r') && (sender == tvTags))
			    || (e.KeyChar == '\t')
			    || (e.KeyChar == (char)0x1B))
				e.Handled = true;
		}
		
		public bool SavingDocNow = false;
		void DoAllOpenedDocuments()
		{
			PluginBase.TRACE("-START-");
            int nbFile = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETNBOPENFILES, 0, 0);
            using (ClikeStringArray cStrArray = new ClikeStringArray(nbFile, Win32.MAX_PATH))
            {
                if (Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETOPENFILENAMES, cStrArray.NativePointer, nbFile) != IntPtr.Zero)
                {
                	DoTags(cStrArray.ManagedStringsUnicode.ToArray());
                }
            }
			PluginBase.TRACE("-END-");
		}
		public void DoTags(string[] files)
		{
			PluginBase.TRACE("-START-");
	    	tsTbxSearch.Clear();
			List<string> flattenedFiles = FlattenDirectories(files);
			
			for (int i = flattenedFiles.Count - 1; i >= 0; i--)
				if (!File.Exists(flattenedFiles[i])) flattenedFiles.RemoveAt(i);
			
        	foreach (string file in flattenedFiles)
        	{
				if (Path.GetExtension(flattenedFiles[0]) == ".c00k!e")
				{
					tsmiSessionCookie.Checked = true;
					Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
					tsSddbtnSession.Image = Properties.Resources.session_sc;
					tsmiSessionSingle.Checked = tsmiSessionNpp.Checked = false;
					tsmiSessionSave.Enabled = tsmiSessionClear.Enabled = true;
					Settings.LoadSession(tvTags, flattenedFiles[0]);
					RefreshTreeView(Source.RefreshType.None, false, 0);
					Focus();
					PluginBase.TRACE("Found .c00k!e file.. finished");
					return;
				}
        	}
			
			if ((flattenedFiles.Count > 1) && (Settings.Configs.SessionMode == Settings.SessionMode.Cookie))
			{
				PluginBase.TRACE(string.Format("Starting with {0} file(s) in cookie mode", flattenedFiles.Count));
				List<string> lstSupportedExt = new List<string>();
				List<string> lstUnsupportedExt = new List<string>();
				string ext = "", lang = "";
				foreach (string file in flattenedFiles)
				{
					try
					{
						Source.GetLanguage(file, ref lang, ref ext);
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
					PluginBase.TRACE("Found multiple extensions.. preparing import dialog");
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
							Source.GetLanguage(file, ref lang, ref ext);
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
			
	    	bool searching = (tsTbxSearch.Text != "");
	    	bool progressBar = !searching && (flattenedFiles.Count > 1);

	    	PluginBase.TRACE(string.Format("Tagging {0} file(s).. searching={1}.. progressBar={2}.. droppedWithModKey={3}.. SavingDocNow={4}",
	    	    flattenedFiles.Count, searching, progressBar, droppedWithModKey, SavingDocNow));
	    	if (progressBar) ProgressBarShow(flattenedFiles.Count);
        	foreach (string file in flattenedFiles)
        	{
				if (progressBar) ProgressBarUpdate();
        		if (droppedWithModKey)
        		{
        			Source.AddIncludeFile(tvTags, file);
					foreach (TreeNode sourceNode in tvTags.Nodes)
					{
						if ((sourceNode.Tag is Source) &&
						   ((sourceNode.Tag as Source).PathUnquoted == file))
							{ sourceNode.Remove(); break; }
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
            				}
            				else
            				{
            					source = new Source(file);
            					incFile.Remove();
            					Source.RemoveEmptyIncludesRootNode(tvTags);
            				}
            			}
            			else if (!SavingDocNow)
            			{
            				source = new Source(file);
            			}
            		}
            		else refreshType = Source.RefreshType.TagTypes;
            		if (source != null)
            		{
            		    if (!(!string.IsNullOrEmpty(source.Error) && !Settings.Configs.ShowInvalidSources))
            		    {
        					source.DrawTreeView(tvTags, tsTbxSearch.Text, refreshType, searching);
        					if (!searching) FoldBySetLevel();
            		    }
            		}
        		}
        	}
        	SelectTagNodeByCurrentLine(true);
        	
        	if (progressBar) ProgressBarHide();
        	
        	UpdateTbCaption();
			PluginBase.TRACE("-END-");
		}
		public void RefreshTreeView(Source.RefreshType refreshTags, bool expandSources, int maxlevel)
		{
			PluginBase.TRACE("-START-");
			tvTags.BeginUpdate();
			
		    if (!Settings.Initialized) return;
		    bool searching = ((tsTbxSearch.Text != "") && !tsBtnSearch.Checked);
			
			if (Settings.Configs.ShowIcons)
				tvTags.ImageList = Settings.IconList;
			else
				tvTags.ImageList = null;

			TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
			RestoreFilteredSourceNodes();
			RestoreFilteredIncludeFileNodes();

			int numSources = 0;
			foreach (TreeNode sourceNode in tvTags.Nodes)
			{
				if (sourceNode.Name == Source.INCLUDES)
					foreach (IncludeFile incFile in sourceNode.Nodes) numSources++;
				else numSources++;
			}
			bool progressBar = !searching && (numSources > 1) && (refreshTags != Source.RefreshType.None);

			if (progressBar) ProgressBarShow(numSources);
			foreach	(TreeNode sourceNode in tvTags.Nodes)
			{
				if (sourceNode.Tag is Source)
				{
					Source source = sourceNode.Tag as Source;
					//if (!string.IsNullOrEmpty(source.Error)) continue;
					if (progressBar) ProgressBarUpdate();
					if (searching)
					{
						source.DrawTreeView(tvTags, tsTbxSearch.Text, refreshTags, true);
					}
					else
					{
						source.DrawTreeView(tvTags, "", refreshTags, false);
					}
				}
				else if (BtnRefreshClicked
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
			HideFilteredIncludeFileNodes();
			HideFilteredSourceNodes();
			if (!searching) FoldBySetLevel();
			
			SetTreeNodeSelection(selectedNode, maxlevel);
			
        	if (progressBar) ProgressBarHide();

        	if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie) &&
        	    (refreshTags != Source.RefreshType.None))
				Settings.SessionChanged = true;
			PluginBase.TRACE("-END-");
		}
		public void ResetTreeView(bool extensionsChanged)
		{
			PluginBase.TRACE("-START-");
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
			PluginBase.TRACE("-END-");
		}
		public void DoCurrentSciBufferTags()
		{
			PluginBase.TRACE("-START-");
			int bufID = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
    		StringBuilder path = new StringBuilder(Win32.MAX_PATH);
    		if ((int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufID, path) != -1)
    		{
    			tvTags.Nodes.Clear();
    			if (File.Exists(path.ToString()))
    			{
    				DoTags(new string[] { path.ToString() });
    			}
    		}
			PluginBase.TRACE("-END-");
		}
		public TreeNode GetSourceNodeByFilepath(string filepath)
		{
			foreach (TreeNode sourceNode in tvTags.Nodes)
			{
				Source source = sourceNode.Tag as Source;
				if (source.PathUnquoted == filepath) return sourceNode;
			}
			return null;
		}			
		public Source GetSourceByFilepath(string filepath)
		{
			foreach (TreeNode sourceNode in tvTags.Nodes)
			{
				if (sourceNode.Tag is Source)
				{
					Source source = sourceNode.Tag as Source;
					if (source.PathUnquoted == filepath) return source;
				}
			}
			return null;
		}			
		public IncludeFile GetIncludeFileByFilepath(string filepath)
		{
			if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
			    && (tvTags.Nodes.Count > 0) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
			{
				foreach (IncludeFile incFile in tvTags.Nodes[tvTags.Nodes.Count - 1].Nodes)
				{
					if (incFile.Name == filepath) return incFile;
				}
			}
			return null;
		}			
		
		void HideFilteredSourceNodes()
		{
			PluginBase.TRACE(string.Format("Hiding {0} source nodes", Source.invisibleSourceNodes.Count));
			foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
			{
				sourceNode.Remove();
			}
		}
		void RestoreFilteredSourceNodes()
		{
		    if (Source.invisibleSourceNodes.Count > 0)
		    {
		    	PluginBase.TRACE(string.Format("Restoring {0} source nodes", Source.invisibleSourceNodes.Count));
    			foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
    			    tvTags.Nodes.Add(sourceNode);
    			Source.invisibleSourceNodes.Clear();
		    }
		}
		void HideFilteredIncludeFileNodes()
		{
			if ((tsTbxSearch.Text != "") && !tsBtnSearch.Checked)
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
    								if (tag.Text.IndexOf(tsTbxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
    									{ found = true; break; }
    							if (!found) Source.invisibleIncludeFileNodes.Add(incFile);
						    }
						    else
						    {
						        // Invalid source node
						        Source.invisibleIncludeFileNodes.Add(incFile);
						    }
						}
						PluginBase.TRACE(string.Format("Hiding {0} include file nodes", Source.invisibleIncludeFileNodes.Count));
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
					PluginBase.TRACE(string.Format("Restoring {0} filtered include file nodes", Source.invisibleIncludeFileNodes.Count));
					TreeNode includesNode = Source.GetIncludesRootNode(tvTags);
					foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes)
					    includesNode.Nodes.Add(incFile);
					includesNode.ToolTipText = includesNode.Nodes.Count.ToString();
					Source.invisibleIncludeFileNodes.Clear();
				}
			}
		}
		
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
			PluginBase.TRACE("");
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
		public void SelectTagNodeByCurrentLine(bool ignoreOldLine)
		{
			if (tsTbxSearch.Text != "") return;
			PluginBase.TRACE("-START-");

    		IntPtr curSci = pluginBase.GetCurrentScintilla();
    		int pos = (int)Win32.SendMessage(curSci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
    		int line = (int)Win32.SendMessage(curSci, SciMsg.SCI_LINEFROMPOSITION, pos, 0);
    		if (!ignoreOldLine && (line == currentLine)) return;
   			currentLine = line;
   			line++;
			
			TreeNode sourceNode = null;
			if (Settings.Configs.SessionMode != Settings.SessionMode.None)
			{
				int bufID = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
        		StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
        		if ((int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufID, sbPath) != -1)
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
			if (!string.IsNullOrEmpty(source.Error)) return;
			string language = source.Language;
			List<string> lstIdentifiers = new List<string>();
			foreach (string tagtype in Settings.Languages[language].TagTypes.Keys)
			{
				if (Settings.Languages[language].TagTypes[tagtype].Show &&
				    Settings.Languages[language].TagTypes[tagtype].TrackCaret)
				{
					if (source.TagTypes[tagtype].Tags.Count > 0)
						lstIdentifiers.Add(tagtype);
				}
			}
			
			if (lstIdentifiers.Count > 0)
			{
				Tag selTag = null;
				if (Settings.Configs.FlatView)
				{
					foreach (Tag tag in sourceNode.Nodes)
					{
						if (lstIdentifiers.Contains(tag.Identifier))
						{
							if (tag.Line > line) continue;
							if ((selTag == null) || (selTag.Line < tag.Line)) selTag = tag;
						}
					}
				}
				else
				{
					foreach (string identifier in lstIdentifiers)
					{
						TreeNode tagTypeNode = sourceNode.Nodes[identifier];
						foreach (Tag tag in tagTypeNode.Nodes)
						{
							if (tag.Line > line) continue;
							if ((selTag == null) || (selTag.Line < tag.Line)) selTag = tag;
						}
					}
				}
				if (selTag != null)
				{
					tvTags.SelectedNode = selTag;
					selTag.EnsureVisible();
					PluginBase.TRACE(string.Format("Selected tag '{0}'", selTag.Text));
				}
			}
			PluginBase.TRACE("-END-");
		}
		void FoldBySetLevel()
		{
			if (Settings.Configs.FoldLevel == 1)
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
			else
			{
				TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
				tvTags.ExpandAll();
				if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
					&& (tvTags.Nodes.Count > 1) && (tvTags.Nodes[tvTags.Nodes.Count - 1].Name == Source.INCLUDES))
					tvTags.Nodes[tvTags.Nodes.Count - 1].Collapse();
				SetTreeNodeSelection(selectedNode, 3);
			}
			PluginBase.TRACE(string.Format("New folding level={0}", Settings.Configs.FoldLevel));
		}
		
		void ShowSelectedTagInNotepadPP()
		{
			PluginBase.TRACE("-START-");
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
			
			pluginBase.PushJump(filepath, line);
			pluginBase.ShowLineInNotepadPP(filepath, line);
			PluginBase.TRACE("-END-");
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
		
		void CmsTvOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PluginBase.TRACE("-START-");
			try
			{
				e.Cancel = true;
				cmsShow.Items.Clear();
				cmsTrackCaret.Items.Clear();
				if (tvTags.SelectedNode != null)
				{
					TreeNode selNode = tvTags.SelectedNode;
					while (selNode.Parent != null) selNode = selNode.Parent;
					Source source = selNode.Tag as Source;
					if (source != null)
					{
						if (string.IsNullOrEmpty(source.Error))
						{
							string name = source.Language;
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
							
							tsmiShow.Visible = true;
							tsmiTrackCaret.Visible = true;
						}
						
						if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie) &&
						    (tvTags.SelectedNode.Parent == null))
						{
							tssTv.Visible = true;
							tsmiRemove.Visible = true;
							tsmiMove.Visible = true;
							tsmiMove.Text = "Move to INCLUDES";
							tsmiMove.ForeColor = Color.OliveDrab;
						}
						else
						{
							tssTv.Visible = false;
							tsmiRemove.Visible = false;
							tsmiMove.Visible = false;
						}
						
						e.Cancel = false;
					}
					else if (tvTags.SelectedNode is IncludeFile)
					{
						tsmiShow.Visible = false;
						tsmiTrackCaret.Visible = false;
						tssTv.Visible = false;
						tsmiRemove.Visible = true;
						tsmiMove.Visible = true;
						tsmiMove.Text = "Move to SOURCES";
						tsmiMove.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
						e.Cancel = false;
					}
					else if (tvTags.SelectedNode.Text == Source.INCLUDES)
					{
						tsmiShow.Visible = false;
						tsmiTrackCaret.Visible = false;
						tssTv.Visible = false;
						tsmiRemove.Visible = true;
						tsmiMove.Visible = false;
						e.Cancel = false;
					}
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
			PluginBase.TRACE("-END-");
		}
		void CmsTvItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			PluginBase.TRACE("-START-");
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
				else if (e.ClickedItem.Text == "Remove")
				{
					tvTags.SelectedNode.Remove();
					if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
						Source.RemoveEmptyIncludesRootNode(tvTags);
				}
				else if (e.ClickedItem.Text == "Move to INCLUDES")
				{
					Source source = tvTags.SelectedNode.Tag as Source;
					Source.AddIncludeFile(tvTags, source.PathUnquoted);
					tvTags.SelectedNode.Remove();
				}
				else if (e.ClickedItem.Text == "Move to SOURCES")
				{
					Source source = new Source((tvTags.SelectedNode as IncludeFile).SourceFile);
					source.DrawTreeView(tvTags, tsTbxSearch.Text, Source.RefreshType.None, false);
					tvTags.SelectedNode.Remove();
					Source.RemoveEmptyIncludesRootNode(tvTags);
				}
				PluginBase.TRACE(string.Format("Finished with '{0}'", e.ClickedItem.Text));
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
			PluginBase.TRACE("-END-");
		}
		void CmsTvClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				// Ugly moments of life.. occasional redrawing issue of treenode!
				TreeNodeSelection selectedNode = GetTreeNodeSelection(tvTags.SelectedNode);
				SetTreeNodeSelection(selectedNode, 3);
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		
		void TsBtnFlatviewMouseUp(object sender, MouseEventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					cmsFlatView.Show(tsBar, tsBtnFlatview.Bounds.X, tsBtnFlatview.Bounds.Y + tsBtnFlatview.Bounds.Height);
				}
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		void TsmiKeepTypesGroupedClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			try
			{
				Settings.Configs.KeepTypesGrouped = tsmiKeepTypesGrouped.Checked;
				RefreshTreeView(Source.RefreshType.None, true, 3);
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
		}
		
		void ProgressBarShow(int numSources)
		{
			PluginBase.TRACE("");
			tsProgress.Visible = true;
			tspbProgress.Width = tvTags.ClientSize.Width;//tvTags.Width - 7;
			tspbProgress.Value = 0;
			tspbProgress.Maximum = numSources;
		}
		void ProgressBarUpdate()
		{
			PluginBase.TRACE("-START-");
			tspbProgress.PerformStep();
			tsProgress.Refresh();
			Application.DoEvents();
			PluginBase.TRACE("-END-");
		}
		void ProgressBarHide()
		{
			PluginBase.TRACE("");
			tsProgress.Visible = false;
		}
		
		public void UpdateTbCaption()
		{
			PluginBase.TRACE("-START-");
			try
			{
				string caption = "SourceCookifier - ";
				if (Settings.Configs.SessionMode == Settings.SessionMode.None)
					caption += "[Single file]";
				else if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
					caption += "[N++ session]";
				else
				{
					string curSessionName = Path.GetFileNameWithoutExtension(Settings.SessionFilePath);
					caption += "\"" + curSessionName + "\"";
					if (Settings.SessionChanged) caption += " *";
				}
				int pCaption = Marshal.ReadInt32(pluginBase._ptrNppTbData, 4);
				byte[] newCaption = new System.Text.UnicodeEncoding().GetBytes(caption);
				int p = 0;
				for (; p < newCaption.Length; p++)
					Marshal.WriteByte((IntPtr)(pCaption + p), newCaption[p]);
				Marshal.WriteInt16((IntPtr)(pCaption + p), 0);
				Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_DMMUPDATEDISPINFO, 0, Handle);
				PluginBase.TRACE(string.Format("New caption={0}", caption));
			}
			catch (Exception ex) { pluginBase.ErrorOut(ex); }
			PluginBase.TRACE("-END-");
		}
		
		#region " Search in session "
		// Following functionality only works with my modded Notepad++ version,
		// because the official Notepad++ doesn't expose a function for plug-ins,
		// which searches a given list of files for a text string...
		void TsBtnSearchClick(object sender, EventArgs e)
		{
			PluginBase.TRACE("");
			Settings.Configs.SearchInSession = tsBtnSearch.Checked;
			if (tsBtnSearch.Checked)
			{
				tsBtnSearch.Image = Properties.Resources.search_session;
				tsBtnSearch.ToolTipText = "String search mode";
			}
			else
			{
				tsBtnSearch.Image = Properties.Resources.search_tags;
				tsBtnSearch.ToolTipText = "Tag search mode";
			}
			RefreshTreeView(Source.RefreshType.None, true, -1);
		}
		#endregion
	}
}
