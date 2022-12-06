using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SourceCookifier
{
    partial class frmSettings : Form
    {
        public SerializableDictionary<string, Language> tempLanguages;
        public SerializableDictionary<string, Language> oldLanguages;
        public bool Changed = false;
        public bool ChangedEx = false;
        public bool Tried = false;
        public frmSettings()
        {
            InitializeComponent();
            Icon = Properties.Resources.cookie_monster;
            try
            {
                oldLanguages = Settings.Languages;
                tempLanguages = new SerializableDictionary<string, Language>();
                foreach (string oldLanguage in Settings.Languages.Keys)
                {
                    Language newLanguage = new Language();
                    newLanguage.BuildIn = Settings.Languages[oldLanguage].BuildIn;
                    foreach (string ext in Settings.Languages[oldLanguage].Extensions)
                        newLanguage.Extensions.Add(ext);
                    newLanguage.CaseSensitive = Settings.Languages[oldLanguage].CaseSensitive;
                    newLanguage.ScopeOperator = Settings.Languages[oldLanguage].ScopeOperator;
                    newLanguage.DisplayAccess = Settings.Languages[oldLanguage].DisplayAccess;
                    newLanguage.DisplayReturnType = Settings.Languages[oldLanguage].DisplayReturnType;
                    newLanguage.DisplayScope = Settings.Languages[oldLanguage].DisplayScope;
                    newLanguage.DisplaySignature = Settings.Languages[oldLanguage].DisplaySignature;
                    foreach (string identifier in Settings.Languages[oldLanguage].TagTypes.Keys)
                    {
                        TagType tagtype = new TagType();
                        tagtype.Description = Settings.Languages[oldLanguage].TagTypes[identifier].Description;
                        tagtype.BuildIn = Settings.Languages[oldLanguage].TagTypes[identifier].BuildIn;
                        tagtype.Show = Settings.Languages[oldLanguage].TagTypes[identifier].Show;
                        tagtype.TrackCaret = Settings.Languages[oldLanguage].TagTypes[identifier].TrackCaret;
                        tagtype.IconFilename = Settings.Languages[oldLanguage].TagTypes[identifier].IconFilename;
                        tagtype.ForeColor = Settings.Languages[oldLanguage].TagTypes[identifier].ForeColor;
                        foreach (string regex in Settings.Languages[oldLanguage].TagTypes[identifier].RegexPatterns)
                            tagtype.RegexPatterns.Add(regex);
                        newLanguage.TagTypes[identifier] = tagtype;
                    }
                    tempLanguages[oldLanguage] = newLanguage;

                    if (newLanguage.BuildIn)
                        lbxLanguages.Items.Add(oldLanguage);
                    else
                        lbxLanguages.Items.Add(oldLanguage + "*");
                }
                
                if (Settings.lstLangSetDlgSelections.Count == 0)
                {
                    Settings.lstLangSetDlgSelections.Add(-1);
                    Settings.lstLangSetDlgSelections.Add(-1);
                    Settings.lstLangSetDlgSelections.Add(-1);
                    Settings.lstLangSetDlgSelections.Add(-1);
                }
                
                lbxLanguages.SelectedIndex = Settings.lstLangSetDlgSelections[0];
                lbxExtensions.SelectedIndex = Settings.lstLangSetDlgSelections[1];
                lbxTagTypes.SelectedIndex = Settings.lstLangSetDlgSelections[2];
                lbxRegex.SelectedIndex = Settings.lstLangSetDlgSelections[3];
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        
        void FrmSettingsFormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.lstLangSetDlgSelections[0] = lbxLanguages.SelectedIndex;
            Settings.lstLangSetDlgSelections[1] = lbxExtensions.SelectedIndex;
            Settings.lstLangSetDlgSelections[2] = lbxTagTypes.SelectedIndex;
            Settings.lstLangSetDlgSelections[3] = lbxRegex.SelectedIndex;
        }
        
        #region " Language "
        void LbxLanguagesSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLanguageDel.Enabled = false;
                btnLanguageExport.Enabled = (lbxLanguages.SelectedItems.Count >= 1);
                ResetExtensionGroup();
                ResetTagTypeGroup();
                ResetAppearanceGroup();
                ResetRegexGroup();
                if (lbxLanguages.SelectedItems.Count == 1)
                {
                    string name = (string)lbxLanguages.SelectedItem;
                    
                    if (name.EndsWith("*"))
                    {
                        name = name.Substring(0, name.Length - 1);
                        btnLanguageDel.Enabled = true;
                    }

                    foreach (string ext in tempLanguages[name].Extensions)
                    {
                        lbxExtensions.Items.Add(ext);
                    }
                    gbxExtension.Enabled = true;
                    
                    gbxSemantics.Enabled = true;
                    cbxSemanticsCaseSensitive.Checked = tempLanguages[name].CaseSensitive;
                    tbxSemanticsScopeOperator.Text = tempLanguages[name].ScopeOperator;

                    gbxDisplay.Enabled = true;
                    cbxDisplayAccess.Checked = tempLanguages[name].DisplayAccess;
                    cbxDisplayReturnType.Checked = tempLanguages[name].DisplayReturnType;
                    cbxDisplayScope.Checked = tempLanguages[name].DisplayScope;
                    cbxDisplaySignature.Checked = tempLanguages[name].DisplaySignature;
                    
                    foreach (string identifier in tempLanguages[name].TagTypes.Keys)
                    {
                        if (tempLanguages[name].TagTypes[identifier].BuildIn)
                            lbxTagTypes.Items.Add(identifier);
                        else
                            lbxTagTypes.Items.Add(identifier + "*");
                    }
                    gbxTagType.Enabled = true;
                    GetFreeIdentifiers();
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnLanguageAddClick(object sender, EventArgs e)
        {
            try
            {
                string name = tbxNewLanguage.Text.Trim();
                
                foreach (int chr in name)
                {
                    if (chr < 0x41 || chr > 0x5A && chr < 0x61 || chr > 0x7A)
                    {
                        MessageBox.Show("Please only use alphabetical chars");
                        return;
                    }
                }
                
                foreach (string _name in tempLanguages.Keys)
                {
                    if (string.Compare(_name, name, true) == 0)
                    {
                        MessageBox.Show("The language name is already in use");
                        return;
                    }
                }

                Language language = new Language();
                language.BuildIn = false;
                tempLanguages.Add(name, language);
                int index = lbxLanguages.Items.Add(name + "*");
                lbxLanguages.SelectedIndex = -1;
                lbxLanguages.SelectedIndex = index;
                Changed = true;
                ChangedEx = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnLanguageDelClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                tempLanguages.Remove(name.Substring(0, name.Length - 1));
                lbxLanguages.Items.Remove(name);
                Changed = true;
                ChangedEx = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TbxNewLanguageTextChanged(object sender, EventArgs e)
        {
            btnLanguageAdd.Enabled = (tbxNewLanguage.Text.Trim() != "");
        }
        void BtnLanguageImportClick(object sender, EventArgs e)
        {
            lbxLanguages.SelectedItems.Clear();
            if (Settings.ImportLanguages(ref tempLanguages))
            {
                Changed = true;
                ChangedEx = true;
                lbxLanguages.Items.Clear();
                foreach (string language in tempLanguages.Keys)
                {
                    if (tempLanguages[language].BuildIn)
                        lbxLanguages.Items.Add(language);
                    else
                        lbxLanguages.Items.Add(language + "*");
                }
            }
        }
        void BtnLanguageExportClick(object sender, EventArgs e)
        {
            if (lbxLanguages.SelectedItems.Count > 0)
            {
                List<string> lstSelectedLangs = new List<string>();
                foreach (string lang in lbxLanguages.SelectedItems)
                {
                    lstSelectedLangs.Add(lang.EndsWith("*") ? lang.Substring(0, lang.Length - 1) : lang);
                }
                Settings.ExportLanguages(ref tempLanguages, lstSelectedLangs);
            }
        }
        #endregion
        
        #region " Extension, Semantics, Display "
        void ResetExtensionGroup()
        {
            gbxExtension.Enabled = false;
            lbxExtensions.Items.Clear();
            btnExtensionDel.Enabled = false;
            tbxNewExtension.Clear();
            gbxSemantics.Enabled = false;
            gbxDisplay.Enabled = false;
        }
        void LbxExtensionsSelectedIndexChanged(object sender, EventArgs e)
        {
            btnExtensionDel.Enabled = (lbxExtensions.SelectedItem != null);
        }
        void TbxNewExtensionTextChanged(object sender, EventArgs e)
        {
            btnExtensionAdd.Enabled = (tbxNewExtension.Text.Trim() != "");
        }
        void BtnExtensionAddClick(object sender, EventArgs e)
        {
            try
            {
                string newExtension = tbxNewExtension.Text.Trim();
                if (newExtension != "")
                {
                    foreach (string _name in tempLanguages.Keys)
                    {
                        bool found = false;
                        if (!Settings.Configs.CaseSensitiveLanguageMaps)
                        {
                            foreach (string ext in tempLanguages[_name].Extensions)
                            {
                                if (string.Compare(ext, newExtension, true) == 0)
                                    { found = true; break; }
                            }
                        }
                        else if (tempLanguages[_name].Extensions.Contains(newExtension))
                        {
                            found = true;
                        }
                        if (found)
                        {
                            MessageBox.Show(string.Format("This extension is already used for '{0}'", _name));
                            return;
                        }
                    }
                    string name = (string)lbxLanguages.SelectedItem;
                    if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                    tempLanguages[name].Extensions.Add(newExtension);
                    lbxExtensions.Items.Add(newExtension);
                    Changed = true;
                    ChangedEx = true;
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnExtensionDelClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string selExt = (string)lbxExtensions.SelectedItem;
                tempLanguages[name].Extensions.Remove(selExt);
                lbxExtensions.Items.Remove(selExt);
                Changed = true;
                ChangedEx = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void SemanticsChanged(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*"))
                    name = name.Substring(0, name.Length - 1);
                if (sender == cbxSemanticsCaseSensitive)
                {
                    tempLanguages[name].CaseSensitive = cbxSemanticsCaseSensitive.Checked;
                }
                else if (sender == tbxSemanticsScopeOperator)
                {
                    tempLanguages[name].ScopeOperator = tbxSemanticsScopeOperator.Text;
                    ChangedEx = true;
                }
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void DisplayExtendedChanged(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*"))
                    name = name.Substring(0, name.Length - 1);
                if (sender == cbxDisplayAccess)
                    tempLanguages[name].DisplayAccess = cbxDisplayAccess.Checked;
                else if (sender == cbxDisplayReturnType)
                    tempLanguages[name].DisplayReturnType = cbxDisplayReturnType.Checked;
                else if (sender == cbxDisplayScope)
                    tempLanguages[name].DisplayScope = cbxDisplayScope.Checked;
                else if (sender == cbxDisplaySignature)
                    tempLanguages[name].DisplaySignature = cbxDisplaySignature.Checked;
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        #endregion
        
        #region " Tagtype "
        void ResetTagTypeGroup()
        {
            gbxTagType.Enabled = false;
            lbxTagTypes.Items.Clear();
            cbxTagTypeAdd.Items.Clear();
            btnTagTypeUp.Enabled = false;
            btnTagTypeDown.Enabled = false;
            btnTagTypeDel.Enabled = false;
        }
        void GetFreeIdentifiers()
        {
            try
            {
                cbxTagTypeAdd.Items.Clear();
                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                for (int i = 0; i < alphabet.Length; i++)
                {
                    string newIdentifier = alphabet[i].ToString();
                    if (lbxTagTypes.Items.Contains(newIdentifier)) continue;
                    if (lbxTagTypes.Items.Contains(newIdentifier + "*")) continue;
                    cbxTagTypeAdd.Items.Add(newIdentifier);
                }
                if (cbxTagTypeAdd.Items.Count > 0)
                {
                    cbxTagTypeAdd.SelectedIndex = 0;
                    btnTagTypeAdd.Enabled = true;
                }
                else btnTagTypeAdd.Enabled = false;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void LbxTagTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            ResetAppearanceGroup();
            ResetRegexGroup();
            try
            {
                if (lbxTagTypes.SelectedItem != null)
                {
                    string name = (string)lbxLanguages.SelectedItem;
                    if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                    string identifier = (string)lbxTagTypes.SelectedItem;
                    if (identifier.Length == 2)
                    {
                        identifier = identifier.Substring(0, 1);
                        btnTagTypeDel.Enabled = true;
                    }
                    else btnTagTypeDel.Enabled = false;
                    int index = lbxTagTypes.SelectedIndex;
                    btnTagTypeUp.Enabled = (index > 0);
                    btnTagTypeDown.Enabled = (index < lbxTagTypes.Items.Count - 1);
                    
                    gbxAppearance.Enabled = true;
                    tbxDescription.Text = tempLanguages[name].TagTypes[identifier].Description;
                    string iconFilename = tempLanguages[name].TagTypes[identifier].IconFilename;
                    tbxIcon.Text = iconFilename;
                    if (!string.IsNullOrEmpty(iconFilename))
                    {
                        string iconPath = Path.Combine(Settings.PluginSubFolderIcons, iconFilename);
                        if (File.Exists(iconPath))
                        {
                            pbxIcon.Image = new Bitmap(iconPath);
                        }
                    }
                    else pbxIcon.Image = null;
                    cbxShow.Checked = tempLanguages[name].TagTypes[identifier].Show;
                    cbxTrackCaret.Checked = tempLanguages[name].TagTypes[identifier].TrackCaret;
                    Color foreColor = Color.FromArgb(tempLanguages[name].TagTypes[identifier].ForeColor);
                    tbxDescription.ForeColor = foreColor;
                    tbxIcon.ForeColor = foreColor;
                    btnForeColor.ForeColor = foreColor;
                    btnAcceptChanges.Enabled = false; // =\
                    btnAcceptChanges.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    btnAcceptChanges.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    
                    gbxRegex.Enabled = true;
                    foreach (string regex in tempLanguages[name].TagTypes[identifier].RegexPatterns)
                    {
                        char sep = regex[0];
                        string[] toks = regex.Split(sep);
                        GetFreeRegexLimiter(toks[1], toks[2]);
                        lbxRegex.Items.Add(toks[1] + REGEXSEP + toks[2]);
                    }
                    tbxRegexInput.Clear();
                    tbxRegexOutput.Clear();
                    cbxRegexCaseSensitive.Checked = false;
                }
                else
                {
                    btnTagTypeDel.Enabled = false;
                    btnTagTypeUp.Enabled = false;
                    btnTagTypeDown.Enabled = false;
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnTagTypeAddClick(object sender, EventArgs e)
        {
            try
            {
                string newIdentifier = cbxTagTypeAdd.Text;
                
                TagType newTagType = new TagType();
                newTagType.BuildIn = false;
                newTagType.Description = "New TagType [" + newIdentifier + "]";
                newTagType.Show = true;
                newTagType.TrackCaret = false;
                newTagType.ForeColor = Color.FromKnownColor(KnownColor.ControlText).ToArgb();
    
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                tempLanguages[name].TagTypes[newIdentifier] = newTagType;
                int index = lbxTagTypes.Items.Add(newIdentifier + "*");
                lbxTagTypes.SelectedIndex = index;
                GetFreeIdentifiers();
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnTagTypeDelClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                tempLanguages[name].TagTypes.Remove(identifier[0].ToString());
                lbxTagTypes.Items.Remove(identifier);
                GetFreeIdentifiers();
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnTagTypeUpClick(object sender, EventArgs e)
        {
            MoveTagTypeUpDown(true);
        }
        void BtnTagTypeDownClick(object sender, EventArgs e)
        {
            MoveTagTypeUpDown(false);
        }
        void MoveTagTypeUpDown(bool up)
        {
            try
            {
                int oldIndex = lbxTagTypes.SelectedIndex;
                int newIndex = oldIndex + (up ? -1 : 1);
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string movingIdentifier = (string)lbxTagTypes.SelectedItem;
                bool buildIn = (movingIdentifier.Length == 1);
                if (!buildIn) movingIdentifier = movingIdentifier[0].ToString();
                
                SerializableDictionary<string, TagType> newTagTypes = new SerializableDictionary<string, TagType>();
                int pos = 0;
                foreach (string identifier in tempLanguages[name].TagTypes.Keys)
                {
                    if (pos == newIndex)
                    {
                        if (!up) newTagTypes.Add(identifier, tempLanguages[name].TagTypes[identifier]);
                        newTagTypes.Add(movingIdentifier, tempLanguages[name].TagTypes[movingIdentifier]);
                        if (up) newTagTypes.Add(identifier, tempLanguages[name].TagTypes[identifier]);
                    }
                    else if (pos != oldIndex) newTagTypes.Add(identifier, tempLanguages[name].TagTypes[identifier]);
                    pos++;
                }
                tempLanguages[name].TagTypes = newTagTypes;
                
                lbxTagTypes.Items.RemoveAt(oldIndex);
                lbxTagTypes.Items.Insert(newIndex, movingIdentifier + (buildIn ? "" : "*"));
                lbxTagTypes.SelectedIndex = newIndex;
                
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        #endregion
        
        #region " Appearance "
        void ResetAppearanceGroup()
        {
            gbxAppearance.Enabled = false;
            tbxDescription.Clear();
            tbxDescription.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            pbxIcon.Image = null;
            tbxIcon.Clear();
            tbxIcon.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            btnForeColor.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            btnAcceptChanges.Enabled = false;
            btnAcceptChanges.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
            btnAcceptChanges.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
        }
        void TagTypeAppearanceChanged(object sender, EventArgs e)
        {
            btnAcceptChanges.Enabled = true;
            btnAcceptChanges.BackColor = Color.Red;
            btnAcceptChanges.ForeColor = Color.White;
        }
        void BtnIconClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = "*";
                ofd.Filter = "Image file (*.*)|*.*";
                ofd.Title = "Load image file";
                ofd.InitialDirectory = Settings.PluginSubFolderIcons;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pbxIcon.Image = new Bitmap(ofd.FileName);
                        tbxIcon.Text = Path.GetFileName(ofd.FileName);
                    }
                    catch (Exception ex) { Main.ErrorOut(ex); }
                }
            }
        }
        void BtnForeColorClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                identifier = identifier[0].ToString();
                using (ColorDialog cd = new ColorDialog())
                {
                    cd.AnyColor = true;
                    cd.FullOpen = true;
                    cd.Color = btnForeColor.ForeColor;
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        tbxDescription.ForeColor = cd.Color;
                        tbxIcon.ForeColor = cd.Color;
                        btnForeColor.ForeColor = cd.Color;
                        btnAcceptChanges.Enabled = true;
                        btnAcceptChanges.BackColor = Color.Red;
                        btnAcceptChanges.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnAcceptChangesClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                identifier = identifier[0].ToString();
                tempLanguages[name].TagTypes[identifier].Description = tbxDescription.Text;
                tempLanguages[name].TagTypes[identifier].Show = cbxShow.Checked;
                tempLanguages[name].TagTypes[identifier].TrackCaret = cbxTrackCaret.Checked;
                tempLanguages[name].TagTypes[identifier].IconFilename = tbxIcon.Text;
                tempLanguages[name].TagTypes[identifier].ForeColor = btnForeColor.ForeColor.ToArgb();
                btnAcceptChanges.Enabled = false;
                btnAcceptChanges.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                btnAcceptChanges.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        #endregion
        
        #region " Regex "
        const string regexLimiter = "/~!#%&;=@";
        char REGEXSEP;
        void GetFreeRegexLimiter(string input, string output)
        {
            for (int i = 0; i < regexLimiter.Length; i++)
            {
                REGEXSEP = regexLimiter[i];
                int counter;
                for (counter = input.Length; counter > 0; counter--)
                    if (input[counter - 1] == REGEXSEP) break;
                if (counter > 0) continue;
                for (counter = output.Length; counter > 0; counter--)
                    if (output[counter - 1] == REGEXSEP) break;
                if (counter > 0) continue;
                return;
            }
            throw new Exception(string.Format(
                "No free regex limiter char left for\nInput: {0}\nOutput: {1}", input, output));
        }
        void ResetRegexGroup()
        {
            gbxRegex.Enabled = false;
            lbxRegex.Items.Clear();
            tbxRegexInput.Clear();
            tbxRegexOutput.Clear();
            cbxRegexCaseSensitive.Checked = false;
            btnRegexAdd.Enabled = false;
            btnRegexDel.Enabled = false;
            btnRegexChange.Enabled = false;
            btnRegexChange.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
            btnRegexChange.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
        }
        void LbxRegexSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbxRegex.SelectedItem != null)
                {
                    string name = (string)lbxLanguages.SelectedItem;
                    if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                    string identifier = (string)lbxTagTypes.SelectedItem;
                    identifier = identifier[0].ToString();
                    int selIndex = lbxRegex.SelectedIndex;
                    string _regex = tempLanguages[name].TagTypes[identifier].RegexPatterns[selIndex];
                    char _sep = _regex[0];
                    string[] toks = _regex.Split(_sep);
                    tbxRegexInput.Text = toks[1];
                    tbxRegexOutput.Text = toks[2].Replace(CTagsExe.FieldSeparatorExtended, '|');
                    cbxRegexCaseSensitive.Checked = !toks[4].Contains("i");
                    btnRegexDel.Enabled = true;
                }
                else
                {
                    btnRegexDel.Enabled = false;
                }
                btnRegexChange.Enabled = false;
                btnRegexChange.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                btnRegexChange.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnRegexAddClick(object sender, EventArgs e)
        {
            try
            {
                string newRegexInput = tbxRegexInput.Text;
                string newRegexOutput = tbxRegexOutput.Text.Replace('|', CTagsExe.FieldSeparatorExtended);
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                identifier = identifier[0].ToString();
                GetFreeRegexLimiter(newRegexInput, newRegexOutput);
                string newLbxItem = newRegexInput + REGEXSEP + newRegexOutput;
                string newRegexEntry = REGEXSEP + newRegexInput + REGEXSEP + newRegexOutput + REGEXSEP +
                    identifier + REGEXSEP + "e" + (cbxRegexCaseSensitive.Checked ? "" : "i");
                if (tempLanguages[name].TagTypes[identifier].RegexPatterns.Contains(newRegexEntry))
                {
                    MessageBox.Show("Your pattern is already contained in this collection.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                tempLanguages[name].TagTypes[identifier].RegexPatterns.Add(newRegexEntry);
                lbxRegex.Items.Add(newLbxItem);
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void BtnRegexDelClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                identifier = identifier[0].ToString();
                int selLbxIndex = lbxRegex.SelectedIndex;
                tempLanguages[name].TagTypes[identifier].RegexPatterns.RemoveAt(selLbxIndex);
                lbxRegex.Items.RemoveAt(selLbxIndex);
                tbxRegexInput.Clear();
                tbxRegexOutput.Clear();
                cbxRegexCaseSensitive.Checked = false;
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        void TbxRegexTextChanged(object sender, EventArgs e)
        {
            btnRegexAdd.Enabled = ((tbxRegexInput.Text.Trim() != "") && (tbxRegexOutput.Text.Trim() != ""));
            if (lbxRegex.SelectedItem != null)
            {
                btnRegexChange.Enabled = true;
                btnRegexChange.BackColor = Color.Red;
                btnRegexChange.ForeColor = Color.White;
            }
            else
            {
                btnRegexChange.Enabled = false;
                btnRegexChange.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                btnRegexChange.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
        }
        void CbxRegexCaseSensitiveCheckedChanged(object sender, EventArgs e)
        {
            if (lbxRegex.SelectedItem != null)
            {
                btnRegexChange.Enabled = true;
                btnRegexChange.BackColor = Color.Red;
                btnRegexChange.ForeColor = Color.White;
            }
            else
            {
                btnRegexChange.Enabled = false;
                btnRegexChange.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                btnRegexChange.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
        }
        void BtnRegexChangeClick(object sender, EventArgs e)
        {
            try
            {
                string name = (string)lbxLanguages.SelectedItem;
                if (name.EndsWith("*")) name = name.Substring(0, name.Length - 1);
                string identifier = (string)lbxTagTypes.SelectedItem;
                identifier = identifier[0].ToString();
                int index = lbxRegex.SelectedIndex;
                string newRegexInput = tbxRegexInput.Text;
                string newRegexOutput = tbxRegexOutput.Text.Replace('|', CTagsExe.FieldSeparatorExtended);
                GetFreeRegexLimiter(newRegexInput, newRegexOutput);
                string newLbxItem = newRegexInput + REGEXSEP + newRegexOutput;
                string newRegexEntry = REGEXSEP + newRegexInput + REGEXSEP + newRegexOutput + REGEXSEP +
                    identifier + REGEXSEP + "e" + (cbxRegexCaseSensitive.Checked ? "" : "i");
                
                tempLanguages[name].TagTypes[identifier].RegexPatterns.RemoveAt(index);
                tempLanguages[name].TagTypes[identifier].RegexPatterns.Insert(index, newRegexEntry);
                lbxRegex.Items.RemoveAt(index);
                lbxRegex.Items.Insert(index, newLbxItem);
                lbxRegex.SelectedIndex = index;
                Changed = true;
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
        #endregion
        
        void BtnTryClick(object sender, EventArgs e)
        {
            try
            {
                if (Changed)
                {
                    Tried = true;
                    Settings.Languages = tempLanguages;
                    Settings.LoadTagTypeIcons(Main.frmMain.tvTags);
                    if (!Main.frmMain.Visible) Main.ShowFrmMain();
                    Main.frmMain.ResetTreeView(ChangedEx);
                }
            }
            catch (Exception ex) { Main.ErrorOut(ex); }
        }
    }
}
