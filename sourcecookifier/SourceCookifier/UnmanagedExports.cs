using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;
using NppPlugin.DllExport;

namespace SourceCookifier
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            Main.TRACE("-START-");
            PluginBase.nppData = notepadPlusData;
            Main.CommandMenuInit();
            Main.TRACE("-END-");
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            Main.TRACE("_funcItems.Items.Count = " + PluginBase._funcItems.Items.Count.ToString());
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        const int NPEM_SOURCECOOKIFIER_ADDTOSESSION_NORMAL = 0x0101;
        const int NPEM_SOURCECOOKIFIER_ADDTOSESSION_INCLUDES = 0x0102;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            Main.TRACE(string.Format("Message=0x{0:x8} wParam=0x{1:x8} lParam=0x{2:x8}", Message, wParam, lParam));
            if (Message == (uint)NppMsg.NPPM_MSGTOPLUGIN)
            {
                CommunicationInfo communicationInfo = (CommunicationInfo)Marshal.PtrToStructure(
                    (IntPtr)lParam, typeof(CommunicationInfo));
                string srcModuleName = Marshal.PtrToStringAuto(communicationInfo.srcModuleName);
                if ((communicationInfo.internalMsg == NPEM_SOURCECOOKIFIER_ADDTOSESSION_NORMAL) ||
                    (communicationInfo.internalMsg == NPEM_SOURCECOOKIFIER_ADDTOSESSION_INCLUDES))
                {
                    string path = Marshal.PtrToStringAuto(communicationInfo.info);
                    Main.TRACE(string.Format("Message=NPPM_MSGTOPLUGIN srcModuleName={0} internalMsg={1} path={2}",
                        srcModuleName, communicationInfo.internalMsg, path));
                    if ((Main.frmMain == null) || !Main.frmMain.Visible)
                        Main.ShowFrmMain();
                    if (!Main.frmMain.tsmiCookieSessionMode.Checked)
                    {
                        Main.frmMain.tsmiCookieSessionMode.Checked = true;
                        Settings.Configs.SessionMode = Settings.SessionMode.Cookie;
                        Main.frmMain.tsDdbtnSession.Image = Properties.Resources.session_mode_sc;
                        Main.frmMain.tsmiSingleFileMode.Checked = Main.frmMain.tsmiNppSessionMode.Checked = false;
                        Main.frmMain.tsmiSessionSave.Enabled = Main.frmMain.tsmiSessionClear.Enabled = true;
                        Main.frmMain.tvTags.Nodes.Clear();
                        Source.invisibleSourceNodes.Clear();
                        Source.invisibleIncludeFileNodes.Clear();
                    }
                    Main.frmMain.droppedWithModKey = (communicationInfo.internalMsg == NPEM_SOURCECOOKIFIER_ADDTOSESSION_INCLUDES);
                    Main.frmMain.DoTags(new string[] { path });
                    Main.frmMain.droppedWithModKey = false;
                }
            }
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Main.PluginName);
            return _ptrPluginName;
        }

        static bool sessionFileOpened = false;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            try
            {
                SCNotification nc = (SCNotification)Marshal.PtrToStructure(notifyCode, typeof(SCNotification));
                if (nc.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_SHUTDOWN");
                    Main.UnSubClassNpp();
                    Main.PluginCleanUp();
                    Marshal.FreeHGlobal(_ptrPluginName);
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_TBMODIFICATION");
                    PluginBase._funcItems.RefreshItems();
                    Main.SetToolBarIcon();
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_READY)
                {
                    if ((Settings.Configs.StartupShowMode == "Show") && ((Main.frmMain == null) || !Main.frmMain.Visible))
                        Main.ShowFrmMain();
                    Settings.HideAtStartup = false;
                    if (Settings.Configs.GoToDefByCtrlAndLmButton)
                        Main.SubClassNpp();
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_FILEBEFOREOPEN)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_FILEBEFOREOPEN");
                    StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
                    if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)nc.nmhdr.idFrom, sbPath) != -1)
                    {
                        string path = sbPath.ToString();
                        if (Path.GetExtension(path) == ("." + Settings.Configs.SessionFileExt))
                        {
                            sessionFileOpened = true;
                            if ((Main.frmMain == null) || !Main.frmMain.Visible)
                            {
                                if (Path.GetExtension(path) == ("." + Settings.Configs.SessionFileExt))
                                    Settings.HideAtStartup = false;
                                Main.ShowFrmMain();
                            }
                            Main.frmMain.DoTags(new string[] { path });
                            if (Settings.Configs.CookieHistoryList.Contains(path))
                                Settings.Configs.CookieHistoryList.Remove(path);
                            Settings.Configs.CookieHistoryList.Insert(0, path);
                        }
                    }
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_FILEOPENED)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_FILEOPENED");
                    if ((Main.frmMain != null) && Main.frmMain.Visible && (Settings.Configs.SessionMode == Settings.SessionMode.Npp))
                    {
                        StringBuilder path = new StringBuilder(Win32.MAX_PATH);
                        if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)nc.nmhdr.idFrom, path) != -1)
                        {
                            if (File.Exists(path.ToString()))
                            {
                                Main.frmMain.DoTags(new string[] { path.ToString() });
                            }
                        }
                    }
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_BUFFERACTIVATED)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_BUFFERACTIVATED");
                    if (sessionFileOpened)
                    {
                        sessionFileOpened = false;
                        Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_CLOSE);
                    }
                    else if (Main.skipTvRefresh)
                    {
                        Main.skipTvRefresh = false;
                    }
                    else if ((Main.frmMain != null) && Main.frmMain.Visible)
                    {
                        StringBuilder path = new StringBuilder(Win32.MAX_PATH);
                        if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)nc.nmhdr.idFrom, path) != -1)
                        {
                            string sPath = path.ToString();
                            if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                            {
                                Main.frmMain.tvTags.Nodes.Clear();
                                if (File.Exists(sPath))
                                {
                                    Main.frmMain.DoTags(new string[] { sPath });
                                }
                            }
                            else if (Settings.Configs.SessionMode == Settings.SessionMode.Npp)
                            {
                                // N++ sends a NPPN_FILEBEFORECLOSE when moving a file from one
                                // view to another, but it doesn't send a NPPN_FILEOPENED afterwards!
                                if (File.Exists(sPath))
                                {
                                    Source source = Main.frmMain.GetSourceByFilepath(sPath);
                                    if (source == null)
                                        Main.frmMain.DoTags(new string[] { sPath });
                                }
                            }
                            else if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                            {
                                if (File.Exists(sPath))
                                {
                                    IncludeFile incFile = Main.frmMain.GetIncludeFileByFilepath(sPath);
                                    if ((incFile != null) && (incFile.Parent != null))
                                    {
                                        Main.frmMain.tvTags.SelectedNode = incFile;
                                        incFile.EnsureVisible();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_FILESAVED)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_FILESAVED");
                    if ((Main.frmMain != null) && Main.frmMain.Visible)
                    {
                        StringBuilder path = new StringBuilder(Win32.MAX_PATH);
                        if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)nc.nmhdr.idFrom, path) != -1)
                        {
                            if (File.Exists(path.ToString()))
                            {
                                Main.frmMain.SavingDocNow = true;
                                Main.frmMain.DoTags(new string[] { path.ToString() });
                                Main.frmMain.SavingDocNow = false;
                            }
                            else if (Settings.Configs.SessionMode == Settings.SessionMode.None)
                            {
                                Main.frmMain.tvTags.Nodes.Clear();
                            }
                        }
                    }
                }
                else if (nc.nmhdr.code == (uint)NppMsg.NPPN_FILEBEFORECLOSE)
                {
                    Main.TRACE("notifyCode.nmhdr.code=NPPN_FILEBEFORECLOSE");
                    try
                    {
                        if ((Main.frmMain != null) && Main.frmMain.Visible && (Settings.Configs.SessionMode == Settings.SessionMode.Npp))
                        {
                            StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
                            if ((int)Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)nc.nmhdr.idFrom, sbPath) != -1)
                            {
                                string path = sbPath.ToString();
                                if (File.Exists(path))
                                {
                                    TreeNode sourceNode = Main.frmMain.GetSourceNodeByFilepath(path);
                                    if (sourceNode != null) sourceNode.Remove();
                                    if (Settings.Configs.ViewMode == Settings.TreeViewMode.ClassSession)
                                    {
                                        foreach (TreeNode node in Main.frmMain.tvTags.Nodes)
                                            Main.frmMain.RemoveGlobalClassViewTags(node, path);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Main.ErrorOut(ex); }
                }
                else if (nc.nmhdr.code == (uint)SciMsg.SCN_PAINTED)
                {
                    Main.TRACE("notifyCode.nmhdr.code=SCN_PAINTED");
                    if ((Main.frmMain != null) && Main.frmMain.Visible) Main.frmMain.SelectTagNodeByCurrentLine(false);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
    }
}
