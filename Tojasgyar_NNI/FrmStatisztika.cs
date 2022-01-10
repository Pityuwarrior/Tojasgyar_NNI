using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tojasgyar_NNI
{
    public partial class FrmStatisztika : Form
    {
        public string ConnectionString { private get; set; }
        public FrmStatisztika()
        {
            ConnectionString =
                "Server = (localdb)\\MSSQLLocalDB;" +
                "Database = TojasGyar;";
            InitializeComponent();
        }


        private void FillDGV()
        {
            DGV.Rows.Clear();
            using (var c = new SqlConnection(ConnectionString))
            {
                List<int> legtobb = new List<int>();
                List<double> osszsuly = new List<double>();
                int tojas = 0;
                double suly = 0;
                c.Open();
                var r = new SqlCommand(
                    "SELECT Termeles.Datum, Tojas.Szin, Termeles.Mennyiseg, Tojas.Suly " +
                    "FROM  Termeles, Tojas, Nyul " +
                    "WHERE Nyul.Id = Termeles.NyulId " +
                    "AND Termeles.NyulId = Tojas.Id " +
                    $"AND Nyul.Nev LIKE '%{cb_nev.Text}%' " +
                    $"AND MONTH(Datum) = {numericUpDown1.Value} " +
                    "ORDER BY Termeles.Datum ASC; ", c).ExecuteReader();
                while (r.Read())
                {
                    DGV.Rows.Add(
                    r.GetDateTime(0).ToString("yyyy-MM-dd"),
                    r[1],
                    r[2] + " db");
                    tojas = Convert.ToInt32(r[2]);
                    suly = (Convert.ToDouble(r[3]) * 1000);
                    legtobb.Add(tojas);
                    osszsuly.Add(suly);
                }

                try
                {
                    textBox1.Text = String.Join("", legtobb.Max());
                }
                catch 
                {
                    textBox1.Text = String.Join("","Nincs adat");
                }

                try
                {
                    textBox2.Text = String.Join("",legtobb.Sum()+" Kg");
                }
                catch
                {
                    textBox2.Text = String.Join("", "Nincs adat");
                }
            }
            
        }

        private void CB()
        {
            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();
                var r = new SqlCommand(
                     "SELECT Nyul.Nev " +
                     "FROM Nyul; ", c).ExecuteReader();
                while (r.Read())
                {
                    cb_nev.Items.Add(r[0]);
                }
                r.Close();
            }
        }

        private void FrmStatisztika_Load(object sender, EventArgs e)
        {
            FillDGV();
            CB();
        }

        private void cb_nev_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDGV();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FillDGV();
        }


    }
}
