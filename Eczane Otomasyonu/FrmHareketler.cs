using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Tesseract; // OCR Kütüphanesi
using System.IO;   // Dosya işlemleri için
using System.Net.Http; // API iletişimi için
using System.Text;     // Encoding işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için
using Newtonsoft.Json; // JSON işlemleri için

namespace Eczane_Otomasyonu
{
    public partial class FrmHareketler : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();
        bool islemYapiliyor = false;

        // Sepet Listesi
        List<SepetItem> _sepet = new List<SepetItem>();

        public FrmHareketler()
        {
            InitializeComponent();

            // --- OLAYLARI GÜVENLİ BAĞLAMA ---
            // 1. Form Yüklenirken
            this.Load -= FrmHareketler_Load;
            this.Load += FrmHareketler_Load;

            // 2. İlaç Seçimi Değişince
            lueIlac.EditValueChanged -= lueIlac_EditValueChanged;
            lueIlac.EditValueChanged += lueIlac_EditValueChanged;

            // 3. İlaç Kutusuna Tıklanırsa (Listeyi Yeniler)
            lueIlac.QueryPopUp -= lueIlac_QueryPopUp;
            lueIlac.QueryPopUp += lueIlac_QueryPopUp;

            // 4. TC Girilince
            txtTc.Leave -= txtTc_Leave;
            txtTc.Leave += txtTc_Leave;

            // 5. Barkod Kutusu
            if (txtBarkod != null)
            {
                txtBarkod.KeyDown -= txtBarkod_KeyDown;
                txtBarkod.KeyDown += txtBarkod_KeyDown;
            }

            // 6. Butonları Bağla (Eski ve Yeni Hepsi)
            try
            {
                var btnSatis = this.Controls.Find("btnSatisYap", true);
                if (btnSatis.Length > 0) { btnSatis[0].Click -= btnSatisYap_Click; btnSatis[0].Click += btnSatisYap_Click; }

                var btnSepet = this.Controls.Find("btnSepeteEkle", true);
                if (btnSepet.Length > 0) { btnSepet[0].Click -= btnSepeteEkle_Click; btnSepet[0].Click += btnSepeteEkle_Click; }

                var btnRecete = this.Controls.Find("btnReceteYukle", true);
                if (btnRecete.Length > 0) { btnRecete[0].Click -= btnReceteYukle_Click; btnRecete[0].Click += btnReceteYukle_Click; }

                var btnRisk = this.Controls.Find("btnRiskAnaliz", true);
                if (btnRisk.Length > 0) { btnRisk[0].Click -= btnRiskAnaliz_Click; btnRisk[0].Click += btnRiskAnaliz_Click; }

                // --- YENİ EKLENEN: Chat Butonu Bağlaması ---
                var btnChat = this.Controls.Find("btnChatGonder", true);
                if (btnChat.Length > 0) { btnChat[0].Click -= btnChatGonder_Click; btnChat[0].Click += btnChatGonder_Click; }
            }
            catch { }
        }

        private void FrmHareketler_Load(object sender, EventArgs e)
        {
            listele();
            ilacListesiGetir();
            temizle();
            dateTarih.DateTime = DateTime.Now;

            lueIlac.Properties.NullText = "İlaç Seçiniz";
            lueIlac.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

            // TC Kimlik Maskesi
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            txtTc.Properties.Mask.EditMask = "00000000000";
            txtTc.Properties.Mask.UseMaskAsDisplayFormat = true;

            // Resim Kutusunu Ayarla (Resim sığsın diye)
            if (pictureEdit1 != null)
            {
                pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                pictureEdit1.Properties.NullText = "Resim Yok";
            }

            gridView1.OptionsBehavior.Editable = false;
        }

