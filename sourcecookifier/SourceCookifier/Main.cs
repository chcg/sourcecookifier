using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NppPluginNET
{
	public partial class PluginBase
	{
		#region " Fields "
		public frmMain frmMain = null; public int idFrmMain = 0;
		frmHelp frmHelp = null;
		Icon iconSourceCookifier2 = null;
		Bitmap bmpSourceCookifier2 = Properties.Resources.cookie_monster_bmp;
		public bool skipTvRefresh = false;
		ContextMenuStrip cmsDefinitions = null;
		public IntPtr _ptrNppTbData = IntPtr.Zero;
		#endregion
		
		#region " StartUp/CleanUp "
		void CommandMenuInit()
		{
			TRACE("-START-");
			try
			{
				StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
				Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
				Settings.ConfigDir = sbIniFilePath.ToString();
				TRACE("nppConfigFolder=" + Settings.ConfigDir);
				if (!Directory.Exists(Settings.ConfigDir)) Directory.CreateDirectory(Settings.ConfigDir);
				Settings.logFilePath = Path.Combine(Settings.ConfigDir, _pluginBaseName + ".errorlog.txt");
				Settings.configSettingsFilePath = Path.Combine(Settings.ConfigDir, _pluginBaseName + ".config.xml");
				Settings.languagesSettingsFilePath = Path.Combine(Settings.ConfigDir, _pluginBaseName + ".languages.xml");
				Settings.languagesModelFilePath = Path.Combine(Settings.ConfigDir, _pluginBaseName + ".languages.model.xml");
	
				try
				{
					Settings.LoadConfigs();
					TRACE("Configs loaded");
				}
				catch (Exception ex)
				{
					ErrorOut(ex);
					TRACE("Resetting configs");
					Settings.Configs = new Settings.Config();
				}
				
				SetCommand(0, "Toogle SourceCookifier", ShowFrmMain, new ShortcutKey(true, true, true, Keys.S));
				SetCommand(1, "", null);
				SetCommand(2, "Go To Definition", GoToDefinition, new ShortcutKey(true, false, true, Keys.Enter));
				SetCommand(3, "Navigate Backward", NavigateBackward, new ShortcutKey(false, true, false, Keys.Left));
				SetCommand(4, "Navigate Forward", NavigateForward, new ShortcutKey(false, true, false, Keys.Right));
				SetCommand(5, "", null);
				#if FIND_IN_SESSION
				SetCommand(6, "Find In Session", FindInSession, new ShortcutKey(true, true, true, Keys.Enter));
				SetCommand(7, "", null);
				SetCommand(8, "Language settings", ShowSettings);
				SetCommand(9, "Options", ShowOptions);
				SetCommand(10, "", null);
				SetCommand(11, "Help && About", ShowHelp);
				#else
				SetCommand(6, "Language settings", ShowSettings);
				SetCommand(7, "Options", ShowOptions);
				SetCommand(8, "", null);
				SetCommand(9, "Help && About", ShowHelp);
				#endif
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		void PluginCleanUp()
		{
			TRACE("-START-");
			if (frmMain != null) frmMain.FrmMainFormClosing();
			try
			{
				Settings.SaveSettings();
				TRACE("Configs saved");
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		#endregion
		
		#region " Menu functions "
		public void ShowFrmMain()
		{
			TRACE("-START-");
			try
			{
				if (frmMain == null)
				{
					InitFrmMain();
					TRACE("frmMain initialized");
				}
				else
				{
					if (frmMain.Visible)
					{
						if (frmMain.Focused || frmMain.tsTbxSearchFilter.Focused || frmMain.tvTags.Focused)
						{
							Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
							Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
							TRACE("Current scintilla focused");
						}
						else
						{
							frmMain.tvTags.Focus();
							TRACE("frmMain focused");
						}
					}
					else
					{
						Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmMain.Handle);
						frmMain.tvTags.Focus();
						if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
							frmMain.DoAllOpenedDocuments();
						TRACE("frmMain shown and focused");
					}
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		void InitFrmMain()
		{
			TRACE("-START-");
			frmMain = new frmMain(this);

			using (Bitmap newBmp = new Bitmap(16, 16))
			{
				Graphics g = Graphics.FromImage(newBmp);
				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Color.Fuchsia;
				colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
				ImageAttributes attr = new ImageAttributes();
				attr.SetRemapTable(colorMap);
				g.DrawImage(bmpSourceCookifier2, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
				iconSourceCookifier2 = Icon.FromHandle(newBmp.GetHicon());
				TRACE("Toolbar icon prepared");
			}
			
			NppTbData _nppTbData = new NppTbData();
			_nppTbData.hClient = frmMain.Handle;
			_nppTbData.pszName = "SourceCookifier												 ";
			_nppTbData.dlgID = idFrmMain;
			_nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
			_nppTbData.hIconTab = (uint)iconSourceCookifier2.Handle;
			_nppTbData.pszModuleName = _pluginModuleName;
			_ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
			Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);
			TRACE("NppTbData allocated");

			Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
			frmMain.tvTags.Focus();
				
			if (Settings.Configs.SessionMode == Settings.SessionMode.None) frmMain.DoCurrentSciBufferTags();

			frmMain.UpdateTbCaption();
			TRACE("-END-");
		}
		
		int jumpPos = 0;
		struct Jump
		{
			public string FilePath;
			public int Line;
		}
		Stack<Jump> jumpStack = new Stack<Jump>();
		void GoToDefinition()
		{
			TRACE("-START-");
			try
			{
				StringBuilder sbWord = new StringBuilder(128);
				if (Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTWORD, 128, sbWord) != IntPtr.Zero)
				{
					String word = sbWord.ToString();
					List<Tag> lstMatches = new List<Tag>();
					List<QuickTag> lstMatchesQ = new List<QuickTag>();

					if (frmMain == null)
					{
						ShowFrmMain();
						Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
					}
					else if (!frmMain.Visible)
					{
						if (Settings.Configs.SessionMode == Settings.SessionMode.None)
							frmMain.DoCurrentSciBufferTags();
						else if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
							frmMain.DoAllOpenedDocuments();
					}

					int numSources = 0;
					TRACE(string.Format("Searching word '{0}'", sbWord));
					if (sbWord.Length > 0)
					{
						foreach (TreeNode sourceNode in frmMain.tvTags.Nodes)
						{
							bool found = false;
							if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
								&& (sourceNode.Name == Source.INCLUDES))
							{
								foreach (IncludeFile incFile in sourceNode.Nodes)
								{
									if (incFile.QuickTags == null) continue;
									bool _found = false;
									foreach (QuickTag qtag in incFile.QuickTags)
										if (string.Compare(qtag.Text, word, !Settings.Languages[qtag.Language].CaseSensitive) == 0)
											{ lstMatchesQ.Add(qtag); _found = true; }
									if (_found) numSources++;
								}
							}
							else
							{
								Source source = (Source)sourceNode.Tag;
								foreach (string tagType in source.TagTypes.Keys)
									foreach (Tag tag in source.TagTypes[tagType].Tags)
										if (string.Compare(tag.TagName, word, !Settings.Languages[tag.Language].CaseSensitive) == 0)
											{ lstMatches.Add(tag); found = true; }
							}
							if (found) numSources++;
						}
						foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
						{
							bool found = false;
							Source source = (Source)sourceNode.Tag;
							foreach (string tagType in source.TagTypes.Keys)
								foreach (Tag tag in source.TagTypes[tagType].Tags)
									if (string.Compare(tag.TagName, word, !Settings.Languages[tag.Language].CaseSensitive) == 0)
										{ lstMatches.Add(tag); found = true; }
							if (found) numSources++;
						}
						foreach (IncludeFile incFile in Source.invisibleIncludeFileNodes)
						{
							if (incFile.QuickTags == null) continue;
							bool _found = false;
							foreach (QuickTag qtag in incFile.QuickTags)
								if (string.Compare(qtag.Text, word, !Settings.Languages[qtag.Language].CaseSensitive) == 0)
									{ lstMatchesQ.Add(qtag); _found = true; }
							if (_found) numSources++;
						}
					}
					TRACE(string.Format("Word found {0} times", numSources));
					
					if ((lstMatches.Count + lstMatchesQ.Count) > 256)
					{
						if (MessageBox.Show("You will get more than 256 matches.\nGoing on with the search might \n"
							+ "deadlock the Notepad++ process.\nDo you want to cancel the search?",
							"SourceCookifier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
							== DialogResult.Yes) return;
						TRACE("Ignoring 256 matches limit");
					}
					
					if ((lstMatches.Count > 0) || (lstMatchesQ.Count > 0))
					{
						TRACE(string.Format("lstMatches.Count={0} lstMatchesQ.Count={1}", lstMatches.Count, lstMatchesQ.Count));
						if ((lstMatches.Count == 1) && (lstMatchesQ.Count == 0))
						{
							PushJump(lstMatches[0].SourceFile, lstMatches[0].Line);
							ShowLineInNotepadPP(lstMatches[0].SourceFile, lstMatches[0].Line, true);
						}
						else if ((lstMatchesQ.Count == 1) && (lstMatches.Count == 0))
						{
							PushJump(lstMatchesQ[0].SourceFile, lstMatchesQ[0].Line);
							ShowLineInNotepadPP(lstMatchesQ[0].SourceFile, lstMatchesQ[0].Line, true);
						}
						else
						{
							string currentSourceFile = "";
							StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
							if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, sbPath) == 1)
								currentSourceFile = sbPath.ToString();
							
							if (cmsDefinitions == null)
							{
								cmsDefinitions = new ContextMenuStrip();
								cmsDefinitions.ItemClicked += new ToolStripItemClickedEventHandler(cmsDefinitions_ItemClicked);
								cmsDefinitions.ImageList = Settings.IconList;
							}
							else
							{
								cmsDefinitions.Items.Clear();
							}
							
							Font fontBold = new Font(cmsDefinitions.Font.FontFamily, cmsDefinitions.Font.Size, FontStyle.Bold);
							Font fontItalic = new Font(cmsDefinitions.Font.FontFamily, cmsDefinitions.Font.Size, FontStyle.Italic);
							Font fontItalicBold = new Font(cmsDefinitions.Font.FontFamily, cmsDefinitions.Font.Size, FontStyle.Italic | FontStyle.Bold);
							
							int numInserts = 0;
							foreach (Tag tag in lstMatches)
							{
								string path = "";
								if (tag.SourceFile != currentSourceFile)
								{
									if (Settings.Configs.ShowGtdToolTips)
										path = " - " + Path.GetFileName(tag.SourceFile);
									else
										path = " - " + tag.SourceFile;
								}
								
								ToolStripMenuItem tsmi = new ToolStripMenuItem(string.Format("{0} [{1}]{2}",
									(tag.ToolTipText != "") ? tag.ToolTipText : tag.Text, tag.Line, path));
								
								if (Settings.Configs.ShowGtdToolTips)
									tsmi.ToolTipText = tag.SourceFile;
								tsmi.ImageIndex = Settings.Languages[tag.Language].TagTypes[tag.Identifier].ImageListIndex;
								tsmi.ForeColor = Color.FromArgb(Settings.Languages[tag.Language].TagTypes[tag.Identifier].ForeColor);
								tsmi.Tag = tag;
								if (tag.SourceFile == currentSourceFile)
								{
									tsmi.Font = fontBold;
									cmsDefinitions.Items.Insert(numInserts++, tsmi);
								}
								else
								{
									cmsDefinitions.Items.Add(tsmi);
								}
							}
							foreach (QuickTag qtag in lstMatchesQ)
							{
								string path = "";
								if (qtag.SourceFile != currentSourceFile)
								{
									if (Settings.Configs.ShowGtdToolTips)
										path = " - " + Path.GetFileName(qtag.SourceFile);
									else
										path = " - " + qtag.SourceFile;
								}
								
								ToolStripMenuItem tsmi = new ToolStripMenuItem(string.Format("{0} [{1}]{2}",
									qtag.FullText, qtag.Line, path));

								if (Settings.Configs.ShowGtdToolTips)
									tsmi.ToolTipText = qtag.SourceFile;
								tsmi.ImageIndex = Settings.Languages[qtag.Language].TagTypes[qtag.Identifier].ImageListIndex;
								tsmi.ForeColor = Color.FromArgb(Settings.Languages[qtag.Language].TagTypes[qtag.Identifier].ForeColor);
								tsmi.Tag = qtag;
								if (qtag.SourceFile == currentSourceFile)
								{
									tsmi.Font = fontItalicBold;
									cmsDefinitions.Items.Insert(numInserts++, tsmi);
								}
								else
								{
									tsmi.Font = fontItalic;
									cmsDefinitions.Items.Add(tsmi);
								}
							}
							if ((numInserts > 0) && (numInserts < cmsDefinitions.Items.Count))
							{
								cmsDefinitions.Items.Insert(numInserts, new ToolStripSeparator());
							}
							
							IntPtr curSci = GetCurrentScintilla();
							int currentPos = (int)Win32.SendMessage(curSci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
							Point pt = new Point();
							pt.X = (int)Win32.SendMessage(curSci, SciMsg.SCI_POINTXFROMPOSITION, 0, currentPos);
							pt.Y = (int)Win32.SendMessage(curSci, SciMsg.SCI_POINTYFROMPOSITION, 0, currentPos);
							Win32.ClientToScreen(curSci, ref pt);
							TRACE(string.Format("Showing cmsDefinitions menu at X={0} Y={1}", pt.X, pt.Y));
							cmsDefinitions.Show(pt);
						}
					}
					else
					{
						Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
						System.Media.SystemSounds.Hand.Play();
					}
				}
				else
				{
					TRACE("NPPM_GETCURRENTWORD failed");
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		void cmsDefinitions_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			TRACE("-START-");
			try
			{
				if (e.ClickedItem.Tag is Tag)
				{
					Tag tag = e.ClickedItem.Tag as Tag;
					PushJump(tag.SourceFile, tag.Line);
					ShowLineInNotepadPP(tag.SourceFile, tag.Line, true);
				}
				else if (e.ClickedItem.Tag is QuickTag)
				{
					QuickTag qtag = e.ClickedItem.Tag as QuickTag;
					PushJump(qtag.SourceFile, qtag.Line);
					ShowLineInNotepadPP(qtag.SourceFile, qtag.Line, true);
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		public void PushJump(string source, int line)
		{
			TRACE("-START-");
			IntPtr curScintilla = GetCurrentScintilla();
			StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
			if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, sbPath) == 1)
			{
				Jump jumpNewPos = new Jump();
				jumpNewPos.FilePath = source;
				jumpNewPos.Line = line;
				Jump jumpOldPos = new Jump();
				jumpOldPos.FilePath = sbPath.ToString();
				if ((jumpOldPos.FilePath != "") && File.Exists(jumpOldPos.FilePath))
				{
					jumpOldPos.Line = (int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0) + 1;
					while (((jumpStack.Count) > jumpPos)
							|| (((jumpStack.Count > 0))
								&& (jumpStack.Peek().FilePath == jumpOldPos.FilePath)
								&& (jumpStack.Peek().Line == jumpOldPos.Line)))
						jumpStack.Pop();
					jumpStack.Push(jumpOldPos);
					jumpStack.Push(jumpNewPos);
					jumpPos = jumpStack.Count;
				}
			}
			TRACE("-END-");
		}
		public void ShowLineInNotepadPP(string source, int line, bool selectTagNodeByCurrentLine)
		{
			TRACE("-START-");
			frmMain.SkipSelectTagNodeByCurrentLine = !selectTagNodeByCurrentLine;
			if (Settings.Configs.SessionMode == Settings.SessionMode.None)
				skipTvRefresh = true;
			TRACE(string.Format("Opening file '{0}'", source));
			Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, source);
			
			IntPtr curScintilla = GetCurrentScintilla();
			Win32.SendMessage(curScintilla, SciMsg.SCI_DOCUMENTEND, 0, 0);
			Win32.SendMessage(curScintilla, SciMsg.SCI_ENSUREVISIBLEENFORCEPOLICY, line - 1, 0);
			Win32.SendMessage(curScintilla, SciMsg.SCI_GOTOLINE, line - 1, 0);
			Win32.SendMessage(curScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
			TRACE("-END-");
		}
		void NavigateBackward()
		{
			TRACE("-START-");
			try
			{
				if ((frmMain == null) || (jumpPos <= 1)) throw new Exception();
				int newPos = jumpStack.Count - (--jumpPos);
				ShowLineInNotepadPP(jumpStack.ToArray()[newPos].FilePath, jumpStack.ToArray()[newPos].Line, true);
			}
			catch { System.Media.SystemSounds.Hand.Play(); }
			TRACE("-END-");
		}
		void NavigateForward()
		{
			TRACE("-START-");
			try
			{
				if ((jumpStack.Count) <= jumpPos) throw new Exception();
				int newPos = jumpStack.Count - (++jumpPos);
				ShowLineInNotepadPP(jumpStack.ToArray()[newPos].FilePath, jumpStack.ToArray()[newPos].Line, true);
			}
			catch { System.Media.SystemSounds.Hand.Play(); }
			TRACE("-END-");
		}
		
		public void ShowSettings()
		{
			TRACE("-START-");
			try
			{
				if (frmMain == null)
				{
					ShowFrmMain();
					Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
				}
				frmSettings frmSettings = new frmSettings(this);
				if (frmSettings.ShowDialog() == DialogResult.OK)
				{
					if (frmSettings.Changed)
					{
						Settings.Languages = frmSettings.tempLanguages;
						Settings.LoadTagTypeIcons(frmMain.tvTags);
						if (frmMain.Visible)
							frmMain.ResetTreeView(frmSettings.ChangedEx);
						TRACE("Changes accepted");
					}
					else
					{
						TRACE("Nothing changed");
					}
				}
				else
				{
					if (frmSettings.Tried)
					{
						TRACE("Resetting tried settings");
						Settings.Languages = frmSettings.oldLanguages;
						Settings.LoadTagTypeIcons(frmMain.tvTags);
						frmMain.ResetTreeView(frmSettings.ChangedEx);
					}
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		} 
		public void ShowOptions()
		{
			TRACE("-START-");
			try
			{
				frmOptions frmOptions = new frmOptions();
				frmOptions.lblBackColor.BackColor = Color.FromArgb(Settings.Configs.BackColor);
				frmOptions.cbxShowMenuAtBottom.Checked = Settings.Configs.ShowMenuAtBottom;
				frmOptions.cbxFlowingMenuButtons.Checked = Settings.Configs.FlowingMenuButtons;
				frmOptions.cbxStartupShowMode.Text = Settings.Configs.StartupShowMode;
				frmOptions.cbxStartupSessionMode.Text = Settings.Configs.StartupSessionMode;
				frmOptions.cbxShowInvalidSources.Checked = Settings.Configs.ShowInvalidSources;
				frmOptions.cbxShowToolTips.Checked = Settings.Configs.ShowToolTips;
				frmOptions.cbxShowGtdToolTips.Checked = Settings.Configs.ShowGtdToolTips;
				frmOptions.tbxFileSizeLimit.Text = Settings.Configs.FileSizeLimit;
				frmOptions.cbxCaseSensitiveExt.Checked = Settings.Configs.CaseSensitiveLanguageMaps;
				if (frmOptions.ShowDialog() == DialogResult.OK)
				{
					Settings.Configs.BackColor = frmOptions.lblBackColor.BackColor.ToArgb();
					Settings.Configs.ShowMenuAtBottom = frmOptions.cbxShowMenuAtBottom.Checked;
					Settings.Configs.FlowingMenuButtons = frmOptions.cbxFlowingMenuButtons.Checked;
					Settings.Configs.StartupShowMode = frmOptions.cbxStartupShowMode.Text;
					Settings.Configs.StartupSessionMode = frmOptions.cbxStartupSessionMode.Text;
					Settings.Configs.ShowInvalidSources = frmOptions.cbxShowInvalidSources.Checked;
					Settings.Configs.ShowToolTips = frmOptions.cbxShowToolTips.Checked;
					Settings.Configs.ShowGtdToolTips = frmOptions.cbxShowGtdToolTips.Checked;
					Settings.Configs.FileSizeLimit = "";
					string sLimit = frmOptions.tbxFileSizeLimit.Text; long l;
					if ((sLimit.EndsWith("mb") || sLimit.EndsWith("kb"))
						&& long.TryParse(sLimit.Replace("mb", "").Replace("kb", ""), out l))
						Settings.Configs.FileSizeLimit = sLimit;
					Settings.Configs.CaseSensitiveLanguageMaps = frmOptions.cbxCaseSensitiveExt.Checked;
					
					if (frmMain != null)
					{
						frmMain.tvTags.BackColor = frmOptions.lblBackColor.BackColor;
						frmMain.tsBar.Dock = Settings.Configs.ShowMenuAtBottom ? DockStyle.Bottom : DockStyle.Top;
						frmMain.tsBar.LayoutStyle = Settings.Configs.FlowingMenuButtons ?
							ToolStripLayoutStyle.Flow : ToolStripLayoutStyle.HorizontalStackWithOverflow;
						frmMain.tsBtnClearSearchFilter.Height = frmMain.tsBtnRefresh.Height;
						frmMain.ttTv.Active = Settings.Configs.ShowToolTips;
					}
					TRACE("Changes accepted");
				}
				else
				{
					TRACE("Nothing changed");
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		public void ShowHelp()
		{
			TRACE("-START-");
			try
			{
				if ((frmHelp == null) || !frmHelp.Visible) frmHelp = new frmHelp();
				frmHelp.Show();
				TRACE("Help window shown");
			}
			catch (Exception ex) { ErrorOut(ex); }
			TRACE("-END-");
		}
		
		#region " Find in session "
		// Following functionality only works with my modded Notepad++ version,
		// because the official Notepad++ doesn't expose a function for plug-ins,
		// which searches a given list of files for a text string...
		int NPPM_LAUNCHFINDINFILESDIRECTLY = 0x0400 + 1000 + 99;
		void FindInSession()
		{
			TRACE("-START-");
			try
			{
				StringBuilder sbWord = new StringBuilder(128);
				if (Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTWORD, 128, sbWord) != IntPtr.Zero)
				{
					if (frmMain == null)
					{
						ShowFrmMain();
						Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
					}
					
					if (sbWord.Length > 0)
					{
						if (!SearchInSession(sbWord.ToString()))
						{
							Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
							System.Media.SystemSounds.Hand.Play();
						}
					}
					else
					{
						Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
						System.Media.SystemSounds.Hand.Play();
					}
				}
				else
				{
					TRACE("NPPM_GETCURRENTWORD failed");
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
		}
		internal bool SearchInSession(string str)
		{
			TRACE("-START-");
			bool ret = false;
			List<string> lstFiles = frmMain.GetAllFileNames();
			
			TRACE(string.Format("Searching word '{0}' in {1} node(s)", str, lstFiles.Count));
			if (lstFiles.Count > 0)
			{
				using (ClikeStringArray cStrArray = new ClikeStringArray(lstFiles))
				{
					Win32.SendMessage(nppData._nppHandle, (NppMsg)NPPM_LAUNCHFINDINFILESDIRECTLY, cStrArray.NativePointer, str);
				}
				ret = true;
			}
			
			TRACE("-END-");
			return ret;
		}
		#endregion
		#endregion
		
		#region " Logging "
		public void ErrorOut(Exception ex)
		{
			TRACE(ex.Message);
			try
			{
				using (TextWriter w = new StreamWriter(Settings.logFilePath, true))
				{
					w.WriteLine(string.Format(
						"\n{0}-{1}-{2} {3}-{4}-{5}:\n" +
						"====================",
						DateTime.Now.Year,
						DateTime.Now.Month.ToString("00"),
						DateTime.Now.Day.ToString("00"),
						DateTime.Now.Hour.ToString("00"),
						DateTime.Now.Minute.ToString("00"),
						DateTime.Now.Second.ToString("00")));
					w.WriteLine(ex.Message);
					w.WriteLine(ex.StackTrace);
				}
				MessageBox.Show("Owing to unfortunate circumstances an error with the following message occured:\n\n"
								+ "\"" + ex.Message + "\"\n\n"
								+ "Hence a logfile has been written to the SourceCookifier folder.\n"
								+ "Please post its content in the forum, if you think it's worth being fixed.\n"
								+ "Sorry for the inconvenience.",
								"SourceCookifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception e)
			{
				MessageBox.Show("Error while attempting to write error logfile:\n" + e.Message,
								"SourceCookifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		[Conditional("TRACE_ALL")]
		public static void TRACE(string msg)
		{
			try
			{
				if (string.IsNullOrEmpty(TRACEPATH))
				{
					TRACEPATH = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\SourceCookifier.TRACE.txt");
					TRACEPID = Process.GetCurrentProcess().Id.ToString("X04");
				}
				using (TextWriter w = new StreamWriter(TRACEPATH, true))
				{
					StackTrace stackTrace = new StackTrace();
					MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
					w.WriteLine(string.Format("{0}:{1:000} <{2}> {3}[{4}] {5}",
						DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond, TRACEPID,
						new String(' ', (stackTrace.FrameCount - 1) * 3), methodBase.Name, msg));
				}
			}
			catch (Exception ex)
			{
				if (!TRACEERRORSHOWN)
				{
					TRACEERRORSHOWN = true;
					MessageBox.Show("Error while attempting to write trace file:\n" + ex.Message,
									"SourceCookifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		static string TRACEPATH, TRACEPID;
		static bool TRACEERRORSHOWN;
		#endregion
	}
}