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
    public partial class frmChangePassword : Form
    {
        String uid;
        public frmChangePassword(String uid)
        {
            InitializeComponent();
            this.uid = uid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new frmUserView(uid).Show();
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text != "" && txtConfirmPass.Text != "")
            {
                if (txtNewPass.Text == txtConfirmPass.Text)
                {
                    int x = new PayrollValidator().changePassword(uid, txtNewPass.Text);
                    if (x == 100)
                    {
                        MessageBox.Show("Password Changed Successfully","Success");
                        new frmUserView(uid).Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error: Invalid User ID","Warning");
                        new frmLogin();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Password does not match","Warning");
                }
            }
            else
            {
                MessageBox.Show("Invalid Password");
            }
        }
    }
}
