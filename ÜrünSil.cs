using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IkinciElTakasUygulamasi
{
    public partial class Form_UrunSil : Form
    {
        public Form_UrunSil()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        void Listele()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();

                // ID sırasına göre gelsin ki karışmasın
                string sorgu = "SELECT * FROM public.urunler ORDER BY \"UrunID\" ASC";
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


        private void Form_UrunSil_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Başlığa tıklanmadıysa
            if (e.RowIndex >= 0)
            {
                // 0. sütun bizim ID'mizdir. O satırdaki değeri alıp kutuya yazıyoruz.
                txtUrunID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
         
            if (txtUrunID.Text == "")
            {
                MessageBox.Show("Lütfen listeden silinecek bir ürün seçin.", "Uyarı");
                return;
            }

            DialogResult onay = MessageBox.Show("Bu ürünü ve ona ait TÜM VERİLERİ (Teklif, Yorum vb.) silmek istediğinize emin misiniz?", "Kritik Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay == DialogResult.Yes)
            {
                try
                {
                    if (baglanti.State != ConnectionState.Open) baglanti.Open();

                    // 1. ADIM: ÖNCE BAĞLI OLAN YAN VERİLERİ TEMİZLE
                    // (Teklifler, Favoriler, Yorumlar, Resimler...)
                    // Not: Table isimlerini tırnaklı yazıyoruz ki hata vermesin.
                    string temizlikSorgusu = @"
                DELETE FROM public.teklifler WHERE ""HedefUrunID"" = @id;
                DELETE FROM public.favoriler WHERE ""UrunID"" = @id;
                DELETE FROM public.yorumlar WHERE ""UrunID"" = @id;
                DELETE FROM public.urunresimleri WHERE ""UrunID"" = @id;
                DELETE FROM public.mesajlar WHERE ""UrunID"" = @id;
                DELETE FROM public.""FiyatGecmisi"" WHERE ""UrunID"" = @id;
            ";

                    NpgsqlCommand cmdTemizle = new NpgsqlCommand(temizlikSorgusu, baglanti);
                    cmdTemizle.Parameters.AddWithValue("@id", int.Parse(txtUrunID.Text));
                    cmdTemizle.ExecuteNonQuery(); // Yan verileri sildik


                    // 2. ADIM: ARTIK ÜRÜNÜN KENDİSİNİ SİLEBİLİRİZ (Engel kalmadı)
                    NpgsqlCommand komut = new NpgsqlCommand("DELETE FROM public.urunler WHERE \"UrunID\" = @id", baglanti);
                    komut.Parameters.AddWithValue("@id", int.Parse(txtUrunID.Text));
                    komut.ExecuteNonQuery();

                    baglanti.Close();

                    MessageBox.Show("Ürün ve ilişkili tüm veriler başarıyla silindi! 🗑️", "İşlem Tamam");

                    // Listeyi Yenile
                    Listele();
                    txtUrunID.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme Hatası: " + ex.Message);
                    if (baglanti.State == ConnectionState.Open) baglanti.Close();
                }
            
        }
    }
            }
    }

