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
	            string nppConfigFolder = sbIniFilePath.ToString();
	            TRACE("nppConfigFolder=" + nppConfigFolder);
	            if (!Directory.Exists(nppConfigFolder)) Directory.CreateDirectory(nppConfigFolder);
				Settings.logFilePath = Path.Combine(nppConfigFolder, _pluginBaseName + ".errorlog.txt");
				Settings.configSettingsFilePath = Path.Combine(nppConfigFolder, _pluginBaseName + ".config.xml");
				Settings.languagesSettingsFilePath = Path.Combine(nppConfigFolder, _pluginBaseName + ".languages.xml");
				Settings.languagesModelFilePath = Path.Combine(nppConfigFolder, _pluginBaseName + ".languages.model.xml");
	
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
	            		if (frmMain.Focused || frmMain.tsTbxSearch.Focused || frmMain.tvTags.Focused)
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
            _nppTbData.pszName = "SourceCookifier                                                 ";
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

        			bool hidden = (frmMain == null) || !frmMain.Visible;
        			if (hidden) ShowFrmMain();

        			int numSources = 0;
        			TRACE(string.Format("Searching word '{0}'", sbWord));
        			if (sbWord.Length > 0)
        			{
        			    frmMain.tsTbxSearch.Clear();
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
		        						if (qtag.Text == word) { lstMatchesQ.Add(qtag); _found = true; }
			        				if (_found) numSources++;
	        					}
	        				}
	        				else if (Settings.Configs.FlatView)
	        				{
	        					foreach (Tag tag in sourceNode.Nodes)
	        						if (tag.Text == word) { lstMatches.Add(tag); found = true; }
	        				}
	        				else
	        				{
	        					foreach (TreeNode tagTypeNode in sourceNode.Nodes)
	        					{
		        					foreach (Tag tag in tagTypeNode.Nodes)
		        						if (tag.Text == word) { lstMatches.Add(tag); found = true; }
	        					}
	        				}
	        				if (found) numSources++;
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
        					ShowLineInNotepadPP(lstMatches[0].SourceFile, lstMatches[0].Line);
        				}
        				else if ((lstMatchesQ.Count == 1) && (lstMatches.Count == 0))
        				{
		        			PushJump(lstMatchesQ[0].SourceFile, lstMatchesQ[0].Line);
        					ShowLineInNotepadPP(lstMatchesQ[0].SourceFile, lstMatchesQ[0].Line);
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
		        				TreeNode sourceNode = tag as TreeNode;
		        				while (sourceNode.Parent != null) sourceNode = sourceNode.Parent;
		        				string language = (sourceNode.Tag as Source).Language;
		        				
        						string path = "";
        						if ((numSources > 1) && (tag.SourceFile != currentSourceFile))
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
								tsmi.ImageIndex = Settings.Languages[language].TagTypes[tag.Identifier].ImageListIndex;
								tsmi.ForeColor = Color.FromArgb(Settings.Languages[language].TagTypes[tag.Identifier].ForeColor);
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
        						if ((numSources > 1) && (qtag.SourceFile != currentSourceFile))
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
        			
        			if (hidden)
        			{
	          			Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
		                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
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
	        		ShowLineInNotepadPP(tag.SourceFile, tag.Line);
        		}
        		else if (e.ClickedItem.Tag is QuickTag)
        		{
        			QuickTag qtag = e.ClickedItem.Tag as QuickTag;
	        		PushJump(qtag.SourceFile, qtag.Line);
	        		ShowLineInNotepadPP(qtag.SourceFile, qtag.Line);
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
        public void ShowLineInNotepadPP(string source, int line)
        {
        	TRACE("-START-");
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
        		ShowLineInNotepadPP(jumpStack.ToArray()[newPos].FilePath, jumpStack.ToArray()[newPos].Line);
        	}
        	catch  { System.Media.SystemSounds.Hand.Play(); }
        	TRACE("-END-");
        }
        void NavigateForward()
        {
        	TRACE("-START-");
        	try
        	{
        		if ((jumpStack.Count) <= jumpPos) throw new Exception();
        		int newPos = jumpStack.Count - (++jumpPos);
        		ShowLineInNotepadPP(jumpStack.ToArray()[newPos].FilePath, jumpStack.ToArray()[newPos].Line);
        	}
        	catch  { System.Media.SystemSounds.Hand.Play(); }
        	TRACE("-END-");
        }
        
        void ShowSettings()
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
		        			frmMain.ResetTreeView(frmSettings.ExtensionChanged);
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
	        			frmMain.ResetTreeView(frmSettings.ExtensionChanged);
	        		}
	        	}
         	}
         	catch (Exception ex) { ErrorOut(ex); }
         	TRACE("-END-");
        } 
        void ShowOptions()
        {
        	TRACE("-START-");
        	try
        	{
        		frmOptions frmOptions = new frmOptions();
        		frmOptions.lblBackColor.BackColor = Color.FromArgb(Settings.Configs.BackColor);
        		frmOptions.cbxCaseSensitiveExt.Checked = Settings.Configs.CaseSensitiveLanguageMaps;
        		frmOptions.cbxShowInvalidSources.Checked = Settings.Configs.ShowInvalidSources;
        		frmOptions.cbxUseCtagsFile.Checked = Settings.Configs.UseTagFile;
        		frmOptions.cbxShowToolTips.Checked = Settings.Configs.ShowToolTips;
        		frmOptions.cbxShowGtdToolTips.Checked = Settings.Configs.ShowGtdToolTips;
        		if (frmOptions.ShowDialog() == DialogResult.OK)
        		{
        			Settings.Configs.BackColor = frmOptions.lblBackColor.BackColor.ToArgb();
        			Settings.Configs.CaseSensitiveLanguageMaps = frmOptions.cbxCaseSensitiveExt.Checked;
        			Settings.Configs.ShowInvalidSources = frmOptions.cbxShowInvalidSources.Checked;
        			Settings.Configs.UseTagFile = frmOptions.cbxUseCtagsFile.Checked;
        			Settings.Configs.ShowToolTips = frmOptions.cbxShowToolTips.Checked;
        			Settings.Configs.ShowGtdToolTips = frmOptions.cbxShowGtdToolTips.Checked;
        			
        			if (frmMain != null)
        			{
        				frmMain.tvTags.BackColor = frmOptions.lblBackColor.BackColor;
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
        void ShowHelp()
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
        int NPPM_LAUNCHFINDINFILESDIRECTLY = 0x0400 + 1000 + 78;
        void FindInSession()
        {
        	TRACE("-START-");
        	try
        	{
	    		StringBuilder sbWord = new StringBuilder(128);
        		if (Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTWORD, 128, sbWord) != IntPtr.Zero)
        		{
        			bool hidden = (frmMain == null) || !frmMain.Visible;
        			if (hidden) ShowFrmMain();
        			frmMain.tsTbxSearch.Clear();
        			
        			IntPtr curSci = GetCurrentScintilla();
        			if (sbWord.Length > 0)
        			{
        				if (!SearchInSession(sbWord.ToString()))
        				{
							Win32.SendMessage(curSci, SciMsg.SCI_GRABFOCUS, 0, 0);
							System.Media.SystemSounds.Hand.Play();
        				}
        			}
        			else
        			{
        				Win32.SendMessage(curSci, SciMsg.SCI_GRABFOCUS, 0, 0);
        				System.Media.SystemSounds.Hand.Play();
        			}
        			
        			if (hidden)
        			{
	          			Win32.SendMessage(curSci, SciMsg.SCI_GRABFOCUS, 0, 0);
		                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
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
			List<string> lstFiles = new List<string>();
			foreach (TreeNode rootNode in frmMain.tvTags.Nodes)
			{
				if (rootNode.Tag is Source)
				{
				    lstFiles.Add(((Source)rootNode.Tag).PathUnquoted);
				}
				else if (rootNode.Name == Source.INCLUDES)
				{
					foreach (IncludeFile incFile in rootNode.Nodes)
					{
						lstFiles.Add(incFile.Name);
					}
				}
			}
			
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