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
using System.Runtime.Serialization;
#endregion

namespace NppPluginNET
{
	[Serializable]
	public class Source
	{
		public string Error = "";
		
		public string PathUnquoted = "";
		public string PathQuoted = "";
		public string Directory = "";
		public string FileName = "";
		public string Extension = "";
		public string Language = "";
		public Dictionary<string, TagType> TagTypes = new Dictionary<string, TagType>();
		
		public int ImageListIndex = 0;
		public static List<TreeNode> invisibleSourceNodes = new List<TreeNode>();
		
		public Source() { }
		public Source(string fullpath)
		{
			LoadSource(fullpath);
		}
		void LoadSource(string fullpath)
		{
			PluginBase.TRACE("-START-");
			Error = "";
			try
			{
				PathUnquoted = fullpath;
				PathQuoted = "\"" + fullpath + "\"";
				Directory = Path.GetDirectoryName(fullpath) + "\\";
				FileName = Path.GetFileName(fullpath);
			
				ImageListIndex = Settings.GetIconListIndex(PathUnquoted);
				
				if (!Settings.Initialized) throw new Exception("CTags not initialized!");

		        GetLanguage(fullpath, ref Language, ref Extension);

				ResetTagTypes();
				PluginBase.TRACE(string.Format("Going to tag: PathQuoted={0} Language={1} Extension={2}", PathQuoted, Language, Extension));
				CTagsExe.DoTags(PathQuoted, Language, Extension);
				PluginBase.TRACE(string.Format("Going to map..."));
				CTagsExe.MapTags(this);
				PluginBase.TRACE("Finished");
			}
			catch (Exception ex)
			{
				PluginBase.TRACE(string.Format("Error={0}", ex.Message));
				Error = ex.Message;
			}
			PluginBase.TRACE("-END-");
		}
		
		public static void GetLanguage(string fullpath, ref string language, ref string extension)
		{
			string filename = Path.GetFileName(fullpath);
			string[] toks = filename.Split('.');
			string extensionPattern = "";
			
			for (int i = 0; i < toks.Length; i++)
			{
				if (toks.Length == 1)
				{
					extensionPattern = toks[0] + ".";
					extension = "";
				}
				else
				{
					extensionPattern = "." + toks[i];
					for (int j = i + 1; j < toks.Length; j++)
						extensionPattern += "." + toks[j];
					extension = extensionPattern;
				}
				
				if (Settings.Configs.CaseSensitiveLanguageMaps)
				{
					foreach (string lang in Settings.Languages.Keys)
					{
						if (Settings.Languages[lang].Extensions.Contains(extensionPattern))
						{
							language = lang;
							return;
						}
					}
				}
				else
				{
					foreach (string lang in Settings.Languages.Keys)
					{
						foreach (string ext in Settings.Languages[lang].Extensions)
						{
							if (string.Compare(ext, extensionPattern, true) == 0)
							{
								language = lang;
								return;
							}
						}
					}
				}
			}
			
			throw new Exception("ERROR: There's no matching extension for this file in your language settings");
		}
		void ResetTagTypes()
		{
			foreach (string identifier in Settings.Languages[Language].TagTypes.Keys)
			{
				TagTypes[identifier] = new TagType();
			}
		}
        
		public void AddTag(Tag tag)
		{
			if (tag == null) return;
			if (string.IsNullOrEmpty(tag.Text)) return;
			if (string.IsNullOrEmpty(tag.SourceFile)) return;
			if (tag.Line == -1) return;
			if (string.IsNullOrEmpty(tag.Identifier)) return;
			TagTypes[tag.Identifier].Tags.Add(tag);
			PluginBase.TRACE("Finished");
		}
		
