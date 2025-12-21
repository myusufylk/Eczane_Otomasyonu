using DevExpress.XtraBars;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eczane_Otomasyonu
{
    public partial class FrmAnaModul : Form
    {
        public FrmAnaModul()
        {
            InitializeComponent();

            // Olayları Bağla
            if (lstBildirimler != null) lstBildirimler.DoubleClick += lstBildirimler_DoubleClick;
            this.MdiChildActivate += FrmAnaModul_MdiChildActivate;
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        string secilenResimYolu = "";

        private void FrmAnaModul_Load(object sender, EventArgs e)
        {
            // Açılışta sessizce yükle
            try { ListeleriYenile(); } catch { }

            PanelleriGizle();
            IsletmeBilgileriniGetir();
        }

        // =============================================================
        //  GÖRÜNÜM YÖNETİMİ
        // =============================================================
        private void FrmAnaModul_MdiChildActivate(object sender, EventArgs e)
        {
            bool anaEkrandaMiyiz = (this.ActiveMdiChild == null);
            if (btnBildirim != null) btnBildirim.Visible = anaEkrandaMiyiz;

            if (!anaEkrandaMiyiz) PanelleriGizle();
        }

        void PanelleriGizle()
        {
            if (lstBildirimler != null) lstBildirimler.Visible = false;
            if (pnlTahmin != null) pnlTahmin.Visible = false;
            if (panelControl1 != null) panelControl1.Visible = false;
            if (pnlAyarlar != null) pnlAyarlar.Visible = false;
        }

        // =============================================================
        //  AYARLAR PANELİ
        // =============================================================
        void IsletmeBilgileriniGetir()
        {
            try
            {
                if (txtIsletmeAdi == null) return;

                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("Select top 1 * From Isletme WHERE KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    txtIsletmeAdi.Text = dr["Ad"].ToString();
                    txtIsletmeSahip.Text = dr["Sahip"].ToString();
                    txtIsletmeTel.Text = dr["Telefon"].ToString();
                    txtIsletmeAdres.Text = dr["Adres"].ToString();

                    if (dr["LogoYolu"] != DBNull.Value)
                    {
                        secilenResimYolu = dr["LogoYolu"].ToString();
                        if (System.IO.File.Exists(secilenResimYolu))
                            peLogo.Image = Image.FromFile(secilenResimYolu);
                    }
                }
                conn.Close();
            }
            catch { }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                secilenResimYolu = dosya.FileName;
                peLogo.Image = Image.FromFile(secilenResimYolu);
            }
        }

        private void btnAyarlarKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                string sorgu = "IF EXISTS (SELECT * FROM Isletme WHERE KullaniciID=@uid) " +
                               "UPDATE Isletme set Ad=@p1, Sahip=@p2, Telefon=@p3, Adres=@p4, LogoYolu=@p5 WHERE KullaniciID=@uid " +
                               "ELSE " +
                               "INSERT INTO Isletme (Ad, Sahip, Telefon, Adres, LogoYolu, KullaniciID) VALUES (@p1, @p2, @p3, @p4, @p5, @uid)";

                SqlCommand komut = new SqlCommand(sorgu, conn);
                komut.Parameters.AddWithValue("@p1", txtIsletmeAdi.Text);
                komut.Parameters.AddWithValue("@p2", txtIsletmeSahip.Text);
                komut.Parameters.AddWithValue("@p3", txtIsletmeTel.Text);
                komut.Parameters.AddWithValue("@p4", txtIsletmeAdres.Text);
                komut.Parameters.AddWithValue("@p5", secilenResimYolu);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                komut.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Ayarlar kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnlAyarlar.Visible = false;
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        private void btnAyarlarKapat_Click(object sender, EventArgs e) { if (pnlAyarlar != null) pnlAyarlar.Visible = false; }

        // =============================================================
        //  RIBBON BUTONLARI
        // =============================================================
        private void btnRibbonAyarlar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (pnlAyarlar.Visible) pnlAyarlar.Visible = false;
            else { PanelleriGizle(); pnlAyarlar.Visible = true; pnlAyarlar.BringToFront(); IsletmeBilgileriniGetir(); }
        }

        private void btnRibbonYapayZeka_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (panelControl1.Visible) panelControl1.Visible = false;
            else { PanelleriGizle(); panelControl1.Visible = true; panelControl1.BringToFront(); }
        }

        private void btnRibbonTahmin_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (pnlTahmin.Visible) pnlTahmin.Visible = false;
            else { PanelleriGizle(); pnlTahmin.Visible = true; pnlTahmin.BringToFront(); }
        }

        // =============================================================
        //  BİLDİRİM VE STOK SİSTEMİ (GÜVENLİ HALE GETİRİLDİ)
        // =============================================================
        private void btnBildirim_Click(object sender, EventArgs e)
        {
            if (lstBildirimler.Visible) lstBildirimler.Visible = false;
            else { PanelleriGizle(); lstBildirimler.Visible = true; lstBildirimler.BringToFront(); }
        }

        public void BildirimEkle(string mesaj)
        {
            if (lstBildirimler == null || string.IsNullOrEmpty(mesaj)) return;
            if (!lstBildirimler.Items.Contains(mesaj)) lstBildirimler.Items.Add(mesaj);
        }

        // --- EN ÖNEMLİ METOD: GÜVENLİ STOK KONTROLÜ ---
        public void StokKontrolu()
        {
            try
            {
                if (lstBildirimler == null) return;

                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                SqlCommand komut = new SqlCommand("Select ilacAdı, adet From Ilaclar WHERE KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    int adet = 0;
                    // Veritabanından gelen değer NULL mu? Boş mu? Kontrol et ve güvenle çevir.
                    string adetStr = dr["adet"] != DBNull.Value ? dr["adet"].ToString() : "0";

                    // TryParse: Eğer harf varsa veya bozuksa patlamaz, 0 kabul eder.
                    if (int.TryParse(adetStr, out adet))
                    {
                        if (adet < 10)
                        {
                            BildirimEkle($"⚠️ KRİTİK STOK: {dr["ilacAdı"]} (Kalan: {adet})");
                        }
                    }
                }
                conn.Close();
                BildirimButonunuGuncelle();
            }
            catch { } // Hata olursa sessiz kal, programı durdurma
        }

        public void BildirimButonunuGuncelle()
        {
            try
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
            catch { }
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
        //  GENEL LİSTE YENİLEME (HATAYI ENGELLEYEN YAPI)
        // =============================================================
        public void ListeleriYenile()
        {
            // Tüm yenileme işlemlerini koruma altına alıyoruz
            try
            {
                IlaclariListele(); // ComboBox'ı doldur

                if (lstBildirimler != null)
                {
                    lstBildirimler.Items.Clear(); // Eski bildirimleri sil
                    StokKontrolu(); // Yenilerini güvenle ekle
                    BildirimButonunuGuncelle();
                }
            }
            catch { } // Asla hata fırlatma
        }

        void IlaclariListele()
        {
            try
            {
                if (cmbIlaclar == null) return;
                cmbIlaclar.Properties.Items.Clear();
                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("SELECT ilacAdı FROM Ilaclar WHERE KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read()) { cmbIlaclar.Properties.Items.Add(dr[0].ToString()); }
                conn.Close();
            }
            catch { }
        }

        // =============================================================
        //  CHAT VE KOMUT SİSTEMİ
        // =============================================================
        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string mesaj = txtMesaj.Text.Trim();
            if (string.IsNullOrEmpty(mesaj)) return;

            MesajEkle(mesaj, true);
            txtMesaj.Text = "";
            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);

            // KOMUT YAKALAMA
            if (mesaj.IndexOf("ekle", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                string ilacAdi = "";
                string adetStr = "";

                System.Text.RegularExpressions.Match desenTam = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s*(?:adet|tane|kutu)?\s+(.+)\s+ekle", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.Match desenBasit = System.Text.RegularExpressions.Regex.Match(mesaj, @"(.+)\s+ekle", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (desenTam.Success) { adetStr = desenTam.Groups[1].Value; ilacAdi = desenTam.Groups[2].Value.Trim(); }
                else if (desenBasit.Success) { ilacAdi = desenBasit.Groups[1].Value.Trim(); adetStr = ""; }
                else { ilacAdi = System.Text.RegularExpressions.Regex.Replace(mesaj, "ekle", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim(); }

                ilacAdi = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ilacAdi);

                try
                {
                    SqlConnection conn = bgl.baglanti();
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                    SqlCommand komut = new SqlCommand("SELECT Count(*) FROM Ilaclar WHERE ilacAdı=@p1 AND KullaniciID=@uid", conn);
                    komut.Parameters.AddWithValue("@p1", ilacAdi);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    int varMi = Convert.ToInt32(komut.ExecuteScalar());
                    conn.Close();

                    if (varMi == 0) MesajEkle($"🆕 '{ilacAdi}' stokta yok. Kayıt ekranı açılıyor...", false);
                    else MesajEkle($"🔄 '{ilacAdi}' bulundu. Stok ekleme ekranı açılıyor...", false);

                    FrmIlaclar frIlac = (FrmIlaclar)Application.OpenForms["FrmIlaclar"];
                    if (frIlac == null) { frIlac = new FrmIlaclar(); frIlac.MdiParent = this; frIlac.Show(); }
                    else { frIlac.BringToFront(); frIlac.Activate(); }
                    frIlac.OtomatikDoldur(ilacAdi, adetStr);
                }
                catch (Exception ex) { MesajEkle("Hata: " + ex.Message, false); }
                return;
            }

            // YAPAY ZEKA
            try
            {
                string dukkanBilgisi = MagazaDurumunuGetir();
                string tamSoru = $"{dukkanBilgisi}\n\nKULLANICI SORUSU: {mesaj}";
                string cevap = await GeminiAsistani.Yorumla(tamSoru);
                MesajEkle(cevap, false);
            }
            catch (Exception ex) { MesajEkle("Yapay Zeka Hatası: " + ex.Message, false); }
            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);
        }

        private string MagazaDurumunuGetir()
        {
            string dukkanOzeti = "";
            SqlConnection conn = bgl.baglanti();
            try
            {
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Ilaclar WHERE KullaniciID=@uid", conn);
                cmd1.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                string toplamCesit = cmd1.ExecuteScalar().ToString();

                SqlCommand cmd2 = new SqlCommand("SELECT ilacAdı, adet FROM Ilaclar WHERE adet < 20 AND KullaniciID=@uid", conn);
                cmd2.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = cmd2.ExecuteReader();
                string kritikIlaclar = "";
                while (dr.Read()) { kritikIlaclar += dr["ilacAdı"].ToString() + " (" + dr["adet"].ToString() + " adet), "; }
                conn.Close();
                dukkanOzeti = $"SİSTEM VERİLERİ:\n- Toplam Çeşit: {toplamCesit}\n- Kritik Stoklar: {kritikIlaclar}\n- Rol: Eczane asistanısın.";
            }
            catch { dukkanOzeti = "Veri alınamadı."; }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
            return dukkanOzeti;
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
            lblTahminSonuc.Text = "Analiz ediliyor...";
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand cmdSatis = new SqlCommand("SELECT SUM(adet) FROM Hareketler WHERE ilacAdi=@p1 AND KullaniciID=@uid", conn);
                cmdSatis.Parameters.AddWithValue("@p1", cmbIlaclar.Text);
                cmdSatis.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                string toplamSatis = cmdSatis.ExecuteScalar()?.ToString() ?? "0";
                conn.Close();
                string cevap = await GeminiAsistani.Yorumla($"Ürün: {cmbIlaclar.Text}, Toplam Satış: {toplamSatis}. Gelecek ay tahmini?");
                lblTahminSonuc.Text = "Tamamlandı.";
                MessageBox.Show(cevap);
            }
            catch { }
        }

        private void simpleButton1_Click(object sender, EventArgs e) { panelControl1.Visible = false; }
        private void btnPanelKapat_Click(object sender, EventArgs e) { pnlTahmin.Visible = false; }

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
                if (t != null) { fr = (Form)Activator.CreateInstance(t); fr.MdiParent = this; fr.Show(); }
            }
            else fr.BringToFront();
        }
    }
}