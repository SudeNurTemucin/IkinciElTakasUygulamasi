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
    public partial class Form_UrunEkle : Form
    {
        public Form_UrunEkle()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open) baglanti.Open();

            try
            {
                NpgsqlCommand komut = new NpgsqlCommand("CALL sp_urunekle(@satici, @kategori, @durum, @baslik, @aciklama, @fiyat)", baglanti);

                komut.Parameters.AddWithValue("@satici", 1);
                komut.Parameters.AddWithValue("@kategori", int.Parse(cmbKategori.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@durum", int.Parse(cmbDurum.SelectedValue.ToString()));

                komut.Parameters.AddWithValue("@baslik", txtBaslik.Text);

                decimal fiyat = 0;
                decimal.TryParse(txtFiyat.Text, out fiyat);
                komut.Parameters.AddWithValue("@fiyat", fiyat);

                komut.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
                komut.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("Ürün, 'sp_urunekle' prosedürü kullanılarak başarıyla eklendi! ✅", "Başarılı");

                txtBaslik.Clear(); txtFiyat.Clear(); txtAciklama.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }

        void KutulariDoldur()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();

                NpgsqlDataAdapter daKat = new NpgsqlDataAdapter("SELECT * FROM public.kategoriler", baglanti);
                DataTable dtKat = new DataTable();
                daKat.Fill(dtKat);

                cmbKategori.DisplayMember = "KategoriAdi"; 
                cmbKategori.ValueMember = "KategoriID";
                cmbKategori.DataSource = dtKat;
                NpgsqlDataAdapter daDurum = new NpgsqlDataAdapter("SELECT * FROM public.urundurumlari", baglanti);
                DataTable dtDurum = new DataTable();
                daDurum.Fill(dtDurum);

                cmbDurum.DisplayMember = "DurumAdi";
                cmbDurum.ValueMember = "DurumID";
                cmbDurum.DataSource = dtDurum;
                string saticiSorgu = "SELECT \"KullaniciID\", \"Ad\" || ' ' || \"Soyad\" AS \"AdSoyad\" FROM public.bireyselKullanicilar";

                NpgsqlDataAdapter daSatici = new NpgsqlDataAdapter(saticiSorgu, baglanti);
                DataTable dtSatici = new DataTable();
                daSatici.Fill(dtSatici);

                cmbSatici.DisplayMember = "AdSoyad"; 
                cmbSatici.ValueMember = "KullaniciID";
                cmbSatici.DataSource = dtSatici;


                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void Form_UrunEkle_Load(object sender, EventArgs e)
        {
            KutulariDoldur();
        }
    }
}
 