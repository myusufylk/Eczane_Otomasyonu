using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace Eczane_Otomasyonu
{
    public partial class FrmHareketler : DevExpress.XtraEditors.XtraForm
    {
        // Bağlantı sınıfın
        SqlBaglantisi bgl = new SqlBaglantisi();

        // Çift tıklama engelleyici kilit
        bool islemYapiliyor = false;

        public FrmHareketler()
        {
            InitializeComponent();

            // --- GÜVENLİ OLAY BAĞLAMA (Çift çalışmayı önler) ---
            this.Load -= FrmHareketler_Load;
            this.Load += FrmHareketler_Load;

            lueIlac.EditValueChanged -= lueIlac_EditValueChanged;
            lueIlac.EditValueChanged += lueIlac_EditValueChanged;

            txtTc.Leave -= txtTc_Leave;
            txtTc.Leave += txtTc_Leave;

            txtAdet.TextChanged -= txtAdet_TextChanged;
            txtAdet.TextChanged += txtAdet_TextChanged;

            txtFiyat.TextChanged -= txtFiyat_TextChanged;
            txtFiyat.TextChanged += txtFiyat_TextChanged;

            gridView1.DoubleClick -= gridView1_DoubleClick;
            gridView1.DoubleClick += gridView1_DoubleClick;

            // Buton tıklama olayını temizleyip bağlıyoruz
            try
            {
                Control btn = this.Controls.Find("btnSatisYap", true)[0];
                btn.Click -= btnSatisYap_Click;
                btn.Click += btnSatisYap_Click;
            }
            catch { }
        }

        // --- FORM YÜKLENİRKEN ---
        private void FrmHareketler_Load(object sender, EventArgs e)
        {
            listele();
            ilacListesiGetir();
            temizle();
            dateTarih.DateTime = DateTime.Now;

            lueIlac.Properties.NullText = "İlaç Seçiniz";
            lueIlac.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

            txtTc.Properties.NullValuePrompt = "TC Kimlik No";
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtTc.Properties.Mask.EditMask = @"\d{11}";

            gridView1.OptionsBehavior.Editable = false;
        }

        // --- ANA MODÜLE ERİŞİM METODU ---
        private FrmAnaModul AnaModuluBul()
        {
            if (this.MdiParent is FrmAnaModul)
            {
                return (FrmAnaModul)this.MdiParent;
            }
            return (FrmAnaModul)Application.OpenForms["FrmAnaModul"];
        }

        // --- LİSTELEME VE VERİ ÇEKME (SADECE BENİM VERİLERİM) ---
        void listele()
        {
            DataTable dt = new DataTable();
            // Sadece giriş yapan kullanıcının hareketlerini getir
            SqlDataAdapter da = new SqlDataAdapter("Select * From Hareketler WHERE KullaniciID=" + MevcutKullanici.Id + " ORDER BY tarih DESC", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
        }

        void ilacListesiGetir()
        {
            DataTable dt = new DataTable();
            // Sadece benim ilaçlarım listelensin
            SqlDataAdapter da = new SqlDataAdapter("Select ilacAdı, fiyat From Ilaclar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
            da.Fill(dt);
            lueIlac.Properties.DataSource = dt;
            lueIlac.Properties.ValueMember = "ilacAdı";
            lueIlac.Properties.DisplayMember = "ilacAdı";
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

        void ToplamHesapla()
        {
            try
            {
                decimal fiyat = 0; int adet = 0;
                decimal.TryParse(txtFiyat.Text, out fiyat);
                int.TryParse(txtAdet.Text, out adet);
                txtToplam.Text = (fiyat * adet).ToString("N2");
            }
            catch { txtToplam.Text = "0.00"; }
        }

        // --- OLAYLAR ---
        private void lueIlac_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIlac.EditValue != null)
            {
                object val = lueIlac.Properties.GetDataSourceRowByKeyValue(lueIlac.EditValue);
                DataRowView row = val as DataRowView;
                if (row != null) txtFiyat.Text = row["fiyat"].ToString();
            }
            ToplamHesapla();
        }

        private void txtTc_Leave(object sender, EventArgs e)
        {
            if (txtTc.Text.Length == 11)
            {
                try
                {
                    // Hasta sorgularken sadece benim hastalarıma bak
                    SqlCommand komut = new SqlCommand("Select Ad + ' ' + Soyad From Hastalar where TC=@p1 AND KullaniciID=@uid", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", txtTc.Text);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                    SqlDataReader dr = komut.ExecuteReader();

                    if (dr.Read())
                    {
                        txtHastaAdi.Text = dr[0].ToString();
                    }

                    bgl.baglanti().Close();
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            }
        }

        private void txtAdet_TextChanged(object sender, EventArgs e) { ToplamHesapla(); }
        private void txtFiyat_TextChanged(object sender, EventArgs e) { ToplamHesapla(); }

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
                if (dr["tarih"] != DBNull.Value) dateTarih.Text = dr["tarih"].ToString();

                try
                {
                    decimal toplam = decimal.Parse(txtToplam.Text);
                    int adet = int.Parse(txtAdet.Text);
                    if (adet > 0) txtFiyat.Text = (toplam / adet).ToString("N2");
                }
                catch { }
            }
        }

        // --- FİŞ YAZDIRMA ---
        private void FisYazdir()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(FisTasarimi);
            PrintPreviewDialog onizleme = new PrintPreviewDialog();
            onizleme.Document = pd;
            onizleme.ShowDialog();
        }

        // --- YENİLENMİŞ PROFESYONEL FİŞ TASARIMI (BENİM İŞLETME BİLGİLERİM) ---
        private void FisTasarimi(object sender, PrintPageEventArgs e)
        {
            // 1. Veritabanından İşletme Bilgilerini Çek
            string eczaneAdi = "ECZANE OTOMASYONU";
            string adres = "";
            string telefon = "";
            string logoYolu = "";

            try
            {
                SqlConnection conn = bgl.baglanti();
                // Sadece benim işletme bilgilerimi getir
                SqlCommand komut = new SqlCommand("Select top 1 * From Isletme WHERE KullaniciID=" + MevcutKullanici.Id, conn);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    eczaneAdi = dr["Ad"].ToString().ToUpper();
                    adres = dr["Adres"].ToString();
                    telefon = "Tel: " + dr["Telefon"].ToString();

                    if (dr["LogoYolu"] != DBNull.Value)
                        logoYolu = dr["LogoYolu"].ToString();
                }
                conn.Close();
            }
            catch { }

            // 2. Fontlar ve Fırça
            Font baslikFont = new Font("Arial", 16, FontStyle.Bold);
            Font altBaslikFont = new Font("Arial", 12, FontStyle.Bold);
            Font icerikFont = new Font("Arial", 10, FontStyle.Regular);
            Font bilgiFont = new Font("Arial", 9, FontStyle.Italic);
            Brush firca = Brushes.Black;

            // 3. Konumlandırma Ayarları
            float sayfaGenislik = e.PageBounds.Width;
            StringFormat ortali = new StringFormat();
            ortali.Alignment = StringAlignment.Center; // Yazıları ortalamak için

            int y = 20; // Başlangıç Yüksekliği
            int satirAraligi = 25;

            // --- ÇİZİM BAŞLIYOR ---

            // A) LOGO (Varsa Çiz)
            if (!string.IsNullOrEmpty(logoYolu) && System.IO.File.Exists(logoYolu))
            {
                Image img = Image.FromFile(logoYolu);
                int resimX = (int)((sayfaGenislik - 100) / 2);
                e.Graphics.DrawImage(img, resimX, y, 100, 80);
                y += 90;
            }

            // B) BAŞLIK (ECZANE ADI)
            e.Graphics.DrawString(eczaneAdi, baslikFont, firca, new RectangleF(0, y, sayfaGenislik, 30), ortali);
            y += 35;

            // C) ADRES VE TELEFON
            e.Graphics.DrawString(adres, bilgiFont, firca, new RectangleF(0, y, sayfaGenislik, 40), ortali);
            y += 40;
            e.Graphics.DrawString(telefon, bilgiFont, firca, new RectangleF(0, y, sayfaGenislik, 20), ortali);
            y += 30;

            // D) ÇİZGİ
            e.Graphics.DrawString("----------------------------------------------------------------", icerikFont, firca, new RectangleF(0, y, sayfaGenislik, 20), ortali);
            y += 20;

            // E) SATIŞ BİLGİLERİ
            int solBosluk = 40;
            e.Graphics.DrawString($"Tarih: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}", icerikFont, firca, solBosluk, y);
            y += satirAraligi;
            e.Graphics.DrawString($"Müşteri: {txtHastaAdi.Text}", icerikFont, firca, solBosluk, y);
            y += satirAraligi;
            e.Graphics.DrawString($"TC Kimlik: {txtTc.Text}", icerikFont, firca, solBosluk, y);
            y += satirAraligi + 10;

            // F) ÜRÜN DETAYLARI
            e.Graphics.DrawString("Ürün", altBaslikFont, firca, solBosluk, y);
            e.Graphics.DrawString("Tutar", altBaslikFont, firca, sayfaGenislik - 150, y);
            y += satirAraligi;

            e.Graphics.DrawString(lueIlac.Text, icerikFont, firca, solBosluk, y);
            e.Graphics.DrawString($"{txtAdet.Text} Adet x {txtFiyat.Text} TL", bilgiFont, firca, solBosluk + 10, y + 20);

            // Toplam Fiyatı Sağa Yasla
            e.Graphics.DrawString($"{txtToplam.Text} TL", altBaslikFont, firca, sayfaGenislik - 150, y);
            y += satirAraligi * 2;

            // G) TOPLAM VE KAPANIŞ
            e.Graphics.DrawString("----------------------------------------------------------------", icerikFont, firca, new RectangleF(0, y, sayfaGenislik, 20), ortali);
            y += 20;

            e.Graphics.DrawString($"GENEL TOPLAM: {txtToplam.Text} TL", baslikFont, firca, new RectangleF(0, y, sayfaGenislik, 30), ortali);
            y += 50;

            e.Graphics.DrawString("Sağlıklı günler dileriz...", bilgiFont, firca, new RectangleF(0, y, sayfaGenislik, 20), ortali);
        }

        // --- SATIŞ YAP BUTONU (ANA İŞLEM - KULLANICI BAZLI) ---
        private async void btnSatisYap_Click(object sender, EventArgs e)
        {
            // 1. KİLİT KONTROLÜ
            if (islemYapiliyor) return;
            islemYapiliyor = true;

            try
            {
                // Validasyon
                if (lueIlac.EditValue == null || txtTc.Text == "" || txtHastaAdi.Text == "" || txtAdet.Text == "")
                {
                    MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int satilanAdet = 0;
                if (!int.TryParse(txtAdet.Text, out satilanAdet) || satilanAdet <= 0) return;

                decimal toplamTutar = 0;
                decimal.TryParse(txtToplam.Text, out toplamTutar);

                SqlConnection conn = bgl.baglanti();

                try
                {
                    // 1. STOK KONTROLÜ (Sadece benim stoğum)
                    SqlCommand cmdStok = new SqlCommand("Select adet From Ilaclar where ilacAdı=@p1 AND KullaniciID=@uid", conn);
                    cmdStok.Parameters.AddWithValue("@p1", lueIlac.Text);
                    cmdStok.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    object stokObj = cmdStok.ExecuteScalar();
                    int mevcutStok = (stokObj != null) ? Convert.ToInt32(stokObj) : 0;

                    if (satilanAdet > mevcutStok)
                    {
                        MessageBox.Show("Stok Yetersiz! Mevcut Stok: " + mevcutStok, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                        return;
                    }

                    // 2. HASTA KAYIT İŞLEMİ (Benim hastalarım)
                    SqlCommand cmdHasta = new SqlCommand("Select count(*) From Hastalar where TC=@p1 AND KullaniciID=@uid", conn);
                    cmdHasta.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdHasta.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    int hastaSayisi = Convert.ToInt32(cmdHasta.ExecuteScalar());

                    if (hastaSayisi == 0)
                    {
                        string tamIsim = txtHastaAdi.Text.Trim();
                        string ad = tamIsim;
                        string soyad = "";
                        int bosluk = tamIsim.LastIndexOf(' ');

                        if (bosluk > 0)
                        {
                            ad = tamIsim.Substring(0, bosluk);
                            soyad = tamIsim.Substring(bosluk + 1);
                        }
                        else { ad = tamIsim; }

                        SqlCommand cmdEkle = new SqlCommand("Insert into Hastalar (TC, Ad, Soyad, KullaniciID) values (@p1, @p2, @p3, @uid)", conn);
                        cmdEkle.Parameters.AddWithValue("@p1", txtTc.Text);
                        cmdEkle.Parameters.AddWithValue("@p2", ad);
                        cmdEkle.Parameters.AddWithValue("@p3", soyad);
                        cmdEkle.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                        cmdEkle.ExecuteNonQuery();
                    }

                    // 3. STOKTAN DÜŞME (Benim stoğumdan düş)
                    SqlCommand cmdDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", satilanAdet);
                    cmdDus.Parameters.AddWithValue("@p2", lueIlac.Text);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

                    // 4. HAREKET KAYDI (Benim hareketlerime ekle)
                    SqlCommand cmdHareket = new SqlCommand("Insert into Hareketler (ilacAdi, adet, toplamFiyat, tarih, hastaAdi, tcNo, KullaniciID) values (@p1,@p2,@p3,@p4,@p5,@p6,@uid)", conn);
                    cmdHareket.Parameters.AddWithValue("@p1", lueIlac.Text);
                    cmdHareket.Parameters.AddWithValue("@p2", satilanAdet);
                    cmdHareket.Parameters.AddWithValue("@p3", toplamTutar);
                    cmdHareket.Parameters.AddWithValue("@p4", dateTarih.DateTime);
                    cmdHareket.Parameters.AddWithValue("@p5", txtHastaAdi.Text);
                    cmdHareket.Parameters.AddWithValue("@p6", txtTc.Text);
                    cmdHareket.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdHareket.ExecuteNonQuery();

                    // 5. SADAKAT KONTROLÜ (Benim satış sayım)
                    SqlCommand cmdSayi = new SqlCommand("Select count(*) From Hareketler where tcNo=@p1 AND KullaniciID=@uid", conn);
                    cmdSayi.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdSayi.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    int alisverisSayisi = Convert.ToInt32(cmdSayi.ExecuteScalar());

                    conn.Close();

                    // BİLGİ VE FİŞ
                    MessageBox.Show("Satış Başarıyla Tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FisYazdir();

                    // 6. ANA MODÜL BİLDİRİM & GEMINI
                    FrmAnaModul anaForm = AnaModuluBul();

                    if (anaForm != null)
                    {
                        // Stok Kontrolü Tetikle
                        anaForm.StokKontrolu();

                        // Gemini Tavsiyesi (Limit: 2)
                        if (alisverisSayisi >= 2)
                        {
                            string soru = $"Analiz: '{txtHastaAdi.Text}' isimli hasta, toplam {alisverisSayisi}. kez alışveriş yaptı.\n" +
                                          $"GÖREV: Eczacıya (bana) yönelik kısa, profesyonel bir bilgi notu yaz. Müşteriye hitap etme.\n" +
                                          $"İÇERİK: Sadakat durumunu belirt ve profesyonel bir aksiyon önerisi sun.\n" +
                                          $"FORMAT: Resmi ve net olsun.";

                            string tavsiye = await GeminiAsistani.Yorumla(soru);

                            // Ana modüldeki listeye ekle
                            anaForm.BildirimEkle($"🤖 MÜŞTERİ NOTU: {tavsiye.Replace("\n", " ")}");
                        }
                    }

                    // Temizlik
                    listele();
                    temizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("İşlem Hatası: " + ex.Message);
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                // KİLİDİ AÇ
                islemYapiliyor = false;
            }
        }
    }
}