using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RiskMap
{
    class Program
    {
        [STAThread]
        static void Main() {
            // Initializing creator
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Creator());
        }
    }
}
