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

            // 6. Butonları Bağla
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

        // --- YENİ EKLENEN HİBRİT ANALİZ METODU (DÜZELTİLDİ: Stok -> adet) ---
        private async Task HibritReceteAnalizi(string ocrMetni)
        {
            string[] satirlar = ocrMetni.Split('\n');

            List<string> direkEklenenler = new List<string>();
            List<string> sqlMuadilOnerileri = new List<string>();
            List<string> tanimsizIlaclar = new List<string>();

            // ADIM 1: Yerel Veritabanı Taraması (Hızlı Aşama)
            SqlConnection conn = bgl.baglanti();
            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (string satir in satirlar)
            {
                string ilacAdi = satir.Trim().Replace("\r", "").Replace(".", "").Trim();
                if (ilacAdi.Length < 3) continue;

                // İlaç Kayıtlı mı? (DÜZELTME: Stok yerine adet kullanıldı)
                SqlCommand komut = new SqlCommand("Select adet, EtkenMadde, fiyat From Ilaclar where ilacAdı=@p1 AND KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@p1", ilacAdi);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read()) // EVET, İlaç Sistemde Var
                {
                    // DÜZELTME: Veritabanından 'adet' okunuyor
                    int stokSayisi = Convert.ToInt32(dr["adet"]);
                    string etkenMadde = dr["EtkenMadde"] != DBNull.Value ? dr["EtkenMadde"].ToString() : "";
                    decimal fiyat = Convert.ToDecimal(dr["fiyat"]);

                    if (stokSayisi > 0)
                    {
                        // Stokta var, sepete ekle
                        var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
                        if (mevcut != null) mevcut.Adet++;
                        else _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = 1, BirimFiyat = fiyat });

                        direkEklenenler.Add(ilacAdi);
                    }
                    else
                    {
                        // Stokta yok ama sistemde kayıtlı -> SQL'den Muadil Bak
                        dr.Close(); // Okuyucuyu kapat
                        string yerelMuadil = SQLdenMuadilGetir(etkenMadde, ilacAdi);
                        sqlMuadilOnerileri.Add(yerelMuadil);
                        goto SonrakiSatir;
                    }
                }
                else // HAYIR, İlaç Sistemde Yok
                {
                    tanimsizIlaclar.Add(ilacAdi);
                }
                dr.Close();

            SonrakiSatir:;
            }
            conn.Close();
            SepetGuncelle();

            // ADIM 2: Tanımsızlar İçin Gemini Devreye Girsin (Akıllı Aşama)
            string aiOnerisi = "";
            if (tanimsizIlaclar.Count > 0)
            {
                string stokListesi = StoktakiTumIlaclariGetir();
                string bilinmeyenler = string.Join(", ", tanimsizIlaclar);

                string prompt = $"Eczane stoğum: [{stokListesi}]. " +
                                $"Reçetede yazan ama veritabanımda bulamadığım metinler: [{bilinmeyenler}]. " +
                                $"Lütfen bu metinlerden hangilerinin ilaç ismi olduğunu tespit et. " +
                                $"Eğer ilaçsa, benim stok listemden buna en uygun muadili (eşdeğeri) öner. " +
                                $"Cevabı sadece şu formatta ver: 'Bulunamayan: [X] -> Önerilen Stok: [Y] (Sebebi: ...)'";

                aiOnerisi = await GeminiyeSor(prompt);
            }

            // ADIM 3: Raporlama
            string rapor = "--- ANALİZ RAPORU ---\n\n";

            if (direkEklenenler.Count > 0)
                rapor += "✅ Stoktan Sepete Eklenenler:\n" + string.Join(", ", direkEklenenler) + "\n\n";

            if (sqlMuadilOnerileri.Count > 0)
                rapor += "🔄 Stokta Yok - Sistem İçi Muadil Önerileri:\n" + string.Join("\n", sqlMuadilOnerileri) + "\n\n";

            if (!string.IsNullOrEmpty(aiOnerisi))
                rapor += "🤖 YAPAY ZEKA ÖNERİLERİ (Bilinmeyen İlaçlar):\n" + aiOnerisi;

            if (direkEklenenler.Count == 0 && sqlMuadilOnerileri.Count == 0 && string.IsNullOrEmpty(aiOnerisi))
                rapor += "Reçeteden okunabilir bir ilaç tespit edilemedi.";

            MessageBox.Show(rapor, "Akıllı Eczane Asistanı");
        }

        // --- YARDIMCI: SQL'den etken maddeye göre muadil bulur (DÜZELTİLDİ: Stok -> adet) ---
        private string SQLdenMuadilGetir(string etkenMadde, string arananIlac)
        {
            if (string.IsNullOrEmpty(etkenMadde)) return $"❌ {arananIlac} (Stok Yok ve Etken Madde Girilmemiş)";

            string sonuc = $"❌ {arananIlac} (Stok Yok)";
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                // DÜZELTME: adet > 0 sorgusu
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
                conn.Close();
            }
            catch { }
            return sonuc;
        }

        // --- YARDIMCI: Gemini için stok listesi hazırlar (DÜZELTİLDİ: Stok -> adet) ---
        private string StoktakiTumIlaclariGetir()
        {
            string liste = "";
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();
                // DÜZELTME: adet > 0 sorgusu
                SqlCommand komut = new SqlCommand("Select ilacAdı From Ilaclar where adet > 0 AND KullaniciID=" + MevcutKullanici.Id, conn);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    liste += dr[0].ToString() + ", ";
                }
                conn.Close();
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
                // Hasta Kaydı
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

                // Satış Hareketleri
                foreach (var item in _sepet)
                {
                    // DÜZELTME: Veritabanında adet sütunu olduğu için burası doğruydu, koruduk.
                    SqlCommand cmdDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", item.Adet);
                    cmdDus.Parameters.AddWithValue("@p2", item.IlacAdi);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

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
                conn.Close();

                MessageBox.Show("Satış Tamamlandı.");
                FisYazdir();

                _sepet.Clear();
                SepetGuncelle();
                listele();
                temizle();
                if (pictureEdit1 != null) pictureEdit1.Image = null;
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); if (conn.State == ConnectionState.Open) conn.Close(); }
            finally { islemYapiliyor = false; }
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
                // DÜZELTME: Veritabanında adet sütunu var
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

        // ============================================================
        // 6. YAPAY ZEKA RİSK ANALİZİ
        // ============================================================
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
                string prompt = $"Elimde şu ilaçlar var: {ilaclar}. " +
                                "Bu ilaçların birlikte kullanılması (etkileşimi) tıbbi açıdan riskli mi? " +
                                "Lütfen cevabını şu formatta ver: 'DURUM: [RİSKLİ/RİSKSİZ] - AÇIKLAMA: [Kısa ve net açıklama]' " +
                                "Eğer ciddi bir hayati risk varsa uyarı işaretleri kullan.";

                string cevap = await GeminiyeSor(prompt);

                if (cevap.Contains("RİSKLİ"))
                {
                    MessageBox.Show(cevap, "⚠️ DİKKAT: RİSKLİ ETKİLEŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("✅ İlaçlar arasında bilinen kritik bir etkileşim yok.\n\n" + cevap, "Güvenli", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yapay Zeka Bağlantı Hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Text = eskiMetin;
                btn.Enabled = true;
            }
        }

        private async Task<string> GeminiyeSor(string soru)
        {
            // 🔑 BURAYA KENDİ GEMINI API KEY'İNİ YAPIŞTIR
            string apiKey = "AIzaSyDvqHcWCL6MFH5RfY4d3w_hH5nZ9cVIhbg";

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = soru } } }
                    }
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                    string sonuc = jsonResponse.candidates[0].content.parts[0].text;
                    return sonuc;
                }
                else
                {
                    return "API Hatası: " + response.ReasonPhrase;
                }
            }
        }
    }
}