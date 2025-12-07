using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // SQL işlemleri için gerekli kütüphane
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices; // Formu hareket ettirmek için gerekli kütüphane
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eczane_Otomasyonu
{
    // Not: DevExpress form özelliklerini tam kullanmak istersen ": Form" yerine ": DevExpress.XtraEditors.XtraForm" yazabilirsin.
    public partial class FrmGiris : Form
    {
        public FrmGiris()
        {
            InitializeComponent();
        }

        // --- 1. ADIM: SQL BAĞLANTISI ---
        // SqlBaglantisi sınıfını oluşturmuştuk, onu burada çağırıyoruz.
        SqlBaglantisi bgl = new SqlBaglantisi();

        // --- 2. ADIM: ÇERÇEVESİZ FORMU HAREKET ETTİRME KODLARI ---
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        // Formun veya Panelin üzerine basılı tutup sürükleyince çalışacak olay
        private void FrmGiris_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // --- 3. ADIM: KAPATMA BUTONU (X) ---
        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Uygulamayı tamamen kapatır
        }

        // --- 4. ADIM: GİRİŞ YAP BUTONU ---
        private void btnGiris_Click(object sender, EventArgs e)
        {
            // Veritabanı bağlantısını açıp sorguyu hazırlıyoruz
            // DİKKAT: Tablo adını 'Kullanicilar', sütunları 'kullaniciAdi' ve 'sifre' olarak düzelttik.
            SqlCommand komut = new SqlCommand("Select * From Kullanicilar where kullaniciAdi=@p1 and sifre=@p2", bgl.baglanti());

            komut.Parameters.AddWithValue("@p1", txtKullaniciAd.Text);
            komut.Parameters.AddWithValue("@p2", txtSifre.Text);

            SqlDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {// Giriş Başarılıysa:
                MessageBox.Show("Giriş Başarılı! Hoşgeldiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 1. Ana formu oluştur
                FrmAnaModul fr = new FrmAnaModul();

                // 2. Ana formu göster
                fr.Show();

                // 3. Giriş ekranını gizle
                this.Hide();
               
            }
            else
            {
                // Giriş Hatalıysa
                MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            bgl.baglanti().Close();
        }
    }
}