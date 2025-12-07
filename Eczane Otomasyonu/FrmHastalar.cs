using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid; // Grid işlemleri için şart

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

        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Hastalar", bgl.baglanti());
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
                // Veritabanı Sırası: 0:ID, 1:TC, 2:Ad, 3:Soyad, 4:Telefon, 5:Guvence, 6:Adres

                // ID kutusu kullanmıyoruz, direkt TC'den başlıyoruz
                txtTc.Text = dr[1].ToString();
                txtAd.Text = dr[2].ToString();      // Ad Ayrı
                txtSoyad.Text = dr[3].ToString();   // Soyad Ayrı
                txtTelefon.Text = dr[4].ToString();
                cmbGuvence.Text = dr[5].ToString();
                txtAdres.Text = dr[6].ToString();
            }
        }

        // --- KAYDET (Ad ve Soyad Ayrı) ---
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // SQL Sorgusu Ad ve Soyad sütunlarına göre güncellendi
                SqlCommand komut = new SqlCommand("insert into Hastalar (TC, Ad, Soyad, Telefon, Guvence, Adres) values (@p1, @p2, @p3, @p4, @p5, @p6)", bgl.baglanti());

                komut.Parameters.AddWithValue("@p1", txtTc.Text);
                komut.Parameters.AddWithValue("@p2", txtAd.Text);    // Ad
                komut.Parameters.AddWithValue("@p3", txtSoyad.Text); // Soyad
                komut.Parameters.AddWithValue("@p4", txtTelefon.Text);
                komut.Parameters.AddWithValue("@p5", cmbGuvence.Text);
                komut.Parameters.AddWithValue("@p6", txtAdres.Text);

                komut.ExecuteNonQuery();
                bgl.baglanti().Close();

                MessageBox.Show("Hasta Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
                temizle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // --- SİL (ID'ye göre silmek en güvenlisidir, kullanıcı görmese bile arka planda kullanırız) ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null)
            {
                MessageBox.Show("Lütfen silinecek hastayı seçiniz.");
                return;
            }

            // Grid'deki gizli ID değerini alıyoruz (Index 0)
            string id = dr[0].ToString();

            if (MessageBox.Show("Hasta kaydını silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("Delete From Hastalar where ID=@p1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                temizle();
            }
        }

        // --- GÜNCELLE (TC'ye Göre - ID Kullanmadan) ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            // Senin istediğin özel mantık: TC kimliğe göre bulup diğer bilgileri güncelliyor.

            SqlCommand komut = new SqlCommand("Update Hastalar set Ad=@p1, Soyad=@p2, Telefon=@p3, Guvence=@p4, Adres=@p5 where TC=@p6", bgl.baglanti());

            komut.Parameters.AddWithValue("@p1", txtAd.Text);       // Ad
            komut.Parameters.AddWithValue("@p2", txtSoyad.Text);    // Soyad
            komut.Parameters.AddWithValue("@p3", txtTelefon.Text);  // Telefon
            komut.Parameters.AddWithValue("@p4", cmbGuvence.Text);  // Guvence
            komut.Parameters.AddWithValue("@p5", txtAdres.Text);    // Adres

            // Koşul (Where)
            komut.Parameters.AddWithValue("@p6", txtTc.Text);       // TC Kimlik No

            try
            {
                int sonuc = komut.ExecuteNonQuery(); // Etkilenen satır sayısı
                bgl.baglanti().Close();

                if (sonuc > 0)
                {
                    MessageBox.Show("Hasta Bilgileri Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bu TC Kimlik Numarasına ait kayıt bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                listele();
                temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme Hatası: " + ex.Message);
                bgl.baglanti().Close();
            }
        }
    }
}