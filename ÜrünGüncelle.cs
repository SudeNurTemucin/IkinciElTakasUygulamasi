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
    public partial class Form_UrunGüncelle : Form
    {
        public Form_UrunGüncelle()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        void Listele()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();

                string sorgu = "SELECT * FROM public.urunler ORDER BY \"UrunID\" ASC";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme Hatası: " + ex.Message);
            }
        }

        private void Form_UrunGüncelle_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = dataGridView1.Rows[e.RowIndex].Cells["UrunID"].Value.ToString();
                txtBaslik.Text = dataGridView1.Rows[e.RowIndex].Cells["Baslik"].Value.ToString();
                txtFiyat.Text = dataGridView1.Rows[e.RowIndex].Cells["Fiyat"].Value.ToString();
                txtAciklama.Text = dataGridView1.Rows[e.RowIndex].Cells["Aciklama"].Value.ToString();
            }

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Lütfen listeden güncellenecek bir ürün seçin.", "Uyarı");
                return;
            }

            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();

                string sorgu = "UPDATE public.urunler SET \"Baslik\"=@baslik, \"Fiyat\"=@fiyat, \"Aciklama\"=@aciklama WHERE \"UrunID\"=@id";

                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@baslik", txtBaslik.Text);

                decimal fiyat = 0;
                decimal.TryParse(txtFiyat.Text, out fiyat);
                komut.Parameters.AddWithValue("@fiyat", fiyat);

                komut.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
                komut.Parameters.AddWithValue("@id", int.Parse(txtID.Text));

                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Ürün Bilgileri Güncellendi! ✅", "Başarılı");
                Listele();
                txtID.Clear(); txtBaslik.Clear(); txtFiyat.Clear(); txtAciklama.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme Hatası: " + ex.Message);
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }
    }
}
