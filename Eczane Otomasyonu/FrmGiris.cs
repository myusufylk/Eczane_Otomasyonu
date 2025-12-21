using System;
using System.Data.SqlClient; // SQL işlemleri için
using System.Windows.Forms;
using System.Text.RegularExpressions; // Telefon format kontrolü için
using System.Net.Mail; // Mail format kontrolü için
using DevExpress.XtraEditors; // Maskeleme özellikleri için kütüphane

namespace Eczane_Otomasyonu
{
    public partial class FrmGiris : Form
    {
        public FrmGiris()
        {
            InitializeComponent();
        }

        // Senin SqlBaglantisi sınıfını çağırıyoruz
        SqlBaglantisi bgl = new SqlBaglantisi();

        // =============================================================
        // 1. FORM YÜKLENİRKEN (BAŞLANGIÇ AYARLARI)
        // =============================================================
        private void FrmGiris_Load(object sender, EventArgs e)
        {
            // Kayıt ve Şifre panellerini başlangıçta gizle
            pnlKayit.Visible = false;
            pnlSifreUnuttum.Visible = false;

            // İstersen panelleri formun ortasına konumlandırabilirsin:
            // pnlKayit.Location = new Point(10, 10); 

            // --- YENİ EKLENEN: TELEFON NUMARASI MASKESİ 📞 ---
            // Bu ayar sayesinde kullanıcı (555) 123-4567 formatını görür.

            // 1. Kayıt Ekranı Telefonu
            if (txtTel_Kayit.Properties != null)
            {
                txtTel_Kayit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                txtTel_Kayit.Properties.Mask.EditMask = "(999) 000-0000";
                txtTel_Kayit.Properties.Mask.UseMaskAsDisplayFormat = true;
            }

            // 2. Şifre Unuttum Ekranı Telefonu
            if (txtTel_Unuttum.Properties != null)
            {
                txtTel_Unuttum.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                txtTel_Unuttum.Properties.Mask.EditMask = "(999) 000-0000";
                txtTel_Unuttum.Properties.Mask.UseMaskAsDisplayFormat = true;
            }
        }

        // =============================================================
        // 2. GİRİŞ YAP BUTONU (ANA EKRAN)
        // =============================================================
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                // Bağlantıyı aç
                SqlConnection conn = bgl.baglanti();

                SqlCommand komut = new SqlCommand("SELECT * FROM TBL_KULLANICILAR WHERE KULLANICIADI=@p1 AND SIFRE=@p2", conn);
                komut.Parameters.AddWithValue("@p1", txtKullaniciAd.Text); // Giriş ekranındaki kullanıcı adı kutusu
                komut.Parameters.AddWithValue("@p2", txtSifre.Text);       // Giriş ekranındaki şifre kutusu

                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    // --- BURASI ÇOK ÖNEMLİ: KULLANICI ID'SİNİ ALIYORUZ 🔑 ---
                    // Artık program bu ID'yi hafızada tutacak ve diğer formlarda
                    // sadece bu ID'ye ait verileri gösterecek.

                    // Not: Veritabanındaki ID kolonunun adının "ID" olduğunu varsayıyorum. 
                    // Eğer kolon adı farklıysa (örn: KULLANICIID) parantez içini ona göre değiştir.
                    if (dr["ID"] != DBNull.Value)
                    {
                        MevcutKullanici.Id = Convert.ToInt32(dr["ID"]);
                    }

                    // Kullanıcı adını da tutalım, belki ekranda "Hoşgeldin Yusuf" yazarız.
                    MevcutKullanici.KullaniciAdi = dr["KULLANICIADI"].ToString();
                    // -----------------------------------------------------------

