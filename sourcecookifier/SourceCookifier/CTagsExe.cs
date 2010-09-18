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
	class CTagsExe
	{
		static string cTagsVersion = "5.8.2.SC";
		static string cTagsExePath;
		static string tagsFilePath;
		static string stdOut;
		public const char FieldSeparator = (char)0x01; // '☺'
		public const char FieldSeparatorExtended = (char)0x02; // '☻'
		
		public static void Init()
		{
			cTagsExePath = Settings.ApplicationDir + "ctags.exe";
			tagsFilePath = Settings.ApplicationDir + "tags";
			stdOut = "";
			if (!File.Exists(cTagsExePath)) throw new Exception("'ctags.exe' not found!");
			if (!CheckCtagsVersion()) throw new Exception("'ctags.exe' version does not fit SourceCookifier version!");
		}
		static bool CheckCtagsVersion()
		{
			RunCTagsExe("--version", false);
			PluginBase.TRACE(stdOut.Substring(0, stdOut.IndexOf('\n')));
			return (stdOut.StartsWith(string.Format("Exuberant Ctags {0}", cTagsVersion)));
		}
		public static void GetLangMaps()
		{
			PluginBase.TRACE("-START-");
			string args = "--list-maps";
			RunCTagsExe(args, false);

			using (StreamReader r = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(stdOut))))
			{
				string line = null;
				while ((line = r.ReadLine()) != null)
				{
					string[] toks = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
					Settings.Languages[toks[0]] = new Language();
					for (int i = 1; i < toks.Length; i++)
					{
						//TODO: "*" ?
						Settings.Languages[toks[0]].Extensions.Add(toks[i].Replace("*", ""));
						PluginBase.TRACE(string.Format("lang={0} ext={1}", toks[0], toks[i].Replace("*", "")));
					}
				}
			}
			PluginBase.TRACE("-END-");
		}
		public static void GetLangTagTypes()
		{
			PluginBase.TRACE("-START-");
			string args = "--list-kinds=all";
			RunCTagsExe(args, false);

			using (StreamReader r = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(stdOut))))
			{
				string line = r.ReadLine();
				while (line != null)
				{
					string lang = line;
					while (((line = r.ReadLine()) != null) && (line[0] == ' '))
					{
						line = line.Trim();
						string identifier = line[0].ToString();
						Settings.Languages[lang].TagTypes[identifier] = new TagType();
						Settings.Languages[lang].TagTypes[identifier].Description = line.Substring(3);
						PluginBase.TRACE(string.Format("lang={0} tagtype={1} desc={2}", lang, identifier, line.Substring(3)));
					}
				}
			}
			PluginBase.TRACE("-END-");
		}

		public static void DoTags(string sourcefile, string language, string extension)
		{
			PluginBase.TRACE("-START-");
			StringBuilder args = new StringBuilder();
			StringBuilder typeFlags = new StringBuilder();
			
			args.Append("--excmd=number ");
			args.Append("--fields=aksS ");
			args.Append((Settings.Configs.UseTagFile ? "" : "-f- "));
			args.Append("--sort=no ");
			
			if (!Settings.Languages[language].BuildIn)
				args.AppendFormat("--langdef={0} ", language);
			
			args.AppendFormat("--langmap={0}:{1} ", language, (!string.IsNullOrEmpty(extension)) ? extension : ".");

			foreach (string tagtypename in Settings.Languages[language].TagTypes.Keys)
			{
				foreach (string regexpattern in Settings.Languages[language].TagTypes[tagtypename].RegexPatterns)
				{
					args.Append(string.Format("--regex-{0}={1} ", language, regexpattern));
				}
				if (Settings.Languages[language].TagTypes[tagtypename].Show)
					typeFlags.Append(tagtypename);
			}
			
			args.AppendFormat("--{0}-kinds={1} ", language, typeFlags.ToString());
			args.Append(sourcefile);

			RunCTagsExe(args.ToString(), Settings.Configs.UseTagFile);
			PluginBase.TRACE("-END-");
		}
		public static void MapTags(Source source)
		{
			PluginBase.TRACE("-START-");
			using (StreamReader r = (Settings.Configs.UseTagFile ? 
			                         new StreamReader(tagsFilePath, Encoding.GetEncoding(1252))
			                         :
			                         new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(stdOut)))))
			{
				string line = null;
				PluginBase.TRACE(string.Format("Language={0}", source.Language));
				while ((line = r.ReadLine()) != null)
				{
					source.AddTag(new Tag(line));
				}
			}
			PluginBase.TRACE("-END-");
		}
		public static void MapQuickTags(IncludeFile incFile, string language)
		{
			PluginBase.TRACE("-START-");
			using (StreamReader r = (Settings.Configs.UseTagFile ? 
			                         new StreamReader(tagsFilePath, Encoding.GetEncoding(1252))
			                         :
			                         new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(stdOut)))))
			{
				string line = null;
				PluginBase.TRACE(string.Format("Language={0}", language));
				while ((line = r.ReadLine()) != null)
				{
					incFile.QuickTags.Add(new QuickTag(line, language));
				}
			}
			PluginBase.TRACE("-END-");
		}
		
		static void RunCTagsExe(string args, bool usetagfile)
		{
			PluginBase.TRACE("-START-");
		    
			Process p = new Process();
			p.StartInfo.WorkingDirectory = Settings.ApplicationDir;
			p.StartInfo.FileName = cTagsExePath;
			p.StartInfo.Arguments = args;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(1252);

			PluginBase.TRACE(string.Format("Running ctags.exe with args={0}", args));
			p.Start();
			
			stdOut = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			if (usetagfile && !File.Exists(tagsFilePath)) throw new Exception("ERROR: Tagfile not found");
			
			if (p.ExitCode != 0) throw new Exception("ERROR: " + stdOut);
			PluginBase.TRACE("-END-");
		}
	}
}
