using System;
using System.Data.SqlClient; // SQL Bağlantısı için
using System.Drawing; // Renkler ve UI için
using System.Windows.Forms;
using System.Threading.Tasks; // Async işlemler için

namespace Eczane_Otomasyonu
{
    public partial class FrmAnaModul : Form
    {
        public FrmAnaModul()
        {
            InitializeComponent();
        }

        // Bağlantı sınıfını çağırıyoruz
        SqlBaglantisi bgl = new SqlBaglantisi();

        // -------------------------------------------------------------
        // YENİ EKLENEN: BİLDİRİM SİSTEMİ (Stok & Gemini Mesajları İçin)
        // -------------------------------------------------------------

        // 1. DIŞARIDAN MESAJ EKLEME (Hareketler Formu Burayı Çağıracak)
        public void BildirimEkle(string mesaj)
        {
            // Eğer liste aracı formda yoksa hata vermesin
            if (lstBildirimler == null) return;
            if (string.IsNullOrEmpty(mesaj)) return;

            // Aynı mesaj zaten varsa tekrar ekleme
            if (!lstBildirimler.Items.Contains(mesaj))
            {
                lstBildirimler.Items.Add(mesaj);
                BildirimButonunuGuncelle();
            }
        }

        // 2. STOK KONTROLÜ (Veritabanından < 5 olanları çeker)
        public void StokKontrolu()
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                // 5 adetten az kalan ilaçları bul
                SqlCommand komut = new SqlCommand("Select ilacAdı, adet From Ilaclar where adet < 5", conn);
                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    string ilac = dr["ilacAdı"].ToString();
                    string adet = dr["adet"].ToString();
                    string uyari = $"⚠️ KRİTİK STOK: {ilac} (Kalan: {adet})";