        // --- İlaç Kutusuna Tıklayınca Listeyi Yeniler ---
        private void lueIlac_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ilacListesiGetir();
        }

        // --- İlaç Listesini Getiren Metod ---
        void ilacListesiGetir()
        {
            try
            {
                DataTable dt = new DataTable();
                // Not: Resim sütunun varsa kalsın, yoksa "Resim" kısmını sil
                SqlDataAdapter da = new SqlDataAdapter("Select ilacAdı, fiyat, Resim From Ilaclar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
                da.Fill(dt);
                lueIlac.Properties.DataSource = dt;
                lueIlac.Properties.ValueMember = "ilacAdı";
                lueIlac.Properties.DisplayMember = "ilacAdı";
            }
            catch { }
        }

        // --- İlaç Seçilince Fiyatı ve Resmi Getir ---
        private void lueIlac_EditValueChanged(object sender, EventArgs e)
        {
            // Önce temizle
            if (pictureEdit1 != null) pictureEdit1.Image = null;
            txtFiyat.Text = "";

            if (lueIlac.EditValue != null)
            {
                object val = lueIlac.Properties.GetDataSourceRowByKeyValue(lueIlac.EditValue);
                DataRowView row = val as DataRowView;
                if (row != null)
                {
                    // 1. Fiyatı Yaz
                    txtFiyat.Text = row["fiyat"].ToString();

                    // 2. Resmi Getir
                    if (pictureEdit1 != null)
                    {
                        try
                        {
                            string resimYolu = row["Resim"].ToString();

                            if (!string.IsNullOrEmpty(resimYolu) && System.IO.File.Exists(resimYolu))
                            {
                                pictureEdit1.Image = Image.FromFile(resimYolu);
                            }
                            else
                            {
                                pictureEdit1.Image = null;
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        // ============================================================
        // 1. SEPET İŞLEMLERİ
        // ============================================================
        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (lueIlac.EditValue == null || txtAdet.Text == "")
            {
                MessageBox.Show("İlaç ve Adet seçmelisiniz.", "Uyarı");
                return;
            }

            string ilacAdi = lueIlac.Text;
            int adet = 0;
            int.TryParse(txtAdet.Text, out adet);
            decimal fiyat = 0;
            decimal.TryParse(txtFiyat.Text, out fiyat);

            // Kontroller
            if (!EtkilesimKontrol(ilacAdi)) return;
            if (!StokYeterliMi(ilacAdi, adet)) return;

            // Sepete Ekle
            var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
            if (mevcut != null)
            {
                mevcut.Adet += adet;
            }
            else
            {
                _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = adet, BirimFiyat = fiyat });
            }

            SepetGuncelle();

            // Temizlik
            lueIlac.EditValue = null;
            txtAdet.Text = "";
            txtFiyat.Text = "";
            if (pictureEdit1 != null) pictureEdit1.Image = null;
        }

        void SepetGuncelle()
        {
            try
            {
                var gridler = this.Controls.Find("gridSepet", true);
                if (gridler.Length > 0)
                {
                    DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)gridler[0];
                    gc.DataSource = null;
                    gc.DataSource = _sepet;
                }

                decimal toplam = _sepet.Sum(x => x.Toplam);
                var lbl = this.Controls.Find("lblToplamTutar", true);
                if (lbl.Length > 0) lbl[0].Text = $"{toplam:C2}";
                else
                {
                    var lbl2 = this.Controls.Find("labelControl2", true);
                    if (lbl2.Length > 0) lbl2[0].Text = $"TOPLAM: {toplam:C2}";
                }
            }
            catch { }
        }

        // ============================================================
        // 2. BARKOD İLE HIZLI SATIŞ
        // ============================================================
        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string okunanBarkod = txtBarkod.Text.Trim();
                if (!string.IsNullOrEmpty(okunanBarkod))
                {
                    BarkodlaSepeteEkle(okunanBarkod);
                    txtBarkod.Text = "";
                    e.SuppressKeyPress = true;
                    txtBarkod.Focus();
                }
            }
        }

        void BarkodlaSepeteEkle(string barkod)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Ilaclar WHERE Barkod=@p1 AND KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@p1", barkod);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string ilacAdi = dr["ilacAdı"].ToString();
                    decimal fiyat = Convert.ToDecimal(dr["fiyat"]);

                    string resimYolu = dr["Resim"].ToString();
                    if (pictureEdit1 != null && !string.IsNullOrEmpty(resimYolu) && File.Exists(resimYolu))
                        pictureEdit1.Image = Image.FromFile(resimYolu);

                    dr.Close();

                    if (!EtkilesimKontrol(ilacAdi)) { conn.Close(); return; }
                    if (!StokYeterliMi(ilacAdi, 1)) { conn.Close(); return; }

                    var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
                    if (mevcut != null) mevcut.Adet++;
                    else _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = 1, BirimFiyat = fiyat });

                    SepetGuncelle();
                    System.Media.SystemSounds.Asterisk.Play();
                }
                else
                {
                    MessageBox.Show("Bu barkoda ait ilaç bulunamadı!", "Uyarı");
                }
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // ============================================================
        // 3. OCR ve CHAT İŞLEMLERİ (HİBRİT YAPI)
        // ============================================================
        private async void btnReceteYukle_Click(object sender, EventArgs e)
        {
            // 1. Dosya Seçtirme Ekranı
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
            dosya.Title = "Reçete Seç";

            if (dosya.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 2. Yol Kontrolü
                    string programKlasoru = AppDomain.CurrentDomain.BaseDirectory;
                    string tessYolu = System.IO.Path.Combine(programKlasoru, "tessdata");

                    if (!System.IO.Directory.Exists(tessYolu))
                    {
                        MessageBox.Show("KRİTİK HATA: 'tessdata' klasörü bulunamadı!\n" + tessYolu, "Klasör Eksik");
                        return;
                    }

                    // 3. Tesseract Başlatma
                    using (var motor = new TesseractEngine(tessYolu, "tur", EngineMode.Default))
                    {
                        using (var resim = Pix.LoadFromFile(dosya.FileName))
                        {
                            using (var sayfa = motor.Process(resim))
                            {
                                string okunanMetin = sayfa.GetText();
                                MessageBox.Show("OCR Okuma Başarılı!\nReçete Analiz Ediliyor...", "Bilgi");

                                // BURADA YENİ HİBRİT METODU ÇAĞIRIYORUZ
                                await HibritReceteAnalizi(okunanMetin);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata Dedektifi
                    string mesaj = "GENEL HATA: " + ex.Message;
                    if (ex.InnerException != null) mesaj += "\n\nDetay: " + ex.InnerException.Message;
                    MessageBox.Show(mesaj, "Hata Dedektifi");
                }
            }
        }

        // --- YENİ EKLENEN VE DÜZELTİLEN REÇETE ANALİZ METODU ---
        private async Task HibritReceteAnalizi(string ocrMetni)
        {
            string[] satirlar = ocrMetni.Split('\n');

            List<string> direkEklenenler = new List<string>();
            List<string> sqlMuadilOnerileri = new List<string>();
            List<string> tanimsizIlaclar = new List<string>();

            SqlConnection conn = bgl.baglanti();
            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (string satir in satirlar)
            {
                // --- 1. AŞAMA: KAPSAMLI TEMİZLİK ---
                string temizSatir = System.Text.RegularExpressions.Regex.Replace(satir, @"^[\d\W]+", "").Trim();
                if (temizSatir.Length < 3) continue;

                string bulunanIlacAdi = "";
                int stokSayisi = 0;
                decimal fiyat = 0;
                string etkenMadde = "";

                // --- 2. AŞAMA: VERİTABANINDA ARAMA ---

                // A) Önce Tam Eşleşme
                SqlCommand komutTam = new SqlCommand("Select Top 1 ilacAdı, adet, EtkenMadde, fiyat From Ilaclar where ilacAdı=@p1 AND KullaniciID=@uid", conn);
                komutTam.Parameters.AddWithValue("@p1", temizSatir);
                komutTam.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = komutTam.ExecuteReader();

                if (dr.Read())
                {
                    bulunanIlacAdi = dr["ilacAdı"].ToString();
                    stokSayisi = Convert.ToInt32(dr["adet"]);
                    fiyat = Convert.ToDecimal(dr["fiyat"]);
                    etkenMadde = dr["EtkenMadde"] != DBNull.Value ? dr["EtkenMadde"].ToString() : "";
                }
                else
                {
                    // B) Bulamazsa İlk Kelimeyi Al
                    dr.Close();
                    string ilkKelime = temizSatir.Split(' ')[0];

                    if (ilkKelime.Length >= 3)
                    {
                        SqlCommand komutBenzer = new SqlCommand("Select Top 1 ilacAdı, adet, EtkenMadde, fiyat From Ilaclar where ilacAdı LIKE @p1 AND KullaniciID=@uid", conn);
                        komutBenzer.Parameters.AddWithValue("@p1", "%" + ilkKelime + "%");
                        komutBenzer.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                        SqlDataReader drBenzer = komutBenzer.ExecuteReader();

                        if (drBenzer.Read())
                        {
                            bulunanIlacAdi = drBenzer["ilacAdı"].ToString();
                            stokSayisi = Convert.ToInt32(drBenzer["adet"]);
                            fiyat = Convert.ToDecimal(drBenzer["fiyat"]);
                            etkenMadde = drBenzer["EtkenMadde"] != DBNull.Value ? drBenzer["EtkenMadde"].ToString() : "";
                        }
                        drBenzer.Close();
                    }
                }
                if (!dr.IsClosed) dr.Close();

                // --- 3. AŞAMA: KARAR VE İŞLEM ---
                if (!string.IsNullOrEmpty(bulunanIlacAdi))
                {
                    // İlaç Bulundu!
                    if (stokSayisi > 0)
                    {
                        // STOKTA VAR -> DİREKT SEPETE EKLE
                        var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == bulunanIlacAdi);
                        if (mevcut != null) mevcut.Adet++;
                        else _sepet.Add(new SepetItem { IlacAdi = bulunanIlacAdi, Adet = 1, BirimFiyat = fiyat });

                        direkEklenenler.Add(bulunanIlacAdi);
                    }
                    else
                    {
                        // STOKTA YOK -> MUADİL ÖNER
                        string yerelMuadil = SQLdenMuadilGetir(etkenMadde, bulunanIlacAdi);
                        sqlMuadilOnerileri.Add(yerelMuadil);
                    }
                }
                else
                {
                    // HİÇBİR ŞEKİLDE BULUNAMADI -> AI LİSTESİNE EKLE
                    tanimsizIlaclar.Add(temizSatir);
                }
            }

            conn.Close();
            SepetGuncelle(); // Sepet tablosunu anında yenile

            // --- 4. AŞAMA: AI DESTEĞİ ---
            string aiOnerisi = "";
            if (tanimsizIlaclar.Count > 0)
            {
                string stokListesi = StoktakiTumIlaclariGetir();
                string bilinmeyenler = string.Join(", ", tanimsizIlaclar);

                string prompt = $"Eczane stoğum: [{stokListesi}]. " +
                                $"Reçeteden okuduğum ama eşleştiremediğim satırlar: [{bilinmeyenler}]. " +
                                $"Bu satırlardaki ilaç isimlerini düzelt ve benim stoğumdaki en uygun karşılığını veya muadilini bul. " +
                                $"Cevap Formatı: 'Tespit: [İlaç] -> Öneri: [Stoktaki] (Not: ...)'";

                aiOnerisi = await GeminiyeSor(prompt);
            }

            // --- 5. AŞAMA: SONUÇ RAPORU ---
            string rapor = "--- 🧾 REÇETE İŞLEM RAPORU ---\n\n";

            if (direkEklenenler.Count > 0)
                rapor += "✅ SEPETE EKLENENLER (Stoktan Düştü):\n" + string.Join(", ", direkEklenenler) + "\n\n";

            if (sqlMuadilOnerileri.Count > 0)
                rapor += "⚠️ STOKTA OLMAYANLAR (Muadil Önerisi):\n" + string.Join("\n", sqlMuadilOnerileri) + "\n\n";

            if (!string.IsNullOrEmpty(aiOnerisi))
                rapor += "🤖 YAPAY ZEKA YORUMU:\n" + aiOnerisi;

            if (direkEklenenler.Count == 0 && sqlMuadilOnerileri.Count == 0 && string.IsNullOrEmpty(aiOnerisi))
                rapor += "Reçete okundu ama geçerli bir ilaç eşleşmesi yapılamadı.";

            MessageBox.Show(rapor, "Asistan Raporu");
        }

        // --- İŞTE EKSİK OLAN YARDIMCI METODLAR BURADA ---
        private string SQLdenMuadilGetir(string etkenMadde, string arananIlac)
        {
            if (string.IsNullOrEmpty(etkenMadde)) return $"❌ {arananIlac} (Stok Yok ve Etken Madde Girilmemiş)";

            string sonuc = $"❌ {arananIlac} (Stok Yok)";
            try
            {
                if (bgl.baglanti().State == ConnectionState.Closed) bgl.baglanti().Open();
                SqlConnection conn = bgl.baglanti();

                SqlCommand komut = new SqlCommand("Select Top 1 ilacAdı, fiyat From Ilaclar where EtkenMadde=@p1 AND adet > 0 AND ilacAdı != @p2 AND KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@p1", etkenMadde);
                komut.Parameters.AddWithValue("@p2", arananIlac);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    sonuc += $" -> 💡 ÖNERİ: {dr["ilacAdı"]} ({dr["fiyat"]} TL)";
                }
                else
                {
                    sonuc += " -> Muadil Bulunamadı (SQL)";
                }
                dr.Close();
            }
            catch { }
            return sonuc;
        }

        private string StoktakiTumIlaclariGetir()
        {
            string liste = "";
            try
            {
                if (bgl.baglanti().State == ConnectionState.Closed) bgl.baglanti().Open();
                SqlConnection conn = bgl.baglanti();
                SqlCommand komut = new SqlCommand("Select ilacAdı From Ilaclar where adet > 0 AND KullaniciID=" + MevcutKullanici.Id, conn);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    liste += dr[0].ToString() + ", ";
                }
                dr.Close();
            }
            catch { }
            return liste;
        }

        public void ChattenSatisYap(string ilacAdi, int adet)
        {
            if (!EtkilesimKontrol(ilacAdi)) return;
            if (!StokYeterliMi(ilacAdi, adet)) return;

            decimal fiyat = 0;
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT fiyat FROM Ilaclar WHERE ilacAdı=@p1 AND KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@p1", ilacAdi);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonuc = cmd.ExecuteScalar();
                conn.Close();
                if (sonuc != null) fiyat = Convert.ToDecimal(sonuc);
            }
            catch { }

            var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
            if (mevcut != null) mevcut.Adet += adet;
            else _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = adet, BirimFiyat = fiyat });

            SepetGuncelle();
            MessageBox.Show($"🛒 {adet} adet {ilacAdi} satış ekranına eklendi!", "Asistan");
        }

        // ============================================================
        // 4. SATIŞ VE FİŞ İŞLEMLERİ
        // ============================================================
        private async void btnSatisYap_Click(object sender, EventArgs e)
        {
            if (islemYapiliyor) return;
            if (_sepet.Count == 0)
            {
                if (lueIlac.EditValue != null && txtAdet.Text != "") btnSepeteEkle_Click(sender, e);
                else { MessageBox.Show("Sepet boş!", "Uyarı"); return; }
            }
            if (_sepet.Count == 0) return;

            islemYapiliyor = true;
            SqlConnection conn = bgl.baglanti();

            try
            {
                // 1. Hasta Kaydı (Otomatik) - MEVCUT YAPI
                SqlCommand cmdHasta = new SqlCommand("Select count(*) From Hastalar where TC=@p1 AND KullaniciID=@uid", conn);
                cmdHasta.Parameters.AddWithValue("@p1", txtTc.Text);
                cmdHasta.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                if (Convert.ToInt32(cmdHasta.ExecuteScalar()) == 0)
                {
                    string tamIsim = txtHastaAdi.Text.Trim();
                    string ad = tamIsim, soyad = "";
                    int bosluk = tamIsim.LastIndexOf(' ');
                    if (bosluk > 0) { ad = tamIsim.Substring(0, bosluk); soyad = tamIsim.Substring(bosluk + 1); }

                    SqlCommand cmdEkle = new SqlCommand("Insert into Hastalar (TC, Ad, Soyad, KullaniciID) values (@p1, @p2, @p3, @uid)", conn);
                    cmdEkle.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdEkle.Parameters.AddWithValue("@p2", ad);
                    cmdEkle.Parameters.AddWithValue("@p3", soyad);
                    cmdEkle.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdEkle.ExecuteNonQuery();
                }

                // 2. Satış Hareketleri - MEVCUT YAPI (Önce burası çalışmalı ki CRM kontrolünde bunları sayalım)
                foreach (var item in _sepet)
                {
                    // Stok Düşme
                    SqlCommand cmdDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", item.Adet);
                    cmdDus.Parameters.AddWithValue("@p2", item.IlacAdi);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

                    // Hareketi Kaydetme
                    SqlCommand cmdHareket = new SqlCommand("Insert into Hareketler (ilacAdi, adet, toplamFiyat, tarih, hastaAdi, tcNo, KullaniciID) values (@p1,@p2,@p3,@p4,@p5,@p6,@uid)", conn);
                    cmdHareket.Parameters.AddWithValue("@p1", item.IlacAdi);
                    cmdHareket.Parameters.AddWithValue("@p2", item.Adet);
                    cmdHareket.Parameters.AddWithValue("@p3", item.Toplam);
                    cmdHareket.Parameters.AddWithValue("@p4", dateTarih.DateTime);
                    cmdHareket.Parameters.AddWithValue("@p5", txtHastaAdi.Text);
                    cmdHareket.Parameters.AddWithValue("@p6", txtTc.Text);
                    cmdHareket.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdHareket.ExecuteNonQuery();
                }

                // =========================================================================
                // YENİ DÜZENLENMİŞ CRM MODÜLÜ (TEST İÇİN GARANTİ ÇALIŞAN VERSİYON)
                // =========================================================================
                try
                {
                    // A) Müşteri Eski mi? (Mantık: Veritabanındaki toplam satır sayısı > Şu an sepetteki ürün sayısı)
                    // Eğer veritabanında 5 kayıt var ama sepette 2 ürün varsa, demek ki önceden 3 tane almış -> ESKİ MÜŞTERİ.
                    SqlCommand cmdToplam = new SqlCommand("SELECT COUNT(*) FROM Hareketler WHERE tcNo=@p1 AND KullaniciID=@uid", conn);
                    cmdToplam.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdToplam.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                    int toplamSatisSayisi = Convert.ToInt32(cmdToplam.ExecuteScalar());
                    bool eskiMusteriMi = toplamSatisSayisi > _sepet.Count;

                    // B) Bilgileri Eksik mi?
                    SqlCommand cmdBilgi = new SqlCommand("SELECT Telefon, Adres, Guvence FROM Hastalar WHERE TC=@p1 AND KullaniciID=@uid", conn);
                    cmdBilgi.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdBilgi.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                    SqlDataReader drBilgi = cmdBilgi.ExecuteReader();
                    bool eksikBilgiVar = false;

                    if (drBilgi.Read())
                    {
                        string tel = drBilgi["Telefon"] != DBNull.Value ? drBilgi["Telefon"].ToString() : "";
                        string adres = drBilgi["Adres"] != DBNull.Value ? drBilgi["Adres"].ToString() : "";
                        string guvence = drBilgi["Guvence"] != DBNull.Value ? drBilgi["Guvence"].ToString() : "";

                        // Telefon çok kısaysa veya adres/güvence boşsa eksik kabul et
                        if (string.IsNullOrWhiteSpace(tel) || tel.Length < 5 ||
                            string.IsNullOrWhiteSpace(adres) || adres.Length < 3 ||
                            string.IsNullOrWhiteSpace(guvence) || guvence == "YOK")
                        {
                            eksikBilgiVar = true;
                        }
                    }
                    drBilgi.Close(); // Okuyucuyu mutlaka kapatıyoruz!

                    // C) KARAR: Eski müşteri ise (yani 2. kez geliyorsa) VE bilgisi eksikse sor
                    if (eskiMusteriMi && eksikBilgiVar)
                    {
                        DialogResult secim = MessageBox.Show(
                            $"Bu hasta ({txtTc.Text}) daha önce de alışveriş yapmış ({toplamSatisSayisi} kalem ürün).\n" +
                            "Ancak Telefon, Adres veya Güvence bilgileri eksik.\n\n" +
                            "Müşteri sadakati için bilgileri şimdi güncellemek ister misiniz?",
                            "CRM - Müşteri Bilgi Tavsiyesi",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (secim == DialogResult.Yes)
                        {
                            // Hasta Kartları formunu aç
                            FrmHastalar fr = new FrmHastalar();
                            if (Application.OpenForms["FrmAnaModul"] != null)
                                fr.MdiParent = Application.OpenForms["FrmAnaModul"];

                            fr.Show();

                            // Kolaylık olsun diye TC'yi panoya kopyala
                            Clipboard.SetText(txtTc.Text);
                            MessageBox.Show("TC Kimlik No kopyalandı. Hasta kartlarında yapıştırarak aratabilirsiniz.", "Bilgi");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata varsa görelim (Boş bırakırsak hatayı bulamayız)
                    MessageBox.Show("CRM Tavsiye Hatası: " + ex.Message);
                }
                // =========================================================================

                conn.Close();

                MessageBox.Show("Satış Tamamlandı.");
                FisYazdir();

                _sepet.Clear();
                SepetGuncelle();
                listele();
                temizle();
                if (pictureEdit1 != null) pictureEdit1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            finally
            {
                islemYapiliyor = false;
            }
        }

        private void FisYazdir()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(FisTasarimi);
            PrintPreviewDialog onizleme = new PrintPreviewDialog();
            onizleme.Document = pd;
            onizleme.ShowDialog();
        }

        private void FisTasarimi(object sender, PrintPageEventArgs e)
        {
            string eczaneAdi = "ECZANE OTOMASYONU";
            string adres = "";
            string telefon = "";
            string logoYolu = "";

            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand komut = new SqlCommand("Select top 1 * From Isletme WHERE KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    eczaneAdi = dr["Ad"].ToString().ToUpper();
                    adres = dr["Adres"].ToString();
                    telefon = dr["Telefon"].ToString();
                    if (dr["LogoYolu"] != DBNull.Value) logoYolu = dr["LogoYolu"].ToString();
                }
                conn.Close();
            }
            catch { }

            Font baslikFont = new Font("Arial", 16, FontStyle.Bold);
            Font altBaslik = new Font("Arial", 10, FontStyle.Bold);
            Font icerik = new Font("Arial", 9);
            Brush firca = Brushes.Black;
            float genislik = e.PageBounds.Width;
            int y = 20;
            int h = 20;
            StringFormat merkez = new StringFormat() { Alignment = StringAlignment.Center };

            if (!string.IsNullOrEmpty(logoYolu) && System.IO.File.Exists(logoYolu))
            {
                try
                {
                    Image img = Image.FromFile(logoYolu);
                    e.Graphics.DrawImage(img, (int)((genislik - 80) / 2), y, 80, 60);
                    y += 70;
                }
                catch { }
            }

            e.Graphics.DrawString(eczaneAdi, baslikFont, firca, new RectangleF(0, y, genislik, 30), merkez); y += 30;
            e.Graphics.DrawString(adres, icerik, firca, new RectangleF(0, y, genislik, 40), merkez); y += 40;
            e.Graphics.DrawString($"Tel: {telefon}", icerik, firca, new RectangleF(0, y, genislik, 20), merkez); y += 30;
            e.Graphics.DrawString("------------------------------------------------", icerik, firca, 10, y); y += h;

            e.Graphics.DrawString($"Tarih: {DateTime.Now}", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"TC: {txtTc.Text}", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"Hasta: {txtHastaAdi.Text}", icerik, firca, 10, y); y += h + 10;

            e.Graphics.DrawString("Ürün Adı", altBaslik, firca, 10, y);
            e.Graphics.DrawString("Adet", altBaslik, firca, 180, y);
            e.Graphics.DrawString("Tutar", altBaslik, firca, 230, y);
            y += h;

            decimal genelToplam = 0;
            foreach (var item in _sepet)
            {
                e.Graphics.DrawString(item.IlacAdi, icerik, firca, 10, y);
                e.Graphics.DrawString(item.Adet.ToString(), icerik, firca, 190, y);
                e.Graphics.DrawString(item.Toplam.ToString("C2"), icerik, firca, 230, y);
                genelToplam += item.Toplam;
                y += h;
            }

            e.Graphics.DrawString("------------------------------------------------", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"TOPLAM: {genelToplam:C2}", baslikFont, firca, 150, y);
        }

        // ============================================================
        // 5. YARDIMCI VE KONTROL METODLARI
        // ============================================================
        bool EtkilesimKontrol(string yeniIlac)
        {
            if (_sepet.Count == 0) return true;
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in _sepet)
                {
                    SqlCommand cmd = new SqlCommand("SELECT RiskMesaji FROM Etkilesimler WHERE (Ilac1=@p1 AND Ilac2=@p2) OR (Ilac1=@p2 AND Ilac2=@p1)", conn);
                    cmd.Parameters.AddWithValue("@p1", yeniIlac.Trim());
                    cmd.Parameters.AddWithValue("@p2", item.IlacAdi.Trim());
                    object sonuc = cmd.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string risk = sonuc.ToString();
                        conn.Close();
                        DialogResult cvp = MessageBox.Show($"⚠️ RİSK: {item.IlacAdi} ve {yeniIlac} birlikte kullanılmamalı!\nSebep: {risk}\n\nYine de ekle?", "Etkileşim Uyarısı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        return (cvp == DialogResult.Yes);
                    }
                }
                conn.Close();
            }
            catch { }
            return true;
        }

        bool StokYeterliMi(string ilacAdi, int istenenAdet)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand cmd = new SqlCommand("Select adet From Ilaclar where ilacAdı=@p1 AND KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@p1", ilacAdi);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonuc = cmd.ExecuteScalar();
                conn.Close();
                int dbStok = (sonuc != null) ? Convert.ToInt32(sonuc) : 0;
                var sepetteki = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
                int sepettekiAdet = sepetteki != null ? sepetteki.Adet : 0;

                if (dbStok < (istenenAdet + sepettekiAdet))
                {
                    MessageBox.Show($"Stok Yetersiz! Eldeki: {dbStok}, Sepette: {sepettekiAdet}", "Hata");
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        void listele() { try { DataTable dt = new DataTable(); SqlDataAdapter da = new SqlDataAdapter("Select * From Hareketler WHERE KullaniciID=" + MevcutKullanici.Id + " ORDER BY tarih DESC", bgl.baglanti()); da.Fill(dt); gridControl1.DataSource = dt; gridView1.BestFitColumns(); } catch { } }

        void temizle() { lueIlac.EditValue = null; txtTc.Text = ""; txtHastaAdi.Text = ""; txtAdet.Text = ""; txtFiyat.Text = ""; }

        private void txtTc_Leave(object sender, EventArgs e) { if (txtTc.Text.Length == 11) { try { SqlCommand komut = new SqlCommand("Select Ad + ' ' + Soyad From Hastalar where TC=@p1 AND KullaniciID=@uid", bgl.baglanti()); komut.Parameters.AddWithValue("@p1", txtTc.Text); komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id); SqlDataReader dr = komut.ExecuteReader(); if (dr.Read()) { txtHastaAdi.Text = dr[0].ToString(); } bgl.baglanti().Close(); } catch { } } }
        private void txtAdet_TextChanged(object sender, EventArgs e) { try { decimal f = decimal.Parse(txtFiyat.Text); int a = int.Parse(txtAdet.Text); } catch { } }
        private void txtFiyat_TextChanged(object sender, EventArgs e) { txtAdet_TextChanged(null, null); }
        private void txtBarkod_EditValueChanged(object sender, EventArgs e) { }

        private async void btnRiskAnaliz_Click(object sender, EventArgs e)
        {
            if (_sepet.Count < 2)
            {
                MessageBox.Show("Etkileşim analizi için sepette en az 2 farklı ilaç olmalıdır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var btn = (SimpleButton)sender;
            string eskiMetin = btn.Text;
            btn.Text = "Analiz Ediliyor...";
            btn.Enabled = false;

            try
            {
                string ilaclar = string.Join(", ", _sepet.Select(x => x.IlacAdi));
                string prompt = $"Elimde şu ilaçlar var: {ilaclar}. Bu ilaçların etkileşimi riskli mi? Format: 'DURUM: [RİSKLİ/RİSKSİZ] - AÇIKLAMA: ...'";
                string cevap = await GeminiyeSor(prompt);

                if (cevap.Contains("RİSKLİ")) MessageBox.Show(cevap, "⚠️ DİKKAT: RİSKLİ ETKİLEŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show("✅ Güvenli.\n\n" + cevap, "Güvenli", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message, "Hata"); }
            finally { btn.Text = eskiMetin; btn.Enabled = true; }
        }

        private async void btnChatGonder_Click(object sender, EventArgs e)
        {
            var txtChat = this.Controls.Find("txtChatMesaj", true);
            if (txtChat.Length == 0) return;
            string soru = txtChat[0].Text;
            if (string.IsNullOrEmpty(soru)) return;

            txtChat[0].Text = "İşleniyor...";

            string prompt = $"Sen bir eczane asistanısın. Analiz et. " +
                            $"Satış ise: 'KOMUT:SATIS|ILAC_ADI|ADET'. " +
                            $"Stok ekleme ise: 'KOMUT:STOK|ILAC_ADI|ADET'. " +
                            $"Normal sohbet ise cevap ver. " +
                            $"Örnek: '5 parol ekle' -> KOMUT:SATIS|Parol|5 " +
                            $"Örnek: 'stoğa 10 parol ekle' -> KOMUT:STOK|Parol|10 " +
                            $"Mesaj: {soru}";

            string cevap = await GeminiyeSor(prompt);

            if (cevap.Contains("KOMUT:"))
            {
                try
                {
                    string[] parcalar = cevap.Replace("KOMUT:", "").Trim().Split('|');
                    string islem = parcalar[0].Trim();
                    string ilac = parcalar[1].Trim();
                    int adet = Convert.ToInt32(parcalar[2].Trim());

                    if (islem == "SATIS") ChattenSatisYap(ilac, adet);
                    else if (islem == "STOK") ChattenStokEkle(ilac, adet);
                }
                catch { MessageBox.Show("Komut anlaşılamadı."); }
            }
            else
            {
                MessageBox.Show("Asistan: " + cevap);
            }
            txtChat[0].Text = "";
        }

        public void ChattenStokEkle(string gelenIlacAdi, int adet)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                string bulunacakIlac = "";
                string aranan = gelenIlacAdi.Trim();

                SqlCommand cmd1 = new SqlCommand("SELECT TOP 1 ilacAdı FROM Ilaclar WHERE ilacAdı LIKE @p1 AND KullaniciID=@uid", conn);
                cmd1.Parameters.AddWithValue("@p1", "%" + aranan + "%");
                cmd1.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonuc1 = cmd1.ExecuteScalar();

                if (sonuc1 != null)
                {
                    bulunacakIlac = sonuc1.ToString();
                }
                else
                {
                    string ilkKelime = aranan.Split(' ')[0];
                    if (ilkKelime.Length > 2)
                    {
                        SqlCommand cmd2 = new SqlCommand("SELECT TOP 1 ilacAdı FROM Ilaclar WHERE ilacAdı LIKE @p1 AND KullaniciID=@uid", conn);
                        cmd2.Parameters.AddWithValue("@p1", "%" + ilkKelime + "%");
                        cmd2.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                        object sonuc2 = cmd2.ExecuteScalar();
                        if (sonuc2 != null) bulunacakIlac = sonuc2.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(bulunacakIlac))
                {
                    SqlCommand cmdEkle = new SqlCommand("UPDATE Ilaclar SET adet=adet+@p2 WHERE ilacAdı=@p1 AND KullaniciID=@uid", conn);
                    cmdEkle.Parameters.AddWithValue("@p1", bulunacakIlac);
                    cmdEkle.Parameters.AddWithValue("@p2", adet);
                    cmdEkle.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdEkle.ExecuteNonQuery();

                    MessageBox.Show($"✅ GÜNCELLENDİ: '{bulunacakIlac}' stoğuna {adet} adet eklendi.", "Asistan");
                    listele();
                }
                else
                {
                    MessageBox.Show($"'{gelenIlacAdi}' stokta yok. Kayıt ekranı açılıyor...", "Yönlendiriliyor");

                    FrmIlaclar frm = (FrmIlaclar)Application.OpenForms["FrmIlaclar"];
                    if (frm == null)
                    {
                        frm = new FrmIlaclar();
                        if (Application.OpenForms["FrmAnaModul"] != null) frm.MdiParent = Application.OpenForms["FrmAnaModul"];
                    }
                    frm.Show();
                    frm.BringToFront();
                    frm.OtomatikDoldur(gelenIlacAdi, adet.ToString());
                }
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        private async Task<string> GeminiyeSor(string soru)
        {
            string apiKey = "AIzaSyDvqHcWCL6MFH5RfY4d3w_hH5nZ9cVIhbg";
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new { contents = new[] { new { parts = new[] { new { text = soru } } } } };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                    return jsonResponse.candidates[0].content.parts[0].text;
                }
                else return "API Hatası";
            }
        }
    }
}