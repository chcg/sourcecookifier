using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SourceCookifier
{
    public class Settings
    {
        public static bool Initialized = false;

        public static bool HideAtStartup = false;

        public static string PluginSubFolderBin = "";
        public static string PluginSubFolderIcons = "";
        public static string PluginSubFolderSetup = "";
        public static string ConfigDir = "";
        public static string logFilePath = "";
        public static string configSettingsFilePath = "";
        public static string languagesSettingsFilePath = "";
        public static string languagesModelFilePath = "";

        public enum SessionMode { None, Cookie, Npp }
        public enum TreeViewMode { Flat, FlatGrouped, Grouped, ClassSingle, ClassSession }
        public class Config
        {
            // General
            public string StartupShowMode = "";
            public string MaxNumShownSymbols = "2048";
            public string FileSizeLimit = "2mb";
            public string FileSizeLimitInc = "5mb";
            public bool GoToDefByCtrlAndLmButton = true;
            public bool ShowGtdToolTips = true;
            // Toolbar
            public bool ShowMenuAtBottom = false;
            public bool FlowingMenuButtons = true;
            // CTags
            public bool HandleCtagsWarnings = false;
            public bool CaseSensitiveLanguageMaps = false;
            // Session
            public string StartupSessionMode = "";
            public string SessionFileExt = "c00k!e";
            public bool SaveRelativePaths = false;
            // Treeview
            public int BackColor = 0x13333337;
            public bool ShowInvalidSources = true;
            public bool ShowToolTips = true;
            public bool ShowPlusMinusSource = true;
            public bool ShowPlusMinusTags = true;
            // Toolbar button states
            public bool SearchInSession = false;
            public SessionMode SessionMode = SessionMode.None;
            public List<string> CookieHistoryList = new List<string>();
            public TreeViewMode ViewMode = TreeViewMode.FlatGrouped;
            public bool ShowIcons = true;
            public bool SortTags = false;
            public int FoldLevel = 2;
        }
        public static Config Configs;
        
        public static SerializableDictionary<string, Language> Languages;
        
        public static void LoadConfigs()
        {
            Main.TRACE("-START-");
            if (File.Exists(configSettingsFilePath))
                Configs = (Config)DeserializeObject(configSettingsFilePath, typeof(Config));
            else
                Configs = new Settings.Config();
            HideAtStartup = (Configs.StartupShowMode == "Hide");
            Main.TRACE(string.Format("SortTags={0},CaseSensitiveLanguageMaps={1},ShowIcons={2}," +
                "FlatView={3},ShowInvalidSources={4},SearchInSession={5},SessionMode={6}," +
                "FoldLevel={7},BackColor={8},ShowToolTips={9},ShowGtdToolTips={10},CookieHistoryList={11}",
                Configs.SortTags, Configs.CaseSensitiveLanguageMaps, Configs.ShowIcons,
                Configs.ViewMode, Configs.ShowInvalidSources, Configs.SearchInSession, Configs.SessionMode,
                Configs.FoldLevel, Configs.BackColor, Configs.ShowToolTips, Configs.ShowGtdToolTips,
                string.Concat(Configs.CookieHistoryList.ToArray())));
            Main.TRACE("-END-");
        }
        public static void LoadSettings(TreeView treeview)
        {
            Main.TRACE("-START-");
            
            // CTags paths
            CTagsExe.Init();
            
            // Use/restore language settings
            if (!File.Exists(languagesModelFilePath))
            {
                Main.TRACE("Language model file not found.. restoring");
                Languages = new SerializableDictionary<string, Language>();
                CTagsExe.GetLangMaps();
                CTagsExe.GetLangTagTypes();
                SerializeObject(Languages, languagesModelFilePath);
            }
            if (!File.Exists(languagesSettingsFilePath))
            {
                File.Copy(languagesModelFilePath, languagesSettingsFilePath);
                Main.TRACE("Language file not found.. copied language model file");
            }
            try
            {
                Languages = (SerializableDictionary<string, Language>)
                    DeserializeObject(languagesSettingsFilePath, typeof(SerializableDictionary<string, Language>));
            }
            catch (Exception ex)
            {
                Main.TRACE("'SourceCookifier.languages.xml' seems to be corrupted.. error while loading: " + ex.Message);
                if (MessageBox.Show(
                    "Your 'SourceCookifier.languages.xml' seems to be\n" +
                    "corrupted and thus can't be loaded. Do you want\n" +
                    "to restore it to the default settings again?",
                    "SourceCookifier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes)
                    {
                        File.Copy(languagesModelFilePath, languagesSettingsFilePath, true);
                        Main.TRACE("Language file overwritten with language model file.. loading it again");
                        Languages = (SerializableDictionary<string, Language>)
                            DeserializeObject(languagesSettingsFilePath, typeof(SerializableDictionary<string, Language>));
                    }
                else throw new Exception("Auto restore of language settings aborted by user.");
            }
            
            // Load icons
            IconList = new ImageList();
            IconList.Images.Add(new Bitmap(16, 16));
            IconIndexCache = new Dictionary<string, int>();

            IconList.Images.Add(Source.INCLUDES, Properties.Resources.includes);
            IconIndexCache[Source.INCLUDES] = IconList.Images.Count - 1;

            if (Configs.ShowIcons) LoadTagTypeIcons(treeview);
            
            Initialized = true;
            Main.TRACE("-END-");
        }
        public static void SaveSettings()
        {
            Main.TRACE("-START-");
            SerializeObject(Languages, languagesSettingsFilePath);
            SerializeObject(Configs, configSettingsFilePath);
            Main.TRACE("-END-");
        }

        public static bool ImportLanguages(ref SerializableDictionary<string, Language> existingLangs)
        {
            Main.TRACE("-START-");
            bool ret = false;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = "xml";
                ofd.Filter = "Languages file (*.xml)|*.xml";
                ofd.Title = "Import languages file";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SerializableDictionary<string, Language> importedLangs;
                        importedLangs = (SerializableDictionary<string, Language>)
                            DeserializeObject(ofd.FileName, typeof(SerializableDictionary<string, Language>));
                        foreach (string language in importedLangs.Keys)
                        {
                            if (existingLangs.ContainsKey(language))
                            {
                                DialogResult res = MessageBox.Show(string.Format("The language '{0}' is already configured.\n"
                                    +"Do you want to overwrite its configuration?", language),
                                    "SourceCookifier", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                                if (res == DialogResult.Cancel) break;
                                else if (res == DialogResult.No) continue;
                            }
                            existingLangs[language] = importedLangs[language];
                            ret = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        string err = string.Format("'{0}' seems to be corrupted.. error while loading: {1}",
                            Path.GetFileName(ofd.FileName), ex.Message);
                        Main.TRACE(err);
                        MessageBox.Show(err, "SourceCookifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            Main.TRACE("-END-: " + ret.ToString());
            return ret;
        }
        public static bool ExportLanguages(ref SerializableDictionary<string, Language> existingLangs, List<string> selectedLangs)
        {
            Main.TRACE("-START-");
            bool ret = false;
            SerializableDictionary<string, Language> exportLangs = new SerializableDictionary<string, Language>();
            foreach (string lang in selectedLangs)
            {
                exportLangs[lang] = existingLangs[lang];
            }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "xml";
                sfd.Filter = "Languages file (*.xml)|*.xml";
                sfd.Title = "Export languages file";
                sfd.FileName = string.Format("SourceCookifier.languages.export.{0}.xml", string.Join("-", selectedLangs.ToArray()));
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SerializeObject(exportLangs, sfd.FileName);
                    ret = true;
                }
            }
            Main.TRACE("-END-: " + ret.ToString());
            return ret;
        }

        public static List<int> lstLangSetDlgSelections = new List<int>();
        
        static void SerializeObject(object sourceObject, string targetPath)
        {
            Main.TRACE("-START-");
            if (sourceObject == null) return;
            XmlSerializer serializer = new XmlSerializer(sourceObject.GetType(), sourceObject.GetType().GetNestedTypes());
            TextWriter writer = new StreamWriter(targetPath);
            serializer.Serialize(writer, sourceObject);
            Main.TRACE(string.Format("Serialized {0}", targetPath));
            writer.Close();
            Main.TRACE("-END-");
        }
        static object DeserializeObject(string sourcePath, Type targetType)
        {
            Main.TRACE("-START-");
            object ret = null;
            XmlSerializer serializer = new XmlSerializer(targetType, targetType.GetNestedTypes());
            using (TextReader reader = new StreamReader(sourcePath))
            {
                XmlReader xr = new XmlTextReader(reader);
                ret = serializer.Deserialize(xr);
                Main.TRACE(string.Format("Deserialized {0}", sourcePath));
            }
            Main.TRACE("-END-");
            return ret;
        }
        
        public static string SessionFilePath = "";
        public static bool SessionChanged = false;
        public static void LoadSession(TreeView treeview, string file)
        {
            Main.TRACE("-START-");
            List<TreeNode> sourceNodes = null;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                sourceNodes = (List<TreeNode>)formatter.Deserialize(fs);
            }
            if ((sourceNodes == null) || (sourceNodes.Count == 0))
                throw new Exception("Session file invalid or empty!");
            Main.TRACE(string.Format("Deserialized {0} nodes from '{1}'", sourceNodes.Count, file));
            SessionFilePath = file;
            treeview.Nodes.Clear();
            Source.invisibleSourceNodes.Clear();
            Source.invisibleIncludeFileNodes.Clear();
            Uri uriBase = new Uri(SessionFilePath);
            foreach	(TreeNode sourceNode in sourceNodes)
            {
                if (sourceNode.Tag is Source)
                {
                    Source source = sourceNode.Tag as Source;
                    if (source != null)
                    {
                        if (!Path.IsPathRooted(source.PathUnquoted))
                        {
                            Uri uriFile = new Uri(source.PathUnquoted, UriKind.Relative);
                            string path = new Uri(uriBase, uriFile).LocalPath;
                            Main.TRACE(string.Format("Converted relative path '{0}' into absolute '{1}'",
                                source.PathUnquoted, path));
                            source.PathUnquoted = path;
                            source.PathQuoted = "\"" + path + "\"";
                            sourceNode.Name = path;
                            foreach (string tagType in source.TagTypes.Keys)
                                foreach (Tag tag in source.TagTypes[tagType].Tags)
                                    tag.SourceFile = path;
                        }
                        source.ImageListIndex = Settings.GetIconListIndex(source.PathUnquoted);
                        sourceNode.ToolTipText = source.PathUnquoted;
                    }
                }
                else
                {
                    foreach (IncludeFile incFile in sourceNode.Nodes)
                    {
                        incFile.QuickTags = incFile.Tag as List<QuickTag>;
                        if (!Path.IsPathRooted(incFile.SourceFile))
                        {
                            Uri uriFile = new Uri(incFile.SourceFile, UriKind.Relative);
                            string path = new Uri(uriBase, uriFile).LocalPath;
                            Main.TRACE(string.Format("Converted relative path '{0}' into absolute '{1}'",
                                incFile.SourceFile, path));
                            incFile.SourceFile = path;
                            incFile.Name = path;
                            incFile.ToolTipText = path;
                            foreach (QuickTag qtag in incFile.QuickTags)
                                qtag.SourceFile = path;
                        }
                        incFile.ImageIndex = incFile.SelectedImageIndex =
                            Settings.GetIconListIndex(incFile.ToolTipText);
                    }
                    sourceNode.ImageIndex = sourceNode.SelectedImageIndex = 1;
                    sourceNode.ToolTipText = sourceNode.Nodes.Count.ToString();
                }
                treeview.Nodes.Add(sourceNode);
            }
            SessionChanged = false;
            Main.TRACE("-END-");
        }
        public static void SaveSession(TreeView treeview)
        {
            Main.TRACE("-START-");
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "c00k!e";
                sfd.Filter = string.Format("Cookie session file (*.{0})|*.{1}",
                    Settings.Configs.SessionFileExt, Settings.Configs.SessionFileExt);
                sfd.Title = "Save SourceCookifier session file";
                if (SessionFilePath != "") sfd.FileName = SessionFilePath;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SessionFilePath = sfd.FileName;
                    List<TreeNode> sourceNodes = new List<TreeNode>();
                    foreach (TreeNode sourceNode in treeview.Nodes)
                    {
                        if (Configs.SaveRelativePaths)
                        {
                            Uri uriBase = new Uri(SessionFilePath);
                            if (sourceNode.Tag is Source)
                            {
                                Source source = sourceNode.Tag as Source;
                                Uri uriFile = new Uri(source.PathUnquoted);
                                string path = Uri.UnescapeDataString(
                                    uriBase.MakeRelativeUri(uriFile).ToString());
                                Main.TRACE(string.Format("Converted absolute path '{0}' into relative '{1}'",
                                    source.PathUnquoted, path));
                                source.PathUnquoted = path;
                                source.PathQuoted = "\"" + path + "\"";
                                sourceNode.Name = path;
                                if (string.IsNullOrEmpty(source.Error))
                                    sourceNode.ToolTipText = path;
                                foreach (string tagType in source.TagTypes.Keys)
                                    foreach (Tag tag in source.TagTypes[tagType].Tags)
                                        tag.SourceFile = path;
                            }
                            else
                            {
                                foreach (IncludeFile incFile in sourceNode.Nodes)
                                {
                                    Uri uriFile = new Uri(incFile.SourceFile);
                                    string path = Uri.UnescapeDataString(
                                        uriBase.MakeRelativeUri(uriFile).ToString());
                                    Main.TRACE(string.Format("Converted absolute path '{0}' into relative '{1}'",
                                        incFile.SourceFile, path));
                                    incFile.SourceFile = path;
                                    incFile.Name = path;
                                    incFile.ToolTipText = path;
                                    foreach (QuickTag qtag in incFile.QuickTags)
                                        qtag.SourceFile = path;
                            }
                            }
                        }
                        sourceNodes.Add(sourceNode);
                    }
                    using (FileStream fs = new FileStream(SessionFilePath, FileMode.Create))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, sourceNodes);
                    }
                    Main.TRACE(string.Format("Serialized {0} nodes to '{1}'", sourceNodes.Count, SessionFilePath));
                    if (Configs.SaveRelativePaths)
                    {
                        Uri uriBase = new Uri(SessionFilePath);
                        foreach (TreeNode sourceNode in treeview.Nodes)
                        {
                            if (sourceNode.Tag is Source)
                            {
                                Source source = sourceNode.Tag as Source;
                                Uri uriFile = new Uri(source.PathUnquoted, UriKind.Relative);
                                string path = new Uri(uriBase, uriFile).LocalPath;
                                Main.TRACE(string.Format("Converted relative path '{0}' into absolute '{1}'",
                                    source.PathUnquoted, path));
                                source.PathUnquoted = path;
                                source.PathQuoted = "\"" + path + "\"";
                                sourceNode.Name = path;
                                if (string.IsNullOrEmpty(source.Error))
                                    sourceNode.ToolTipText = path;
                                foreach (string tagType in source.TagTypes.Keys)
                                    foreach (Tag tag in source.TagTypes[tagType].Tags)
                                        tag.SourceFile = path;
                            }
                            else
                            {
                                foreach (IncludeFile incFile in sourceNode.Nodes)
                                {
                                    Uri uriFile = new Uri(incFile.SourceFile, UriKind.Relative);
                                    string path = new Uri(uriBase, uriFile).LocalPath;
                                    Main.TRACE(string.Format("Converted relative path '{0}' into absolute '{1}'",
                                        incFile.SourceFile, path));
                                    incFile.SourceFile = path;
                                    incFile.Name = path;
                                    incFile.ToolTipText = path;
                                    foreach (QuickTag qtag in incFile.QuickTags)
                                        qtag.SourceFile = path;
                                }
                            }
                        }
                    }
                    SessionChanged = false;
                    if (!Configs.CookieHistoryList.Contains(SessionFilePath))
                        Configs.CookieHistoryList.Insert(0, SessionFilePath);
                }
            }
            Main.TRACE("-END-");
        }

        public static ImageList IconList;
        public static bool TagTypeIconsLoaded = false;
        public static void LoadTagTypeIcons(TreeView treeview)
        {
            Main.TRACE("-START-");
            foreach (string language in Languages.Keys)
            {
                foreach (string tagtypename in Languages[language].TagTypes.Keys)
                {
                    string iconFilename = Languages[language].TagTypes[tagtypename].IconFilename;
                    if (!string.IsNullOrEmpty(iconFilename))
                    {
                        string iconFullPath = Path.Combine(PluginSubFolderIcons, iconFilename);
                        if (File.Exists(iconFullPath))
                        {
                            Main.TRACE(string.Format("Loading icon '{0}' for language '{1}'", iconFullPath, language));
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
            Main.TRACE("-END-");
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
            Main.TRACE(string.Format("{0}={1}", ext, IconIndexCache[ext]));
            return IconIndexCache[ext];
        }
    }
}