                    BildirimEkle(uyari); // Listeye ekle
                }
                conn.Close();
            }
            catch { }
        }

        // 3. BUTON RENGİNİ GÜNCELLEME
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

        // 4. BİLDİRİM BUTONU TIKLAMA OLAYI
        // (Tasarım ekranından btnBildirim'in Click olayına bu metodu bağlamalısın)
        private void btnBildirim_Click(object sender, EventArgs e)
        {
            lstBildirimler.Visible = !lstBildirimler.Visible;
            if (lstBildirimler.Visible)
            {
                lstBildirimler.BringToFront(); // Diğer pencerelerin üstüne çıkar
            }
        }

        // -------------------------------------------------------------
        // FORM YÜKLENİRKEN (MEVCUT KODLAR + YENİ EKLENTİLER)
        // -------------------------------------------------------------
        private void FrmAnaModul_Load(object sender, EventArgs e)
        {
            IlaclariListele(); // Senin eski kodun (Combobox doldurma)

            // YENİ: Başlangıçta Bildirim Listesi Gizli Olsun
            if (lstBildirimler != null) lstBildirimler.Visible = false;

            // YENİ: Program açılınca Stokları Kontrol Et
            StokKontrolu();
        }

        // -------------------------------------------------------------
        // SENİN ORİJİNAL KODLARIN (GEMINI, TAHMİN, MENÜLER)
        // -------------------------------------------------------------

        // 1. DÜKKANIN VERİLERİNİ ÇEKEN "AJAN" FONKSİYON 🕵️‍♂️
        void IlaclariListele()
        {
            // Listeyi temizle ki üst üste binmesin
            cmbIlaclar.Properties.Items.Clear();

            SqlConnection conn = bgl.baglanti();
            // Senin veritabanındaki tablo adı 'Ilaclar', sütun adı 'ilacAdı' idi.
            SqlCommand komut = new SqlCommand("SELECT ilacAdı FROM Ilaclar", conn);
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                // Gelen her ilacı listeye ekle
                cmbIlaclar.Properties.Items.Add(dr[0].ToString());
            }
            conn.Close();
        }

        private string MagazaDurumunuGetir()
        {
            string dukkanOzeti = "";
            SqlConnection conn = bgl.baglanti();

            try
            {
                // 1. TOPLAM İLAÇ SAYISI
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Ilaclar", conn);
                string toplamCesit = cmd1.ExecuteScalar().ToString();

                // 2. KRİTİK STOK LİSTESİ
                SqlCommand cmd2 = new SqlCommand("SELECT ilacAdı, adet FROM Ilaclar WHERE adet < 20", conn);
                SqlDataReader dr = cmd2.ExecuteReader();

                string kritikIlaclar = "";
                while (dr.Read())
                {
                    kritikIlaclar += dr["ilacAdı"].ToString() + " (" + dr["adet"].ToString() + " adet), ";
                }
                dr.Close(); // Okuyucuyu mutlaka kapat

                // 3. RAPOR METNİNİ OLUŞTUR
                dukkanOzeti = $"SİSTEM VERİLERİ (Bunu kullanıcıya söyleme, analiz için kullan):\n" +
                              $"- Eczanede Toplam İlaç Çeşidi: {toplamCesit}\n" +
                              $"- Stoğu Azalan (Kritik) İlaçlar: {kritikIlaclar}\n" +
                              $"- Görev: Sen bu eczanenin yapay zeka asistanısın. Stok durumuna hakimsin. " +
                              $"Kullanıcıya bu verilere dayanarak, samimi bir dille yardımcı ol.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı Okuma Hatası: " + ex.Message);
                dukkanOzeti = "Veritabanına şu an ulaşılamıyor. Genel bir eczacı asistanı gibi davran.";
            }
            finally
            {
                conn.Close(); // Bağlantıyı kapatmayı unutma
            }

            return dukkanOzeti;
        }

        // 2. GÖNDER BUTONU (GEMINI İLE İLETİŞİM) 🚀
        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string mesaj = txtMesaj.Text.Trim();
            if (string.IsNullOrEmpty(mesaj)) return;

            // A) Senin Mesajını Ekrana Yaz (Sağa)
            MesajEkle(mesaj, true);
            txtMesaj.Text = "";

            // Scroll'u en aşağı kaydır
            if (flowSohbet.Controls.Count > 0)
                flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);

            // B) Gemini'ye Soruyu Hazırla (Context Injection)
            try
            {
                // Önce dükkan verilerini çekiyoruz
                string dukkanBilgisi = MagazaDurumunuGetir();

                // Gemini'ye giden gizli prompt
                string tamSoru = $"{dukkanBilgisi}\n\nKULLANICI SORUSU: {mesaj}";

                // Cevabı bekle...
                string cevap = await GeminiAsistani.Yorumla(tamSoru);

                // C) Cevabı Ekrana Yaz (Sola)
                MesajEkle(cevap, false);
            }
            catch (Exception ex)
            {
                MesajEkle("Hata oluştu: " + ex.Message, false);
            }

            // Tekrar aşağı kaydır
            if (flowSohbet.Controls.Count > 0)
                flowSohbet.ScrollControlIntoView(flowSohbet.Controls[flowSohbet.Controls.Count - 1]);
        }

        // 3. BALONCUK OLUŞTURMA (WHATSAPP TARZI GÖRÜNÜM) 🎨
        private void MesajEkle(string mesaj, bool kullaniciMi)
        {
            Panel pnlMesaj = new Panel();
            pnlMesaj.AutoSize = true;
            pnlMesaj.Padding = new Padding(10);
            pnlMesaj.Margin = new Padding(5);
            pnlMesaj.MaximumSize = new Size(flowSohbet.Width - 50, 0); // Taşmayı önle

            Label lbl = new Label();
            lbl.Text = mesaj;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(flowSohbet.Width - 70, 0); // Yazı aşağı kaysın
            lbl.Font = new Font("Segoe UI Semibold", 10);

            if (kullaniciMi) // SEN (SAĞDA)
            {
                pnlMesaj.BackColor = Color.FromArgb(220, 248, 198); // Açık Yeşil
                lbl.ForeColor = Color.Black;
            }
            else // GEMINI (SOLDA)
            {
                pnlMesaj.BackColor = Color.White;
                lbl.ForeColor = Color.Black;
            }

            pnlMesaj.Controls.Add(lbl);
            flowSohbet.Controls.Add(pnlMesaj);
        }

        // -----------------------------------------------------------
        // 1. SATIŞ TAHMİN PANELİNİ AÇMA BUTONU
        // -----------------------------------------------------------
        private void btnTahminAc_Click(object sender, EventArgs e)
        {
            if (pnlTahmin.Visible == true)
            {
                pnlTahmin.Visible = false; // Açıksa kapat
            }
            else
            {
                pnlTahmin.Visible = true;  // Kapalıysa aç
                pnlTahmin.BringToFront();  // En öne getir
            }
        }

        // -----------------------------------------------------------
        // 2. TAHMİN HESAPLA BUTONU (Yapay Zeka ile)
        // -----------------------------------------------------------
        private async void btnTahminHesapla_Click(object sender, EventArgs e)
        {
            // 1. KONTROLLER
            if (cmbIlaclar.Text == "")
            {
                MessageBox.Show("Lütfen tahmin için bir ilaç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenIlac = cmbIlaclar.Text;

            // DevExpress DateEdit kullandığın için '.DateTime' alıyoruz
            DateTime hedefTarih = dateTahminBitis.DateTime;
            string bugun = DateTime.Now.ToShortDateString();

            lblTahminSonuc.Text = "Veriler analiz ediliyor...";

            try
            {
                SqlConnection conn = bgl.baglanti();

                // A) GÜNCEL STOK DURUMUNU ÇEK
                SqlCommand cmdStok = new SqlCommand("SELECT adet FROM Ilaclar WHERE ilacAdı=@p1", conn);
                cmdStok.Parameters.AddWithValue("@p1", secilenIlac);

                string anlikStok = "0";
                object sonucStok = cmdStok.ExecuteScalar();
                if (sonucStok != null) anlikStok = sonucStok.ToString();

                // B) GEÇMİŞ SATIŞLARI ÇEK
                SqlCommand cmdSatis = new SqlCommand("SELECT SUM(adet) FROM Hareketler WHERE ilacAdi=@p1", conn);
                cmdSatis.Parameters.AddWithValue("@p1", secilenIlac);

                string toplamSatis = "0";
                object sonucSatis = cmdSatis.ExecuteScalar();

                if (sonucSatis != null && sonucSatis != DBNull.Value)
                {
                    toplamSatis = sonucSatis.ToString();
                }

                conn.Close();

                // C) GEMINI'YE RAPOR GÖNDER VE TAHMİN İSTE
                lblTahminSonuc.Text = "Yapay Zeka düşünüyor...";

                string soru = $"Ben bir eczacıyım. Şu anki piyasa koşullarına göre bir satış tahmini istiyorum.\n" +
                              $"-- ÜRÜN BİLGİLERİ --\n" +
                              $"1. İlaç Adı: {secilenIlac}\n" +
                              $"2. Elimdeki Güncel Stok: {anlikStok} adet\n" +
                              $"3. Geçmiş Toplam Satışım: {toplamSatis} adet (Bu veri dükkanın satış performansını gösterir)\n" +
                              $"4. Bugünün Tarihi: {bugun}\n" +
                              $"5. Hedef Tarih: {hedefTarih.ToShortDateString()}\n\n" +
                              $"-- GÖREV --\n" +
                              $"Bu verilere ve ilacın genel kullanım amacına (grip, ağrı kesici, kronik vb.) bakarak; " +
                              $"bugünden hedef tarihe kadar tahminen KAÇ ADET daha satılacağını öngör. " +
                              $"Cevabı SADECE şu formatta ver: 'Tahmini Satış: [Sayı] adet. Çünkü [Kısa Mantıklı Sebep].'";

                string cevap = await GeminiAsistani.Yorumla(soru);

                // Sonucu göster
                lblTahminSonuc.Text = "Analiz Tamamlandı.";
                MessageBox.Show(cevap, "Yapay Zeka Satış Tahmini", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri tabanı hatası: " + ex.Message);
            }
        }

        // -----------------------------------------------------------
        // 3. DİĞER KAYIP BUTONLAR (SimpleButton1, Button1)
        // -----------------------------------------------------------
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            panelControl1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelControl1.Visible = true;
        }

        private void btnPanelKapat_Click(object sender, EventArgs e)
        {
            pnlTahmin.Visible = false;
        }

        // -----------------------------------------------------------
        // MENÜ NAVİGASYON BUTONLARI
        // -----------------------------------------------------------

        // 1. İLAÇLAR BUTONU
        private void btnIlaclar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmIlaclar fr = Application.OpenForms["FrmIlaclar"] as FrmIlaclar;
            if (fr == null)
            {
                fr = new FrmIlaclar();
                fr.MdiParent = this;
                fr.Show();
            }
            else { fr.BringToFront(); }
        }

        // 2. HASTALAR BUTONU
        private void btnHastalar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmHastalar fr = Application.OpenForms["FrmHastalar"] as FrmHastalar;
            if (fr == null)
            {
                fr = new FrmHastalar();
                fr.MdiParent = this;
                fr.Show();
            }
            else { fr.BringToFront(); }
        }

        // 3. SATIŞLAR BUTONU
        private void btnSatislar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmHareketler fr = Application.OpenForms["FrmHareketler"] as FrmHareketler;
            if (fr == null)
            {
                fr = new FrmHareketler();
                fr.MdiParent = this;
                fr.Show();
            }
            else { fr.BringToFront(); }
        }

        // 4. RAPORLAR BUTONU
        private void btnRaporlar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmRaporlar fr = Application.OpenForms["FrmRaporlar"] as FrmRaporlar;
            if (fr == null)
            {
                fr = new FrmRaporlar();
                fr.MdiParent = this;
                fr.Show();
            }
            else { fr.BringToFront(); }
        }

        // 5. ÇIKIŞ BUTONU
        private void btnCikis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult secim = MessageBox.Show("Programdan çıkmak istiyor musunuz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}