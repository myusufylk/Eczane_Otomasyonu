using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eczane_Otomasyonu
{
    // =============================================================
    // 1. FORM SINIFI (EN ÜSTTE)
    // =============================================================
    public partial class FrmAnaModul : Form
    {
        // SEPET LİSTESİ
        List<SepetItem> _sepet = new List<SepetItem>();

        public FrmAnaModul()
        {
            InitializeComponent();

            // Olayları Bağla
            if (lstBildirimler != null) lstBildirimler.DoubleClick += lstBildirimler_DoubleClick;
            this.MdiChildActivate += FrmAnaModul_MdiChildActivate;

            // --- SADECE ENTER TUŞU İLE GÖNDERME KALDI ---
            if (txtMesaj != null)
            {
                txtMesaj.KeyDown -= txtMesaj_KeyDown;
                txtMesaj.KeyDown += txtMesaj_KeyDown;
            }
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        string secilenResimYolu = "";

        // --- FORM YÜKLENİRKEN ---
        private void FrmAnaModul_Load(object sender, EventArgs e)
        {
            // 1. TAM EKRAN BAŞLAT
            this.WindowState = FormWindowState.Maximized;

            try { ListeleriYenile(); } catch { }
            PanelleriGizle();
            IsletmeBilgileriniGetir();

            // Satış Tamamla Butonunu Güvenli Bağla
            var btnTamamla = this.Controls.Find("btnSatisTamamla", true);
            if (btnTamamla.Length > 0)
            {
                btnTamamla[0].Click -= btnSatisTamamla_Click;
                btnTamamla[0].Click += btnSatisTamamla_Click;
            }

            // (Otomatik Focus kodu kaldırıldı)
        }

        // --- ENTER TUŞUNU YAKALAMA METODU ---
        private void txtMesaj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // "Dın" sesini engelle
                btnGonder_Click(sender, e); // Gönder butonuna tıkla
            }
        }

        // --- FORM KAPANDIĞINDA PROGRAMI ÖLDÜR ---
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        // --- YAPAY ZEKA İÇİN VERİ TOPLAYAN METOD ---
        string MagazaVerileriniTopla()
        {
            string veri = "GÜNCEL MAĞAZA VERİLERİ:\n";
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                // 1. Kritik Stoklar
                SqlCommand cmdStok = new SqlCommand("SELECT ilacAdı, adet FROM Ilaclar WHERE adet < 20 AND KullaniciID=@uid", conn);
                cmdStok.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = cmdStok.ExecuteReader();
                veri += "--- AZALAN STOKLAR ---\n";
                while (dr.Read())
                {
                    veri += $"{dr[0]}: {dr[1]} adet kaldı.\n";
                }
                dr.Close();

                // 2. Bugünün Cirosu
                SqlCommand cmdCiro = new SqlCommand("SELECT SUM(toplamFiyat) FROM Hareketler WHERE tarih >= @t1 AND KullaniciID=@uid", conn);
                cmdCiro.Parameters.AddWithValue("@t1", DateTime.Today);
                cmdCiro.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object ciro = cmdCiro.ExecuteScalar();
                veri += $"\n--- BUGÜNKÜ PERFORMANS ---\nBugünkü Ciro: {(ciro != DBNull.Value ? decimal.Parse(ciro.ToString()).ToString("C2") : "0 TL")}\n";

                conn.Close();
            }
            catch { }

            return veri;
        }

        // --- SOHBET VE KOMUT MERKEZİ ---
        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string mesaj = txtMesaj.Text.Trim();
            if (string.IsNullOrEmpty(mesaj)) return;

            MesajEkle(mesaj, true);
            txtMesaj.Text = "";

            // (Mesaj gönderdikten sonra otomatik odaklanma kaldırıldı)

            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);

            string kucukMesaj = mesaj.ToLower();

            // YENİ HASTA (SEPETİ TEMİZLE)
            if (kucukMesaj.Contains("yeni hasta") || kucukMesaj.Contains("temizle"))
            {
                _sepet.Clear();
                SepetGuncelle();
                MesajEkle("🗑️ Sepet temizlendi. Yeni hasta için hazır.", false);
                return;
            }

            // EKLEME
            if (kucukMesaj.Contains("ekle")) { KomutEkle(mesaj); return; }

            // SATIŞ
            if (kucukMesaj.Contains("sat") || kucukMesaj.Contains("ver") || kucukMesaj.Contains("düş")) { KomutSat(mesaj); return; }

            // RAPOR
            if (kucukMesaj.Contains("ciro") || kucukMesaj.Contains("kazan") || kucukMesaj.Contains("hasılat") || kucukMesaj.Contains("bugün") || kucukMesaj.Contains("rapor") || kucukMesaj.Contains("dün")) { KomutRapor(mesaj); return; }

            // BİLGİ
            if (kucukMesaj.Contains("ne kadar") || kucukMesaj.Contains("kaç") || kucukMesaj.Contains("fiyat") || kucukMesaj.Contains("stok")) { if (KomutBilgi(mesaj)) return; }

            // --- GELİŞMİŞ YAPAY ZEKA ---
            try
            {
                string gonderilecekSoru = mesaj;

                // A) ANALİZ İSTEĞİ (Veritabanı verisiyle git)
                if (kucukMesaj.Contains("analiz") || kucukMesaj.Contains("durum nedir") || kucukMesaj.Contains("özet geç"))
                {
                    string dukkanVerisi = MagazaVerileriniTopla();
                    gonderilecekSoru = $"{dukkanVerisi}\n\nBu verilere bakarak bana kısa bir yönetici özeti ve tavsiye ver.";
                    MesajEkle("📊 Veriler analiz ediliyor...", false);
                }
                // B) HASTA TAVSİYESİ (Rol yaparak git)
                else if (kucukMesaj.Contains("hastam") || kucukMesaj.Contains("önerirsin") || kucukMesaj.Contains("şikayeti"))
                {
                    gonderilecekSoru = $"Şu an karşımda bir hasta var. Şikayeti: '{mesaj}'. Ona reçetesiz ne önerebilirim ve nelere dikkat etmeli? (Uyarı: Doktora gitmesini hatırlat)";
                }
                // C) GENEL SOHBET (Normal git)
                else
                {
                    string temelBilgi = MagazaDurumunuGetir(); // Basit özet
                    gonderilecekSoru = $"{temelBilgi}\n\nKULLANICI SORUSU: {mesaj}";
                }

                string cevap = await GeminiAsistani.Yorumla(gonderilecekSoru);
                MesajEkle(cevap, false);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ServiceUnavailable") || ex.Message.Contains("503")) MesajEkle("🔌 Yapay zeka sunucusu şu an çok yoğun. Lütfen bekleyin.", false);
                else MesajEkle("⚠️ Yapay Zeka Hatası.", false);
            }

            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);
        }

        // --- SEPETE EKLEME İŞLEMİ (Eski Satış Komutu Artık Buraya Geliyor) ---
        void KomutSat(string mesaj)
        {
            string ilacAdi = "";
            int adet = 1;

            System.Text.RegularExpressions.Match desenTam = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s*(?:adet|tane|kutu)?\s+(.+)\s+(?:sat|ver|düş|ekle)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Match desenBasit = System.Text.RegularExpressions.Regex.Match(mesaj, @"(.+)\s+(?:sat|ver|düş|ekle)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (desenTam.Success) { int.TryParse(desenTam.Groups[1].Value, out adet); ilacAdi = desenTam.Groups[2].Value.Trim(); }
            else if (desenBasit.Success) { ilacAdi = desenBasit.Groups[1].Value.Trim(); }
            else { MesajEkle("❌ İlaç adı anlaşılamadı.", false); return; }

            ilacAdi = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ilacAdi);

            // 1. RİSK KONTROLÜ
            if (!EtkilesimKontrol(ilacAdi)) return;

            // 2. STOK KONTROLÜ VE SEPETE EKLEME
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT adet, fiyat FROM Ilaclar WHERE ilacAdı=@p1 AND KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@p1", ilacAdi);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    int stok = Convert.ToInt32(dr["adet"]);
                    decimal fiyat = Convert.ToDecimal(dr["fiyat"]);

                    var sepetteki = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
                    int sepettekiAdet = sepetteki != null ? sepetteki.Adet : 0;

                    if (stok >= (adet + sepettekiAdet))
                    {
                        if (sepetteki != null) sepetteki.Adet += adet;
                        else _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = adet, BirimFiyat = fiyat });

                        MesajEkle($"🛒 Sepete Eklendi: {adet} x {ilacAdi}", false);
                        SepetGuncelle();
                    }
                    else MesajEkle($"❌ Yetersiz Stok! Stok: {stok}, Sepette: {sepettekiAdet}, İstenen: {adet}", false);
                }
                else MesajEkle($"❌ '{ilacAdi}' bulunamadı.", false);

                conn.Close();
            }
            catch (Exception ex) { MesajEkle("Hata: " + ex.Message, false); }
        }

        void SepetGuncelle()
        {
            var gridler = this.Controls.Find("gridSepet", true);
            if (gridler.Length > 0)
            {
                DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)gridler[0];
                gc.DataSource = null;
                gc.DataSource = _sepet;

                decimal toplam = _sepet.Sum(x => x.Toplam);
                var lbl = this.Controls.Find("lblToplamTutar", true);
                if (lbl.Length > 0) lbl[0].Text = "TOPLAM: " + toplam.ToString("C2");
            }
        }

        bool EtkilesimKontrol(string yeniIlac)
        {
            if (_sepet.Count == 0) return true;
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                foreach (var item in _sepet)
                {
                    SqlCommand cmd = new SqlCommand("SELECT RiskMesaji FROM Etkilesimler WHERE (Ilac1=@p1 AND Ilac2=@p2) OR (Ilac1=@p2 AND Ilac2=@p1)", conn);
                    cmd.Parameters.AddWithValue("@p1", yeniIlac.Trim());
                    cmd.Parameters.AddWithValue("@p2", item.IlacAdi.Trim());

                    object sonuc = cmd.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string uyari = sonuc.ToString();
                        conn.Close();
                        DialogResult cevap = MessageBox.Show($"{uyari}\n\nSepetteki {item.IlacAdi} ile {yeniIlac} riskli!\nSepete eklemeye devam edilsin mi?", "RİSK UYARISI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        return (cevap == DialogResult.Yes);
                    }
                }
                conn.Close();
            }
            catch { }
            return true;
        }

        private void btnSatisTamamla_Click(object sender, EventArgs e)
        {
            if (_sepet.Count == 0) { MessageBox.Show("Sepet boş!"); return; }

            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                foreach (var item in _sepet)
                {
                    // 1. Stoktan Düş
                    SqlCommand cmdDus = new SqlCommand("UPDATE Ilaclar SET adet = adet - @p1 WHERE ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", item.Adet);
                    cmdDus.Parameters.AddWithValue("@p2", item.IlacAdi);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

                    // 2. Kasaya İşle
                    SqlCommand cmdHareket = new SqlCommand("INSERT INTO Hareketler (ilacAdi, adet, toplamFiyat, tarih, KullaniciID) VALUES (@p1, @p2, @p3, @p4, @p5)", conn);
                    cmdHareket.Parameters.AddWithValue("@p1", item.IlacAdi);
                    cmdHareket.Parameters.AddWithValue("@p2", item.Adet);
                    cmdHareket.Parameters.AddWithValue("@p3", item.Toplam);
                    cmdHareket.Parameters.AddWithValue("@p4", DateTime.Now);
                    cmdHareket.Parameters.AddWithValue("@p5", MevcutKullanici.Id);
                    cmdHareket.ExecuteNonQuery();
                }
                conn.Close();

                MesajEkle("✅ Satış başarıyla tamamlandı.", false);
                _sepet.Clear();
                SepetGuncelle();
                ListeleriYenile();
            }
            catch (Exception ex) { MessageBox.Show("Satış Hatası: " + ex.Message); }
        }

        // --- DİĞER STANDART KODLAR ---
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
                        if (System.IO.File.Exists(secilenResimYolu)) peLogo.Image = Image.FromFile(secilenResimYolu);
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
            if (dosya.ShowDialog() == DialogResult.OK) { secilenResimYolu = dosya.FileName; peLogo.Image = Image.FromFile(secilenResimYolu); }
        }

        private void btnAyarlarKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                string sorgu = "IF EXISTS (SELECT * FROM Isletme WHERE KullaniciID=@uid) UPDATE Isletme set Ad=@p1, Sahip=@p2, Telefon=@p3, Adres=@p4, LogoYolu=@p5 WHERE KullaniciID=@uid ELSE INSERT INTO Isletme (Ad, Sahip, Telefon, Adres, LogoYolu, KullaniciID) VALUES (@p1, @p2, @p3, @p4, @p5, @uid)";
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
                    string adetStr = dr["adet"] != DBNull.Value ? dr["adet"].ToString() : "0";
                    if (int.TryParse(adetStr, out adet))
                    {
                        if (adet < 10) BildirimEkle($"⚠️ KRİTİK STOK: {dr["ilacAdı"]} (Kalan: {adet})");
                    }
                }
                conn.Close();
                BildirimButonunuGuncelle();
            }
            catch { }
        }

        public void BildirimButonunuGuncelle()
        {
            try
            {
                if (btnBildirim == null) return;
                int sayi = lstBildirimler.Items.Count;
                if (sayi > 0) { btnBildirim.Text = $"BİLDİRİMLER ({sayi})"; btnBildirim.Appearance.BackColor = Color.Red; btnBildirim.Appearance.ForeColor = Color.White; }
                else { btnBildirim.Text = "BİLDİRİMLER"; btnBildirim.Appearance.BackColor = Color.Transparent; btnBildirim.Appearance.ForeColor = Color.Black; }
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

        public void ListeleriYenile()
        {
            try
            {
                IlaclariListele();
                if (lstBildirimler != null) { lstBildirimler.Items.Clear(); StokKontrolu(); BildirimButonunuGuncelle(); }
            }
            catch { }
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

        void KomutEkle(string mesaj)
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
        }

        bool KomutBilgi(string mesaj)
        {
            bool bulundu = false;
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ilacAdı, adet, fiyat FROM Ilaclar WHERE KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string dbIlacAdi = dr["ilacAdı"].ToString();
                    if (mesaj.IndexOf(dbIlacAdi, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string fiyat = dr["fiyat"].ToString();
                        string stok = dr["adet"].ToString();
                        MesajEkle($"ℹ️ BİLGİ: {dbIlacAdi}\n📦 Stok: {stok}\n🏷️ Fiyat: {decimal.Parse(fiyat):C2}", false);
                        bulundu = true;
                        break;
                    }
                }
                conn.Close();
            }
            catch { }
            return bulundu;
        }

        void KomutRapor(string mesaj)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                DateTime baslangicTarihi = TarihBul(mesaj);
                DateTime bitisTarihi = baslangicTarihi.AddDays(1);
                string raporTarihi = baslangicTarihi.ToShortDateString();
                SqlCommand cmdCiro = new SqlCommand("SELECT SUM(toplamFiyat) FROM Hareketler WHERE tarih >= @t1 AND tarih < @t2 AND KullaniciID=@uid", conn);
                cmdCiro.Parameters.AddWithValue("@t1", baslangicTarihi);
                cmdCiro.Parameters.AddWithValue("@t2", bitisTarihi);
                cmdCiro.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonuc = cmdCiro.ExecuteScalar();
                string ciro = (sonuc != DBNull.Value && sonuc != null) ? decimal.Parse(sonuc.ToString()).ToString("C2") : "0,00 ₺";
                SqlCommand cmdAdet = new SqlCommand("SELECT SUM(adet) FROM Hareketler WHERE tarih >= @t1 AND tarih < @t2 AND KullaniciID=@uid", conn);
                cmdAdet.Parameters.AddWithValue("@t1", baslangicTarihi);
                cmdAdet.Parameters.AddWithValue("@t2", bitisTarihi);
                cmdAdet.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonucAdet = cmdAdet.ExecuteScalar();
                string toplamKutu = (sonucAdet != DBNull.Value && sonucAdet != null) ? sonucAdet.ToString() : "0";
                conn.Close();
                MesajEkle($"📊 RAPOR ({raporTarihi}):\n💰 Ciro: {ciro}\n📦 Satış: {toplamKutu} Kutu", false);
            }
            catch (Exception ex) { MesajEkle($"⚠️ Rapor alınamadı. Hata: {ex.Message}", false); }
        }

        DateTime TarihBul(string mesaj)
        {
            mesaj = mesaj.ToLower();
            if (mesaj.Contains("dün")) return DateTime.Today.AddDays(-1);
            if (mesaj.Contains("bugün")) return DateTime.Today;
            var matchGunOnce = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s+gün\s+önce");
            if (matchGunOnce.Success) { int gun = int.Parse(matchGunOnce.Groups[1].Value); return DateTime.Today.AddDays(-gun); }
            string[] aylar = { "ocak", "şubat", "mart", "nisan", "mayıs", "haziran", "temmuz", "ağustos", "eylül", "ekim", "kasım", "aralık" };
            for (int i = 0; i < aylar.Length; i++) { if (mesaj.Contains(aylar[i])) { var matchAy = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s+" + aylar[i]); if (matchAy.Success) { int gun = int.Parse(matchAy.Groups[1].Value); int yil = DateTime.Now.Year; if ((i + 1) > DateTime.Now.Month) yil--; return new DateTime(yil, i + 1, gun); } } }
            var matchTarih = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d{1,2})\.(\d{1,2})\.(\d{4})");
            if (matchTarih.Success) return DateTime.Parse(matchTarih.Value);
            return DateTime.Today;
        }

        private string MagazaDurumunuGetir() { string dukkanOzeti = ""; SqlConnection conn = bgl.baglanti(); try { SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Ilaclar WHERE KullaniciID=@uid", conn); cmd1.Parameters.AddWithValue("@uid", MevcutKullanici.Id); string toplamCesit = cmd1.ExecuteScalar().ToString(); SqlCommand cmd2 = new SqlCommand("SELECT ilacAdı, adet FROM Ilaclar WHERE adet < 20 AND KullaniciID=@uid", conn); cmd2.Parameters.AddWithValue("@uid", MevcutKullanici.Id); SqlDataReader dr = cmd2.ExecuteReader(); string kritikIlaclar = ""; while (dr.Read()) { kritikIlaclar += dr["ilacAdı"].ToString() + " (" + dr["adet"].ToString() + " adet), "; } conn.Close(); dukkanOzeti = $"SİSTEM VERİLERİ:\n- Toplam Çeşit: {toplamCesit}\n- Kritik Stoklar: {kritikIlaclar}\n- Rol: Eczane asistanısın."; } catch { dukkanOzeti = "Veri alınamadı."; } finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); } return dukkanOzeti; }
        private void MesajEkle(string mesaj, bool kullaniciMi) { Panel pnlMesaj = new Panel(); pnlMesaj.AutoSize = true; pnlMesaj.Padding = new Padding(10); pnlMesaj.Margin = new Padding(5); pnlMesaj.MaximumSize = new Size(flowSohbet.Width - 50, 0); Label lbl = new Label(); lbl.Text = mesaj; lbl.AutoSize = true; lbl.MaximumSize = new Size(flowSohbet.Width - 70, 0); lbl.Font = new Font("Segoe UI Semibold", 10); if (kullaniciMi) { pnlMesaj.BackColor = Color.FromArgb(220, 248, 198); lbl.ForeColor = Color.Black; } else { pnlMesaj.BackColor = Color.White; lbl.ForeColor = Color.Black; } pnlMesaj.Controls.Add(lbl); flowSohbet.Controls.Add(pnlMesaj); }
        private async void btnTahminHesapla_Click(object sender, EventArgs e) { if (cmbIlaclar.Text == "") { MessageBox.Show("İlaç seçin."); return; } lblTahminSonuc.Text = "Analiz ediliyor..."; try { SqlConnection conn = bgl.baglanti(); SqlCommand cmdSatis = new SqlCommand("SELECT SUM(adet) FROM Hareketler WHERE ilacAdi=@p1 AND KullaniciID=@uid", conn); cmdSatis.Parameters.AddWithValue("@p1", cmbIlaclar.Text); cmdSatis.Parameters.AddWithValue("@uid", MevcutKullanici.Id); string toplamSatis = cmdSatis.ExecuteScalar()?.ToString() ?? "0"; conn.Close(); string cevap = await GeminiAsistani.Yorumla($"Ürün: {cmbIlaclar.Text}, Toplam Satış: {toplamSatis}. Gelecek ay tahmini?"); lblTahminSonuc.Text = "Tamamlandı."; MessageBox.Show(cevap); } catch { } }
        private void simpleButton1_Click(object sender, EventArgs e) { panelControl1.Visible = false; }
        private void btnPanelKapat_Click(object sender, EventArgs e) { pnlTahmin.Visible = false; }
        private void btnIlaclar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmIlaclar"); }
        private void btnHastalar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmHastalar"); }
        private void btnSatislar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmHareketler"); }
        private void btnRaporlar_ItemClick(object sender, ItemClickEventArgs e) { AcForm("FrmRaporlar"); }
        private void btnCikis_ItemClick(object sender, ItemClickEventArgs e) { if (MessageBox.Show("Çıkış?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes) Application.Exit(); }
        void AcForm(string formAdi) { Form fr = Application.OpenForms[formAdi]; if (fr == null) { Type t = Type.GetType("Eczane_Otomasyonu." + formAdi); if (t != null) { fr = (Form)Activator.CreateInstance(t); fr.MdiParent = this; fr.Show(); } } else fr.BringToFront(); }
    }

    // =============================================================
    // 2. YARDIMCI SINIF EN ALTA (FORM KAPANDIKTAN SONRA)
    // =============================================================
    public class SepetItem
    {
        public string IlacAdi { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Toplam { get { return Adet * BirimFiyat; } }
    }

} // Namespace Parantezi