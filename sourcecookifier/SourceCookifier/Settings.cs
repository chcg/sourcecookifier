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
using System.Runtime.Serialization.Formatters.Binary;
#endregion

namespace NppPluginNET
{
	public class Settings
	{
		public static bool Initialized = false;
		
		public static string ApplicationDir = "";
		public static string ConfigDir = "";
		public static string logFilePath = "";
		public static string configSettingsFilePath = "";
		public static string languagesSettingsFilePath = "";
		public static string languagesModelFilePath = "";
		public static string iconFolder = "";

		public enum SessionMode { None, Cookie, Npp }
		public enum TreeViewMode { Flat, FlatGrouped, Grouped, ClassSingle, ClassSession }
		public class Config
		{
			public bool SortTags = false;
			public bool CaseSensitiveLanguageMaps = false;
			public bool ShowIcons = true;
			public TreeViewMode ViewMode = TreeViewMode.FlatGrouped;
			public bool ShowInvalidSources = true;
			public bool SearchInSession = false;
			public string FileSizeLimit = "";
			public bool ShowMenuAtBottom = false;
			public bool FlowingMenuButtons = true;
			
			public string StartupShowMode = "";
			public SessionMode SessionMode = SessionMode.None;
			public string StartupSessionMode = "";
			public int FoldLevel = 2;
			
			public int BackColor = 0x13333337;
			public bool ShowToolTips = true;
			public bool ShowGtdToolTips = true;
			
			public List<string> CookieHistoryList = new List<string>();
		}
		public static Config Configs;
		
		public static SerializableDictionary<string, Language> Languages;
		
		public static void LoadConfigs()
		{
			PluginBase.TRACE("-START-");
			ApplicationDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
			if (File.Exists(configSettingsFilePath))
				Configs = (Config)DeserializeObject(configSettingsFilePath, typeof(Config));
			else
				Configs = new Settings.Config();
			PluginBase.TRACE(string.Format("SortTags={0},CaseSensitiveLanguageMaps={1},ShowIcons={2}," +
				"FlatView={3},ShowInvalidSources={4},SearchInSession={5},SessionMode={6}," +
				"FoldLevel={7},BackColor={8},ShowToolTips={9},ShowGtdToolTips={10},CookieHistoryList={11}",
				Configs.SortTags, Configs.CaseSensitiveLanguageMaps, Configs.ShowIcons,
				Configs.ViewMode, Configs.ShowInvalidSources, Configs.SearchInSession, Configs.SessionMode,
				Configs.FoldLevel, Configs.BackColor, Configs.ShowToolTips, Configs.ShowGtdToolTips,
				string.Concat(Configs.CookieHistoryList.ToArray())));
			PluginBase.TRACE("-END-");
		}
		public static void LoadSettings(TreeView treeview)
		{
			PluginBase.TRACE("-START-");
			iconFolder = ApplicationDir + "icons\\";
			
			// CTags paths
			CTagsExe.Init();
			
			// Use/restore language settings
			if (!File.Exists(languagesModelFilePath))
			{
				PluginBase.TRACE("Language model file not found.. restoring");
				Languages = new SerializableDictionary<string, Language>();
				CTagsExe.GetLangMaps();
				CTagsExe.GetLangTagTypes();
				SerializeObject(Languages, languagesModelFilePath);
			}
			if (!File.Exists(languagesSettingsFilePath))
			{
				File.Copy(languagesModelFilePath, languagesSettingsFilePath);
				PluginBase.TRACE("Language file not found.. copied language model file");
			}
			Languages = (SerializableDictionary<string, Language>)
				DeserializeObject(languagesSettingsFilePath, typeof(SerializableDictionary<string, Language>));
			
			// Load icons
			IconList = new ImageList();
			IconList.Images.Add(new Bitmap(16, 16));
			IconIndexCache = new Dictionary<string, int>();

			IconList.Images.Add(Source.INCLUDES, Properties.Resources.includes);
			IconIndexCache[Source.INCLUDES] = IconList.Images.Count - 1;

			if (Configs.ShowIcons) LoadTagTypeIcons(treeview);
			
			Initialized = true;
			PluginBase.TRACE("-END-");
		}
		public static void SaveSettings()
		{
			PluginBase.TRACE("-START-");
			SerializeObject(Languages, languagesSettingsFilePath);
			SerializeObject(Configs, configSettingsFilePath);
			PluginBase.TRACE("-END-");
		}
		
		public static List<int> lstLangSetDlgSelections = new List<int>();
		
		static void SerializeObject(object sourceObject, string targetPath)
		{
			PluginBase.TRACE("-START-");
			if (sourceObject == null) return;
			XmlSerializer serializer = new XmlSerializer(sourceObject.GetType());
			TextWriter writer = new StreamWriter(targetPath);
			serializer.Serialize(writer, sourceObject);
			PluginBase.TRACE(string.Format("Serialized {0}", targetPath));
			writer.Close();
			PluginBase.TRACE("-END-");
		}
		static object DeserializeObject(string sourcePath, Type targetType)
		{
			PluginBase.TRACE("-START-");
			object ret = null;
			XmlSerializer serializer = new XmlSerializer(targetType);
			using (TextReader reader = new StreamReader(sourcePath))
			{
				XmlReader xr = new XmlTextReader(reader);
				ret = serializer.Deserialize(xr);
				PluginBase.TRACE(string.Format("Deserialized {0}", sourcePath));
			}
			PluginBase.TRACE("-END-");
			return ret;
		}
		
