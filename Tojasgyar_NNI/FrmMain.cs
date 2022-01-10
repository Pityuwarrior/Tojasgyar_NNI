using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tojasgyar_NNI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bt_term_Click(object sender, EventArgs e)
        {
            FrmTermeles ter = new FrmTermeles();
            ter.ShowDialog();
        }

        private void bt_stat_Click(object sender, EventArgs e)
        {
            FrmStatisztika sta = new FrmStatisztika();
            sta.ShowDialog();
        }
    }
}
