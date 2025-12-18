using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
// AlertControl sildik, çünkü artık Ana Modülde listeye ekliyoruz.

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

        // --- YARDIMCI METOD: ANA MODÜLÜ BULMA ---
        // Bu metod, bildirimi göndereceğimiz Ana Formu bulur.
        private FrmAnaModul AnaModuluBul()
        {
            if (this.MdiParent is FrmAnaModul)
            {
                return (FrmAnaModul)this.MdiParent;
            }
            return (FrmAnaModul)Application.OpenForms["FrmAnaModul"];
        }

        // --- LİSTELEME VE VERİ ÇEKME ---
        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Hareketler ORDER BY tarih DESC", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
        }

        void ilacListesiGetir()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select ilacAdı, fiyat From Ilaclar", bgl.baglanti());
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
                    SqlCommand komut = new SqlCommand("Select Ad + ' ' + Soyad From Hastalar where TC=@p1", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", txtTc.Text);
                    SqlDataReader dr = komut.ExecuteReader();

                    if (dr.Read())
                    {
                        txtHastaAdi.Text = dr[0].ToString();
                    }
                    else
                    {
                        // Sadece bilgi veriyoruz
                        // MessageBox.Show("Kayıtlı hasta bulunamadı...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void FisTasarimi(object sender, PrintPageEventArgs e)
        {
            Font baslik = new Font("Arial", 14, FontStyle.Bold);
            Font altBaslik = new Font("Arial", 10, FontStyle.Bold);
            Font normal = new Font("Arial", 10, FontStyle.Regular);
            Brush firca = Brushes.Black;
            int x = 20; int y = 20; int satir = 25;

            string logoYolu = Application.StartupPath + "\\logo.png";
            if (System.IO.File.Exists(logoYolu))
            {
                Image img = Image.FromFile(logoYolu);
                e.Graphics.DrawImage(img, x + 40, y, 100, 80);
                y += 90;
            }

            e.Graphics.DrawString("ECZANE OTOMASYONU", baslik, firca, x + 20, y);
            y += satir + 10;
            e.Graphics.DrawString("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), normal, firca, x, y);
            y += satir;
            e.Graphics.DrawString("-----------------------------------------", normal, firca, x, y);
            y += satir;
            e.Graphics.DrawString("İlaç: " + lueIlac.Text, altBaslik, firca, x, y);
            y += satir;
            e.Graphics.DrawString("Adet: " + txtAdet.Text + " x " + txtFiyat.Text + " TL", normal, firca, x, y);
            y += satir;
            e.Graphics.DrawString("-----------------------------------------", normal, firca, x, y);
            y += satir;
            e.Graphics.DrawString("TOPLAM: " + txtToplam.Text + " TL", baslik, firca, x, y);
            y += satir + 20;
            e.Graphics.DrawString("Müşteri: " + txtHastaAdi.Text, normal, firca, x, y);
            y += satir;
            e.Graphics.DrawString("TC: " + txtTc.Text, normal, firca, x, y);
            y += satir + 20;
            e.Graphics.DrawString("Sağlıklı günler dileriz...", new Font("Arial", 8, FontStyle.Italic), firca, x, y);
        }

        // --- SATIŞ YAP BUTONU (ANA İŞLEM) ---
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
                    // ---------------------------------------------------
                    // 1. STOK KONTROLÜ (Veritabanından)
                    // ---------------------------------------------------
                    SqlCommand cmdStok = new SqlCommand("Select adet From Ilaclar where ilacAdı=@p1", conn);
                    cmdStok.Parameters.AddWithValue("@p1", lueIlac.Text);
                    object stokObj = cmdStok.ExecuteScalar();
                    int mevcutStok = (stokObj != null) ? Convert.ToInt32(stokObj) : 0;

                    if (satilanAdet > mevcutStok)
                    {
                        MessageBox.Show("Stok Yetersiz! Mevcut Stok: " + mevcutStok, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                        return;
                    }

                    // ---------------------------------------------------
                    // 2. HASTA KAYIT İŞLEMİ (Yoksa Ekle)
                    // ---------------------------------------------------
                    SqlCommand cmdHasta = new SqlCommand("Select count(*) From Hastalar where TC=@p1", conn);
                    cmdHasta.Parameters.AddWithValue("@p1", txtTc.Text);
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
                        else
                        {
                            ad = tamIsim;
                        }

                        // SENİN YAZDIĞIN HASTA EKLEME KODU:
                        SqlCommand cmdEkle = new SqlCommand("Insert into Hastalar (TC, Ad, Soyad) values (@p1, @p2, @p3)", conn);
                        cmdEkle.Parameters.AddWithValue("@p1", txtTc.Text);
                        cmdEkle.Parameters.AddWithValue("@p2", ad);
                        cmdEkle.Parameters.AddWithValue("@p3", soyad);
                        cmdEkle.ExecuteNonQuery();
                    }

                    // ---------------------------------------------------
                    // 3. STOKTAN DÜŞME İŞLEMİ
                    // ---------------------------------------------------
                    SqlCommand cmdDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2", conn);
                    cmdDus.Parameters.AddWithValue("@p1", satilanAdet);
                    cmdDus.Parameters.AddWithValue("@p2", lueIlac.Text);
                    cmdDus.ExecuteNonQuery();

                    // ---------------------------------------------------
                    // 4. HAREKETLERE KAYDETME İŞLEMİ
                    // ---------------------------------------------------
                    SqlCommand cmdHareket = new SqlCommand("Insert into Hareketler (ilacAdi, adet, toplamFiyat, tarih, hastaAdi, tcNo) values (@p1,@p2,@p3,@p4,@p5,@p6)", conn);
                    cmdHareket.Parameters.AddWithValue("@p1", lueIlac.Text);
                    cmdHareket.Parameters.AddWithValue("@p2", satilanAdet);
                    cmdHareket.Parameters.AddWithValue("@p3", toplamTutar);
                    cmdHareket.Parameters.AddWithValue("@p4", dateTarih.DateTime);
                    cmdHareket.Parameters.AddWithValue("@p5", txtHastaAdi.Text);
                    cmdHareket.Parameters.AddWithValue("@p6", txtTc.Text);
                    cmdHareket.ExecuteNonQuery();

                    // ---------------------------------------------------
                    // 5. SADAKAT KONTROLÜ (Gemini İçin Sayaç)
                    // ---------------------------------------------------
                    SqlCommand cmdSayi = new SqlCommand("Select count(*) From Hareketler where tcNo=@p1", conn);
                    cmdSayi.Parameters.AddWithValue("@p1", txtTc.Text);
                    int alisverisSayisi = Convert.ToInt32(cmdSayi.ExecuteScalar());

                    conn.Close();

                    // BİLGİ VE FİŞ
                    MessageBox.Show("Satış Başarıyla Tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FisYazdir();

                    // ---------------------------------------------------
                    // 6. ANA MODÜLE HABER GÖNDERME (YENİ ÖZELLİK) 📡
                    // ---------------------------------------------------
                    FrmAnaModul anaForm = AnaModuluBul();

                    if (anaForm != null)
                    {
                        // A) Stok Kontrolünü Tetikle (Ana Modüldeki Bildirim Listesine düşer)
                        anaForm.StokKontrolu();

                        // B) Gemini Tavsiyesi Varsa Gönder (Listeye düşer)
                        // Limit şimdilik 2 (Test için)
                        if (alisverisSayisi >= 2)
                        {
                            string soru = $"Analiz: '{txtHastaAdi.Text}' isimli hasta, toplam {alisverisSayisi}. kez alışveriş yaptı.\n" +
                                          $"GÖREV: Eczacıya (bana) yönelik kısa, profesyonel bir bilgi notu yaz. Müşteriye hitap etme.\n" +
                                          $"İÇERİK: Sadakat durumunu belirt ve profesyonel bir aksiyon önerisi sun.\n" +
                                          $"FORMAT: Resmi ve net olsun.";

                            string tavsiye = await GeminiAsistani.Yorumla(soru);

                            // Ana modüldeki listeye ekliyoruz. Alert çıkmıyor, liste doluyor.
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