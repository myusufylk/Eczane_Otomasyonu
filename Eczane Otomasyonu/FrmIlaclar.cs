using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eczane_Otomasyonu
{
    public partial class FrmIlaclar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();
        public string resimDosyaYolu = "";
        bool islemYapiliyor = false;

        public FrmIlaclar()
        {
            InitializeComponent();

            // --- 🛠️ GÖRÜNÜM AYARLARI (TAM KAPLAMA) ---

            // 1. Tabloyu formun içine tam oturt
            gridControl1.Dock = DockStyle.Fill;

            // 2. BURASI DEĞİŞTİ: Sütunlar ekranı tamamen doldursun (Sağda boşluk kalmasın)
            gridView1.OptionsView.ColumnAutoWidth = true;

            // 3. Kaydırma çubukları otomatik (Sığmazsa çıkar, sığarsa çıkmaz)
            gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;

            // ------------------------------------------------

            // Olayları Bağla
            try
            {
                gridView1.RowClick -= gridView1_RowClick;
                gridView1.RowClick += gridView1_RowClick;

                Bagla("btnKaydet", btnKaydet_Click);
                Bagla("btnSil", btnSil_Click);
                Bagla("btnGuncelle", btnGuncelle_Click);
                Bagla("btnResimSec", btnResimSec_Click);
            }
            catch { }
        }

        void Bagla(string butonAdi, EventHandler olay)
        {
            var btn = this.Controls.Find(butonAdi, true);
            if (btn.Length > 0)
            {
                btn[0].Click -= olay;
                btn[0].Click += olay;
            }
        }

        private void FrmIlaclar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        void listele()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select siraNo, ilacKodu, ilacAdı, fiyat, adet, resim From Ilaclar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;

                // --- 🛠️ ÇÖZÜM BURASI ---
                // Grid'deki sütunları veritabanına göre sıfırlayıp tekrar oluşturur.
                // Böylece "siraNo" ismi eşleşmeme sorunu ortadan kalkar.
                gridView1.PopulateColumns();

                // --- GÖRÜNÜM AYARLARI ---
                gridView1.OptionsView.ColumnAutoWidth = true; // Ekrana yayıl
                gridView1.BestFitColumns(); // İçeriğe göre genişle

                // Başlıkları Güzelleştir (İsteğe Bağlı)
                gridView1.Columns["siraNo"].Caption = "SIRA NO";
                gridView1.Columns["ilacKodu"].Caption = "İLAÇ KODU";
                gridView1.Columns["ilacAdı"].Caption = "İLAÇ ADI";
                gridView1.Columns["fiyat"].Caption = "FİYAT";
                gridView1.Columns["adet"].Caption = "ADET";

                // Resim yolu sütununu gizlemek istersen (Gerek yoksa):
                // gridView1.Columns["resim"].Visible = false;
            }
            catch { }
        }

        void AnaModuluGuncelle()
        {
            try { if (this.MdiParent is FrmAnaModul anaModul) anaModul.ListeleriYenile(); } catch { }
        }

        void temizle()
        {
            txtsiraNo.Text = "";
            txtKod.Text = "";
            txtAd.Text = "";
            txtFiyat.Text = "";
            txtAdet.Text = "";
            picResim.Image = null;
            resimDosyaYolu = "";
        }

        void resimYukle(string yol)
        {
            if (!string.IsNullOrEmpty(yol) && File.Exists(yol)) { picResim.Image = Image.FromFile(yol); resimDosyaYolu = yol; }
            else { picResim.Image = null; resimDosyaYolu = ""; }
        }

       
        private void txtAd_Leave(object sender, EventArgs e) { if (txtAd.Text.Trim() != "" && txtKod.Text == "") VeriGetir("ilacAdı", txtAd.Text.Trim()); }

        void VeriGetir(string kolon, string deger)
        {
            try
            {
                SqlCommand komut = new SqlCommand($"Select * From Ilaclar where {kolon}=@p1 AND KullaniciID=@uid", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", deger);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if (kolon == "ilacAdı") txtKod.Text = dr["ilacKodu"].ToString();
                    if (kolon == "ilacKodu") txtAd.Text = dr["ilacAdı"].ToString();
                    txtFiyat.Text = dr["fiyat"].ToString();
                    resimYukle(dr["resim"].ToString());
                }
                bgl.baglanti().Close();
            }
            catch { }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                DataRow dr = gridView1.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    txtsiraNo.Text = dr["siraNo"].ToString();
                    txtKod.Text = dr["ilacKodu"].ToString();
                    txtAd.Text = dr["ilacAdı"].ToString();
                    txtFiyat.Text = dr["fiyat"].ToString();
                    txtAdet.Text = dr["adet"].ToString();
                    resimYukle(dr["resim"].ToString());
                }
            }
            catch { }
        }

        private void btnKaydet_Click(object sender, EventArgs e) { if (!islemYapiliyor) IslemYap("kaydet"); }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (islemYapiliyor) return;
            if (gridView1.GetDataRow(gridView1.FocusedRowHandle) == null) { MessageBox.Show("Seçim yapınız."); return; }
            IslemYap("guncelle");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) { MessageBox.Show("Silinecek satırı seçiniz."); return; }
            string id = dr["siraNo"].ToString();

            if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    SqlCommand komut = new SqlCommand("Delete From Ilaclar where siraNo=@p1 AND KullaniciID=@uid", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    komut.ExecuteNonQuery();
                    bgl.baglanti().Close();

                    listele();
                    temizle();
                    AnaModuluGuncelle();
                    MessageBox.Show("Silindi.");
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            if (dosya.ShowDialog() == DialogResult.OK) resimYukle(dosya.FileName);
        }

        void IslemYap(string tur)
        {
            if (txtKod.Text.Trim() == "" || txtAd.Text.Trim() == "" || txtAdet.Text == "" || txtFiyat.Text.Trim() == "")
            {
                MessageBox.Show("Eksik bilgi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal fiyat = 0;
            int adet = 0;
            if (!decimal.TryParse(txtFiyat.Text.Replace(".", ","), out fiyat) || !int.TryParse(txtAdet.Text, out adet))
            {
                MessageBox.Show("Sayı hatası. Fiyat veya Adet yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            islemYapiliyor = true;
            SqlConnection conn = bgl.baglanti();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                if (tur == "kaydet")
                {
                    SqlCommand komutKontrol = new SqlCommand("Select Count(*) From Ilaclar where ilacKodu=@p1 AND KullaniciID=@uid", conn);
                    komutKontrol.Parameters.AddWithValue("@p1", txtKod.Text.Trim());
                    komutKontrol.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    int sayi = Convert.ToInt32(komutKontrol.ExecuteScalar());

                    if (sayi > 0)
                    {
                        if (MessageBox.Show("İlaç zaten var. Eklensin mi?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SqlCommand komut = new SqlCommand("Update Ilaclar set adet=adet+@p1, fiyat=@p2, resim=@p3, ilacAdı=@p4 where ilacKodu=@p5 AND KullaniciID=@uid", conn);
                            komut.Parameters.AddWithValue("@p1", adet);
                            komut.Parameters.AddWithValue("@p2", fiyat);
                            komut.Parameters.AddWithValue("@p3", resimDosyaYolu);
                            komut.Parameters.AddWithValue("@p4", txtAd.Text);
                            komut.Parameters.AddWithValue("@p5", txtKod.Text);
                            komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Eklendi.");
                        }
                    }
                    else
                    {
                        SqlCommand komut = new SqlCommand("insert into Ilaclar (ilacKodu, ilacAdı, fiyat, adet, resim, KullaniciID) values (@p1, @p2, @p3, @p4, @p5, @uid)", conn);
                        komut.Parameters.AddWithValue("@p1", txtKod.Text);
                        komut.Parameters.AddWithValue("@p2", txtAd.Text);
                        komut.Parameters.AddWithValue("@p3", fiyat);
                        komut.Parameters.AddWithValue("@p4", adet);
                        komut.Parameters.AddWithValue("@p5", resimDosyaYolu);
                        komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Kaydedildi.");
                    }
                }
                else if (tur == "guncelle")
                {
                    string id = txtsiraNo.Text;
                    SqlCommand komut = new SqlCommand("Update Ilaclar set ilacKodu=@p1, ilacAdı=@p2, fiyat=@p3, adet=@p4, resim=@p5 where siraNo=@p6 AND KullaniciID=@uid", conn);
                    komut.Parameters.AddWithValue("@p1", txtKod.Text);
                    komut.Parameters.AddWithValue("@p2", txtAd.Text);
                    komut.Parameters.AddWithValue("@p3", fiyat);
                    komut.Parameters.AddWithValue("@p4", adet);
                    komut.Parameters.AddWithValue("@p5", resimDosyaYolu);
                    komut.Parameters.AddWithValue("@p6", id);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Güncellendi.");
                }

                if (conn.State == ConnectionState.Open) conn.Close();
                listele();
                temizle();
                AnaModuluGuncelle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { islemYapiliyor = false; if (conn.State == ConnectionState.Open) conn.Close(); }
        }

        public void OtomatikDoldur(string gelenAd, string gelenAdet)
        {
            temizle();
            txtAd.Text = gelenAd;
            txtAdet.Text = gelenAdet;
            txtAd_Leave(this, EventArgs.Empty);

            if (txtKod.Text == "")
            {
                txtKod.Focus();
                MessageBox.Show($"'{gelenAd}' stokta yok. Kod ve Fiyat giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
       
    }
    }