		public static string SessionFilePath = "";
		public static bool SessionChanged = false;
		public static void LoadSession(TreeView treeview, string file)
		{
			PluginBase.TRACE("-START-");
			List<TreeNode> sourceNodes;
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				sourceNodes = (List<TreeNode>)formatter.Deserialize(fs);
			}
			if ((sourceNodes == null) && (sourceNodes.Count == 0))
				throw new Exception("Session file invalid or empty!");
			PluginBase.TRACE(string.Format("Deserialized {0} nodes from '{1}'", sourceNodes.Count, file));
			treeview.Nodes.Clear();
			Source.invisibleSourceNodes.Clear();
			Source.invisibleIncludeFileNodes.Clear();
			foreach	(TreeNode sourceNode in sourceNodes)
			{
				if (sourceNode.Tag is Source)
				{
					Source source = sourceNode.Tag as Source;
					if (source != null)
					{
						source.ImageListIndex = Settings.GetIconListIndex(source.PathUnquoted);
						sourceNode.ToolTipText = source.PathUnquoted;
					}
				}
				else
				{
					foreach (IncludeFile incFile in sourceNode.Nodes)
					{
						incFile.ImageIndex = incFile.SelectedImageIndex =
							Settings.GetIconListIndex(incFile.ToolTipText);
						incFile.QuickTags = incFile.Tag as List<QuickTag>;
					}
					sourceNode.ImageIndex = sourceNode.SelectedImageIndex = 1;
					sourceNode.ToolTipText = sourceNode.Nodes.Count.ToString();
				}
				treeview.Nodes.Add(sourceNode);
			}
			SessionFilePath = file;
			SessionChanged = false;
			PluginBase.TRACE("-END-");
		}
		public static void SaveSession(TreeView treeview)
		{
			PluginBase.TRACE("-START-");
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.DefaultExt = "c00k!e";
				sfd.Filter = "Cookie session file (*.c00k!e)|*.c00k!e";
				sfd.Title = "Save SourceCookifier session file";
				if (SessionFilePath != "") sfd.FileName = SessionFilePath;
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					List<TreeNode> sourceNodes = new List<TreeNode>();
					foreach	(TreeNode sourceNode in treeview.Nodes) sourceNodes.Add(sourceNode);
					using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
					{
						BinaryFormatter formatter = new BinaryFormatter();
						formatter.Serialize(fs, sourceNodes);
					}
					PluginBase.TRACE(string.Format("Serialized {0} nodes to '{1}'", sourceNodes.Count, sfd.FileName));
					SessionFilePath = sfd.FileName;
					SessionChanged = false;
					if (!Configs.CookieHistoryList.Contains(sfd.FileName))
						Configs.CookieHistoryList.Insert(0, sfd.FileName);
				}
			}
			PluginBase.TRACE("-END-");
		}

		public static ImageList IconList;
		public static bool TagTypeIconsLoaded = false;
		public static void LoadTagTypeIcons(TreeView treeview)
		{
			PluginBase.TRACE("-START-");
			foreach (string language in Languages.Keys)
			{
				foreach (string tagtypename in Languages[language].TagTypes.Keys)
				{
					string iconFilename = Languages[language].TagTypes[tagtypename].IconFilename;
					if (!string.IsNullOrEmpty(iconFilename))
					{
						string iconFullPath = iconFolder + iconFilename;
						if (File.Exists(iconFullPath))
						{
							PluginBase.TRACE(string.Format("Loading icon '{0}' for language '{1}'", iconFullPath, language));
							int index = 0;
							if (IconList.Images.ContainsKey(iconFilename))
							{
								index = IconList.Images.IndexOfKey(iconFilename);
							}
							else
							{
								IconList.Images.Add(iconFilename, new Bitmap(iconFullPath));
								index = IconList.Images.Count - 1;
							}
							Languages[language].TagTypes[tagtypename].ImageListIndex = index;
						}
					}
				}
			}
			treeview.ImageList = IconList;
			TagTypeIconsLoaded = true;
			PluginBase.TRACE("-END-");
		}
		
		static Dictionary<string, int> IconIndexCache;
		public static int GetIconListIndex(string filepath)
		{
			string ext = Path.GetExtension(filepath);
			if (!IconIndexCache.ContainsKey(ext))
			{
				IconList.Images.Add(SHGetIcon.GetBmp(filepath, true), Color.Black);
				IconIndexCache[ext] = IconList.Images.Count - 1;
			}
			PluginBase.TRACE(string.Format("{0}={1}", ext, IconIndexCache[ext]));
			return IconIndexCache[ext];
		}
	}
}
