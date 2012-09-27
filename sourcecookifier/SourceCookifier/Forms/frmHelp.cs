using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SourceCookifier
{
	partial class frmHelp : Form
	{
		public frmHelp()
		{
			InitializeComponent();
			Icon = Properties.Resources.cookie_monster;
			try { rtbHelp.Rtf = Properties.Resources.help_rtf; } catch { }
		}
		
		void RtbHelpLinkClicked(object sender, LinkClickedEventArgs e)
		{
			try { System.Diagnostics.Process.Start(e.LinkText); } catch { }
		}
	}
}
