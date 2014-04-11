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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPass_Click(object sender, EventArgs e)
        {

        }

        private void txtUid_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUid.Text = "";
            txtPass.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUid.Text != "" && txtPass.Text != "")
            {
                int check = new PayrollValidator().validateLogin(txtUid.Text, txtPass.Text);
                if (check == 100)
                {
                    //MessageBox.Show("Welcome Admin", "Greeting");
                    new frmAdminView().Show();
                    this.Close();
                }
                else if (check == 200)
                {
                    //MessageBox.Show("Welcome User", "Greeting");
                    new frmUserView(txtUid.Text).Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Access Denied", "Warning");
            }
            else
                MessageBox.Show("Invalid User ID and/or Password", "Warning");
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
             btnLogin_Click(sender,e);
        }
  
        
    }
}
