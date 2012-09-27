using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SourceCookifier
{
    partial class frmChooseExtensions : Form
    {
        public frmChooseExtensions()
        {
            InitializeComponent();
            Icon = Properties.Resources.cookie_monster;
        }
        
        [DllImport("user32")]
        extern static int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        void FrmChooseExtensionsShown(object sender, EventArgs e)
        {
            try
            {
                // Bring-topmost-and-focus-by-mouse-click-hack
                mouse_event(2, 0, 0, 0, 0); // MOUSEEVENT_LeftDown
                mouse_event(4, 0, 0, 0, 0); // MOUSEEVENT_LeftUp
            }
            catch { }
        }

        private void lblAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < lbExtensions.Items.Count; i++)
            {
                lbExtensions.SetItemChecked(i, true);
            }
        }

        private void lblNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < lbExtensions.Items.Count; i++)
            {
                lbExtensions.SetItemChecked(i, false);
            }
        }

        private void lbExtensions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btnOK.PerformClick();
        }
    }
}
