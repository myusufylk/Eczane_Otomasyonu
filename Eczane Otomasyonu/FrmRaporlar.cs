using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts; // Grafik kütüphanesi

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
            // Başlangıç Tarihleri: Bu ayın başı ve bugün
            DateTime bugun = DateTime.Now;
            dateBaslangic.DateTime = new DateTime(bugun.Year, bugun.Month, 1);
            dateBitis.DateTime = bugun;

            // Verileri Getir
            VerileriGuncelle();

            // Görsel Ayarlar
            GrafikGorselAyarlari();
        }

        // --- MERKEZİ GÜNCELLEME METODU ---
        void VerileriGuncelle()
        {
            string t1 = dateBaslangic.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
            string t2 = dateBitis.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";

            enCokSatanlar(t1, t2);
            gunlukCiro(t1, t2);
            KartBilgileriniGetir(t1, t2);
        }

        // --- 1. KPI KARTLARI (ÖZET BİLGİLER) ---
        void KartBilgileriniGetir(string t1, string t2)
        {
            SqlConnection conn = bgl.baglanti();
            try
            {
                // A) Toplam Ciro
                SqlCommand cmdCiro = new SqlCommand("SELECT SUM(toplamFiyat) FROM Hareketler WHERE tarih BETWEEN @p1 AND @p2", conn);
                cmdCiro.Parameters.AddWithValue("@p1", DateTime.Parse(t1));
                cmdCiro.Parameters.AddWithValue("@p2", DateTime.Parse(t2));
                object ciroSonuc = cmdCiro.ExecuteScalar();
                lblToplamCiro.Text = (ciroSonuc != DBNull.Value) ? string.Format("{0:C2}", ciroSonuc) : "0,00 ₺";

                // B) Toplam Hasta
                SqlCommand cmdHasta = new SqlCommand("SELECT COUNT(DISTINCT tcNo) FROM Hareketler WHERE tarih BETWEEN @p1 AND @p2", conn);
                cmdHasta.Parameters.AddWithValue("@p1", DateTime.Parse(t1));
                cmdHasta.Parameters.AddWithValue("@p2", DateTime.Parse(t2));
                object hastaSonuc = cmdHasta.ExecuteScalar();
                lblToplamHasta.Text = (hastaSonuc != null) ? hastaSonuc.ToString() + " Kişi" : "0 Kişi";

                // C) Toplam Stok
                SqlCommand cmdStok = new SqlCommand("SELECT SUM(adet) FROM Ilaclar", conn);
                object stokSonuc = cmdStok.ExecuteScalar();
                lblToplamStok.Text = (stokSonuc != null) ? stokSonuc.ToString() + " Kutu" : "0 Kutu";
            }
            catch (Exception ex) { MessageBox.Show("Kart Bilgisi Hatası: " + ex.Message); }
            finally { conn.Close(); }
        }

        // --- 2. GRAFİK VERİLERİ ---
        void enCokSatanlar(string t1, string t2)
        {
            SqlConnection conn = bgl.baglanti();
            try
            {
                SqlCommand komut = new SqlCommand("Select Top 5 ilacAdi, SUM(adet) from Hareketler WHERE (ilacAdi IS NOT NULL AND ilacAdi <> '') AND (tarih BETWEEN @p1 AND @p2) group by ilacAdi order by SUM(adet) desc", conn);
                komut.Parameters.AddWithValue("@p1", DateTime.Parse(t1));
                komut.Parameters.AddWithValue("@p2", DateTime.Parse(t2));

                SqlDataReader dr = komut.ExecuteReader();

                chartIlaclar.Series[0].Points.Clear();
                chartIlaclar.Series[0].ArgumentScaleType = ScaleType.Qualitative;
                chartIlaclar.Series[0].Label.TextPattern = "{A} (%{VP:P0})";

                while (dr.Read())
                {
                    chartIlaclar.Series[0].Points.AddPoint(dr[0].ToString(), int.Parse(dr[1].ToString()));
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        void gunlukCiro(string t1, string t2)
        {
            SqlConnection conn = bgl.baglanti();
            try
            {
                SqlCommand komut = new SqlCommand("Select FORMAT(tarih, 'dd.MM.yyyy'), SUM(toplamFiyat) from Hareketler WHERE tarih BETWEEN @p1 AND @p2 group by FORMAT(tarih, 'dd.MM.yyyy') ORDER BY MIN(tarih)", conn);
                komut.Parameters.AddWithValue("@p1", DateTime.Parse(t1));
                komut.Parameters.AddWithValue("@p2", DateTime.Parse(t2));

                SqlDataReader dr = komut.ExecuteReader();

                chartCiro.Series[0].Points.Clear();
                while (dr.Read())
                {
                    chartCiro.Series[0].Points.AddPoint(Convert.ToDateTime(dr[0]), Convert.ToDouble(dr[1]));
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        // --- 3. BUTON OLAYLARI ---
        private void btnFiltrele_Click(object sender, EventArgs e)
        {
            VerileriGuncelle();
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF Dosyası|*.pdf";
            save.FileName = "Ciro_Raporu_" + DateTime.Now.ToShortDateString();
            if (save.ShowDialog() == DialogResult.OK)
            {
                chartCiro.ExportToPdf(save.FileName);
                MessageBox.Show("PDF Başarıyla Kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Dosyası|*.xlsx";
            save.FileName = "Satis_Analizi_" + DateTime.Now.ToShortDateString();
            if (save.ShowDialog() == DialogResult.OK)
            {
                chartCiro.ExportToXlsx(save.FileName);
                MessageBox.Show("Excel Dosyası Oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- GÖRSEL AYARLAR ---
        void GrafikGorselAyarlari()
        {
            chartIlaclar.PaletteName = "Office 2013";
            chartCiro.PaletteName = "Office 2013";

            Series seriIlac = chartIlaclar.Series[0];
            seriIlac.ChangeView(ViewType.Doughnut);

            ((DoughnutSeriesLabel)seriIlac.Label).Position = PieSeriesLabelPosition.Outside;
            ((DoughnutSeriesLabel)seriIlac.Label).TextColor = Color.Black;
            ((DoughnutSeriesLabel)seriIlac.Label).BackColor = Color.Transparent;
            ((DoughnutSeriesLabel)seriIlac.Label).Border.Visibility = DevExpress.Utils.DefaultBoolean.False;

            chartIlaclar.AnimationStartMode = ChartAnimationMode.OnLoad;

            if (chartCiro.Diagram is XYDiagram)
            {
                XYDiagram diyagram = (XYDiagram)chartCiro.Diagram;
                diyagram.AxisY.Label.TextPattern = "{V:C2}";
                diyagram.AxisY.GridLines.Color = Color.LightGray;

                // --- İŞTE DÜZELTİLEN SATIR BURASI ---
                // System.Drawing... yerine DevExpress.XtraCharts... kullanıyoruz.
                diyagram.AxisY.GridLines.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash;
            }

            chartCiro.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
            chartCiro.AnimationStartMode = ChartAnimationMode.OnLoad;
        }
    }
}