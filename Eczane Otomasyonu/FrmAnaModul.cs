using System;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Eczane_Otomasyonu
{
    public partial class FrmAnaModul : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmAnaModul()
        {
            InitializeComponent();
        }

        // --- FORM TANIMLAMALARI (Sürekli yeni pencere açmasın diye) ---
        FrmIlaclar fr_ilac;
        FrmHastalar fr_hasta; // <--- YENİ EKLEDİĞİMİZ KISIM
        FrmHareketler fr_hrkt;
        FrmRaporlar fr_rapor;

        // --- 1. İLAÇLAR BUTONU ---
        private void btnIlaclar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (fr_ilac == null || fr_ilac.IsDisposed)
            {
                fr_ilac = new FrmIlaclar();
                fr_ilac.MdiParent = this;
                fr_ilac.Show();
            }
        }

        // --- 2. HASTALAR BUTONU (GÜNCELLENDİ) ---
        private void btnHastalar_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Eğer form açık değilse oluştur ve aç
            if (fr_hasta == null || fr_hasta.IsDisposed)
            {
                fr_hasta = new FrmHastalar();
                fr_hasta.MdiParent = this; // Ana pencerenin içinde açılmasını sağlar
                fr_hasta.Show();
            }
        }

        // --- ÇIKIŞ BUTONU ---
        private void btnCikis_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnSatislar_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Eğer form açık değilse oluştur ve aç
            if (fr_hrkt == null || fr_hrkt.IsDisposed)
            {
                fr_hrkt = new FrmHareketler();
                fr_hrkt.MdiParent = this; // Ana pencerenin içinde açılmasını sağlar
                fr_hrkt.Show();
            }
        }
       
        private void btnRaporlar_ItemClick_1(object sender, ItemClickEventArgs e)
        {

            if (fr_rapor == null || fr_rapor.IsDisposed)
            {
                fr_rapor = new FrmRaporlar();
                fr_rapor.MdiParent = this;
                fr_rapor.Show();
            }
        }
    }
}