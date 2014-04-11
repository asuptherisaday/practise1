using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrialFront
{
    public partial class frmStart : Form
    {
        int x = 30;
        int R, G, B;
        public frmStart()
        {
            InitializeComponent();
            lblTitle.ForeColor = Color.FromArgb(255, 0, 0, 255);
            R = lblTitle.ForeColor.R;
            G = lblTitle.ForeColor.G;
            B = lblTitle.ForeColor.B;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            x--;
            G += 6;
            B -= 6;
            lblTitle.ForeColor = Color.FromArgb(255, R, G, B);
            if (x < 1)
            {
                timer1.Stop();
                new frmLogin().Show();
                this.Hide();
            }
        }
    }
}
