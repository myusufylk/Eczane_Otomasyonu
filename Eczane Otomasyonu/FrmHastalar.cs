using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;

namespace Eczane_Otomasyonu
{
    public partial class FrmHastalar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();

        public FrmHastalar()
        {
            InitializeComponent();

            // --- MANUEL BAĞLANTILAR (Garanti Yöntem) ---
            this.Load += FrmHastalar_Load;
            gridView1.RowClick += gridView1_RowClick;

            // Buton tıklama olaylarını da bağlayalım (Eğer tasarımda bağlı değilse)
            try
            {
                this.Controls.Find("btnKaydet", true)[0].Click += btnKaydet_Click;
                this.Controls.Find("btnSil", true)[0].Click += btnSil_Click;
                this.Controls.Find("btnGuncelle", true)[0].Click += btnGuncelle_Click;
            }
            catch { }
        }

        // --- FORM YÜKLENİRKEN ---
        private void FrmHastalar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();

            // Grid Ayarı
            gridView1.OptionsBehavior.Editable = false;

            // --- TC KİMLİK AYARI ---
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtTc.Properties.Mask.EditMask = @"\d{11}";
            txtTc.Properties.MaxLength = 11;

            // --- TELEFON AYARI ---
            txtTelefon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            txtTelefon.Properties.Mask.EditMask = "(999) 000-00-00";

            // --- GÜVENCE KUTUSU ---
            cmbGuvence.Properties.Items.Clear();
            cmbGuvence.Properties.Items.AddRange(new string[] { "SGK", "Bağkur", "Emekli Sandığı", "Özel Sigorta", "Yeşil Kart", "Yok" });
            cmbGuvence.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        // --- LİSTELEME (SADECE BENİM HASTALARIM) ---
        void listele()
        {
            DataTable dt = new DataTable();
            // SADECE GİRİŞ YAPAN KULLANICININ HASTALARINI ÇEK
            SqlDataAdapter da = new SqlDataAdapter("Select * From Hastalar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            txtTc.Text = "";
            txtAd.Text = "";
            txtSoyad.Text = "";
            txtTelefon.Text = "";
            cmbGuvence.Text = "";
            txtAdres.Text = "";
        }

        // --- GRID TIKLAMA (Verileri Kutulara Çekme) ---
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                // Veritabanı sütun isimlerine göre çekmek daha güvenlidir
                // (Index yerine ["KolonAdi"] kullanmak sütun sırası değişse bile çalışır)
                try
                {
                    txtTc.Text = dr["TC"].ToString();
                    txtAd.Text = dr["Ad"].ToString();
                    txtSoyad.Text = dr["Soyad"].ToString();
                    txtTelefon.Text = dr["Telefon"].ToString();
                    cmbGuvence.Text = dr["Guvence"].ToString();
                    txtAdres.Text = dr["Adres"].ToString();
                }
                catch { }
            }
        }

        // --- KAYDET (BENİM ID'MLE KAYDET) ---
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();

                // KullaniciID sütununa da ekleme yapıyoruz
                SqlCommand komut = new SqlCommand("insert into Hastalar (TC, Ad, Soyad, Telefon, Guvence, Adres, KullaniciID) values (@p1, @p2, @p3, @p4, @p5, @p6, @uid)", conn);

                komut.Parameters.AddWithValue("@p1", txtTc.Text);
                komut.Parameters.AddWithValue("@p2", txtAd.Text);
                komut.Parameters.AddWithValue("@p3", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p4", txtTelefon.Text);
                komut.Parameters.AddWithValue("@p5", cmbGuvence.Text);
                komut.Parameters.AddWithValue("@p6", txtAdres.Text);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id); // <-- BENİM ID'M

                komut.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Hasta Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
                temizle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // --- SİL (SADECE BENİM HASTAMI SİL) ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null)
            {
                MessageBox.Show("Lütfen silinecek hastayı seçiniz.");
                return;
            }

            string id = dr["ID"].ToString(); // ID kolonu

            if (MessageBox.Show("Hasta kaydını silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SqlConnection conn = bgl.baglanti();
                // Güvenlik: Sadece ID yetmez, KullaniciID de tutmalı (Başkası benim verimi silemesin)
                SqlCommand komut = new SqlCommand("Delete From Hastalar where ID=@p1 AND KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@p1", id);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                komut.ExecuteNonQuery();
                conn.Close();

                listele();
                temizle();
            }
        }

        // --- GÜNCELLE (SADECE BENİM HASTAMI GÜNCELLE) ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlConnection conn = bgl.baglanti();

            // Güncelleme şartına KullaniciID ekliyoruz
            SqlCommand komut = new SqlCommand("Update Hastalar set Ad=@p1, Soyad=@p2, Telefon=@p3, Guvence=@p4, Adres=@p5 where TC=@p6 AND KullaniciID=@uid", conn);

            komut.Parameters.AddWithValue("@p1", txtAd.Text);
            komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", txtTelefon.Text);
            komut.Parameters.AddWithValue("@p4", cmbGuvence.Text);
            komut.Parameters.AddWithValue("@p5", txtAdres.Text);
            komut.Parameters.AddWithValue("@p6", txtTc.Text);
            komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id); // <-- GÜVENLİK

            try
            {
                int sonuc = komut.ExecuteNonQuery();
                conn.Close();

                if (sonuc > 0)
                {
                    MessageBox.Show("Hasta Bilgileri Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bu TC Kimlik Numarasına ait kayıt bulunamadı (veya size ait değil)!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                listele();
                temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme Hatası: " + ex.Message);
                conn.Close();
            }
        }
    }
}