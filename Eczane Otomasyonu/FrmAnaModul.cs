using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using DevExpress.XtraBars; // Ribbon olayları için gerekli

namespace Eczane_Otomasyonu
{
    public partial class FrmAnaModul : Form
    {
        public FrmAnaModul()
        {
            InitializeComponent();

            // 1. ListBox Çift Tıklama (Silme İçin)
            if (lstBildirimler != null)
            {
                lstBildirimler.DoubleClick += lstBildirimler_DoubleClick;
            }

            // 2. SAYFA DEĞİŞİMİNİ TAKİP ET (Gizleme/Gösterme İçin)
            this.MdiChildActivate += FrmAnaModul_MdiChildActivate;
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        string secilenResimYolu = ""; // Logo yolunu tutmak için

        // =============================================================
        //  FORM YÜKLENİRKEN
        // =============================================================
        private void FrmAnaModul_Load(object sender, EventArgs e)
        {
            IlaclariListele();

            // Başlangıçta tüm panelleri gizle
            PanelleriGizle();

            // Stok Kontrolü Başlasın
            StokKontrolu();

            // Dükkan Bilgilerini ve Logoyu Getir
            IsletmeBilgileriniGetir();
        }

        // =============================================================
        //  GÖRÜNÜM YÖNETİMİ (ANA EKRAN vs DİĞER SAYFALAR) 👁️
        // =============================================================

        private void FrmAnaModul_MdiChildActivate(object sender, EventArgs e)
        {
            // Eğer ActiveMdiChild 'null' ise, hiç açık sayfa yoktur -> Ana Ekradasın.
            bool anaEkrandaMiyiz = (this.ActiveMdiChild == null);

            // 1. BİLDİRİM BUTONU (Sol Alttaki)
            if (btnBildirim != null) btnBildirim.Visible = anaEkrandaMiyiz;

            // 2. AÇIK PANELLERİ KAPAT
            // Başka sayfaya geçince açık kalan chat, tahmin veya ayarları gizle
            if (!anaEkrandaMiyiz)
            {
                PanelleriGizle();
            }
        }

        // Yardımcı Metod: Tüm ekstra panelleri tek seferde kapatır
        void PanelleriGizle()
        {
            if (lstBildirimler != null) lstBildirimler.Visible = false;
            if (pnlTahmin != null) pnlTahmin.Visible = false;
            if (panelControl1 != null) panelControl1.Visible = false;
            if (pnlAyarlar != null) pnlAyarlar.Visible = false; // Ayarlar paneli
        }

        // =============================================================
        //  YENİ: AYARLAR PANELİ İŞLEMLERİ (LOGO & BİLGİLER) ⚙️
        // =============================================================

        // 1. VERİLERİ GETİR
        void IsletmeBilgileriniGetir()
        {
            try
            {
                if (txtIsletmeAdi == null) return; // Panel henüz eklenmediyse hata vermesin

                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("Select top 1 * From Isletme", conn);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    txtIsletmeAdi.Text = dr["Ad"].ToString();
                    txtIsletmeSahip.Text = dr["Sahip"].ToString();
                    txtIsletmeTel.Text = dr["Telefon"].ToString();
                    txtIsletmeAdres.Text = dr["Adres"].ToString();

                    // Logo varsa yükle
                    if (dr["LogoYolu"] != DBNull.Value)
                    {
                        secilenResimYolu = dr["LogoYolu"].ToString();
                        if (System.IO.File.Exists(secilenResimYolu))
                        {
                            peLogo.Image = Image.FromFile(secilenResimYolu);
                        }
                    }
                }
                conn.Close();
            }
            catch { }
        }

        // 2. LOGO SEÇME BUTONU
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
            dosya.Title = "Eczane Logosu Seç";

            if (dosya.ShowDialog() == DialogResult.OK)
            {
                secilenResimYolu = dosya.FileName;
                peLogo.Image = Image.FromFile(secilenResimYolu); // Ekranda göster
            }
        }

        // 3. KAYDET BUTONU
        private void btnAyarlarKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                // Bilgileri ve Logo Yolunu Güncelle
                SqlCommand komut = new SqlCommand("Update Isletme set Ad=@p1, Sahip=@p2, Telefon=@p3, Adres=@p4, LogoYolu=@p5", conn);
                komut.Parameters.AddWithValue("@p1", txtIsletmeAdi.Text);
                komut.Parameters.AddWithValue("@p2", txtIsletmeSahip.Text);
                komut.Parameters.AddWithValue("@p3", txtIsletmeTel.Text);
                komut.Parameters.AddWithValue("@p4", txtIsletmeAdres.Text);
                komut.Parameters.AddWithValue("@p5", secilenResimYolu); // Resim yolu
                komut.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Ayarlar ve Logo başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnlAyarlar.Visible = false; // İşlem bitince kapat
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // 4. KAPAT BUTONU
        private void btnAyarlarKapat_Click(object sender, EventArgs e)
        {
            if (pnlAyarlar != null) pnlAyarlar.Visible = false;
        }

