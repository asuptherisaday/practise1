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
    public partial class frmAdminView : Form
    {
        String[] eids;
        String[] leavesTypes;
        String[] desTypes;
        public frmAdminView()
        {
            InitializeComponent();
            eids = new EmployeeList().retriveEIDs();
            foreach (String eid in eids)
            {
                cboEid.Items.Add(eid);
                cboEidV.Items.Add(eid);
                cboEidR.Items.Add(eid);
            }
            leavesTypes = new String[] { "FD", "HD", "CL", "SL", "EL", "AB","OT" };
            foreach (String leaveType in leavesTypes)
                cboAttendance.Items.Add(leaveType);
            desTypes = new PayrollReportGenerator().getDesignations();
            foreach (String desType in desTypes)
            {
                cboDesSelect.Items.Add(desType);
                cboDes.Items.Add(desType);
            }
        }

        private void txtLname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFname_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAdminView_Load(object sender, EventArgs e)
        {
            panelAddEmployee.Visible = false;
            panelMarkAttendance.Visible = false;
            panelViewEmployee.Visible = false;
            panelUpdatePayroll.Visible = false;
            panelReport.Visible = false;
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            panelAddEmployee.Visible = true;
            panelMarkAttendance.Visible=false;
            panelViewEmployee.Visible = false;
            panelUpdatePayroll.Visible = false;
            panelReport.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelAddEmployee.Visible = false;
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            int x;
            if(cboAttendance.SelectedItem.ToString()=="OT")
                x =new Attendance().markAttendance(cboEid.SelectedItem.ToString(), cboAttendance.SelectedItem.ToString(),int.Parse(txtOthours.Text), dateattendence.Value);
            else
                x = new Attendance().markAttendance(cboEid.SelectedItem.ToString(), cboAttendance.SelectedItem.ToString(), 0, dateattendence.Value);
            switch (x)
            {
                case 100: MessageBox.Show("Attendance Marked Successfully","NOTICE");
                    break;
                case 200: MessageBox.Show("CL(casual leave) not available","WARNING");
                    break;
                case 300: MessageBox.Show("EL(earned leave) not available","WARNING");
                    break;
                case 400: MessageBox.Show("SL(sick leave) not available","WARNING");
                    break;
                case 500: MessageBox.Show("Half day can not be marked","WARNING");
                    break;
                case 700: MessageBox.Show("Todays attendance already marked","NOTICE");
                    break;
                default: MessageBox.Show("Error:"+x);
                    break;
            }
        }

        private void btnCancelAttendance_Click(object sender, EventArgs e)
        {
            panelMarkAttendance.Visible = false;
        }

        private void cboAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAttendance.SelectedItem.ToString() == "OT") 
            {
                label4.Visible = true;
                txtOthours.Visible = true;
            }
            else
            {
                label4.Visible = false;
                txtOthours.Visible = false;                
            }
            btnMark.Enabled = true;

        }

        private void cboEid_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboAttendance.Enabled = true;
        }

        private void lblAttendance_Click(object sender, EventArgs e)
        {

        }

        private void lblEid_Click(object sender, EventArgs e)
        {

        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            panelMarkAttendance.Visible = true;
            panelAddEmployee.Visible = false;
            panelViewEmployee.Visible = false;
            panelUpdatePayroll.Visible = false;
            panelReport.Visible = false;
        }

        private void btnViewEmployee_Click(object sender, EventArgs e)
        {
            panelViewEmployee.Visible = true;
            panelAddEmployee.Visible = false;
            panelMarkAttendance.Visible = false;
            panelUpdatePayroll.Visible = false;
            panelReport.Visible = false;
        }

        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            panelUpdatePayroll.Visible = false;
        }

        private void btnUpdatePayroll_Click(object sender, EventArgs e)
        {
            panelUpdatePayroll.Visible = true;
            panelViewEmployee.Visible = false;
            panelAddEmployee.Visible = false;
            panelMarkAttendance.Visible = false;
            panelReport.Visible = false;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            panelReport.Visible = true;
            panelAddEmployee.Visible = false;
            panelMarkAttendance.Visible = false;
            panelViewEmployee.Visible = false;
            panelUpdatePayroll.Visible = false;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            new frmEditEmployee(cboEidV.SelectedItem.ToString()).Show();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFname.Text != "" && txtMname.Text != "" && txtLname.Text != "" && txtMobile.Text != "" && txtEmail.Text != "" && txtBankAcc.Text != "" && txtAddress.Text != "" && txtPf.Text != "")
            {
                int x = new Employee(txtFname.Text, txtMname.Text, txtLname.Text, cboGender.SelectedItem.ToString() == "Male" ? 'M' : 'F', dateDob.Value, dateDoj.Value, txtMobile.Text, txtEmail.Text, txtBankAcc.Text, cboDes.SelectedItem.ToString(), txtAddress.Text, txtPf.Text).addEmployee();
                MessageBox.Show("Employee Added Successfully. EID: " +x+ ". Password: "+x+". Please note your EID. We recommend you change your password immediately.","NOTICE");
                new frmAdminView().Show();
                this.Close();
            }
            else
                MessageBox.Show("All fields are mandatory","WARNING");
        }

        private void cboEidV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void cboEidV_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            String uid = cboEidV.SelectedItem.ToString();
            Employee ex = new Employee().retriveEmployee(uid);
            lblNameV.Text = ex.fname + " " + ex.mname + " " + ex.lname;
            lblGenderV.Text = ex.gender == 'M' ? "Male" : "Female";
            lblDobV.Text = ex.dob.Day.ToString() + "/" + ex.dob.Month.ToString() + "/" + ex.dob.Year.ToString();
            lblDojV.Text = ex.doj.Day.ToString() + "/" + ex.doj.Month.ToString() + "/" + ex.doj.Year.ToString();
            lblMobileV.Text = ex.mobile;
            lblEmailV.Text = ex.email;
            lblDesV.Text = ex.designation;
            lblAddressV.Text = ex.address;
            btnEdit.Enabled = true;
            btnRemove.Enabled = true;
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            int x=new Employee().removeEmployee(cboEidV.SelectedItem.ToString());
            switch (x)
            {
                case 100: MessageBox.Show("Employee Removed Successully","NOTICE");
                    break;
                default: MessageBox.Show("Error: " + x);
                    break;
            }
            new frmAdminView().Show();
            this.Close();
        }

        private void cboDesSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[] payrollscheme = new PayrollReportGenerator().getPayrollScheme(cboDesSelect.SelectedItem.ToString());
            txtBasic.Text=payrollscheme[0];
            txtDA.Text=payrollscheme[1];
            txtHRA.Text=payrollscheme[2];
            txtMA.Text=payrollscheme[3];
            txtOT.Text=payrollscheme[4];
            txtTax.Text=payrollscheme[5];
            btnUpdate.Enabled = true;
            btnCancelUpdate.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtBasic.Text == "" || txtDA.Text == "" || txtHRA.Text == "" || txtMA.Text == "" || txtOT.Text == "" || txtTax.Text == "")
                MessageBox.Show("All fields are mandatory.", "WARNING");
            else
            {
                int x = new PayrollReportGenerator().updatePayrollScheme(cboDesSelect.SelectedItem.ToString(), new String[] { txtBasic.Text, txtDA.Text, txtHRA.Text, txtMA.Text, txtOT.Text, txtTax.Text });
                switch (x)
                {
                    case 100: MessageBox.Show("Schema Updated Successully.","NOTICE");
                        break;
                    default: MessageBox.Show("Error: " + x);
                        break;
                }
                new frmAdminView().Show();
                this.Close();
            }
        }

        private void btnResetLeaves_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure ?", "CONFIRMATION", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                bool x = new EmployeeList().resetAnnualLeaves();
                switch (x)
                {
                    case true: MessageBox.Show("Leaves Resetted","NOTICE");
                        break;
                    case false: MessageBox.Show("Error");
                        break;
                }
            }
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            String[] report =new PayrollReportGenerator().showReport(cboYearR.SelectedItem.ToString(),cboMonthR.SelectedItem.ToString(),cboEidR.SelectedItem.ToString());
            if (report[0] == "NULL")
                MessageBox.Show("No report found. Please generate report first.","WARNING");
            else
                new frmReport(cboEidR.SelectedItem.ToString(),report).Show();
        }

        private void btnNewReport_Click(object sender, EventArgs e)
        {
            DateTime date = new DateTime(int.Parse(cboYearR.SelectedItem.ToString()),int.Parse(cboMonthR.SelectedItem.ToString()),DateTime.DaysInMonth(int.Parse(cboYearR.SelectedItem.ToString()),int.Parse(cboMonthR.SelectedItem.ToString())));
            int x = new PayrollReportGenerator().generateReport(cboEidR.SelectedItem.ToString(), date);
            switch (x)
            {
                case 100: MessageBox.Show("Report Generated Successfully.","NOTICE");
                    break;
                default: MessageBox.Show("No records found for report generation.","WARNING");
                    break;
            }
        }

        private void cboEidR_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Enabled = true;
            cboYearR.Enabled = true;

        }

        private void cboYearR_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Enabled=true;
            cboMonthR.Enabled = true;
        }

        private void cboMonthR_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNewReport.Enabled = true;
            btnShowReport.Enabled = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtFname_Enter(object sender, EventArgs e)
        {
            txtFname.Text = "";
        }

        private void txtMname_Enter(object sender, EventArgs e)
        {
            txtMname.Text = "";
        }

        private void txtLname_Enter(object sender, EventArgs e)
        {
            txtLname.Text = "";
        }
    }
}
