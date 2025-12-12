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
    public partial class FrmTeklifler : Form
    {
        public FrmTeklifler()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=IkinciElEsyaVeTakas;Username=postgres;Password=12993406");

        // LİSTELEME
        void Listele()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                string sorgu = "SELECT * FROM public.teklifler ORDER BY \"TeklifID\" DESC";

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

        private void FrmTeklifler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        // 1. ÜYE KAYIT BUTONU
        private void btnUyeKayit_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                string rasgeleEmail = txtAd.Text.ToLower() + new Random().Next(100, 999) + "@mail.com";

                NpgsqlCommand komut = new NpgsqlCommand("CALL sp_bireyseluyekayit(@p1, @p2, @p3, @p4, @p5)", baglanti);
                komut.Parameters.AddWithValue("@p1", rasgeleEmail);
                komut.Parameters.AddWithValue("@p2", "12345");
                komut.Parameters.AddWithValue("@p3", txtAd.Text);
                komut.Parameters.AddWithValue("@p4", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p5", txtTC.Text);
                komut.ExecuteNonQuery();

                MessageBox.Show($"Kayıt Başarılı!\nEmail: {rasgeleEmail}", "Tamam");
                txtAd.Clear(); txtSoyad.Clear(); txtTC.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { if (baglanti.State == ConnectionState.Open) baglanti.Close(); }
        }

        // 2. TEKLİF VER BUTONU
        private void btnTeklifVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("CALL sp_teklifver(@p1, @p2, @p3, @p4)", baglanti);
                komut.Parameters.AddWithValue("@p1", 2);
                komut.Parameters.AddWithValue("@p2", int.Parse(txtHedefUrunID.Text));
                komut.Parameters.AddWithValue("@p3", DBNull.Value);
                komut.Parameters.AddWithValue("@p4", decimal.Parse(txtTeklifTutar.Text));
                komut.ExecuteNonQuery();

                MessageBox.Show("Teklif Gönderildi!", "Başarılı");
                Listele();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { if (baglanti.State == ConnectionState.Open) baglanti.Close(); }
        }

        // 3. TEKLİF ONAYLA BUTONU
        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            try
            {
                int secilenID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("CALL sp_teklifonayla(@id)", baglanti);
                komut.Parameters.AddWithValue("@id", secilenID);
                komut.ExecuteNonQuery();

                MessageBox.Show("Teklif Onaylandı!", "Başarılı");
                Listele();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { if (baglanti.State == ConnectionState.Open) baglanti.Close(); }
        }
    }
}