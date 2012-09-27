
namespace SourceCookifier
{
    partial class frmChooseExtensions
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbExtensions = new System.Windows.Forms.CheckedListBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.lblAll = new System.Windows.Forms.LinkLabel();
            this.lblNone = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 174);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbExtensions
            // 
            this.lbExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbExtensions.CheckOnClick = true;
            this.lbExtensions.FormattingEnabled = true;
            this.lbExtensions.Location = new System.Drawing.Point(12, 28);
            this.lbExtensions.Name = "lbExtensions";
            this.lbExtensions.Size = new System.Drawing.Size(108, 109);
            this.lbExtensions.TabIndex = 4;
            this.lbExtensions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbExtensions_KeyPress);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(9, 9);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(40, 13);
            this.lblSelect.TabIndex = 5;
            this.lblSelect.Text = "Select:";
            // 
            // lblAll
            // 
            this.lblAll.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lblAll.AutoSize = true;
            this.lblAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAll.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblAll.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblAll.Location = new System.Drawing.Point(55, 9);
            this.lblAll.Name = "lblAll";
            this.lblAll.Size = new System.Drawing.Size(21, 13);
            this.lblAll.TabIndex = 2;
            this.lblAll.TabStop = true;
            this.lblAll.Text = "All";
            this.lblAll.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.lblAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAll_LinkClicked);
            // 
            // lblNone
            // 
            this.lblNone.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lblNone.AutoSize = true;
            this.lblNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNone.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblNone.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblNone.Location = new System.Drawing.Point(83, 9);
            this.lblNone.Name = "lblNone";
            this.lblNone.Size = new System.Drawing.Size(37, 13);
            this.lblNone.TabIndex = 3;
            this.lblNone.TabStop = true;
            this.lblNone.Text = "None";
            this.lblNone.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.lblNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblNone_LinkClicked);
            // 
            // frmChooseExtensions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(132, 212);
            this.Controls.Add(this.lblNone);
            this.Controls.Add(this.lblAll);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.lbExtensions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChooseExtensions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this.Shown += new System.EventHandler(this.FrmChooseExtensionsShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.CheckedListBox lbExtensions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.LinkLabel lblAll;
        private System.Windows.Forms.LinkLabel lblNone;
    }
}
