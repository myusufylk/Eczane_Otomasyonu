using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks; // Asenkron işlemler için
using System.Windows.Forms;
using System.Net.Http;       // API iletişimi için
using System.Text;           // Encoding işlemleri için
using Newtonsoft.Json;       // JSON işlemleri için

namespace Eczane_Otomasyonu
{
    public partial class FrmIlaclar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();
        public string resimDosyaYolu = "";
        bool islemYapiliyor = false;

        public FrmIlaclar()
        {
            InitializeComponent();

            // --- 🛠️ GÖRÜNÜM AYARLARI ---
            gridControl1.Dock = DockStyle.Fill;
            gridView1.OptionsView.ColumnAutoWidth = true;
            gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;

            // Olayları Bağla
            try
            {
                gridView1.RowClick -= gridView1_RowClick;
                gridView1.RowClick += gridView1_RowClick;

                Bagla("btnKaydet", btnKaydet_Click);
                Bagla("btnSil", btnSil_Click);
                Bagla("btnGuncelle", btnGuncelle_Click);
                Bagla("btnResimSec", btnResimSec_Click);

                // YENİ EKLENEN BUTON BAĞLAMASI
                Bagla("btnEtkenBul", btnEtkenBul_Click);
            }
            catch { }
        }

        void Bagla(string butonAdi, EventHandler olay)
        {
            var btn = this.Controls.Find(butonAdi, true);
            if (btn.Length > 0)
            {
                btn[0].Click -= olay;
                btn[0].Click += olay;
            }
        }

