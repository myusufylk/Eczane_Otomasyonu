using System;
using System.Data;
using System.Drawing; // Resim için şart
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO; // Dosya işlemleri için şart

namespace Eczane_Otomasyonu
{
    public partial class FrmIlaclar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();

        // Resim yolunu hafızada tutmak için değişken
        public string resimDosyaYolu = "";

        public FrmIlaclar()
        {
            InitializeComponent();
            // Tıklama olayını garantiye alıyoruz
            gridView1.RowClick += gridView1_RowClick;
        }

        private void FrmIlaclar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select siraNo, ilacKodu, ilacAdı, fiyat, adet, resim From Ilaclar", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            txtsiraNo.Text = "";
            txtKod.Text = "";
            txtAd.Text = "";
            txtFiyat.Text = "";
            txtAdet.Text = "";
            picResim.Image = null;
            resimDosyaYolu = "";
        }

        // --- YARDIMCI METOT: RESMİ EKRANA BASAN FONKSİYON ---
        void resimYukle(string yol)
        {
            // Eğer yol boş değilse ve dosya gerçekten bilgisayarda varsa
            if (!string.IsNullOrEmpty(yol) && File.Exists(yol))
            {
                picResim.Image = Image.FromFile(yol);
                resimDosyaYolu = yol;
            }
            else
            {
                picResim.Image = null; // Resim yoksa kutuyu boşalt
                resimDosyaYolu = "";
            }
        }

        // --- 1. İLAÇ KODU GİRİLİNCE (Leave Olayı) ---
        private void txtKod_Leave(object sender, EventArgs e)
        {
            if (txtKod.Text.Trim() == "") return;

            SqlCommand komut = new SqlCommand("Select * From Ilaclar where ilacKodu=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", txtKod.Text.Trim());
            SqlDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {
                // Kod girildi, diğer bilgileri getir
                txtAd.Text = dr["ilacAdı"].ToString();
                txtFiyat.Text = dr["fiyat"].ToString();

                // Resmi getir
                resimYukle(dr["resim"].ToString());
            }
            bgl.baglanti().Close();
        }

        // --- 2. İLAÇ ADI GİRİLİNCE (Leave Olayı) - YENİ ÖZELLİK ---
        private void txtAd_Leave(object sender, EventArgs e)
        {
            if (txtAd.Text.Trim() == "") return;

            // Eğer kod zaten doluysa tekrar arama yapıp çakışmasın
            if (txtKod.Text != "") return;

            SqlCommand komut = new SqlCommand("Select * From Ilaclar where ilacAdı=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", txtAd.Text.Trim());
            SqlDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {
                // İsim girildi, kod ve diğerlerini getir
                txtKod.Text = dr["ilacKodu"].ToString();
                txtFiyat.Text = dr["fiyat"].ToString();

                // Resmi getir
                resimYukle(dr["resim"].ToString());
            }
            bgl.baglanti().Close();
        }

        // --- GRID TIKLAMA (LİSTEDEN SEÇME) ---
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                txtsiraNo.Text = dr["siraNo"].ToString();
                txtKod.Text = dr["ilacKodu"].ToString();
                txtAd.Text = dr["ilacAdı"].ToString();
                txtFiyat.Text = dr["fiyat"].ToString();
                txtAdet.Text = dr["adet"].ToString();

                // Resmi getir
                resimYukle(dr["resim"].ToString());
            }
        }

        // --- KAYDET BUTONU (STOK ARTTIRMA + GÜNCELLEME) ---
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtKod.Text.Trim() == "" || txtAd.Text.Trim() == "" || txtAdet.Text == "")
            {
                MessageBox.Show("Lütfen Kod, Ad ve Adet giriniz.");
                return;
            }

            try
            {
                // İlaç var mı kontrol et
                SqlCommand komutKontrol = new SqlCommand("Select Count(*) From Ilaclar where ilacKodu=@p1", bgl.baglanti());
                komutKontrol.Parameters.AddWithValue("@p1", txtKod.Text.Trim());
                int sayi = Convert.ToInt32(komutKontrol.ExecuteScalar());
                bgl.baglanti().Close();

                if (sayi > 0)
                {
                    // VARSA GÜNCELLE (STOK EKLE)
                    if (MessageBox.Show("Bu ilaç zaten var. Girilen adet stoga EKLENSİN Mİ?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlCommand komut = new SqlCommand("Update Ilaclar set adet=adet+@p1, fiyat=@p2, resim=@p3, ilacAdı=@p4 where ilacKodu=@p5", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", int.Parse(txtAdet.Text));
                        komut.Parameters.AddWithValue("@p2", decimal.Parse(txtFiyat.Text));
                        komut.Parameters.AddWithValue("@p3", resimDosyaYolu);
                        komut.Parameters.AddWithValue("@p4", txtAd.Text);
                        komut.Parameters.AddWithValue("@p5", txtKod.Text);

                        komut.ExecuteNonQuery();
                        bgl.baglanti().Close();
                        MessageBox.Show("Stok Eklendi.");
                    }
                }
                else
                {
                    // YOKSA YENİ EKLE
                    SqlCommand komut = new SqlCommand("insert into Ilaclar (ilacKodu, ilacAdı, fiyat, adet, resim) values (@p1, @p2, @p3, @p4, @p5)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", txtKod.Text);
                    komut.Parameters.AddWithValue("@p2", txtAd.Text);
                    komut.Parameters.AddWithValue("@p3", decimal.Parse(txtFiyat.Text));
                    komut.Parameters.AddWithValue("@p4", int.Parse(txtAdet.Text));
                    komut.Parameters.AddWithValue("@p5", resimDosyaYolu);

                    komut.ExecuteNonQuery();
                    bgl.baglanti().Close();
                    MessageBox.Show("Yeni İlaç Eklendi.");
                }
                listele();
                temizle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // --- RESİM SEÇ BUTONU ---
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;";
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                resimYukle(dosya.FileName);
            }
        }

        // --- GÜNCELLE ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) return;
            string id = dr[0].ToString();

            SqlCommand komut = new SqlCommand("Update Ilaclar set ilacKodu=@p1, ilacAdı=@p2, fiyat=@p3, adet=@p4, resim=@p5 where siraNo=@p6", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", txtKod.Text);
            komut.Parameters.AddWithValue("@p2", txtAd.Text);
            komut.Parameters.AddWithValue("@p3", decimal.Parse(txtFiyat.Text));
            komut.Parameters.AddWithValue("@p4", int.Parse(txtAdet.Text));

            // Güncel resim yolunu yaz
            komut.Parameters.AddWithValue("@p5", resimDosyaYolu);

            komut.Parameters.AddWithValue("@p6", id);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Güncellendi");
            listele();
            temizle();
        }

        // --- SİL ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) { MessageBox.Show("Satır seçiniz"); return; }
            string id = dr[0].ToString();

            if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("Delete From Ilaclar where siraNo=@p1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                temizle();
            }
        }
    }
}