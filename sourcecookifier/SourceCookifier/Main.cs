using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;

namespace SourceCookifier
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "SourceCookifier";
        internal static frmMain frmMain = null;
        internal static int idFrmMain = 0;
        static frmHelp frmHelp = null;
        static Bitmap bmpSourceCookifier = Properties.Resources.cookie_monster.ToBitmap();
        static Icon iconSourceCookifier2 = null;
        static Bitmap bmpSourceCookifier2 = Properties.Resources.cookie_monster_bmp;
        internal static bool skipTvRefresh = false;
        static ContextMenuStrip cmsDefinitions = null;
        internal static IntPtr _ptrNppTbData = IntPtr.Zero;
        #endregion

        #region " StartUp/CleanUp "
        static Main()
        {
            // Following code let's you reference any additional .net assembly
            // from a subfolder which has the same name as your plugin. Example:
            // ...\plugins\MyNppPlugin.dll
            // ...\plugins\MyNppPlugin\AdditionalAssembly1.dll
            // ...\plugins\MyNppPlugin\AdditionalAssembly2.dll
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromPluginSubFolder);
        }
        static Assembly LoadFromPluginSubFolder(object sender, ResolveEventArgs args)
        {
            string pluginPath = typeof(Main).Assembly.Location;
            string pluginName = Path.GetFileNameWithoutExtension(pluginPath);
            string pluginSubFolder = Path.Combine(Path.GetDirectoryName(pluginPath), pluginName);
            string assemblyPath = Path.Combine(pluginSubFolder, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            else
            {
                // HACK: allow loading of session files created with older assembly versions
                Assembly asm = typeof(Main).Assembly;
                if (args.Name.Contains("SourceCookifier, Version="))
                    return typeof(Main).Assembly;
            }
            return null;
        }
        internal static void CommandMenuInit()
        {
            TRACE("-START-");
            try
            {
                StringBuilder sbPluginConfigDir = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbPluginConfigDir);
                Settings.ConfigDir = sbPluginConfigDir.ToString();
                TRACE("nppConfigFolder=" + Settings.ConfigDir);
                Settings.ConfigDir = Path.Combine(Settings.ConfigDir, "SourceCookifier");
                if (!Directory.Exists(Settings.ConfigDir)) Directory.CreateDirectory(Settings.ConfigDir);
                Settings.logFilePath = Path.Combine(Settings.ConfigDir, PluginName + ".errorlog.txt");
                Settings.configSettingsFilePath = Path.Combine(Settings.ConfigDir, PluginName + ".config.xml");
                Settings.languagesSettingsFilePath = Path.Combine(Settings.ConfigDir, PluginName + ".languages.xml");
                Settings.languagesModelFilePath = Path.Combine(Settings.ConfigDir, PluginName + ".languages.model.xml");

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

                PluginBase.SetCommand(0, "Toogle SourceCookifier", ShowFrmMain, new ShortcutKey(true, true, true, Keys.S));
                PluginBase.SetCommand(1, "", null);
                PluginBase.SetCommand(2, "Go To Definition", GoToDefinition, new ShortcutKey(true, false, true, Keys.Enter));
                PluginBase.SetCommand(3, "Navigate Backward", NavigateBackward, new ShortcutKey(false, true, false, Keys.Left));
                PluginBase.SetCommand(4, "Navigate Forward", NavigateForward, new ShortcutKey(false, true, false, Keys.Right));
                PluginBase.SetCommand(5, "", null);
                #if FIND_IN_SESSION
                PluginBase.SetCommand(6, "Find In Session", FindInSession, new ShortcutKey(true, true, true, Keys.Enter));
                PluginBase.SetCommand(7, "", null);
                PluginBase.SetCommand(8, "Language settings", ShowSettings);
                PluginBase.SetCommand(9, "Options", ShowOptions);
                PluginBase.SetCommand(10, "", null);
                PluginBase.SetCommand(11, "Help && About", ShowHelp);
                #else
                PluginBase.SetCommand(6, "Language settings", ShowSettings);
                PluginBase.SetCommand(7, "Options", ShowOptions);
                PluginBase.SetCommand(8, "", null);
                PluginBase.SetCommand(9, "Help && About", ShowHelp);
                #endif
            }
            catch (Exception ex) { ErrorOut(ex); }
            TRACE("-END-");
        }
        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = bmpSourceCookifier.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idFrmMain]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }
        internal static void PluginCleanUp()
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
        internal static void ShowFrmMain()
        {
            TRACE("-START-");
            try
            {
                if (Settings.HideAtStartup)
                {
                    Settings.HideAtStartup = false;
                    return;
                }
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
                            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
                            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
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
                        Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmMain.Handle);
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
        static void InitFrmMain()
        {
            TRACE("-START-");
            frmMain = new frmMain();

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
            _nppTbData.pszModuleName = PluginName + ".dll";
            _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
            Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);
            TRACE("NppTbData allocated");

            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            frmMain.tvTags.Focus();

            if (Settings.Configs.SessionMode == Settings.SessionMode.None) frmMain.DoCurrentSciBufferTags();

            frmMain.UpdateTbCaption();
            TRACE("-END-");
        }

        static int jumpPos = 0;
        struct Jump
        {
            public string FilePath;
            public int Line;
        }
        static Stack<Jump> jumpStack = new Stack<Jump>();
        static void GoToDefinition()
        {
            TRACE("-START-");
            try
            {
                StringBuilder sbWord = new StringBuilder(4096);
                if (Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTWORD, 4096, sbWord) != IntPtr.Zero)
                {
                    String word = sbWord.ToString();
                    List<Tag> lstMatches = new List<Tag>();
                    List<QuickTag> lstMatchesQ = new List<QuickTag>();

                    if (frmMain == null)
                    {
                        ShowFrmMain();
                        Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
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
                            if ((long)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, sbPath) == 1)
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

                            IntPtr curSci = PluginBase.GetCurrentScintilla();
                            long currentPos = (long)Win32.SendMessage(curSci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
                            Point pt = new Point();
                            pt.X = (int)(long)Win32.SendMessage(curSci, SciMsg.SCI_POINTXFROMPOSITION, 0, currentPos);
                            pt.Y = (int)(long)Win32.SendMessage(curSci, SciMsg.SCI_POINTYFROMPOSITION, 0, currentPos);
                            Win32.ClientToScreen(curSci, ref pt);
                            TRACE(string.Format("Showing cmsDefinitions menu at X={0} Y={1}", pt.X, pt.Y));
                            cmsDefinitions.Show(pt);
                        }
                    }
                    else
                    {
                        Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
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
        static void cmsDefinitions_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
        internal static void PushJump(string source, int line)
        {
            TRACE("-START-");
            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
            StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
            if ((long)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, sbPath) == 1)
            {
                Jump jumpNewPos = new Jump();
                jumpNewPos.FilePath = source;
                jumpNewPos.Line = line;
                Jump jumpOldPos = new Jump();
                jumpOldPos.FilePath = sbPath.ToString();
                if ((jumpOldPos.FilePath != "") && File.Exists(jumpOldPos.FilePath))
                {
                    jumpOldPos.Line = (int)(long)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0) + 1;
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
        internal static void ShowLineInNotepadPP(string source, int line, bool selectTagNodeByCurrentLine)
        {
            TRACE("-START-");
            frmMain.SkipSelectTagNodeByCurrentLine = !selectTagNodeByCurrentLine;
            if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                skipTvRefresh = true;
            TRACE(string.Format("Opening file '{0}'", source));
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, source);

            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
            long currentPos = (long)Win32.SendMessage(curScintilla, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            long currentLine = (long)Win32.SendMessage(curScintilla, SciMsg.SCI_LINEFROMPOSITION, currentPos, 0);
            if ((line != 1) && (line - 1 != currentLine))
            {
                Win32.SendMessage(curScintilla, SciMsg.SCI_DOCUMENTEND, 0, 0);
                Win32.SendMessage(curScintilla, SciMsg.SCI_ENSUREVISIBLEENFORCEPOLICY, line - 1, 0);
                Win32.SendMessage(curScintilla, SciMsg.SCI_GOTOLINE, line - 1, 0);
            }
            Win32.SendMessage(curScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
            TRACE("-END-");
        }
        static void NavigateBackward()
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
        static void NavigateForward()
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

        internal static void ShowSettings()
        {
            TRACE("-START-");
            try
            {
                if (frmMain == null)
                {
                    ShowFrmMain();
                    Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
                }
                frmSettings frmSettings = new frmSettings();
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
        internal static void ShowOptions()
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
                frmOptions.cbxSaveRelativePaths.Checked = Settings.Configs.SaveRelativePaths;
                frmOptions.cbxHandleCtagsWarnings.Checked = Settings.Configs.HandleCtagsWarnings;
                frmOptions.cbxShowToolTips.Checked = Settings.Configs.ShowToolTips;
                frmOptions.cbxShowPlusMinusSource.Checked = Settings.Configs.ShowPlusMinusSource;
                frmOptions.cbxShowPlusMinusTags.Checked = Settings.Configs.ShowPlusMinusTags;
                frmOptions.cbxGoToDefByCtrlAndLmButton.Checked = Settings.Configs.GoToDefByCtrlAndLmButton;
                frmOptions.cbxShowGtdToolTips.Checked = Settings.Configs.ShowGtdToolTips;
                frmOptions.tbxMaxNumShownSymbols.Text = Settings.Configs.MaxNumShownSymbols;
                frmOptions.tbxFileSizeLimit.Text = Settings.Configs.FileSizeLimit;
                frmOptions.tbxFileSizeLimitInc.Text = Settings.Configs.FileSizeLimitInc;
                frmOptions.tbxSessionFileExt.Text = Settings.Configs.SessionFileExt;
                frmOptions.cbxCaseSensitiveExt.Checked = Settings.Configs.CaseSensitiveLanguageMaps;
                if (frmOptions.ShowDialog() == DialogResult.OK)
                {
                    Settings.Configs.BackColor = frmOptions.lblBackColor.BackColor.ToArgb();
                    Settings.Configs.ShowMenuAtBottom = frmOptions.cbxShowMenuAtBottom.Checked;
                    Settings.Configs.FlowingMenuButtons = frmOptions.cbxFlowingMenuButtons.Checked;
                    Settings.Configs.StartupShowMode = frmOptions.cbxStartupShowMode.Text;
                    Settings.Configs.StartupSessionMode = frmOptions.cbxStartupSessionMode.Text;
                    Settings.Configs.ShowInvalidSources = frmOptions.cbxShowInvalidSources.Checked;
                    Settings.Configs.SaveRelativePaths = frmOptions.cbxSaveRelativePaths.Checked;
                    Settings.Configs.HandleCtagsWarnings = frmOptions.cbxHandleCtagsWarnings.Checked;
                    Settings.Configs.ShowToolTips = frmOptions.cbxShowToolTips.Checked;
                    Settings.Configs.ShowPlusMinusSource = frmOptions.cbxShowPlusMinusSource.Checked;
                    Settings.Configs.ShowPlusMinusTags = frmOptions.cbxShowPlusMinusTags.Checked;
                    Settings.Configs.GoToDefByCtrlAndLmButton = frmOptions.cbxGoToDefByCtrlAndLmButton.Checked;
                    Settings.Configs.ShowGtdToolTips = frmOptions.cbxShowGtdToolTips.Checked;
                    
                    Settings.Configs.MaxNumShownSymbols = "";
                    string sLimit = frmOptions.tbxMaxNumShownSymbols.Text; long l;
                    if (long.TryParse(sLimit, out l))
                        Settings.Configs.MaxNumShownSymbols = sLimit;

                    Settings.Configs.FileSizeLimit = "";
                    sLimit = frmOptions.tbxFileSizeLimit.Text;
                    if ((sLimit.EndsWith("mb") || sLimit.EndsWith("kb"))
                        && long.TryParse(sLimit.Replace("mb", "").Replace("kb", ""), out l))
                        Settings.Configs.FileSizeLimit = sLimit;

                    Settings.Configs.FileSizeLimitInc = "";
                    sLimit = frmOptions.tbxFileSizeLimitInc.Text;
                    if ((sLimit.EndsWith("mb") || sLimit.EndsWith("kb"))
                       && long.TryParse(sLimit.Replace("mb", "").Replace("kb", ""), out l))
                        Settings.Configs.FileSizeLimitInc = sLimit;
                    
                    Settings.Configs.SessionFileExt = frmOptions.tbxSessionFileExt.Text.Trim();
                    if (Settings.Configs.SessionFileExt == "")
                        Settings.Configs.SessionFileExt = "c00k!e";
                    Settings.Configs.CaseSensitiveLanguageMaps = frmOptions.cbxCaseSensitiveExt.Checked;

                    if (Settings.Configs.GoToDefByCtrlAndLmButton)
                        Main.SubClassNpp();
                    else
                        Main.UnSubClassNpp();

                    if (frmMain != null)
                    {
                        frmMain.tvTags.BackColor = frmOptions.lblBackColor.BackColor;
                        frmMain.tvTags.ShowPlusMinus = frmOptions.cbxShowPlusMinusTags.Checked;
                        frmMain.tvTags.ShowRootLines = frmOptions.cbxShowPlusMinusSource.Checked;
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
        internal static void ShowHelp()
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
        const int NPPM_LAUNCHFINDINFILESDIRECTLY = 0x0400 + 1000 + 99;
        internal static void FindInSession()
        {
            TRACE("-START-");
            try
            {
                
                StringBuilder sbWord = new StringBuilder(4096);
                if (Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTWORD, 4096, sbWord) != IntPtr.Zero)
                {
                    if (frmMain == null)
                    {
                        ShowFrmMain();
                        Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmMain.Handle);
                    }

                    if (sbWord.Length > 0)
                    {
                        if (!SearchInSession(sbWord.ToString()))
                        {
                            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
                            System.Media.SystemSounds.Hand.Play();
                        }
                    }
                    else
                    {
                        Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
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
        internal static bool SearchInSession(string str)
        {
            TRACE("-START-");
            bool ret = false;
            List<string> lstFiles = frmMain.GetAllFileNames();

            TRACE(string.Format("Searching word '{0}' in {1} node(s)", str, lstFiles.Count));
            if (lstFiles.Count > 0)
            {
                using (ClikeStringArray cStrArray = new ClikeStringArray(lstFiles))
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (NppMsg)NPPM_LAUNCHFINDINFILESDIRECTLY, cStrArray.NativePointer, str);
                }
                ret = true;
            }

            TRACE("-END-");
            return ret;
        }
        #endregion
        #endregion

        #region " N++ subclassing "
        internal static IntPtr oldMainWndProc = IntPtr.Zero;
        internal static Win32.WindowProc newMainWndProc = new Win32.WindowProc(MainWndProc);
        internal static IntPtr oldSecondWndProc = IntPtr.Zero;
        internal static Win32.WindowProc newSecondWndProc = new Win32.WindowProc(SecondWndProc);
        internal static void SubClassNpp()
        {
            if (oldMainWndProc == IntPtr.Zero)
            {
                oldMainWndProc = Win32.SetWindowLongPtrW(PluginBase.nppData._scintillaMainHandle, Win32.GWL_WNDPROC, newMainWndProc);
            }
            if (oldSecondWndProc == IntPtr.Zero)
            {
                oldSecondWndProc = Win32.SetWindowLongPtrW(PluginBase.nppData._scintillaSecondHandle, Win32.GWL_WNDPROC, newSecondWndProc);
            }
        }
        internal static void UnSubClassNpp()
        {
            if (oldMainWndProc != IntPtr.Zero)
            {
                Win32.SetWindowLongPtrW(PluginBase.nppData._scintillaMainHandle, Win32.GWL_WNDPROC, oldMainWndProc);
                oldMainWndProc = IntPtr.Zero;
            }
            if (oldSecondWndProc != IntPtr.Zero)
            {
                Win32.SetWindowLongPtrW(PluginBase.nppData._scintillaSecondHandle, Win32.GWL_WNDPROC, oldSecondWndProc);
                oldSecondWndProc = IntPtr.Zero;
            }
        }
        internal static long MainWndProc(IntPtr hWnd, uint Msg, long wParam, long lParam)
        {
            return CommonWndProc(oldMainWndProc, hWnd, Msg, wParam, lParam);
        }
        internal static long SecondWndProc(IntPtr hWnd, uint Msg, long wParam, long lParam)
        {
            return CommonWndProc(oldSecondWndProc, hWnd, Msg, wParam, lParam);
        }
        internal static long CommonWndProc(IntPtr oldWndProc, IntPtr hWnd, long Msg, long wParam, long lParam)
        {
            switch (Msg)
            {
                case Win32.WM_LBUTTONUP:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        long res = Win32.CallWindowProcW(oldWndProc, hWnd, Msg, wParam, lParam);
                        GoToDefinition();
                        return res;
                    }
                    break;
                //case Win32.WM_KEYDOWN:
                //    break;
                default:
                    break;
            }
            return Win32.CallWindowProcW(oldWndProc, hWnd, Msg, wParam, lParam);
        }
        #endregion

        #region " Logging "
        internal static void ErrorOut(Exception ex)
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
        internal static void TRACE(string msg)
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