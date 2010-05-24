using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NppPluginNET
{
	partial class frmHelp : Form
	{
		public frmHelp()
		{
			InitializeComponent();
			Icon = Properties.Resources.cookie_monster;
			try { rtbHelp.Rtf = Encoding.ASCII.GetString(Properties.Resources.help); } catch { }
		}
		
		void RtbHelpLinkClicked(object sender, LinkClickedEventArgs e)
		{
			try { System.Diagnostics.Process.Start(e.LinkText); } catch { }
		}
	}
}
