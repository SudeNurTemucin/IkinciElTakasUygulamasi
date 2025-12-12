
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
    public partial class Form_AnaMenu : Form
    {
        public Form_AnaMenu()
        {
            InitializeComponent();
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
          Form_UrunAra fr = new Form_UrunAra ();
          fr.ShowDialog();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Form_UrunEkle fr = new Form_UrunEkle();
            fr.ShowDialog();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            Form_UrunSil fr = new Form_UrunSil();
            fr.ShowDialog();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            Form_UrunGüncelle fr = new Form_UrunGüncelle();
            fr.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Programdan çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                Application.Exit(); 
            }
        }

        private void btnIstatistik_Click(object sender, EventArgs e)
        {
            FrmIstatistik fr = new FrmIstatistik();
            fr.ShowDialog();
        }

        private void btnTeklif_Click(object sender, EventArgs e)
        {
            FrmTeklifler fr = new FrmTeklifler();
            fr.ShowDialog();
        }
    }
}

