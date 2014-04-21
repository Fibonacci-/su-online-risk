using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SUOnlineRisk;

namespace SURiskGUI
{
    public partial class StateSelector : Form
    {
        public MainState state;
        public StateSelector()
        {
            InitializeComponent();
            this.comboBox1.Items.Add(MainState.Initialize);
            this.comboBox1.Items.Add(MainState.Fortify);
            this.comboBox1.Items.Add(MainState.Attack);
            this.comboBox1.Items.Add(MainState.AttackDone);
            this.comboBox1.Items.Add(MainState.Reinforce);
            this.comboBox1.Items.Add(MainState.Distribute);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            state = (MainState) comboBox1.SelectedItem;
        }
    }
}
