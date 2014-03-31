using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskMessageThread
{
    public partial class LoginDialog : Form
    {
        public LoginDialog()
        {
            InitializeComponent();
        }
        public string getLogin()
        {
            return this.textBox1.Text;
        }
        public string getPassword()
        {
            return this.textBox2.Text;
        }
    }
}
