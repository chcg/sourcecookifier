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
		public string ScopeOperator = "";
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

				GetLanguage(fullpath, ref Language, ref Extension, ref ScopeOperator);

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
		
		public static void GetLanguage(string fullpath, ref string language, ref string extension, ref string scopeOperator)
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
							scopeOperator = Settings.Languages[lang].ScopeOperator;
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
								scopeOperator = Settings.Languages[lang].ScopeOperator;
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
		public bool DrawTreeView(TreeView treeview, string filter, RefreshType refreshTags, bool expandSource)
		{
			PluginBase.TRACE("-START-");
			bool retShown = false;
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
						return retShown;
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
	
			int tagTypeCounter = 0;
			if (Error == "")
			{
				sourceNode.ToolTipText = PathUnquoted;
				sourceNode.ForeColor = treeview.ForeColor;
				
				foreach (string tagtypename in TagTypes.Keys)
				{
					PluginBase.TRACE(string.Format("Drawing lang={0} tagtype={1} tags.count={2}", Language, tagtypename, TagTypes[tagtypename].Tags.Count));
					if (TagTypes[tagtypename].Tags.Count == 0) continue;
					if (!Settings.Languages[Language].TagTypes[tagtypename].Show) continue;
					
					int imageListIndex;
					Color tagTypeColor;
					int tagCounter;
					switch (Settings.Configs.ViewMode)
					{
						case Settings.TreeViewMode.Flat:
						case Settings.TreeViewMode.FlatGrouped:
							imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
							tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
							tagCounter = 0;
							foreach (Tag tag in TagTypes[tagtypename].Tags)
							{
								if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									TreeNode tagNode = sourceNode.Nodes[sourceNode.Nodes.Add(tag)];
									tagNode.Nodes.Clear();
									tagNode.ImageIndex = tagNode.SelectedImageIndex = imageListIndex;
									tagNode.ForeColor = tagTypeColor;
									tagCounter++;
								}
							}
							if (tagCounter > 0) tagTypeCounter++;
							break;
						case Settings.TreeViewMode.Grouped:
							TreeNode typeNode = new TreeNode(Settings.Languages[Language].TagTypes[tagtypename].Description);
							imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
							tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
							tagCounter = 0;
							foreach (Tag tag in TagTypes[tagtypename].Tags)
							{
								if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									TreeNode tagNode = typeNode.Nodes[typeNode.Nodes.Add(tag)];
									tagNode.Nodes.Clear();
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
							break;
						case Settings.TreeViewMode.ClassSingle:
							imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
							tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
							tagCounter = 0;
							foreach (Tag tag in TagTypes[tagtypename].Tags)
							{
								if (tag.Parent != null) tag.Parent.Nodes.Remove(tag);
								if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									TreeNode curNode = sourceNode;
									if (!string.IsNullOrEmpty(tag.Scope))
									{
										int iFoundNamespace = 0;
										string[] scopeToks = tag.Scope.Split(
											new string[] { tag.ScopeOperator }, StringSplitOptions.RemoveEmptyEntries);
										for (int i = scopeToks.Length; i > 0 ; i--)
										{
											string scopeToFind = string.Join(tag.ScopeOperator, scopeToks, 0, i);
											foreach	(TreeNode n in curNode.Nodes)
											{
												string t = (n is Tag) ? (n as Tag).TagName : n.Text;
												if (t == scopeToFind)
												{
													curNode = n;
													iFoundNamespace = i;
													break;
												}
											}
											if (iFoundNamespace != 0) break;
										}
										for (int i = iFoundNamespace; i < scopeToks.Length ; i++)
										{
											string scopeToFind = string.Join(tag.ScopeOperator, scopeToks, i, 1);
											foreach	(TreeNode n in curNode.Nodes)
											{
												string t = (n is Tag) ? (n as Tag).TagName : n.Text;
												if (t == scopeToFind)
												{
													curNode = n;
													break;
												}
											}
										}
									}
									TreeNode tagNode = curNode.Nodes[curNode.Nodes.Add(tag)];
									tagNode.ImageIndex = tagNode.SelectedImageIndex = imageListIndex;
									tagNode.ForeColor = tagTypeColor;
									tagCounter++;
								}
							}
							if (tagCounter > 0) tagTypeCounter++;
							break;
						case Settings.TreeViewMode.ClassSession:
							imageListIndex = Settings.Languages[Language].TagTypes[tagtypename].ImageListIndex;
							tagTypeColor = Color.FromArgb(Settings.Languages[Language].TagTypes[tagtypename].ForeColor);
							tagCounter = 0;
							foreach (Tag tag in TagTypes[tagtypename].Tags)
							{
								if (tag.Parent != null) tag.Parent.Nodes.Remove(tag);
								if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									TreeNode curNode = null;
									if (!string.IsNullOrEmpty(tag.Scope))
									{
										string[] scopeToks = tag.Scope.Split(
											new string[] { tag.ScopeOperator }, StringSplitOptions.RemoveEmptyEntries);
										foreach	(string scopeTok in scopeToks)
										{
											bool _found = false;
											TreeNodeCollection coll = null;
											if (curNode == null)
												coll = treeview.Nodes;
											else
												coll = curNode.Nodes;
											foreach	(TreeNode n in coll)
											{
												string t = (n is Tag) ? (n as Tag).TagName : n.Text;
												if (t == scopeTok)
												{
													curNode = n;
													_found = true;
													break;
												}
											}
											if (!_found)
											{
												if (curNode == null)
													curNode = treeview.Nodes.Add(scopeTok);
												else
													curNode = curNode.Nodes.Add(scopeTok);
												curNode.Name = Tag.PSEUDO_TAG;
                                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                                dict["language"] = Language;
                                                dict["tagtypename"] = tagtypename;
                                                dict["line"] = tag.Line;
												curNode.Tag = dict;
												curNode.ForeColor = Color.FromArgb(
													treeview.ForeColor.A,
													(treeview.BackColor.R + treeview.ForeColor.R) / 2,
													(treeview.BackColor.G + treeview.ForeColor.G) / 2,
													(treeview.BackColor.B + treeview.ForeColor.B) / 2);
											}
										}
									}
									TreeNode tagNode = null;
									if (curNode == null)
									{
										foreach	(TreeNode n in treeview.Nodes)
											if (n.Text == tag.Text)
											{
												tagNode = n;
												break;
											}
										if (tagNode == null)
											tagNode = treeview.Nodes[treeview.Nodes.Add(tag)];
									}
									else
									{
										tagNode = curNode.Nodes[curNode.Nodes.Add(tag)];
									}
									tagNode.ImageIndex = tagNode.SelectedImageIndex = imageListIndex;
									tagNode.ForeColor = tagTypeColor;
									tagCounter++;
								}
							}
							if (tagCounter > 0) tagTypeCounter++;
							break;
						default: break;
					}
				}
			}
			else
			{
				sourceNode.ToolTipText = Error;
				sourceNode.ForeColor = Color.Red;
			}
			
			sourceNode.Tag = this;
			sourceNode.ImageIndex = sourceNode.SelectedImageIndex = ImageListIndex;
			sourceNode.Name = PathUnquoted;
			
			if ((tagTypeCounter == 0) && !string.IsNullOrEmpty(filter))
			{
				PluginBase.TRACE("tagTypeCounter=0.. hiding it");
				if (!invisibleSourceNodes.Contains(sourceNode))
					invisibleSourceNodes.Add(sourceNode);
			}
			else
			{
				if (newSourceNode)
				{
					treeview.Nodes.Add(sourceNode);
				}
				if (!string.IsNullOrEmpty(filter) && Settings.Configs.ViewMode.ToString().EndsWith("Class"))
					sourceNode.ExpandAll();
				else if (expandSource)
				{
					if (Settings.Configs.ViewMode == Settings.TreeViewMode.ClassSession)
						foreach (TreeNode n in treeview.Nodes)
							n.Expand();
					else
						sourceNode.Expand();
				}
				retShown = true;
				PluginBase.TRACE(string.Format("Finished drawing.. newSourceNode={0} expandSource={1}", newSourceNode, expandSource));
			}
			
			if ((Settings.Configs.SessionMode == Settings.SessionMode.Cookie) && (refreshTags != RefreshType.None))
				Settings.SessionChanged = true;
			PluginBase.TRACE("-END-");
			return retShown;
		}
		
		TreeNode GetRootNode(TreeView treeview)
		{
			foreach (TreeNode sourceNode in treeview.Nodes)
			{
				Source source = sourceNode.Tag as Source;
				if (source != null)
				{
					if (source.PathUnquoted == PathUnquoted)
					{
						return sourceNode;
					}
				}
			}
			foreach (TreeNode sourceNode in Source.invisibleSourceNodes)
			{
				Source source = sourceNode.Tag as Source;
				if (source.PathUnquoted == PathUnquoted)
				{
					return sourceNode;
				}
			}
			return null;
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
		public static void AddIncludeFile(TreeView treeview, string path, string filter)
		{
			TreeNode includesNode = GetIncludesRootNode(treeview);
			IncludeFile incFile = null;
			foreach (IncludeFile inc in includesNode.Nodes)
				if (inc.ToolTipText == path) { incFile = inc; break; }
			if (!string.IsNullOrEmpty(filter))
				foreach (IncludeFile inc in Source.invisibleIncludeFileNodes)
					if (inc.Name == path)
						{incFile = inc; break; }
			
			if (incFile == null)
			{
				incFile = new IncludeFile(path);
				incFile.NodeFont = fontIncludeItalic;
			}
			else
			{
				incFile.RefreshTags();
			}
			
			MoveFilteredIncludeFile(incFile, includesNode, filter);
			
			Settings.SessionChanged = true;
		}
		public static void MoveFilteredIncludeFile(IncludeFile incFile, TreeNode includesNode, string filter)
		{
			if (!string.IsNullOrEmpty(incFile.Error) && !Settings.Configs.ShowInvalidSources)
			{
				if (includesNode.Nodes.Count == 0) includesNode.Remove();
			}
			else
			{
				bool found = false;
				if (string.IsNullOrEmpty(filter))
					found = true;
				else if (incFile.QuickTags != null)
					foreach (QuickTag tag in incFile.QuickTags)
						if (tag.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
							{ found = true; break; }
				
				if (found)
				{
					if (incFile.Parent == null)
					{
						includesNode.Nodes.Add(incFile);
						includesNode.ToolTipText = includesNode.Nodes.Count.ToString();
						Source.invisibleIncludeFileNodes.Remove(incFile);
					}
				}
				else
				{
					if (incFile.Parent != null)
					{
						Source.invisibleIncludeFileNodes.Add(incFile);
						includesNode.Nodes.Remove(incFile);
						if (includesNode.Nodes.Count == 0) includesNode.Remove();
					}
				}
			}
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
		
		public bool DisplayAccess = false;
		public bool DisplayReturnType = false;
		public bool DisplayScope = false;
		public bool DisplaySignature = false;
		
		public bool CaseSensitive = true;
		public string ScopeOperator = ".";
		
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
		public string Language = "";
		public string ScopeOperator = "";
		public string SourceFile = "";
		public int Line = -1;
		public string TagName = "";
		public string Identifier = "";
		public string ParentIdentifier = "";
		public string Scope = "";
		public string Signature = "";
		public string Access = "";
		public string ReturnType = "";
		
		public const string PSEUDO_TAG = "PSEUDO_TAG";
		
		public Tag(string line, string language, string scopeOperator)
		{
			PluginBase.TRACE(string.Format("Trying to add:{0}", line));
			if (string.IsNullOrEmpty(line)) return;
			if (line.StartsWith("!")) return;
			if (line.StartsWith("ctags.exe: Warning:")) return;
		
			Language = language;
			ScopeOperator = scopeOperator;
			string[] fields = line.Split(CTagsExe.FieldSeparator); // 'Ø'... originally '\t';
			PluginBase.TRACE(string.Format("Split into {0} fields", fields.Length));
			string[] customFields = fields[0].Split(CTagsExe.FieldSeparatorExtended); // 'Ï'... user sees '|'
			PluginBase.TRACE(string.Format("Split into {0} custom fields", customFields.Length));
			TagName = customFields[0];
			for (int i = 1; i < customFields.Length; i++)
				GetExtendedField(customFields[i]);
			
			if ((Settings.Configs.ViewMode == Settings.TreeViewMode.ClassSession) && (string.IsNullOrEmpty(Scope)))
			{
				string[] scopeToks = TagName.Split(new string[] { ScopeOperator }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < scopeToks.Length - 1; i++)
					Scope += scopeToks[i] + ScopeOperator;
				TagName = scopeToks[scopeToks.Length - 1];
			}
			
			base.Name = fields[2].Split(';')[0]; // KEY!
			SourceFile = fields[1];
			Line = int.Parse(base.Name);
			Identifier = fields[3];
			PluginBase.TRACE(string.Format("Text={0} Name={1} SourceFile={2} Line={3} Identifier={4}",
				TagName, base.Name, SourceFile, Line, Identifier));
			
			for (int i = 4; i < fields.Length; i++)
				GetExtendedField(fields[i]);

			if (Settings.Languages[language].DisplayAccess)
				base.Text += Access;
			if (Settings.Languages[language].DisplayReturnType)
				base.Text += ReturnType;
			if (Settings.Languages[language].DisplayScope)
				base.Text += Scope;
			base.Text += TagName;
			if (Settings.Languages[language].DisplaySignature)
				base.Text += Signature;
			
			base.ToolTipText = Access + ReturnType + Scope + TagName + Signature;
			
			PluginBase.TRACE(string.Format("Added:{0}", base.ToolTipText));
		}
		void GetExtendedField(string field)
		{
			if (field.StartsWith("access:"))
				Access = field.Replace("access:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("returntype:"))
				ReturnType = field.Replace("returntype:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("signature:"))
				Signature = " " + field.Replace("signature:", "").Replace('\t', ' ');
			else
			{
				int indexColon = field.IndexOf(':');
				if (indexColon >= 0)
					ParentIdentifier = field.Substring(0, indexColon).Replace('\t', ' ');
				Scope = field.Substring(indexColon + 1).Replace('\t', ' ') + ScopeOperator;
			}
			PluginBase.TRACE(string.Format("field={0} Access={1} ReturnType={2} Signature={3} Scope={4}",
				field, Access, ReturnType, Signature, Scope));
		}
		
		public Tag() { }
		protected Tag(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Language = info.GetString("Language");
			ScopeOperator = info.GetString("ScopeOperator");
			SourceFile = info.GetString("SourceFile");
			Line = info.GetInt32("Line");
			TagName = info.GetString("TagName");
			Identifier = info.GetString("Identifier");
			ParentIdentifier = info.GetString("ParentIdentifier");
			Scope = info.GetString("Scope");
			Signature = info.GetString("Signature");
			Access = info.GetString("Access");
			ReturnType = info.GetString("ReturnType");
			base.ToolTipText = info.GetString("ToolTipText");
		}
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Language", Language);
			info.AddValue("ScopeOperator", ScopeOperator);
			info.AddValue("SourceFile", SourceFile);
			info.AddValue("Line", Line);
			info.AddValue("TagName", TagName);
			info.AddValue("Identifier", Identifier);
			info.AddValue("ParentIdentifier", ParentIdentifier);
			info.AddValue("Scope", Scope);
			info.AddValue("Signature", Signature);
			info.AddValue("Access", Access);
			info.AddValue("ReturnType", ReturnType);
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
		public string ScopeOperator = "";
		
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
				Source.GetLanguage(path, ref Language, ref Extension, ref ScopeOperator);
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
				CTagsExe.MapQuickTags(this, Language, ScopeOperator);
				
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
			ScopeOperator = info.GetString("ScopeOperator");
		}
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Error", Error);
			info.AddValue("SourceFile", SourceFile);
			info.AddValue("ToolTipText", ToolTipText);
			info.AddValue("Extension", Extension);
			info.AddValue("Language", Language);
			info.AddValue("ScopeOperator", ScopeOperator);
			base.Serialize(info, context);
		}
	}
	
	[Serializable]
	public class QuickTag
	{
		public string Language = "";
		public string ScopeOperator = "";
		public string SourceFile = "";
		public string Text = "";
		public int Line = -1;
		public string Identifier = "";
		public string ParentIdentifier = "";
		public string Scope = "";
		public string Signature = "";
		public string Access = "";
		public string ReturnType = "";
		public string FullText = "";

		public QuickTag(string line, string language, string scopeOperator)
		{
			PluginBase.TRACE(string.Format("Trying to add:{0}", line));
			if (string.IsNullOrEmpty(line) || string.IsNullOrEmpty(language)) return;
			if (line.StartsWith("!")) return;
			if (line.StartsWith("ctags.exe: Warning:")) return;
		
			Language = language;
			ScopeOperator = scopeOperator;
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
			FullText = Access + ReturnType + Scope + Text + Signature;
			PluginBase.TRACE(string.Format("Added:{0}", FullText));
		}
		void GetExtendedField(string field)
		{
			if (field.StartsWith("access:"))
				Access = field.Replace("access:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("returntype:"))
				ReturnType = field.Replace("returntype:", "").Replace('\t', ' ') + " ";
			else if (field.StartsWith("signature:"))
				Signature = " " + field.Replace("signature:", "").Replace('\t', ' ');
			else
			{
				int indexColon = field.IndexOf(':');
				if (indexColon >= 0)
					ParentIdentifier = field.Substring(0, indexColon).Replace('\t', ' ');
				Scope = field.Substring(indexColon + 1).Replace('\t', ' ') + ScopeOperator;
			}
			
			PluginBase.TRACE(string.Format("field={0} Access={1} ReturnType={2} Signature={3} Scope={4}",
				field, ReturnType, Access, Signature, Scope));
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
						if ((Settings.Configs.ViewMode == Settings.TreeViewMode.Flat)
						    || (Settings.Configs.ViewMode == Settings.TreeViewMode.Grouped)
						    || (tag1.Identifier == tag2.Identifier))
						{
							if (Settings.Configs.SortTags)
							{
								return string.Compare(tag1.TagName, tag2.TagName);
							}
							else
							{
								if (tag1.Line > tag2.Line) return 1;
								else if (tag1.Line < tag2.Line) return -1;
							}
						}
						else
						{
							int index1 = 0, index2 = 0;
							foreach (string identifier in Settings.Languages[tag1.Language].TagTypes.Keys)
								if (identifier == tag1.Identifier) break; else index1++;
							foreach (string identifier in Settings.Languages[tag2.Language].TagTypes.Keys)
								if (identifier == tag2.Identifier) break; else index2++;
							if (index1 == index2) return string.Compare(tag1.TagName, tag2.TagName);
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
					else
					{
						string lang1 = "", lang2 = "";
						string type1 = "", type2 = "";
						int line1 = 0, line2 = 0;
						int index1 = 0, index2 = 0;
						
						if (obj1 is Tag)
						{
							lang1 = (obj1 as Tag).Language;
							type1 = (obj1 as Tag).Identifier;
							line1 = (obj1 as Tag).Line;
						}
						else if (node1.Name == Tag.PSEUDO_TAG)
						{
							lang1 = (string)(node1.Tag as Dictionary<string, object>)["language"];
							type1 = (string)(node1.Tag as Dictionary<string, object>)["tagtypename"];
							line1 = (int)(node1.Tag as Dictionary<string, object>)["line"];
						}
						if (obj2 is Tag)
						{
							lang2 = (obj2 as Tag).Language;
							type2 = (obj2 as Tag).Identifier;
							line2 = (obj2 as Tag).Line;
						}
						else if (node2.Name == Tag.PSEUDO_TAG)
						{
							lang2 = (string)(node2.Tag as Dictionary<string, object>)["language"];
							type2 = (string)(node2.Tag as Dictionary<string, object>)["tagtypename"];
							line2 = (int)(node2.Tag as Dictionary<string, object>)["line"];
						}
						
						foreach (string identifier in Settings.Languages[lang1].TagTypes.Keys)
							if (identifier == type1) break; else index1++;
						foreach (string identifier in Settings.Languages[lang2].TagTypes.Keys)
							if (identifier == type2) break; else index2++;
						if (index1 == index2)
						{
							if (Settings.Configs.SortTags)
							{
								return string.Compare(node1.Text, node2.Text);
							}
							else
							{
								if (line1 > line2) return 1;
								else if (line1 < line2) return -1;
							}
						}
						return (index1 > index2) ? 1 : -1;
					}
				}
			}
			catch { }

			return 0;
		}
	}
}
