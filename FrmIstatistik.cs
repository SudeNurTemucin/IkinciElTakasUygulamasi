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
    public partial class FrmIstatistik : Form
    {
        public FrmIstatistik()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        private void FrmIstatistik_Load(object sender, EventArgs e)
        {
                try
                {
                    baglanti.Open();

                    // 1. TOPLAM ÜRÜN SAYISI (COUNT)
                    NpgsqlCommand komut1 = new NpgsqlCommand("SELECT COUNT(*) FROM public.urunler", baglanti);
                    object sonuc1 = komut1.ExecuteScalar();
                    lblToplamUrun.Text = sonuc1.ToString(); // Sonucu yazdır

                    // 2. TOPLAM STOK DEĞERİ (SUM)
                    NpgsqlCommand komut2 = new NpgsqlCommand("SELECT SUM(\"Fiyat\") FROM public.urunler", baglanti);
                    object sonuc2 = komut2.ExecuteScalar();

                    if (sonuc2 != DBNull.Value && sonuc2 != null)
                    {
                        lblToplamPara.Text = sonuc2.ToString() + " ₺";
                    }

                    // 3. EN PAHALI ÜRÜN (MAX)
                    string sorgu3 = "SELECT \"Baslik\" FROM public.urunler ORDER BY \"Fiyat\" DESC LIMIT 1";
                    NpgsqlCommand komut3 = new NpgsqlCommand(sorgu3, baglanti);
                    object sonuc3 = komut3.ExecuteScalar();

                    if (sonuc3 != null)
                    {
                        lblEnPahali.Text = sonuc3.ToString();
                    }
                    baglanti.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hesaplama Hatası: " + ex.Message);
                    if (baglanti.State == ConnectionState.Open) baglanti.Close();
                }
            }
    }
}
