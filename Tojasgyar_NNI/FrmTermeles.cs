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
    public partial class FrmTermeles : Form
    {
        public string ConnectionString { private get; set; }
        public FrmTermeles()
        {
            ConnectionString =
                "Server = (localdb)\\MSSQLLocalDB;" +
                "Database = TojasGyar;";
            InitializeComponent();
        }

        private void FrmTermeles_Load(object sender, EventArgs e)
        {
            FillDGV();
            CB();
        }
        private void FillDGV()
        {
            DGV.Rows.Clear();
            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();

                var r = new SqlCommand(
                    "SELECT Termeles.Datum, Tojas.Szin, Termeles.Mennyiseg, Nyul.Nev " +
                    "FROM  Termeles, Tojas, Nyul " +
                    "WHERE Nyul.Id = Termeles.NyulId " +
                    "AND Termeles.NyulId = Tojas.Id " +
                    "ORDER BY Termeles.Datum ASC; ", c).ExecuteReader();
                while (r.Read())
                {
                    DGV.Rows.Add(
                    r.GetDateTime(0).ToString("yyyy-MM-dd"),
                    r[1],
                    r[2] + " db",
                    r[3]);
                }
            }
        }

        private void CB()
        {
            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();
                var r = new SqlCommand(
                    "SELECT Tojas.Szin " +
                    "FROM Tojas; ", c).ExecuteReader();
                while (r.Read())
                {
                    cb_szin.Items.Add(r[0]);
                }
                r.Close();
                
                r = new SqlCommand(
                    "SELECT Nyul.Nev " +
                    "FROM Nyul; ", c).ExecuteReader();
                while (r.Read())
                {
                    cb_nev.Items.Add(r[0]);
                }
                //cb_szin.SelectedIndex = 0;
                //cb_nev.SelectedIndex = 0;
            }
        }

        private void bt_fel_Click(object sender, EventArgs e)
        {
            using (var c = new SqlConnection(ConnectionString))
            {
                bool van = true;
                c.Open();
                int nyid = 0;
                try
                {
                    var r = new SqlCommand(
                         "SELECT Nyul.Id " +
                         "FROM  Nyul " +
                         $"WHERE Nyul.Nev Like '{cb_nev.Text}';",
                         c).ExecuteReader();
                    r.Read();
                    nyid = r.GetInt32(0);
                    r.Close();
                }
                catch 
                {
                    MessageBox.Show("Válaszon ki egy színt!");
                    van = false;
                }
                int tojasid = 0;
                try
                {
                    var r2 = new SqlCommand(
                        "SELECT Tojas.Id " +
                        "FROM  Tojas " +
                        $"WHERE Tojas.Szin Like '{cb_szin.Text}';",
                        c).ExecuteReader();
                    r2.Read();
                    tojasid = r2.GetInt32(0);
                    r2.Close();
                }
                catch
                {
                    MessageBox.Show("Válaszon ki egy nevet!");
                    van = false;
                }
                if(van)
                {
                    new SqlCommand(
                     "INSERT INTO Termeles Values" +
                     $"('{dateTimePicker1.Value.ToString("yyyy-MM-dd")}', {nyid}, {tojasid}, {numericUpDown1.Value});", c).ExecuteNonQuery();
                     MessageBox.Show("Sikeres felvétel!");
                    FillDGV();
                }
            }

                
                
            
        }
    }
}
