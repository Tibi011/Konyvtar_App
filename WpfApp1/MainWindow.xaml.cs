using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString = "server=localhost;user=root;password =;database=konyvtar_db;";
        public MainWindow()
        {
            InitializeComponent();
            Betoltes();
        }

        private void Betoltes()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM KONYVEK";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Konyvtar.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba az adatbetöltés során!");
                }
            }

        }

        private void Hozzaad_Button_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "INSERT INTO konyvek (cim, szerzo, kiadev, mufaj) VALUES (@c, @h, @k, @m)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@c", txtCim.Text);
                cmd.Parameters.AddWithValue("@h", txtSzerzo.Text);
                cmd.Parameters.AddWithValue("@k", txtKiadev.Text);
                cmd.Parameters.AddWithValue("@m", txtMufaj.Text);
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }


        private void Modosit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Konyvtar.SelectedItem == null) return;
            DataRowView drv = (DataRowView)Konyvtar.SelectedItem;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "UPDATE konyvek SET cim=@c, szerzo=@h, kiadev=@k, mufaj=@m WHERE id=@id";


                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@c", txtCim.Text);
                cmd.Parameters.AddWithValue("@h", txtSzerzo.Text);
                cmd.Parameters.AddWithValue("@k", txtKiadev.Text);
                cmd.Parameters.AddWithValue("@m", txtMufaj.Text);
                cmd.Parameters.AddWithValue("@id", drv["id"]);
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }

        private void Konyvtar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Konyvtar.SelectedItem == null) return;
            DataRowView drv = (DataRowView)Konyvtar.SelectedItem;
            txtCim.Text = drv["cim"].ToString();
            txtSzerzo.Text = drv["szerzo"].ToString();
            txtKiadev.Text = drv["kiadev"].ToString();
            txtMufaj.Text = drv["mufaj"].ToString();
        }

        private void Torles_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Konyvtar.SelectedItem == null) return;
            DataRowView drv = (DataRowView)Konyvtar.SelectedItem;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "DELETE FROM konyvek WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", drv["id"]);
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }

        private void Frissites_Button_Click(object sender, RoutedEventArgs e)
        {
            txtCim.Clear();
            txtSzerzo.Clear();
            txtKiadev.Clear();
            txtMufaj.Clear();
            Betoltes();
        }


    }
}
