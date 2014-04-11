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
    public partial class frmReport : Form
    {
        String[] report;
        String eid;
        public frmReport(String eid,String[] report)
        {
            InitializeComponent();
            this.eid = eid;
            this.report = report;
            this.Text = this.Text +" EID: "+ eid;
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            lblDate.Text=report[0];
            lblBasic.Text=report[1];
            lblDA.Text=report[2];
            lblHRA.Text=report[3];
            lblMA.Text=report[4];
            lblOT.Text=report[5];
            lblTax.Text=report[6];
            lblPF.Text=report[7];
            lblTotal.Text=report[8];
            lblFD.Text="FD="+report[9];
            lblHD.Text="HD="+report[10];
            lblCL.Text="CL="+report[11];
            lblSL.Text="SL="+report[12];
            lblEL.Text="EL="+report[13];
            lblAB.Text="AB="+report[14];
        }
    }
}
