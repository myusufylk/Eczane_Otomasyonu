
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors; 

namespace Eczane_Otomasyonu
{
    public partial class FrmHareketler : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();

        public FrmHareketler()
        {
            InitializeComponent();

            // FORM YÜKLENİNCE ÇALIŞACAK KODU GARANTİLİYORUZ
            this.Load += FrmHareketler_Load;

            // ÇİFT TIKLAMA OLAYINI GARANTİLİYORUZ
            gridView1.DoubleClick += gridView1_DoubleClick;
        }

        private void FrmHareketler_Load(object sender, EventArgs e)
        {
            listele(); // Geçmiş satışları grid'e getirir
            ilacListesiGetir(); // İlaçları doldur
            temizle(); // Kutuları temizle

            dateTarih.DateTime = DateTime.Now; // Tarih bugüne ayarlı

            // --- 1. İLAÇ SEÇİM KUTUSU AYARI ---
            lueIlac.Properties.NullText = "İlaç Seçiniz";
            lueIlac.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

            // --- 2. TC KİMLİK AYARI (PLACEHOLDER + MASKE) ---
            txtTc.Properties.NullValuePrompt = "TC Kimlik No";
            txtTc.Properties.NullValuePromptShowForEmptyValue = true;

            // Maske Ayarı (Sadece 11 Rakam)
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtTc.Properties.Mask.EditMask = @"\d{11}";

            // --- 3. DİĞER KUTULAR İÇİN PLACEHOLDERLAR ---
            txtAdet.Properties.NullValuePrompt = "Adet";
            txtAdet.Properties.NullValuePromptShowForEmptyValue = true;

            txtHastaAdi.Properties.NullValuePrompt = "Hasta Adı Soyadı";
            txtHastaAdi.Properties.NullValuePromptShowForEmptyValue = true;

            // Grid sadece okunabilir olsun
            gridView1.OptionsBehavior.Editable = false;
        }

        // --- LİSTELEME METODU ---
        void listele()
        {
            DataTable dt = new DataTable();

            // BURASI DEĞİŞTİ: "ORDER BY tarih DESC" (En yeni tarih en üstte)
            SqlDataAdapter da = new SqlDataAdapter("Select * From Hareketler ORDER BY tarih DESC", bgl.baglanti());

            da.Fill(dt);
            gridControl1.DataSource = dt;

            // Sütunları düzenle
            gridView1.PopulateColumns();
            gridView1.BestFitColumns();
        }

        // --- İLAÇLARI VERİTABANINDAN ÇEKME ---
        void ilacListesiGetir()
        {
            DataTable dt = new DataTable();
            // İlaçlar tablosundan İsim ve Fiyatı çekiyoruz
            SqlDataAdapter da = new SqlDataAdapter("Select ilacAdı, fiyat From Ilaclar", bgl.baglanti());
            da.Fill(dt);

            lueIlac.Properties.ValueMember = "ilacAdı";  // Arka planda tutulan değer
            lueIlac.Properties.DisplayMember = "ilacAdı"; // Ekranda görünen isim
            lueIlac.Properties.DataSource = dt;

            // Sütun ayarı (Sadece Ad ve Fiyat görünsün)
            lueIlac.Properties.PopulateColumns();
            if (lueIlac.Properties.Columns.Count > 0)
            {
                lueIlac.Properties.Columns["ilacAdı"].Caption = "İlaç Adı";
                lueIlac.Properties.Columns["fiyat"].Caption = "Fiyat";
            }
        }

        void temizle()
        {
            lueIlac.EditValue = null;
            txtTc.Text = "";
            txtHastaAdi.Text = "";
            txtAdet.Text = "";
            txtFiyat.Text = "";
            txtToplam.Text = "";
        }

