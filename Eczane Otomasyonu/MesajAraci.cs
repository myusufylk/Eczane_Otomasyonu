using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eczane_Otomasyonu
{
    public partial class MesajAraci : UserControl
    {
        public MesajAraci()
        {
            InitializeComponent();
        }

        // --- TASARIM ÖGELERİ ---
        private Label lblMesaj;
        private Label lblZaman;
        private PictureBox pbAvatar;
        private Panel pnlBalon;

        // Mesajı Ayarlayan Fonksiyon
        public void MesajAyarla(string mesaj, bool kullaniciMi, Image avatarResmi)
        {
            // 1. Temel Ayarlar
            this.Width = 400; // Baloncuğun kapsayıcı genişliği
            this.Padding = new Padding(10);
            this.BackColor = Color.Transparent;

            // 2. Avatar (Profil Resmi)
            pbAvatar = new PictureBox();
            pbAvatar.Size = new Size(40, 40);
            pbAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            pbAvatar.Image = avatarResmi; // Resmi dışarıdan alıyoruz

            // 3. Mesaj Balonu (Panel)
            pnlBalon = new Panel();
            pnlBalon.Padding = new Padding(10);

            // 4. Mesaj Metni
            lblMesaj = new Label();
            lblMesaj.Text = mesaj;
            lblMesaj.MaximumSize = new Size(250, 0); // Metin çok uzunsa aşağı kaysın
            lblMesaj.AutoSize = true;
            lblMesaj.Font = new Font("Segoe UI", 10);
            lblMesaj.BackColor = Color.Transparent;

            // 5. Zaman
            lblZaman = new Label();
            lblZaman.Text = DateTime.Now.ToString("HH:mm");
            lblZaman.Font = new Font("Segoe UI", 7, FontStyle.Italic);
            lblZaman.AutoSize = true;
            lblZaman.ForeColor = Color.Gray;
            lblZaman.BackColor = Color.Transparent;

            // --- KONUMLANDIRMA (WHATSAPP MANTIĞI) ---
            if (kullaniciMi)
            {
                // SAĞ TARAFA YASLA (Sen)
                pbAvatar.Location = new Point(this.Width - 50, 5); // Avatar en sağda

                pnlBalon.BackColor = Color.FromArgb(220, 248, 198); // WhatsApp Yeşili

                // Balonu ve metni oluşturmadan konum hesabı yapamayız, önce ekleyelim
            }
            else
            {
                // SOL TARAFA YASLA (Yapay Zeka)
                pbAvatar.Location = new Point(10, 5); // Avatar en solda

                pnlBalon.BackColor = Color.White; // Beyaz
            }

            // Kontrolleri birbirine ekle
            pnlBalon.Controls.Add(lblMesaj);
            pnlBalon.Controls.Add(lblZaman);

            // Metin konumu
            lblMesaj.Location = new Point(10, 10);

            // Panel boyutunu metne göre ayarla (+ boşluklar)
            pnlBalon.Size = new Size(lblMesaj.Width + 20, lblMesaj.Height + 35);

            // Zaman etiketini balonun sağ altına koy
            lblZaman.Location = new Point(pnlBalon.Width - lblZaman.Width - 5, pnlBalon.Height - 15);

            // ŞİMDİ BALONUN KONUMUNU AYARLA
            if (kullaniciMi)
            {
                // Sağa yaslı (Avatarın soluna)
                pnlBalon.Location = new Point(this.Width - 60 - pnlBalon.Width, 5);
            }
            else
            {
                // Sola yaslı (Avatarın sağına)
                pnlBalon.Location = new Point(60, 5);
            }

            // UserControl'ün kendi yüksekliğini ayarla ki üst üste binmesinler
            this.Height = pnlBalon.Height + 20;

            // Ekrana Ekle
            this.Controls.Add(pbAvatar);
            this.Controls.Add(pnlBalon);
        }
    }
}