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
    public partial class frmUserView : Form
    {
        String uid;
        public frmUserView(String uid)
        {
            InitializeComponent();
            this.uid = uid;
            lblEidV.Text = this.uid;
            Employee e = new Employee().retriveEmployee(uid);
            lblNameV.Text = e.fname + " " + e.mname + " " + e.lname;
            lblGenderV.Text = e.gender == 'M' ? "Male" : "Female";
            lblDobV.Text = e.dob.Day.ToString() + "/" + e.dob.Month.ToString() + "/" + e.dob.Year.ToString();
            lblDojV.Text = e.doj.Day.ToString() + "/" + e.doj.Month.ToString() + "/" + e.doj.Year.ToString();
            lblMobileV.Text = e.mobile;
            lblEmailV.Text = e.email;
            lblDesV.Text = e.designation;
            lblAddressV.Text = e.address;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Close();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            new frmChangePassword(uid).Show();
            this.Close();
        }

        private void btnSalaryStatus_Click(object sender, EventArgs e)
        {
            String[] report = new PayrollReportGenerator().showReport(DateTime.Today.Year.ToString(), (DateTime.Today.Month-1).ToString(), uid);
            if (report[0] == "NULL")
                MessageBox.Show("No report found.","WARNING");
            else
                new frmReport(uid, report).Show();
        }

        
    }
}
