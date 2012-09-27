using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceCookifier
{
    partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
            Icon = Properties.Resources.cookie_monster;
        }
        
        void BtnTvBackColorClick(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.AnyColor = true;
                cd.FullOpen = true;
                cd.Color = lblBackColor.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    lblBackColor.BackColor = cd.Color;
                }
            }
        }
    }
}