                    // Giriş Başarılı -> Ana Modülü Aç
                    FrmAnaModul fr = new FrmAnaModul();
                    fr.Show();
                    this.Hide(); // Giriş ekranını gizle
                }
                else
                {
                    MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Bağlantıyı kapat
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı Hatası: " + ex.Message);
            }
        }

        // =============================================================
        // 3. KAYIT OL BUTONU (VALIDASYONLU)
        // =============================================================
        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            try
            {
                // A) Boşluk Kontrolü
                if (txtKadi_Kayit.Text == "" || txtSifre_Kayit.Text == "")
                {
                    MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Maskeden gelen ( ) - karakterlerini temizleyip sadece rakamları alıyoruz
                string temizTelefon = txtTel_Kayit.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                // B) Telefon Format Kontrolü (Sadece 10 veya 11 haneli rakam)
                // (Maske kullandığımız için temizTelefon değişkenini kontrol ediyoruz)
                if (!Regex.IsMatch(temizTelefon, @"^\d{10,11}$"))
                {
                    MessageBox.Show("Lütfen geçerli bir telefon numarası giriniz! (Sadece rakam, örn: 05551234567)", "Hatalı Telefon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // C) Mail Format Kontrolü
                try
                {
                    MailAddress mail = new MailAddress(txtMail_Kayit.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Lütfen geçerli bir E-Posta adresi giriniz! (Örn: isim@mail.com)", "Hatalı Mail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // D) Veritabanına Kayıt
                SqlConnection conn = bgl.baglanti();

                // Not: ID kolonu otomatik artan (Identity) olduğu için Insert sorgusuna yazılmaz.
                SqlCommand komut = new SqlCommand("INSERT INTO TBL_KULLANICILAR (KULLANICIADI, SIFRE, TELEFON, MAIL) VALUES (@p1, @p2, @p3, @p4)", conn);
                komut.Parameters.AddWithValue("@p1", txtKadi_Kayit.Text);
                komut.Parameters.AddWithValue("@p2", txtSifre_Kayit.Text);
                komut.Parameters.AddWithValue("@p3", temizTelefon); // Temizlenmiş telefonu kaydediyoruz
                komut.Parameters.AddWithValue("@p4", txtMail_Kayit.Text);

                komut.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Kayıt Başarıyla Oluşturuldu! Giriş yapabilirsiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Paneli kapat, ana ekrana dön ve kutuları temizle
                pnlKayit.Visible = false;
                txtKadi_Kayit.Text = ""; txtSifre_Kayit.Text = ""; txtTel_Kayit.Text = ""; txtMail_Kayit.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt Hatası: " + ex.Message);
            }
        }

        // =============================================================
        // 4. ŞİFRE GÜNCELLEME BUTONU (VALIDASYONLU)
        // =============================================================
        private void btnSifreGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                // Maskeden gelen ( ) - karakterlerini temizleyip sadece rakamları alıyoruz
                string temizTelefon = txtTel_Unuttum.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                // A) Telefon Format Kontrolü
                if (!Regex.IsMatch(temizTelefon, @"^\d{10,11}$"))
                {
                    MessageBox.Show("Telefon numarası hatalı formatta!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // B) Mail Format Kontrolü
                try
                {
                    MailAddress mail = new MailAddress(txtMail_Unuttum.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Mail adresi geçersiz formatta!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // C) Veritabanı Kontrolü
                SqlConnection conn = bgl.baglanti();

                // Kullanıcıyı doğrula (Temizlenmiş telefon ile)
                SqlCommand komut = new SqlCommand("SELECT * FROM TBL_KULLANICILAR WHERE KULLANICIADI=@p1 AND TELEFON=@p2 AND MAIL=@p3", conn);
                komut.Parameters.AddWithValue("@p1", txtKAdı_Unuttum.Text);
                komut.Parameters.AddWithValue("@p2", temizTelefon);
                komut.Parameters.AddWithValue("@p3", txtMail_Unuttum.Text);

                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    // Bilgiler Doğru -> Şifreyi Güncelle
                    dr.Close(); // Okuyucuyu kapat

                    SqlCommand komutGuncelle = new SqlCommand("UPDATE TBL_KULLANICILAR SET SIFRE=@sifre WHERE KULLANICIADI=@kadi", conn);
                    komutGuncelle.Parameters.AddWithValue("@sifre", txtYeniSifre.Text);
                    komutGuncelle.Parameters.AddWithValue("@kadi", txtKAdı_Unuttum.Text);

                    komutGuncelle.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Şifreniz başarıyla değiştirildi. Yeni şifrenizle giriş yapabilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Paneli kapat ve temizle
                    pnlSifreUnuttum.Visible = false;
                    txtKAdı_Unuttum.Text = ""; txtYeniSifre.Text = ""; txtTel_Unuttum.Text = ""; txtMail_Unuttum.Text = "";
                }
                else
                {
                    MessageBox.Show("Girdiğiniz bilgiler sistemdekilerle uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem Hatası: " + ex.Message);
            }
        }

        // =============================================================
        // 5. EKRAN GEÇİŞLERİ VE DİĞER BUTONLAR
        // =============================================================

        // "Kayıt Ol" Linkine Tıklayınca
        private void lnkKayitOl_Click(object sender, EventArgs e)
        {
            pnlSifreUnuttum.Visible = false;
            pnlKayit.Visible = true;
            pnlKayit.BringToFront();
        }

        // "Şifremi Unuttum" Linkine Tıklayınca
        private void lnkSifreUnuttum_Click(object sender, EventArgs e)
        {
            pnlKayit.Visible = false;
            pnlSifreUnuttum.Visible = true;
            pnlSifreUnuttum.BringToFront();
        }

        // Panellerdeki "Geri Dön" Butonları
        private void btnGeri_Kayit_Click(object sender, EventArgs e)
        {
            pnlKayit.Visible = false;
        }

        private void btnGeri_Unuttum_Click(object sender, EventArgs e)
        {
            pnlSifreUnuttum.Visible = false;
        }

        // Programı Kapatma Butonu (X)
        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}