        // =============================================================
        //  RIBBON BUTONLARI (YAPAY ZEKA, TAHMİN, AYARLAR) 🖱️
        // =============================================================

        // 1. AYARLAR BUTONU
        private void btnRibbonAyarlar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (pnlAyarlar.Visible)
                pnlAyarlar.Visible = false;
            else
            {
                PanelleriGizle(); // Diğerlerini kapat
                pnlAyarlar.Visible = true;
                pnlAyarlar.BringToFront();
                IsletmeBilgileriniGetir(); // Güncel veriyi çek
            }
        }

        // 2. YAPAY ZEKA BUTONU
        private void btnRibbonYapayZeka_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (panelControl1.Visible)
                panelControl1.Visible = false;
            else
            {
                PanelleriGizle();
                panelControl1.Visible = true;
                panelControl1.BringToFront();
            }
        }

        // 3. SATIŞ TAHMİNİ BUTONU
        private void btnRibbonTahmin_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (pnlTahmin.Visible)
                pnlTahmin.Visible = false;
            else
            {
                PanelleriGizle();
                pnlTahmin.Visible = true;
                pnlTahmin.BringToFront();
            }
        }

        // =============================================================
        //  BİLDİRİM SİSTEMİ (SOL ALT BUTON & STOK) 🔔
        // =============================================================

        private void btnBildirim_Click(object sender, EventArgs e)
        {
            if (lstBildirimler.Visible)
                lstBildirimler.Visible = false;
            else
            {
                PanelleriGizle();
                lstBildirimler.Visible = true;
                lstBildirimler.BringToFront();
            }
        }

        public void BildirimEkle(string mesaj)
        {
            if (lstBildirimler == null || string.IsNullOrEmpty(mesaj)) return;

            if (!lstBildirimler.Items.Contains(mesaj))
            {
                lstBildirimler.Items.Add(mesaj);
                BildirimButonunuGuncelle();
            }
        }

        public void StokKontrolu()
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("Select ilacAdı, adet From Ilaclar where adet < 5", conn);
                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    string ilac = dr["ilacAdı"].ToString();
                    string adet = dr["adet"].ToString();
                    string uyari = $"⚠️ KRİTİK STOK: {ilac} (Kalan: {adet})";
                    BildirimEkle(uyari);
                }
                conn.Close();
            }
            catch { }
        }

        void BildirimButonunuGuncelle()
        {
            if (btnBildirim == null) return;

            int sayi = lstBildirimler.Items.Count;
            if (sayi > 0)
            {
                btnBildirim.Text = $"BİLDİRİMLER ({sayi})";
                btnBildirim.Appearance.BackColor = Color.Red;
                btnBildirim.Appearance.ForeColor = Color.White;
            }
            else
            {
                btnBildirim.Text = "BİLDİRİMLER";
                btnBildirim.Appearance.BackColor = Color.Transparent;
                btnBildirim.Appearance.ForeColor = Color.Black;
            }
        }

        private void lstBildirimler_DoubleClick(object sender, EventArgs e)
        {
            if (lstBildirimler.SelectedItem != null)
            {
                lstBildirimler.Items.Remove(lstBildirimler.SelectedItem);
                BildirimButonunuGuncelle();
                if (lstBildirimler.Items.Count == 0) lstBildirimler.Visible = false;
            }
        }

        // =============================================================
        //  MEVCUT FONKSİYONLAR (HİÇBİRİ SİLİNMEDİ)
        // =============================================================

        void IlaclariListele()
        {
            try
            {
                cmbIlaclar.Properties.Items.Clear();
                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("SELECT ilacAdı FROM Ilaclar", conn);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read()) { cmbIlaclar.Properties.Items.Add(dr[0].ToString()); }
                conn.Close();
            }
            catch { }
        }

        private string MagazaDurumunuGetir()
        {
            string dukkanOzeti = "";
            SqlConnection conn = bgl.baglanti();
            try
            {
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Ilaclar", conn);
                string toplamCesit = cmd1.ExecuteScalar().ToString();
                SqlCommand cmd2 = new SqlCommand("SELECT ilacAdı, adet FROM Ilaclar WHERE adet < 20", conn);
                SqlDataReader dr = cmd2.ExecuteReader();
                string kritikIlaclar = "";
                while (dr.Read()) { kritikIlaclar += dr["ilacAdı"].ToString() + " (" + dr["adet"].ToString() + " adet), "; }
                dr.Close();
                dukkanOzeti = $"SİSTEM VERİLERİ:\n- Toplam Çeşit: {toplamCesit}\n- Kritik Stoklar: {kritikIlaclar}\n- Rol: Eczane asistanısın.";
            }
            catch (Exception ex) { dukkanOzeti = "Hata."; MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
            return dukkanOzeti;
        }

        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string mesaj = txtMesaj.Text.Trim();
            if (string.IsNullOrEmpty(mesaj)) return;

            MesajEkle(mesaj, true);
            txtMesaj.Text = "";
            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);

            try
            {
                string dukkanBilgisi = MagazaDurumunuGetir();
                string tamSoru = $"{dukkanBilgisi}\n\nKULLANICI SORUSU: {mesaj}";
                string cevap = await GeminiAsistani.Yorumla(tamSoru);
                MesajEkle(cevap, false);
            }
            catch (Exception ex) { MesajEkle("Hata: " + ex.Message, false); }

            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);
        }

        private void MesajEkle(string mesaj, bool kullaniciMi)
        {
            Panel pnlMesaj = new Panel();
            pnlMesaj.AutoSize = true;
            pnlMesaj.Padding = new Padding(10);
            pnlMesaj.Margin = new Padding(5);
            pnlMesaj.MaximumSize = new Size(flowSohbet.Width - 50, 0);

            Label lbl = new Label();
            lbl.Text = mesaj;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(flowSohbet.Width - 70, 0);
            lbl.Font = new Font("Segoe UI Semibold", 10);

            if (kullaniciMi) { pnlMesaj.BackColor = Color.FromArgb(220, 248, 198); lbl.ForeColor = Color.Black; }
            else { pnlMesaj.BackColor = Color.White; lbl.ForeColor = Color.Black; }

            pnlMesaj.Controls.Add(lbl);
            flowSohbet.Controls.Add(pnlMesaj);
        }

        private async void btnTahminHesapla_Click(object sender, EventArgs e)
        {
            if (cmbIlaclar.Text == "") { MessageBox.Show("İlaç seçin."); return; }
            string secilenIlac = cmbIlaclar.Text;
            lblTahminSonuc.Text = "Analiz ediliyor...";

            DateTime hedefTarih = dateTahminBitis.DateTime;
            string bugun = DateTime.Now.ToShortDateString();
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand cmdStok = new SqlCommand("SELECT adet FROM Ilaclar WHERE ilacAdı=@p1", conn);
                cmdStok.Parameters.AddWithValue("@p1", secilenIlac);
                string anlikStok = cmdStok.ExecuteScalar()?.ToString() ?? "0";

                SqlCommand cmdSatis = new SqlCommand("SELECT SUM(adet) FROM Hareketler WHERE ilacAdi=@p1", conn);
                cmdSatis.Parameters.AddWithValue("@p1", secilenIlac);
                string toplamSatis = cmdSatis.ExecuteScalar()?.ToString() ?? "0";
                conn.Close();

                string soru = $"Eczacı Satış Tahmini: Ürün {secilenIlac}, Stok {anlikStok}, Toplam Geçmiş Satış {toplamSatis}, Hedef Tarih {hedefTarih}. Tahmin et.";
                string cevap = await GeminiAsistani.Yorumla(soru);
                lblTahminSonuc.Text = "Tamamlandı.";
                MessageBox.Show(cevap, "Tahmin Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // Panel kapatma butonları (Tasarımda varsa)
        private void simpleButton1_Click(object sender, EventArgs e) { panelControl1.Visible = false; }
        private void btnPanelKapat_Click(object sender, EventArgs e) { pnlTahmin.Visible = false; }

        // =============================================================
        //  NAVİGASYON BUTONLARI (AYNI)
        // =============================================================
        private void btnIlaclar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmIlaclar"); }
        private void btnHastalar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmHastalar"); }
        private void btnSatislar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmHareketler"); }
        private void btnRaporlar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmRaporlar"); }
        private void btnCikis_ItemClick(object sender, ItemClickEventArgs e) { if (MessageBox.Show("Çıkış?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes) Application.Exit(); }

        void AcForm(string formAdi)
        {
            Form fr = Application.OpenForms[formAdi];
            if (fr == null)
            {
                Type t = Type.GetType("Eczane_Otomasyonu." + formAdi);
                if (t != null)
                {
                    fr = (Form)Activator.CreateInstance(t);
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            else { fr.BringToFront(); }
        }
    }
}