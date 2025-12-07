using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms; // MessageBox için gerekli
using DevExpress.XtraEditors;

namespace Eczane_Otomasyonu
{
    public partial class FrmRaporlar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();

        public FrmRaporlar()
        {
            InitializeComponent();
        }

        private void FrmRaporlar_Load(object sender, EventArgs e)
        {
            enCokSatanlar();
            gunlukCiro();
        }

        // --- GRAFİK 1: EN ÇOK SATAN 5 İLAÇ (PASTA) ---
        void enCokSatanlar()
        {
            SqlConnection conn = bgl.baglanti();
            try
            {
                // İsmi boş olmayan ilk 5 ilacı getir
                SqlCommand komut = new SqlCommand("Select Top 5 ilacAdi, SUM(adet) from Hareketler WHERE ilacAdi IS NOT NULL AND ilacAdi <> '' group by ilacAdi order by SUM(adet) desc", conn);
                SqlDataReader dr = komut.ExecuteReader();

                chartIlaclar.Series[0].Points.Clear();

                // 1. İsimlerin (String) girileceğini belirtiyoruz
                chartIlaclar.Series[0].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;

                // 2. GÖRÜNÜM AYARI: İlaç Adı ve Yüzdeyi yan yana yaz (Örnek: Parol: %25,00)
                chartIlaclar.Series[0].Label.TextPattern = "{A} (%{VP:P2})";

                // İstersen sadece isim için: "{A}" 
                // İstersen İsim ve Adet için: "{A} - {V} Adet" yazabilirsin.

                while (dr.Read())
                {
                    chartIlaclar.Series[0].Points.AddPoint(dr[0].ToString(), int.Parse(dr[1].ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("En Çok Satanlar Hatası: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // --- GRAFİK 2: GÜNLÜK CİRO (SÜTUN) ---
        void gunlukCiro()
        {
            SqlConnection conn = bgl.baglanti();
            try
            {
               
                // Tarihi 'Gün.Ay.Yıl' formatına çevirip grupluyoruz
                SqlCommand komut = new SqlCommand("Select FORMAT(tarih, 'dd.MM.yyyy'), SUM(toplamFiyat) from Hareketler group by FORMAT(tarih, 'dd.MM.yyyy')", conn);
                SqlDataReader dr = komut.ExecuteReader();

                chartCiro.Series[0].Points.Clear();

                while (dr.Read())
                {
                    // Grafiğe ekle: (Tarih, Toplam Para)
                    // Veritabanından gelen string tarihi DateTime'a, parayı double'a çeviriyoruz
                    chartCiro.Series[0].Points.AddPoint(Convert.ToDateTime(dr[0]), Convert.ToDouble(dr[1]));
                }
            }
            catch (Exception ex)
            {
                // Hata varsa ekranda görelim
                MessageBox.Show("Günlük Ciro Grafiği Hatası: " + ex.Message);
            }
            finally
            {
                // Hata olsa da olmasa da bağlantıyı kapat
                conn.Close();
            }
        }
    }
}