        private void FrmIlaclar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        // --- LİSTELEME METODU (Barkod ve EtkenMadde Eklendi) ---
        void listele()
        {
            try
            {
                DataTable dt = new DataTable();
                // SQL Sorgusuna 'Barkod' ve 'EtkenMadde' eklendi
                SqlDataAdapter da = new SqlDataAdapter("Select siraNo, Barkod, ilacKodu, ilacAdı, fiyat, adet, EtkenMadde, resim From Ilaclar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;

                gridView1.PopulateColumns();
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.BestFitColumns();

                // Başlıkları Güzelleştir
                if (gridView1.Columns["siraNo"] != null) gridView1.Columns["siraNo"].Caption = "SIRA NO";
                if (gridView1.Columns["Barkod"] != null) gridView1.Columns["Barkod"].Caption = "BARKOD"; // Yeni Başlık
                if (gridView1.Columns["ilacKodu"] != null) gridView1.Columns["ilacKodu"].Caption = "İLAÇ KODU";
                if (gridView1.Columns["ilacAdı"] != null) gridView1.Columns["ilacAdı"].Caption = "İLAÇ ADI";
                if (gridView1.Columns["fiyat"] != null) gridView1.Columns["fiyat"].Caption = "FİYAT";
                if (gridView1.Columns["adet"] != null) gridView1.Columns["adet"].Caption = "ADET";
                if (gridView1.Columns["EtkenMadde"] != null) gridView1.Columns["EtkenMadde"].Caption = "ETKEN MADDE";
            }
            catch { }
        }

        void AnaModuluGuncelle()
        {
            try { if (this.MdiParent is FrmAnaModul anaModul) anaModul.ListeleriYenile(); } catch { }
        }

        void temizle()
        {
            txtsiraNo.Text = "";
            txtKod.Text = "";
            txtAd.Text = "";
            txtFiyat.Text = "";
            txtAdet.Text = "";

            // Eğer tasarımda txtEtkenMadde varsa temizle
            var txtEtken = this.Controls.Find("txtEtkenMadde", true);
            if (txtEtken.Length > 0) txtEtken[0].Text = "";

            // Eğer tasarımda txtBarkod varsa temizle
            var txtBarkod = this.Controls.Find("txtBarkod", true);
            if (txtBarkod.Length > 0) txtBarkod[0].Text = "";

            picResim.Image = null;
            resimDosyaYolu = "";
        }

        void resimYukle(string yol)
        {
            if (!string.IsNullOrEmpty(yol) && File.Exists(yol)) { picResim.Image = Image.FromFile(yol); resimDosyaYolu = yol; }
            else { picResim.Image = null; resimDosyaYolu = ""; }
        }

        private void txtAd_Leave(object sender, EventArgs e) { if (txtAd.Text.Trim() != "" && txtKod.Text == "") VeriGetir("ilacAdı", txtAd.Text.Trim()); }

        void VeriGetir(string kolon, string deger)
        {
            try
            {
                SqlCommand komut = new SqlCommand($"Select * From Ilaclar where {kolon}=@p1 AND KullaniciID=@uid", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", deger);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if (kolon == "ilacAdı") txtKod.Text = dr["ilacKodu"].ToString();
                    if (kolon == "ilacKodu") txtAd.Text = dr["ilacAdı"].ToString();
                    txtFiyat.Text = dr["fiyat"].ToString();

                    // Veri getirirken etken maddeyi de getir
                    var txtEtken = this.Controls.Find("txtEtkenMadde", true);
                    if (txtEtken.Length > 0 && dr["EtkenMadde"] != DBNull.Value)
                        txtEtken[0].Text = dr["EtkenMadde"].ToString();

                    // Barkodu getir
                    var txtBarkod = this.Controls.Find("txtBarkod", true);
                    if (txtBarkod.Length > 0 && dr["Barkod"] != DBNull.Value)
                        txtBarkod[0].Text = dr["Barkod"].ToString();

                    resimYukle(dr["resim"].ToString());
                }
                bgl.baglanti().Close();
            }
            catch { }
        }

        // --- GRID SATIR TIKLAMA (Verileri Kutulara Taşıma) ---
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                DataRow dr = gridView1.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    txtsiraNo.Text = dr["siraNo"].ToString();
                    txtKod.Text = dr["ilacKodu"].ToString();
                    txtAd.Text = dr["ilacAdı"].ToString();
                    txtFiyat.Text = dr["fiyat"].ToString();
                    txtAdet.Text = dr["adet"].ToString();

                    // Etken Maddeyi Yükle
                    var txtEtken = this.Controls.Find("txtEtkenMadde", true);
                    if (txtEtken.Length > 0) txtEtken[0].Text = dr["EtkenMadde"].ToString();

                    // Barkodu Yükle
                    var txtBarkod = this.Controls.Find("txtBarkod", true);
                    if (txtBarkod.Length > 0) txtBarkod[0].Text = dr["Barkod"].ToString();

                    resimYukle(dr["resim"].ToString());
                }
            }
            catch { }
        }

        private void btnKaydet_Click(object sender, EventArgs e) { if (!islemYapiliyor) IslemYap("kaydet"); }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (islemYapiliyor) return;
            if (gridView1.GetDataRow(gridView1.FocusedRowHandle) == null) { MessageBox.Show("Seçim yapınız."); return; }
            IslemYap("guncelle");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) { MessageBox.Show("Silinecek satırı seçiniz."); return; }
            string id = dr["siraNo"].ToString();

            if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    SqlCommand komut = new SqlCommand("Delete From Ilaclar where siraNo=@p1 AND KullaniciID=@uid", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    komut.ExecuteNonQuery();
                    bgl.baglanti().Close();

                    listele();
                    temizle();
                    AnaModuluGuncelle();
                    MessageBox.Show("Silindi.");
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            if (dosya.ShowDialog() == DialogResult.OK) resimYukle(dosya.FileName);
        }

        // ============================================================
        //  YAPAY ZEKA İLE ETKEN MADDE BULMA METODLARI
        // ============================================================

        // 1. Butona Tıklanınca Çalışacak Kod
        private async void btnEtkenBul_Click(object sender, EventArgs e)
        {
            if (txtAd.Text == "")
            {
                MessageBox.Show("Lütfen önce ilaç adını yazın.", "Uyarı");
                return;
            }

            // Butonu bul ve kilitle
            var btn = (SimpleButton)sender;
            string eskiYazi = btn.Text;
            btn.Text = "Aranıyor...";
            btn.Enabled = false;

            try
            {
                string ilacAdi = txtAd.Text;
                string prompt = $"'{ilacAdi}' isimli ilacın etken maddesi nedir? " +
                                $"Cevap olarak SADECE etken maddenin ismini yaz. " +
                                $"Ekstra açıklama, nokta veya cümle kurma. " +
                                $"Örnek cevap: 'Parasetamol'";

                string etkenMadde = await GeminiyeSor(prompt);

                // Kutuyu bul ve yaz
                var txtEtken = this.Controls.Find("txtEtkenMadde", true);
                if (txtEtken.Length > 0) txtEtken[0].Text = etkenMadde;
                else MessageBox.Show("Etken Madde kutusu (txtEtkenMadde) bulunamadı! Tasarımda ekli mi?", "Hata");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                btn.Text = eskiYazi;
                btn.Enabled = true;
            }
        }

        // 2. Gemini API ile İletişim Kuran Metod
        private async Task<string> GeminiyeSor(string soru)
        {
            string apiKey = "AIzaSyDvqHcWCL6MFH5RfY4d3w_hH5nZ9cVIhbg"; // Senin API Anahtarın
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new { contents = new[] { new { parts = new[] { new { text = soru } } } } };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                        return jsonResponse.candidates[0].content.parts[0].text.ToString().Trim();
                    }
                }
                catch { }
                return "Bulunamadı";
            }
        }

        // ============================================================

        void IslemYap(string tur)
        {
            if (txtKod.Text.Trim() == "" || txtAd.Text.Trim() == "" || txtAdet.Text == "" || txtFiyat.Text.Trim() == "")
            {
                MessageBox.Show("Eksik bilgi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal fiyat = 0;
            int adet = 0;
            if (!decimal.TryParse(txtFiyat.Text.Replace(".", ","), out fiyat) || !int.TryParse(txtAdet.Text, out adet))
            {
                MessageBox.Show("Sayı hatası. Fiyat veya Adet yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Etken Madde kutusunun değerini al
            string etkenMaddeDegeri = "";
            var txtEtken = this.Controls.Find("txtEtkenMadde", true);
            if (txtEtken.Length > 0) etkenMaddeDegeri = txtEtken[0].Text;

            // Barkod kutusunun değerini al (Güvenli şekilde)
            string barkodDegeri = "";
            var txtBarkod = this.Controls.Find("txtBarkod", true);
            if (txtBarkod.Length > 0) barkodDegeri = txtBarkod[0].Text;

            islemYapiliyor = true;
            SqlConnection conn = bgl.baglanti();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                if (tur == "kaydet")
                {
                    SqlCommand komutKontrol = new SqlCommand("Select Count(*) From Ilaclar where ilacKodu=@p1 AND KullaniciID=@uid", conn);
                    komutKontrol.Parameters.AddWithValue("@p1", txtKod.Text.Trim());
                    komutKontrol.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    int sayi = Convert.ToInt32(komutKontrol.ExecuteScalar());

                    if (sayi > 0)
                    {
                        if (MessageBox.Show("İlaç zaten var. Eklensin mi?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // UPDATE işlemine Barkod ve EtkenMadde eklendi
                            SqlCommand komut = new SqlCommand("Update Ilaclar set adet=adet+@p1, fiyat=@p2, resim=@p3, ilacAdı=@p4, EtkenMadde=@p6, Barkod=@p8 where ilacKodu=@p5 AND KullaniciID=@uid", conn);
                            komut.Parameters.AddWithValue("@p1", adet);
                            komut.Parameters.AddWithValue("@p2", fiyat);
                            komut.Parameters.AddWithValue("@p3", resimDosyaYolu);
                            komut.Parameters.AddWithValue("@p4", txtAd.Text);
                            komut.Parameters.AddWithValue("@p5", txtKod.Text);
                            komut.Parameters.AddWithValue("@p6", etkenMaddeDegeri);
                            komut.Parameters.AddWithValue("@p8", barkodDegeri); // Barkod
                            komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Eklendi.");
                        }
                    }
                    else
                    {
                        // INSERT işlemine Barkod ve EtkenMadde eklendi
                        SqlCommand komut = new SqlCommand("insert into Ilaclar (ilacKodu, ilacAdı, fiyat, adet, resim, EtkenMadde, Barkod, KullaniciID) values (@p1, @p2, @p3, @p4, @p5, @p6, @p8, @uid)", conn);
                        komut.Parameters.AddWithValue("@p1", txtKod.Text);
                        komut.Parameters.AddWithValue("@p2", txtAd.Text);
                        komut.Parameters.AddWithValue("@p3", fiyat);
                        komut.Parameters.AddWithValue("@p4", adet);
                        komut.Parameters.AddWithValue("@p5", resimDosyaYolu);
                        komut.Parameters.AddWithValue("@p6", etkenMaddeDegeri);
                        komut.Parameters.AddWithValue("@p8", barkodDegeri); // Barkod
                        komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Kaydedildi.");
                    }
                }
                else if (tur == "guncelle")
                {
                    string id = txtsiraNo.Text;
                    // UPDATE işlemine Barkod ve EtkenMadde eklendi
                    SqlCommand komut = new SqlCommand("Update Ilaclar set ilacKodu=@p1, ilacAdı=@p2, fiyat=@p3, adet=@p4, resim=@p5, EtkenMadde=@p7, Barkod=@p8 where siraNo=@p6 AND KullaniciID=@uid", conn);
                    komut.Parameters.AddWithValue("@p1", txtKod.Text);
                    komut.Parameters.AddWithValue("@p2", txtAd.Text);
                    komut.Parameters.AddWithValue("@p3", fiyat);
                    komut.Parameters.AddWithValue("@p4", adet);
                    komut.Parameters.AddWithValue("@p5", resimDosyaYolu);
                    komut.Parameters.AddWithValue("@p6", id);
                    komut.Parameters.AddWithValue("@p7", etkenMaddeDegeri);
                    komut.Parameters.AddWithValue("@p8", barkodDegeri); // Barkod
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Güncellendi.");
                }

                if (conn.State == ConnectionState.Open) conn.Close();
                listele();
                temizle();
                AnaModuluGuncelle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { islemYapiliyor = false; if (conn.State == ConnectionState.Open) conn.Close(); }
        }

        public void OtomatikDoldur(string gelenAd, string gelenAdet)
        {
            temizle();
            txtAd.Text = gelenAd;
            txtAdet.Text = gelenAdet;
            txtAd_Leave(this, EventArgs.Empty);

            if (txtKod.Text == "")
            {
                txtKod.Focus();
                MessageBox.Show($"'{gelenAd}' stokta yok. Kod ve Fiyat giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}