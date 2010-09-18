using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NppPluginNET
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
	}
}