		public enum	RefreshType { None, Tags, TagTypes, Source }
		public void DrawTreeView(TreeView treeview, string filter, RefreshType refreshTags, bool expandSource)
		{
			PluginBase.TRACE("-START-");
			TreeNode sourceNode = null;
			bool newSourceNode = false;
			try
			{
				if (refreshTags == RefreshType.Source)
				{
					sourceNode = GetRootNode(treeview);
					
					LoadSource(PathUnquoted);
					
					if ((sourceNode != null) && (!string.IsNullOrEmpty(Error) && !Settings.Configs.ShowInvalidSources))
					{
						PluginBase.TRACE("Invalid.. removing source node");
						sourceNode.Remove();
						return;
					}
					else
					{
						PluginBase.TRACE("Clearing source node tags");
						sourceNode.Nodes.Clear();
					}
				}
	
				sourceNode = GetRootNode(treeview);
				newSourceNode = (sourceNode == null);
				if (newSourceNode)
				{
					PluginBase.TRACE(string.Format("Creating new source node for '{0}'", FileName));
					sourceNode = new TreeNode(FileName);
					sourceNode.ToolTipText = PathUnquoted;
				}
				else if (!string.IsNullOrEmpty(Language))
				{
					if (refreshTags == RefreshType.TagTypes)
					{
						Error = "";
						ImageListIndex = Settings.GetIconListIndex(PathUnquoted);
						TagTypes = new Dictionary<string, TagType>();
						sourceNode.Nodes.Clear();
						ResetTagTypes();
						CTagsExe.DoTags(PathQuoted, Language, Extension);
						CTagsExe.MapTags(this);
					}
					else if (refreshTags == RefreshType.Tags)
					{
						Error = "";
					    sourceNode.Nodes.Clear();
						ResetTagTypes();
						CTagsExe.DoTags(PathQuoted, Language, Extension);
						CTagsExe.MapTags(this);
					}
					else if (refreshTags == RefreshType.None)
					{
						sourceNode.Nodes.Clear();
					}
					PluginBase.TRACE(string.Format("Refreshing done ({0})", refreshTags));
				}
			}
			catch (Exception ex)
			{
				PluginBase.TRACE(ex.Message);
				Error = ex.Message;
			}
	
			int tagTypeCounter = -1;
			if (Error == "")
			{
				sourceNode.ToolTipText = PathUnquoted;
				sourceNode.ForeColor = treeview.ForeColor;
				
				tagTypeCounter = 0;
				foreach (string tagtypename in TagTypes.Keys)
				{
					PluginBase.TRACE(string.Format("Drawing lang={0} tagtype={1} tags.count={2}", Language, tagtypename, TagTypes[tagtypename].Tags.Count));
					if (TagTypes[tagtypename].Tags.Count == 0) continue;
					if (!Settings.Languages[Language].TagTypes[tagtypename].Show) continue;
					
					if (Settings.Configs.FlatView)
					{
						int imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
						Color tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
						int tagCounter = 0;
						foreach (Tag tag in TagTypes[tagtypename].Tags)
						{
							if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								TreeNode tagNode = sourceNode.Nodes[sourceNode.Nodes.Add(tag)];
								tagNode.ImageIndex = tagNode.SelectedImageIndex = imageListIndex;
								tagNode.ForeColor = tagTypeColor;
								tagCounter++;
							}
						}
						if (tagCounter > 0) tagTypeCounter++;
					}
					else
					{
						TreeNode typeNode = new TreeNode(Settings.Languages[Language].TagTypes[tagtypename].Description);
						int imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
						Color tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
						int tagCounter = 0;
						foreach (Tag tag in TagTypes[tagtypename].Tags)
						{
							if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								TreeNode tagNode = typeNode.Nodes[typeNode.Nodes.Add(tag)];
								tagNode.ImageIndex = tagNode.SelectedImageIndex = imageListIndex;
								tagNode.ForeColor = tagTypeColor;
								tagCounter++;
							}
						}
						if (tagCounter > 0)
						{
							typeNode.ToolTipText = tagCounter.ToString();
							typeNode.Tag = TagTypes[tagtypename];
							typeNode.ImageIndex = typeNode.SelectedImageIndex = imageListIndex;
							typeNode.ForeColor = tagTypeColor;
							typeNode.Name = tagtypename; // KEY!
							if (!string.IsNullOrEmpty(filter)) typeNode.Expand();
							sourceNode.Nodes.Add(typeNode);
							tagTypeCounter++;
						}
					}
				}
			}
			else
			{
				sourceNode.ToolTipText = Error;
				sourceNode.ForeColor = Color.Red;
				if (!string.IsNullOrEmpty(filter))
				{
					invisibleSourceNodes.Add(sourceNode);
				}
			}
			
			sourceNode.Tag = this;
			sourceNode.ImageIndex = sourceNode.SelectedImageIndex = ImageListIndex;
			sourceNode.Name = PathUnquoted;
			
			if ((tagTypeCounter == 0) && !string.IsNullOrEmpty(filter))
			{
				PluginBase.TRACE("tagTypeCounter=0.. hiding it");
				invisibleSourceNodes.Add(sourceNode);
			}
			else
			{
				if (newSourceNode) treeview.Nodes.Add(sourceNode);
				if (expandSource) sourceNode.Expand();
				PluginBase.TRACE(string.Format("Finished drawing.. newSourceNode={0} expandSource={1}", newSourceNode, expandSource));
			}
			
