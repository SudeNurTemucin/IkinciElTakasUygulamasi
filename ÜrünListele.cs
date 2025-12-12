using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace IkinciElTakasUygulamasi
{
    public partial class Form_UrunAra : Form
    {
        public Form_UrunAra()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        void UrunleriListele()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                string sorgu = "SELECT * FROM public.urunler";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void buttonListele_Click(object sender, EventArgs e)
        {
            txtAra.Clear();
            UrunleriListele();
            MessageBox.Show("Liste güncellendi.", "Bilgi");
        }

        private void Form_Listele_Load(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            // Arama kutusu boşsa uyarı ver
            if (txtAra.Text.Trim() == "")
            {
                MessageBox.Show("Lütfen aranacak kelimeyi girin.");
                UrunleriListele();
                return;
            }

            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                string sorgu = "SELECT * FROM public.urunler WHERE \"Baslik\" ILIKE @kelime";

                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@kelime", "%" + txtAra.Text + "%");

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama Hatası: " + ex.Message);
                baglanti.Close();
            }

        }
    }
}
