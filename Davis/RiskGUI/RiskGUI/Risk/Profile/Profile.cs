using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskGUI
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();

            usernameText.MaxLength = 15;
            passwordText.PasswordChar = '*';
            passwordText.MaxLength = 10;

        }

        private void backButton(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton(object sender, EventArgs e)
        {
            if (usernameText.Text == "")
            {
                MessageBox.Show("Please enter a valid username.");
                usernameText.Focus();
            }
            else if (passwordText.Text == "")
            {
                MessageBox.Show("Please enter a valid password.");
                passwordText.Focus();
            }

            string userName1, userPassword1;
            userName1 = usernameText.Text;
            userPassword1 = passwordText.Text;

            DataSet set = new DataSet("Profiles");

            foreach (DataRow row in set.Tables["Profiles"].Rows)
            {
                if (row["userName"].ToString() == "userName1")
                {
                    if (row["userPassword"].ToString() == "userPassword1")
                    {
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("You have entered a wrong username or the password.");
                }
            }
        }
    }
}
