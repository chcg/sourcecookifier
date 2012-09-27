using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SourceCookifier
{
    class CTagsExe
    {
        static string cTagsVersion = "5.8.7.SC";
        static string cTagsExePath;
        static string tagsFilePath;
        static string stdOut;
        static int numTagsWritten;
        public const char FieldSeparator = (char)0x01; // '☺'
        public const char FieldSeparatorExtended = (char)0x02; // '☻'
        
        public static void Init()
        {
            cTagsExePath = Path.Combine(Settings.PluginSubFolder, "ctags.exe");
            tagsFilePath = Path.Combine(Settings.ConfigDir, "SourceCookifier.tags");
            stdOut = "";
            Main.TRACE("CTags.exe path = " + cTagsExePath);
            if (!File.Exists(cTagsExePath)) throw new Exception("'ctags.exe' not found!");
            if (!CheckCtagsVersion()) throw new Exception("'ctags.exe' version does not fit SourceCookifier version!");
        }
        static bool CheckCtagsVersion()
        {
            Environment.SetEnvironmentVariable("SourceCookifierVersion",
                Assembly.GetExecutingAssembly().GetName().Version.ToString(), EnvironmentVariableTarget.Process);
            RunCTagsExe("--version", false);
            Main.TRACE(stdOut.Substring(0, stdOut.IndexOf('\n')));
            return (stdOut.Contains(string.Format("Exuberant Ctags {0}", cTagsVersion)));
        }
        public static void GetLangMaps()
        {
            Main.TRACE("-START-");
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
                        Main.TRACE(string.Format("lang={0} ext={1}", toks[0], toks[i].Replace("*", "")));
                    }
                }
            }
            Main.TRACE("-END-");
        }
        public static void GetLangTagTypes()
        {
            Main.TRACE("-START-");
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
                        Main.TRACE(string.Format("lang={0} tagtype={1} desc={2}", lang, identifier, line.Substring(3)));
                    }
                }
            }
            Main.TRACE("-END-");
        }

        public static void DoTags(string sourcefile, string language, string extension, string filesizelimit)
        {
            Main.TRACE("-START-");

            if (!string.IsNullOrEmpty(filesizelimit))
            {
                bool bMB = filesizelimit.EndsWith("mb");
                long iLimit = Convert.ToInt64(filesizelimit.Replace(bMB ? "mb" : "kb", ""));
                long iFileSize = new FileInfo(sourcefile.Replace("\"", "")).Length / (bMB ? 1048576 : 1024);
                string sFileSize = string.Format("{0}{1}", iFileSize, (bMB ? "mb" : "kb"));
                if (iFileSize > iLimit)
                {
                    throw new Exception(string.Format("Custom filesize limit [{0}] reached: {1}!",
                        filesizelimit, sFileSize));
                }
            }
            
            StringBuilder args = new StringBuilder();
            StringBuilder typeFlags = new StringBuilder();
            
            args.AppendFormat("-f \"{0}\" ", tagsFilePath);
            args.Append("--excmd=number ");
            args.Append("--fields=aksST ");
            args.Append("--sort=no ");
            
            if (!Settings.Languages[language].BuildIn)
                args.AppendFormat("--langdef={0} ", language);
            
            args.AppendFormat("--langmap={0}:{1} ", language, (!string.IsNullOrEmpty(extension)) ? extension : ".");

            foreach (string tagtypename in Settings.Languages[language].TagTypes.Keys)
            {
                if (Settings.Languages[language].TagTypes[tagtypename].Show)
                {
                    foreach (string regexpattern in Settings.Languages[language].TagTypes[tagtypename].RegexPatterns)
                    {
                        args.Append(string.Format("--regex-{0}=\"{1}\" ", language, regexpattern));
                    }
                    typeFlags.Append(tagtypename);
                }
            }

            args.AppendFormat("--{0}-kinds={1} ", language, typeFlags.ToString());
            args.Append(sourcefile);

            RunCTagsExe(args.ToString(), true);
            Main.TRACE("-END-");
        }
        public enum MapTagsResult { OK, MoveToIncludes }
        public static MapTagsResult MapTags(Source source)
        {
            Main.TRACE("-START-");
            HandleCTagsStdOut();

            if (numTagsWritten > Convert.ToInt32(Settings.Configs.MaxNumShownSymbols))
            {
                string msg = string.Format("Too many symbols: {0} [custom limit: {1}]",
                        numTagsWritten, Settings.Configs.MaxNumShownSymbols);
                Main.TRACE(msg);
                if (Settings.Configs.SessionMode == Settings.SessionMode.Cookie)
                {
                    return MapTagsResult.MoveToIncludes;
                }
                else
                {
                    throw new Exception(msg);
                }
            }

            using (StreamReader r = new StreamReader(tagsFilePath, Encoding.GetEncoding(1252)))
            {
                string line = null;
                Main.TRACE(string.Format("Language={0}", source.Language));
                while ((line = r.ReadLine()) != null)
                {
                    source.AddTag(new Tag(line, source.Language, source.ScopeOperator));
                }
            }
            Main.TRACE("-END-");
            return MapTagsResult.OK;
        }
        public static void MapQuickTags(IncludeFile incFile, string language, string scopeOperator)
        {
            Main.TRACE("-START-");
            HandleCTagsStdOut();
            using (StreamReader r = new StreamReader(tagsFilePath, Encoding.GetEncoding(1252)))
            {
                string line = null;
                Main.TRACE(string.Format("Language={0}", language));
                while ((line = r.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("!_TAG_")) continue;
                    incFile.QuickTags.Add(new QuickTag(line, language, scopeOperator));
                }
            }
            Main.TRACE("-END-");
        }

        static void HandleCTagsStdOut()
        {
            Main.TRACE("-START-");
            Main.TRACE("StdOut = " + stdOut);
            numTagsWritten = 0;
            if (!string.IsNullOrEmpty(stdOut))
            {
                foreach (string line in stdOut.Split(
                    new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.Contains("SC-Info:"))
                    {
                        string[] toks = line.Split(
                            new string[] { "SC-Info:" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] info = toks[1].Trim().Split('=');
                        if (info[0] == "NUM_TAGS")
                        {
                            numTagsWritten = Convert.ToInt32(info[1]);
                        }
                    }
                    else if (Settings.Configs.HandleCtagsWarnings)
                    {
                        throw new Exception(line);
                    }
                }
            }
            Main.TRACE("numTagsWritten = " + numTagsWritten);
            Main.TRACE("-END-");
        }
        
        static void RunCTagsExe(string args, bool usetagfile)
        {
            Main.TRACE("-START-");
            
            Process p = new Process();
            p.StartInfo.WorkingDirectory = Settings.PluginSubFolder;
            p.StartInfo.FileName = cTagsExePath;
            p.StartInfo.Arguments = "--options=NONE " + args;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(1252);

            Main.TRACE(string.Format("Running ctags.exe with args={0}", args));
            p.Start();
            
            stdOut = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            if (usetagfile && !File.Exists(tagsFilePath)) throw new Exception("ERROR: Tagfile not found");
            
            if (p.ExitCode != 0) throw new Exception("ERROR: " + stdOut);
            Main.TRACE("-END-");
        }
    }
}
