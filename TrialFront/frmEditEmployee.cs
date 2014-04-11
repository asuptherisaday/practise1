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
    public partial class frmEditEmployee : Form
    {
        String eid;
        public frmEditEmployee(String eid)
        {
            Employee emp = new Employee().retriveEmployee(eid);
            InitializeComponent();
            this.eid = eid;
            this.Text = this.Text + " Eid: " + eid;
            txtFname.Text = emp.fname;
            txtMname.Text = emp.mname;
            txtLname.Text = emp.lname;
            dateDob.Value = emp.dob;
            dateDoj.Value = emp.doj;
            cboGender.SelectedItem = emp.gender == 'M' ? "Male" : "Female";
            txtMobile.Text = emp.mobile;
            txtEmail.Text = emp.email;
            txtBankAcc.Text = emp.bankaccount;
            txtPf.Text = emp.pf;
            txtAddress.Text = emp.address;
            String[] desTypes = new PayrollReportGenerator().getDesignations();
            foreach (String desType in desTypes)
                cboDes.Items.Add(desType);
            cboDes.SelectedItem = emp.designation;

        }

        private void frmEditEmployee_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new frmAdminView().Show();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int x = new Employee().updateEmployee(eid, txtFname.Text, txtMname.Text, txtLname.Text, cboGender.SelectedItem.ToString() == "Male" ? 'M' : 'F', dateDob.Value, dateDoj.Value, txtMobile.Text, txtEmail.Text, txtBankAcc.Text, cboDes.SelectedItem.ToString(), txtAddress.Text, txtPf.Text);
            switch (x)
            {
                case 100: MessageBox.Show("Changes Saved Successully");
                    break;
                default: MessageBox.Show("Error: " + x);
                    break;
            }
            new frmAdminView().Show();
            this.Close();
        }
    }
}
