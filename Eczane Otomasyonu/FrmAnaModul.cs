using DevExpress.XtraBars;
using DevExpress.XtraEditors;
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
    public partial class FrmAnaModul : Form
    {
        // =============================================================
        // DEĞİŞKENLER VE BAĞLANTILAR
        // =============================================================
        List<SepetItem> _sepet = new List<SepetItem>();
        SqlBaglantisi bgl = new SqlBaglantisi();
        string secilenResimYolu = "";

        public FrmAnaModul()
        {
            InitializeComponent();

            // Olayları Güvenli Bağla
            if (lstBildirimler != null) lstBildirimler.DoubleClick += lstBildirimler_DoubleClick;
            this.MdiChildActivate += FrmAnaModul_MdiChildActivate;

            // ENTER TUŞU İLE GÖNDERME
            if (txtMesaj != null)
            {
                txtMesaj.KeyDown -= txtMesaj_KeyDown;
                txtMesaj.KeyDown += txtMesaj_KeyDown;
            }
        }

        // =============================================================
        // 🛠️ KESİN ÇÖZÜM: PAINT EVENT (BOYAMA OLAYI)
        // =============================================================
        private void FrmAnaModul_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            try { ListeleriYenile(); } catch { }
            PanelleriGizle();
            IsletmeBilgileriniGetir();

            var btnTamamla = this.Controls.Find("btnSatisTamamla", true);
            if (btnTamamla.Length > 0)
            {
                btnTamamla[0].Click -= btnSatisTamamla_Click;
                btnTamamla[0].Click += btnSatisTamamla_Click;
            }

            // MdiClient'ı bul ve Boyama (Paint) olayını ele geçir.
            // Bu kod, DevExpress ne yaparsa yapsın resmi zorla oraya çizer.
            foreach (Control ctl in this.Controls)
            {
                if (ctl is MdiClient)
                {
                    MdiClient mdi = (MdiClient)ctl;

                    // 1. Olay: Ekran her yenilendiğinde resmini çiz
                    mdi.Paint += (s, p) =>
                    {
                        if (this.BackgroundImage != null)
                        {
                            // Resmi MdiClient'ın boyutlarına gerdirerek çiz
                            p.Graphics.DrawImage(this.BackgroundImage, mdi.ClientRectangle);
                        }
                    };

                    // 2. Olay: Pencere boyutu değişirse (Resize) tekrar boyamayı tetikle
                    mdi.Resize += (s, r) => mdi.Invalidate();

                    break;
                }
            }
        }
        // =============================================================

        // =============================================================
        // 🔒 GÜVENLİ SOHBET SİSTEMİ (KOTA DOSTU)
        // =============================================================

        private void txtMesaj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnGonder_Click(sender, e);
            }
        }

        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string mesaj = txtMesaj.Text.Trim();
            if (string.IsNullOrEmpty(mesaj)) return;

            txtMesaj.Enabled = false;
            btnGonder.Enabled = false;

            MesajEkle("👤 SİZ: " + mesaj, true);
            txtMesaj.Text = "";
            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);

            string kucukMesaj = mesaj.ToLower();

            if (kucukMesaj.Contains("yeni hasta") || kucukMesaj.Contains("temizle"))
            {
                _sepet.Clear(); SepetGuncelle();
                MesajEkle("🗑️ Sepet temizlendi.", false);
                KilidiAc(); return;
            }

            // --- DÜZELTİLEN KISIM: STOK KELİMELERİ ---
            // Artık "stoğa", "depo", "giriş" kelimelerini de tanıyor.
            if ((kucukMesaj.Contains("stok") || kucukMesaj.Contains("stoğ") || kucukMesaj.Contains("depo") || kucukMesaj.Contains("tanımla")) && kucukMesaj.Contains("ekle"))
            {
                KomutEkle(mesaj);
                KilidiAc(); return;
            }

            if (kucukMesaj.Contains("sat") || kucukMesaj.Contains("ver") || kucukMesaj.Contains("düş") || kucukMesaj.Contains("ekle"))
            {
                KomutSat(mesaj);
                KilidiAc(); return;
            }

            if (kucukMesaj.Contains("ciro") || kucukMesaj.Contains("rapor")) { KomutRapor(mesaj); KilidiAc(); return; }
            if (kucukMesaj.Contains("stok") || kucukMesaj.Contains("fiyat") || kucukMesaj.Contains("var mı")) { if (KomutBilgi(mesaj)) { KilidiAc(); return; } }

            try
            {
                MesajEkle("🤖 PharmAI yazıyor...", false);
                string gonderilecekSoru = "";
                if (kucukMesaj.Contains("analiz") || kucukMesaj.Contains("özet"))
                {
                    string dukkanVerisi = MagazaVerileriniTopla();
                    gonderilecekSoru = $"{dukkanVerisi}\n\nSORU: Bu verilere göre eczanemin durumu nedir? Yönetici özeti geç.";
                }
                else
                {
                    gonderilecekSoru = $"KULLANICI SORUSU: {mesaj}\n\n(Rolün: Eczacı Asistanı. Cevap kısa ve Türkçe olsun.)";
                }

                string cevap = await GeminiAsistani.Yorumla(gonderilecekSoru);
                MesajEkle(cevap, false);
            }
            catch (Exception ex)
            {
                MesajEkle("⚠️ Hata: " + ex.Message, false);
            }
            finally
            {
                KilidiAc();
                txtMesaj.Focus();
            }

            if (flowSohbet.Controls.Count > 0) flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);
        }

        void KilidiAc()
        {
            txtMesaj.Enabled = true;
            btnGonder.Enabled = true;
        }

        void KomutSat(string mesaj)
        {
            string girilenIlacAdi = "";
            int adet = 1;

            var desenSayiOnce = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s*(?:adet|tane|kutu)?\s+(.+)\s+(?:sat|ver|düş|ekle)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var desenIsimOnce = System.Text.RegularExpressions.Regex.Match(mesaj, @"(.+)\s+(\d+)\s*(?:adet|tane|kutu)?\s+(?:sat|ver|düş|ekle)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var desenSadeceIsim = System.Text.RegularExpressions.Regex.Match(mesaj, @"(.+)\s+(?:sat|ver|düş|ekle)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (desenSayiOnce.Success) { int.TryParse(desenSayiOnce.Groups[1].Value, out adet); girilenIlacAdi = desenSayiOnce.Groups[2].Value.Trim(); }
            else if (desenIsimOnce.Success) { girilenIlacAdi = desenIsimOnce.Groups[1].Value.Trim(); int.TryParse(desenIsimOnce.Groups[2].Value, out adet); }
            else if (desenSadeceIsim.Success) { girilenIlacAdi = desenSadeceIsim.Groups[1].Value.Trim(); }
            else { MesajEkle("❌ İlaç adı anlaşılamadı.", false); return; }

            string tamIlacAdi = "";
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmdBul = new SqlCommand("SELECT TOP 1 ilacAdı FROM Ilaclar WHERE ilacAdı LIKE @p1 AND KullaniciID=@uid", conn);
                cmdBul.Parameters.AddWithValue("@p1", "%" + girilenIlacAdi + "%");
                cmdBul.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                object sonuc = cmdBul.ExecuteScalar();
                conn.Close();

                if (sonuc != null) tamIlacAdi = sonuc.ToString();
                else
                {
                    MesajEkle($"❌ '{girilenIlacAdi}' bulunamadı.", false);
                    return;
                }
            }
            catch { return; }

            MesajEkle($"✅ '{tamIlacAdi}' bulundu. Satış ekranına gönderiliyor...", false);

            FrmHareketler frSatis = (FrmHareketler)Application.OpenForms["FrmHareketler"];
            if (frSatis == null)
            {
                frSatis = new FrmHareketler();
                frSatis.MdiParent = this;
                frSatis.Show();
            }
            else
            {
                frSatis.BringToFront();
                frSatis.Activate();
            }

            frSatis.ChattenSatisYap(tamIlacAdi, adet);
        }

        void KomutEkle(string mesaj)
        {
            int adet = 1;
            var sayiMatch = System.Text.RegularExpressions.Regex.Match(mesaj, @"\d+");
            if (sayiMatch.Success)
            {
                adet = int.Parse(sayiMatch.Value);
                mesaj = mesaj.Remove(sayiMatch.Index, sayiMatch.Length);
            }

            string[] yasakliKelimeler = { "adet", "tane", "kutu", "ekle", "lütfen", "stok", "stoğuna", "stoğa", "gir", "yap", "koy" };
            foreach (var kelime in yasakliKelimeler)
            {
                mesaj = System.Text.RegularExpressions.Regex.Replace(mesaj, $@"\b{kelime}\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            string arananKelime = mesaj.Trim();
            if (string.IsNullOrEmpty(arananKelime)) return;

            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand komut = new SqlCommand("SELECT TOP 1 ilacAdı FROM Ilaclar WHERE ilacAdı LIKE @p1 AND KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@p1", "%" + arananKelime + "%");
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                object sonuc = komut.ExecuteScalar();
                conn.Close();

                if (sonuc != null)
                {
                    string tamIlacAdi = sonuc.ToString();
                    MesajEkle($"✅ '{tamIlacAdi}' bulundu. Stok Giriş ekranı açılıyor...", false);

                    FrmIlaclar frIlac = (FrmIlaclar)Application.OpenForms["FrmIlaclar"];
                    if (frIlac == null) { frIlac = new FrmIlaclar(); frIlac.MdiParent = this; frIlac.Show(); }
                    else { frIlac.BringToFront(); frIlac.Activate(); }

                    frIlac.OtomatikDoldur(tamIlacAdi, adet.ToString());
                }
                else
                {
                    MesajEkle($"❌ '{arananKelime}' bulunamadı. Yeni kayıt ekranı açılıyor.", false);
                    FrmIlaclar fr = (FrmIlaclar)Application.OpenForms["FrmIlaclar"];
                    if (fr == null) { fr = new FrmIlaclar(); fr.MdiParent = this; fr.Show(); }

                    // --- DÜZELTME: Verileri forma gönder ---
                    fr.OtomatikDoldur(arananKelime, adet.ToString());
                }
            }
            catch (Exception ex) { MesajEkle("Hata: " + ex.Message, false); }
        }

        bool KomutBilgi(string mesaj)
        {
            bool bulundu = false;
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();
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

        string MagazaVerileriniTopla()
        {
            string veri = "GÜNCEL MAĞAZA VERİLERİ:\n";
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand cmdStok = new SqlCommand("SELECT TOP 10 ilacAdı, adet FROM Ilaclar WHERE adet < 20 AND KullaniciID=@uid ORDER BY adet ASC", conn);
                cmdStok.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = cmdStok.ExecuteReader();
                veri += "--- AZALAN KRİTİK STOKLAR (İlk 10) ---\n";
                while (dr.Read()) { veri += $"{dr[0]}: {dr[1]} adet kaldı.\n"; }
                dr.Close();
                conn.Close();
            }
            catch { }
            return veri;
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

            if (kullaniciMi)
            {
                pnlMesaj.BackColor = Color.FromArgb(220, 248, 198);
                lbl.ForeColor = Color.Black;
            }
            else
            {
                pnlMesaj.BackColor = Color.White;
                lbl.ForeColor = Color.Black;
            }

            pnlMesaj.Controls.Add(lbl);
            flowSohbet.Controls.Add(pnlMesaj);
        }

        DateTime TarihBul(string mesaj)
        {
            mesaj = mesaj.ToLower();
            if (mesaj.Contains("dün")) return DateTime.Today.AddDays(-1);
            if (mesaj.Contains("bugün")) return DateTime.Today;
            var matchGunOnce = System.Text.RegularExpressions.Regex.Match(mesaj, @"(\d+)\s+gün\s+önce");
            if (matchGunOnce.Success) { int gun = int.Parse(matchGunOnce.Groups[1].Value); return DateTime.Today.AddDays(-gun); }
            return DateTime.Today;
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

        private void btnSatisTamamla_Click(object sender, EventArgs e)
        {
            if (_sepet.Count == 0) { MessageBox.Show("Sepet boş!"); return; }
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();
                foreach (var item in _sepet)
                {
                    SqlCommand cmdDus = new SqlCommand("UPDATE Ilaclar SET adet = adet - @p1 WHERE ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", item.Adet);
                    cmdDus.Parameters.AddWithValue("@p2", item.IlacAdi);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

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

        private void btnRibbonYapayZeka_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (panelControl1.Visible) panelControl1.Visible = false;
            else { PanelleriGizle(); panelControl1.Visible = true; panelControl1.BringToFront(); if (txtMesaj != null) txtMesaj.Focus(); }
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
                if (conn.State == ConnectionState.Closed) conn.Open();
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
        void AcForm(string formAdi) { Form fr = Application.OpenForms[formAdi]; if (fr == null) { Type t = Type.GetType("Eczane_Otomasyonu." + formAdi); if (t != null) { fr = (Form)Activator.CreateInstance(t); fr.MdiParent = this; fr.Show(); } } else fr.BringToFront(); }
    }

    public class SepetItem
    {
        public string IlacAdi { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Toplam { get { return Adet * BirimFiyat; } }
    }
}