        // --- İLAÇ SEÇİLİNCE FİYATI GETİRME ---
        private void lueIlac_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIlac.EditValue != null && gridView1.FocusedRowHandle < 0)
            {
                object val = lueIlac.Properties.GetDataSourceRowByKeyValue(lueIlac.EditValue);
                DataRowView row = val as DataRowView;
                if (row != null)
                {
                    txtFiyat.Text = row["fiyat"].ToString();
                }
            }
        }

        // --- TC GİRİLİNCE HASTA ADINI GETİRME ---
        private void txtTc_Leave(object sender, EventArgs e)
        {
            if (txtTc.Text.Length == 11)
            {
                SqlCommand komut = new SqlCommand("Select AdSoyad From Hastalar where TC=@p1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", txtTc.Text);
                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    txtHastaAdi.Text = dr[0].ToString(); // İsim bulundu
                }
                bgl.baglanti().Close();
            }
        }

        // --- TOPLAM HESAPLAMA ---
        private void txtAdet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal fiyat = decimal.Parse(txtFiyat.Text);
                int adet = int.Parse(txtAdet.Text);
                txtToplam.Text = (fiyat * adet).ToString();
            }
            catch { }
        }

        // --- ÇİFT TIKLAMA İLE VERİLERİ KUTULARA ÇEKME ---
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null)
            {
                lueIlac.EditValue = dr["ilacAdi"].ToString();
                txtTc.Text = dr["tcNo"].ToString();
                txtHastaAdi.Text = dr["hastaAdi"].ToString();
                txtAdet.Text = dr["adet"].ToString();
                txtToplam.Text = dr["toplamFiyat"].ToString();

                if (dr["tarih"] != DBNull.Value)
                    dateTarih.Text = dr["tarih"].ToString();

                // Birim Fiyatı Hesapla
                try
                {
                    decimal toplam = decimal.Parse(txtToplam.Text);
                    int adet = int.Parse(txtAdet.Text);
                    if (adet > 0)
                    {
                        txtFiyat.Text = (toplam / adet).ToString("N2");
                    }
                }
                catch { }
            }
        }

        // --- SATIŞ YAP BUTONU (STOK DÜŞMELİ VE GARANTİLİ) ---
        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            // 1. Alan Kontrolü
            if (lueIlac.EditValue == null || txtTc.Text == "" || txtHastaAdi.Text == "" || txtAdet.Text == "")
            {
                MessageBox.Show("Lütfen ilaç, adet ve hasta bilgilerini eksiksiz giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Stok Kontrolü
            int satilanAdet = int.Parse(txtAdet.Text);
            int mevcutStok = 0;
            string secilenIlacAdi = lueIlac.Text;

            SqlCommand komutStok = new SqlCommand("Select adet From Ilaclar where ilacAdı=@p1", bgl.baglanti());
            komutStok.Parameters.AddWithValue("@p1", secilenIlacAdi.Trim());
            SqlDataReader drStok = komutStok.ExecuteReader();
            if (drStok.Read())
            {
                mevcutStok = int.Parse(drStok[0].ToString());
            }
            bgl.baglanti().Close();

            if (satilanAdet > mevcutStok)
            {
                MessageBox.Show("Stok Yetersiz! Mevcut Stok: " + mevcutStok, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 3. Hasta Kontrolü / Kaydı
                SqlCommand komutKontrol = new SqlCommand("Select count(*) From Hastalar where TC=@p1", bgl.baglanti());
                komutKontrol.Parameters.AddWithValue("@p1", txtTc.Text);
                int hastaSayisi = Convert.ToInt32(komutKontrol.ExecuteScalar());
                bgl.baglanti().Close();

                if (hastaSayisi == 0)
                {
                    SqlCommand komutEkle = new SqlCommand("Insert into Hastalar (TC, AdSoyad) values (@p1, @p2)", bgl.baglanti());
                    komutEkle.Parameters.AddWithValue("@p1", txtTc.Text);
                    komutEkle.Parameters.AddWithValue("@p2", txtHastaAdi.Text);
                    komutEkle.ExecuteNonQuery();
                    bgl.baglanti().Close();
                }

                // 4. İLAÇ STOKTAN DÜŞME İŞLEMİ (UPDATE)
                SqlCommand komutStokDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2", bgl.baglanti());
                komutStokDus.Parameters.AddWithValue("@p1", satilanAdet);
                komutStokDus.Parameters.AddWithValue("@p2", secilenIlacAdi.Trim());
                komutStokDus.ExecuteNonQuery();
                bgl.baglanti().Close();

                // 5. SATIŞI GEÇMİŞE KAYDETME
                SqlCommand komutSatis = new SqlCommand("Insert into Hareketler (ilacAdi, adet, toplamFiyat, tarih, hastaAdi, tcNo) values (@p1,@p2,@p3,@p4,@p5,@p6)", bgl.baglanti());
                komutSatis.Parameters.AddWithValue("@p1", secilenIlacAdi.Trim());
                komutSatis.Parameters.AddWithValue("@p2", satilanAdet);
                komutSatis.Parameters.AddWithValue("@p3", decimal.Parse(txtToplam.Text));
                komutSatis.Parameters.AddWithValue("@p4", dateTarih.DateTime);
                komutSatis.Parameters.AddWithValue("@p5", txtHastaAdi.Text);
                komutSatis.Parameters.AddWithValue("@p6", txtTc.Text);

                komutSatis.ExecuteNonQuery();
                bgl.baglanti().Close();

                MessageBox.Show("Satış Yapıldı ve Stoktan Düşüldü.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 6. LİSTEYİ YENİLE
                listele();
                temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satış Hatası: " + ex.Message);
                bgl.baglanti().Close();
            }
        }
    }
}