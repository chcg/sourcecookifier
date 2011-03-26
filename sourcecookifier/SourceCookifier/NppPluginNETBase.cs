using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NppPluginNET
{
	public partial class PluginBase
	{
		#region " Fields "
		public string _pluginBaseName = "SourceCookifier";
		public string _pluginModuleName = "SourceCookifier.dll";
		public NppData nppData;
		public FuncItems _funcItems = new FuncItems();
		#endregion

		#region " Notepad++ callbacks "
		public void __setInfo(NppData notpadPlusData)
		{
			TRACE("-START-");
			nppData = notpadPlusData;
			CommandMenuInit();
			TRACE("-END-");
		}
		public IntPtr __getFuncsArray(ref int nbF)
		{
			TRACE("_funcItems.Items.Count = " + _funcItems.Items.Count.ToString());
			nbF = _funcItems.Items.Count;
			return _funcItems.NativePointer;
		}
		public uint __messageProc(uint Message, uint wParam, uint lParam)
		{
			TRACE(string.Format("Message=0x{0:x8} wParam=0x{1:x8} lParam=0x{2:x8}", Message, wParam, lParam));
			return 1;
		}
		bool sessionFileOpened = false;
		public void __beNotified(SCNotification notifyCode)
		{
			try
			{
				if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_SHUTDOWN");
					PluginCleanUp();
				}
				else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_TBMODIFICATION");
					_funcItems.RefreshItems();
				}
				else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_FILEBEFOREOPEN)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_FILEBEFOREOPEN");
					StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
					if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)notifyCode.nmhdr.idFrom, sbPath) != -1)
					{
						string path = sbPath.ToString();
						if (Path.GetExtension(path) == ".c00k!e")
						{
							sessionFileOpened = true;
							if (frmMain == null) ShowFrmMain();
							frmMain.DoTags(new string[] { path });
							if (Settings.Configs.CookieHistoryList.Contains(path))
								Settings.Configs.CookieHistoryList.Remove(path);
							Settings.Configs.CookieHistoryList.Insert(0, path);
						}
					}
				}
				else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_FILEOPENED)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_FILEOPENED");
					if ((frmMain != null) && frmMain.Visible && (Settings.Configs.SessionMode == Settings.SessionMode.Npp))
					{
						StringBuilder path = new StringBuilder(Win32.MAX_PATH);
						if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)notifyCode.nmhdr.idFrom, path) != -1)
						{
							if (File.Exists(path.ToString()))
							{
								frmMain.DoTags(new string[] { path.ToString() });
							}
						}
					}
				}
				else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_BUFFERACTIVATED)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_BUFFERACTIVATED");
					if (sessionFileOpened)
					{
						sessionFileOpened = false;
						Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_CLOSE);
					}
					else if (skipTvRefresh)
					{
						skipTvRefresh = false;
					}
					else if ((frmMain != null) && frmMain.Visible)
					{
						StringBuilder path = new StringBuilder(Win32.MAX_PATH);
						if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)notifyCode.nmhdr.idFrom, path) != -1)
						{
							if (Settings.Configs.SessionMode == Settings.SessionMode.None)
							{
								frmMain.tvTags.Nodes.Clear();
								if (File.Exists(path.ToString()))
								{
									frmMain.DoTags(new string[] { path.ToString() });
								}
							}
							else if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
							{
								IncludeFile incFile = frmMain.GetIncludeFileByFilepath(path.ToString());
								if ((incFile != null) && (incFile.Parent != null))
								{
									frmMain.tvTags.SelectedNode = incFile;
									incFile.EnsureVisible();
								}
							}
						}
					}
				}
				else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_FILESAVED)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_FILESAVED");
					if ((frmMain != null) && frmMain.Visible)
					{
						StringBuilder path = new StringBuilder(Win32.MAX_PATH);
						if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)notifyCode.nmhdr.idFrom, path) != -1)
						{
							if (File.Exists(path.ToString()))
							{
								frmMain.SavingDocNow = true;
								frmMain.DoTags(new string[] { path.ToString() });
								frmMain.SavingDocNow = false;
							}
							else if (Settings.Configs.SessionMode == Settings.SessionMode.None)
							{
								frmMain.tvTags.Nodes.Clear();
							}
						}
					}
				}
				else if(notifyCode.nmhdr.code == (uint)NppMsg.NPPN_FILEBEFORECLOSE)
				{
					TRACE("notifyCode.nmhdr.code=NPPN_FILEBEFORECLOSE");
					try
					{
						if ((frmMain != null) && frmMain.Visible && (Settings.Configs.SessionMode == Settings.SessionMode.Npp))
						{
							StringBuilder sbPath = new StringBuilder(Win32.MAX_PATH);
							if ((int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLPATHFROMBUFFERID, (int)notifyCode.nmhdr.idFrom, sbPath) != -1)
							{
								string path = sbPath.ToString();
								if (File.Exists(path))
								{
									TreeNode sourceNode = frmMain.GetSourceNodeByFilepath(path);
									if (sourceNode != null) sourceNode.Remove();
									if (Settings.Configs.ViewMode == Settings.TreeViewMode.ClassSession)
									{
										foreach (TreeNode node in frmMain.tvTags.Nodes)
											frmMain.RemoveGlobalClassViewTags(node, path);
									}
								}
							}
						}
					}
					catch (Exception ex) { ErrorOut(ex); }
				}
				else if (notifyCode.nmhdr.code == (uint)SciMsg.SCN_PAINTED)
				{
					TRACE("notifyCode.nmhdr.code=SCN_PAINTED");
					if ((frmMain != null) && frmMain.Visible) frmMain.SelectTagNodeByCurrentLine(false);
				}
			}
			catch (Exception ex) { ErrorOut(ex); }
		}
		#endregion

		#region " Helper "
		void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
		{
			SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
		}
		void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
		{
			SetCommand(index, commandName, functionPointer, shortcut, false);
		}
		void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
		{
			SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
		}
		void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
		{
			FuncItem funcItem = new FuncItem();
			funcItem._cmdID = index;
			funcItem._itemName = commandName;
			if (functionPointer != null)
			{
				funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
				TRACE(string.Format("index={0} commandName={1} functionPointer={2} checkOnInit={3}", index, commandName, functionPointer.Method, checkOnInit));
			}
			else
			{
				TRACE(string.Format("index={0} commandName={1} functionPointer=0 checkOnInit={2}", index, commandName, checkOnInit));
			}
			if (shortcut._key != 0)
				funcItem._pShKey = shortcut;
			funcItem._init2Check = checkOnInit;
			_funcItems.Add(funcItem);
		}

		public IntPtr GetCurrentScintilla()
		{
			int curScintilla;
			Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
			TRACE("curScintilla=" + curScintilla.ToString());
			return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
		}
		#endregion
	}
}