			if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie) &&
			    (refreshTags != RefreshType.None))
				Settings.SessionChanged = true;
			PluginBase.TRACE("-END-");
		}
		
		TreeNode GetRootNode(TreeView treeview)
		{
			TreeNode retNode = null;
			foreach (TreeNode sourceNode in treeview.Nodes)
			{
				Source source = sourceNode.Tag as Source;
				if (source != null)
				{
					if (source.PathUnquoted == PathUnquoted)
					{
						retNode = sourceNode;
						break;
					}
				}
			}
			return retNode;
		}
		
		public const string INCLUDES = "***INCLUDES***";
		public static List<IncludeFile> invisibleIncludeFileNodes = new List<IncludeFile>();
		public static Font fontIncludeItalic = null;
		public static TreeNode GetIncludesRootNode(TreeView treeview)
		{
			if (fontIncludeItalic == null)
				fontIncludeItalic = new Font(treeview.Font.FontFamily, treeview.Font.Size, FontStyle.Italic);
			if (!treeview.Nodes.ContainsKey(INCLUDES))
			{
				TreeNode includesNode = null;
				includesNode = treeview.Nodes.Add(INCLUDES, INCLUDES);
				includesNode.ImageIndex = includesNode.SelectedImageIndex = 1;
				includesNode.NodeFont = fontIncludeItalic;
				includesNode.ForeColor = Color.OliveDrab;
				return includesNode;
			}
			else return treeview.Nodes[INCLUDES];
		}
		public static void AddIncludeFile(TreeView treeview, string path)
		{
			TreeNode includesNode = GetIncludesRootNode(treeview);
			IncludeFile incFile = null;
			foreach (IncludeFile inc in includesNode.Nodes)
				if (inc.ToolTipText == path) { incFile = inc; break; }
			
			if (incFile == null)
			{
				incFile = new IncludeFile(path);
				if (!string.IsNullOrEmpty(incFile.Error) && !Settings.Configs.ShowInvalidSources)
				{
					if (includesNode.Nodes.Count == 0) includesNode.Remove();
				}
				else
				{
					incFile.NodeFont = fontIncludeItalic;
					includesNode.Nodes.Add(incFile);
					includesNode.ToolTipText = includesNode.Nodes.Count.ToString();
				}
			}
			else
			{
				incFile.RefreshTags();
			}
			Settings.SessionChanged = true;
		}
		public static void RemoveEmptyIncludesRootNode(TreeView treeview)
		{
			if (treeview.Nodes.ContainsKey(INCLUDES) && (treeview.Nodes[INCLUDES].Nodes.Count == 0))
				treeview.Nodes[INCLUDES].Remove();
			Settings.SessionChanged = true;
		}
	}
	
	public class Language
	{
		public Language() { }

		public bool BuildIn = true;
		public List<string> Extensions = new List<string>();
		
		public SerializableDictionary<string, TagType> TagTypes = new SerializableDictionary<string, TagType>();
	}
	
	[Serializable]
	public class TagType
	{
		public TagType() { }

		public string Description = "";
		public bool BuildIn = true;
		public List<string> RegexPatterns = new List<string>();
		
		[XmlIgnore]
		public List<Tag> Tags = new List<Tag>();

		public bool Show = true;
		public bool TrackCaret = false;
		public string IconFilename = "";
		public int ForeColor = 0;
		[XmlIgnore]
		public int ImageListIndex = 0;
	}
	
	[Serializable]
	public class Tag : TreeNode
	{
		public string SourceFile = "";
		public int Line = -1;
		public string Identifier = "";
		public string Scope = "";
		public string Signature = "";
		public string Access = "";
		
		public Tag(string line)
		{
			PluginBase.TRACE(string.Format("Trying to add:{0}", line));
			if (string.IsNullOrEmpty(line)) return;
			if (line.StartsWith("!")) return;
			if (line.StartsWith("ctags.exe: Warning:")) return;
		
			string[] fields = line.Split(CTagsExe.FieldSeparator); // 'Ø'... originally '\t';
			PluginBase.TRACE(string.Format("Split into {0} fields", fields.Length));
			string[] customFields = fields[0].Split(CTagsExe.FieldSeparatorExtended); // 'Ï'... user sees '|'
			PluginBase.TRACE(string.Format("Split into {0} custom fields", customFields.Length));
			base.Text = customFields[0];
			for (int i = 1; i < customFields.Length; i++)
				GetExtendedField(customFields[i]);
			
			base.Name = fields[2].Split(';')[0]; // KEY!
			SourceFile = fields[1];
			Line = int.Parse(base.Name);
			Identifier = fields[3];
			PluginBase.TRACE(string.Format("Text={0} Name={1} SourceFile={2} Line={3} Identifier={4}",
			    base.Text, base.Name, SourceFile, Line, Identifier));
			
			for (int i = 4; i < fields.Length; i++)
				GetExtendedField(fields[i]);

			base.ToolTipText = Access + Scope + Text + Signature;
			PluginBase.TRACE(string.Format("Added:{0}", base.ToolTipText));
		}
		void GetExtendedField(string field)
		{
			if (field.StartsWith("access:"))
				Access = field.Replace("access:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("signature:"))
				Signature = " " + field.Replace("signature:", "").Replace('\t', ' ');
			else
			{
				try { Scope = field.Split(':')[1].Replace('\t', ' ') + "."; }
				catch { Scope = "***Invalid extended field: \"" + field + "\" *** "; }
			}
			PluginBase.TRACE(string.Format("field={0} Access={1} Signature={2} Scope={3}", field, Access, Signature, Scope));
		}
		
		public Tag() { }
		protected Tag(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			SourceFile = info.GetString("SourceFile");
			Line = info.GetInt32("Line");
			Identifier = info.GetString("Identifier");
			Scope = info.GetString("Scope");
			Signature = info.GetString("Signature");
			base.ToolTipText = info.GetString("ToolTipText");
		}
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("SourceFile", SourceFile);
			info.AddValue("Line", Line);
			info.AddValue("Identifier", Identifier);
			info.AddValue("Scope", Scope);
			info.AddValue("Signature", Signature);
			info.AddValue("Access", Access);
			info.AddValue("ToolTipText", base.ToolTipText);
			base.Serialize(info, context);
		}
	}
	
	[Serializable]
	public class IncludeFile : TreeNode
	{
		public string Error = "";

		public string SourceFile = "";
		public List<QuickTag> QuickTags;
		public string Extension = "";
		public string Language = "";
		
		public IncludeFile(string path)
		{
			PluginBase.TRACE("-START-");
			try
			{
				SourceFile = path;
				Text = Path.GetFileName(path);
				ToolTipText = path;
				Name = path;
				ImageIndex = SelectedImageIndex = Settings.GetIconListIndex(path);
				Source.GetLanguage(path, ref Language, ref Extension);
				ForeColor = Color.OliveDrab;
				RefreshTags();
				PluginBase.TRACE(string.Format("Sourcefile={0} language={1}", path, Language));
			}
			catch (Exception ex)
			{
				PluginBase.TRACE(string.Format("ERROR={0}", ex.Message));
				Error = ex.Message;
				ToolTipText = Error;
				ForeColor = Color.Red;
			}
			PluginBase.TRACE("-END-");
		}
		
		public void RefreshTags()
		{
			PluginBase.TRACE("-START-");
			if (string.IsNullOrEmpty(Error))
			{
				QuickTags = new List<QuickTag>();
				
				string pathQuoted = "\"" + SourceFile + "\"";
				
				if (!Settings.Initialized) throw new Exception("CTags not initialized!");
				CTagsExe.DoTags(pathQuoted, Language, Extension);
				CTagsExe.MapQuickTags(this, Language);
				
				Tag = QuickTags;
				Settings.SessionChanged = true;
			}
			PluginBase.TRACE("-END-");
		}
		
		public IncludeFile() { }
		protected IncludeFile(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Error = info.GetString("Error");
			SourceFile = info.GetString("SourceFile");
			ToolTipText = info.GetString("ToolTipText");
			Extension = info.GetString("Extension");
			Language = info.GetString("Language");
		}
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Error", Error);
			info.AddValue("SourceFile", SourceFile);
			info.AddValue("ToolTipText", ToolTipText);
			info.AddValue("Extension", Extension);
			info.AddValue("Language", Language);
			base.Serialize(info, context);
		}
	}
	
	[Serializable]
	public class QuickTag
	{
		public string Language = "";
		public string SourceFile = "";
		public string Text = "";
		public int Line = -1;
		public string Identifier = "";
		public string Scope = "";
		public string Signature = "";
		public string Access = "";
		public string FullText = "";

		public QuickTag(string line, string language)
		{
			PluginBase.TRACE(string.Format("Trying to add:{0}", line));
			if (string.IsNullOrEmpty(line) || string.IsNullOrEmpty(language)) return;
			if (line.StartsWith("!")) return;
			if (line.StartsWith("ctags.exe: Warning:")) return;
		
			Language = language;
			string[] fields = line.Split(CTagsExe.FieldSeparator); // 'Ø'... originally '\t';
			PluginBase.TRACE(string.Format("Split into {0} fields", fields.Length));
			string[] customFields = fields[0].Split(CTagsExe.FieldSeparatorExtended); // 'Ï'... user sees '|'
			PluginBase.TRACE(string.Format("Split into {0} custom fields", customFields.Length));
			SourceFile = fields[1];
			Text = customFields[0];
			for (int i = 1; i < customFields.Length; i++)
				GetExtendedField(customFields[i]);
			Line = int.Parse(fields[2].Split(';')[0]);
			Identifier = fields[3];
			PluginBase.TRACE(string.Format("Text={0} SourceFile={2} Line={3} Identifier={4}",
			    Text, SourceFile, Line, Identifier));
			for (int i = 4; i < fields.Length; i++)
				GetExtendedField(fields[i]);
			FullText = Access + Scope + Text + Signature;
			PluginBase.TRACE(string.Format("Added:{0}", FullText));
		}
		void GetExtendedField(string field)
		{
			if (field.StartsWith("access:"))
				Access = field.Replace("access:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("signature:"))
				Signature = " " + field.Replace("signature:", "").Replace('\t', ' ');
			else
			{
				try { Scope = field.Split(':')[1].Replace('\t', ' ') + "."; }
				catch { Scope = "***Invalid extended field: \"" + field + "\" *** "; }
			}
			PluginBase.TRACE(string.Format("field={0} Access={1} Signature={2} Scope={3}", field, Access, Signature, Scope));
		}
	}
	
	public class TagNodeSorter : System.Collections.IComparer
	{
	    public int Compare(object obj1, object obj2)
	    {
	    	try
	    	{
		    	if ((obj1 is Tag) && (obj2 is Tag))
		    	{
			        Tag tag1 = obj1 as Tag;
			    	Tag tag2 = obj2 as Tag;
		
			    	if ((tag1 != null) && (tag2 != null))
			    	{
			    	    if (!Settings.Configs.FlatView || !Settings.Configs.KeepTypesGrouped || (tag1.Identifier == tag2.Identifier))
			    	    {
			    	        if (Settings.Configs.SortTags)
			    	        {
			    	            return string.Compare(tag1.Text, tag2.Text);
			    	        }
			    	        else
			    	        {
			    	            if (tag1.Line > tag2.Line) return 1;
			    	            else if (tag1.Line < tag2.Line) return -1;
			    	        }
			    	    }
			    	    else
			    	    {
			    	        string language = (tag1.Parent.Tag as Source).Language;
			    	        int index1 = 0, index2 = 0;
			    	        foreach (string identifier in Settings.Languages[language].TagTypes.Keys)
			    	            if (identifier == tag1.Identifier) break; else index1++;
			    	        foreach (string identifier in Settings.Languages[language].TagTypes.Keys)
			    	            if (identifier == tag2.Identifier) break; else index2++;
			    	        if (index1 == index2) return 0;
			    	        return (index1 > index2) ? 1 : -1;
			    	    }
			    	}
		    	}
		    	else if ((obj1 is IncludeFile) && (obj2 is IncludeFile))
		    	{
		    		IncludeFile inc1 = obj1 as IncludeFile;
		    		IncludeFile inc2 = obj2 as IncludeFile;
		    		
		    		return string.Compare(inc1.Text, inc2.Text);
		    	}
		    	else
		    	{
		    		TreeNode node1 = obj1 as TreeNode;
		    		TreeNode node2 = obj2 as TreeNode;
		    		
		    		if ((node1.Tag is Source) && (node2.Tag is Source))
		    		{
		    			return string.Compare(node1.Text, node2.Text);
		    		}
		    		else if ((node1.Tag is TagType) && (node1.Tag is TagType))
		    		{
		    			string language = (node1.Parent.Tag as Source).Language;
		    			TagType tagType1 = node1.Tag as TagType;
		    			TagType tagType2 = node2.Tag as TagType;
		    			int index1 = 0, index2 = 0;
		    			foreach (string identifier in Settings.Languages[language].TagTypes.Keys)
		    				if (identifier == node1.Name) break; else index1++;
		    			foreach (string identifier in Settings.Languages[language].TagTypes.Keys)
		    				if (identifier == node2.Name) break; else index2++;
		    			if (index1 == index2) return 0;
		    			return (index1 > index2) ? 1 : -1;
		    		}
		    		else if (node1.Name == Source.INCLUDES) return 1;
		    		else if (node2.Name == Source.INCLUDES) return -1;
	    		}
	    	}
	    	catch { }

			return 0;
	    }
	}
}
