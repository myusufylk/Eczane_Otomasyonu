using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Linq;

namespace Eczane_Otomasyonu
{
    public partial class FrmHareketler : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();
        bool islemYapiliyor = false;

        // SepetItem sınıfı FrmAnaModul.cs içinde tanımlı, oradan kullanıyoruz.
        List<SepetItem> _sepet = new List<SepetItem>();

        public FrmHareketler()
        {
            InitializeComponent();

            // Olayları Bağla
            this.Load -= FrmHareketler_Load;
            this.Load += FrmHareketler_Load;

            lueIlac.EditValueChanged -= lueIlac_EditValueChanged;
            lueIlac.EditValueChanged += lueIlac_EditValueChanged;

            txtTc.Leave -= txtTc_Leave;
            txtTc.Leave += txtTc_Leave;

            // Manuel Buton Bağlama
            try
            {
                var btnSatis = this.Controls.Find("btnSatisYap", true);
                if (btnSatis.Length > 0) { btnSatis[0].Click -= btnSatisYap_Click; btnSatis[0].Click += btnSatisYap_Click; }

                var btnSepet = this.Controls.Find("btnSepeteEkle", true);
                if (btnSepet.Length > 0) { btnSepet[0].Click -= btnSepeteEkle_Click; btnSepet[0].Click += btnSepeteEkle_Click; }
            }
            catch { }
        }

        private void FrmHareketler_Load(object sender, EventArgs e)
        {
            listele();
            ilacListesiGetir();
            temizle();
            dateTarih.DateTime = DateTime.Now;

            lueIlac.Properties.NullText = "İlaç Seçiniz";
            lueIlac.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

            // TC Kimlik Maskesi (Görünür olması için Simple yaptık)
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            txtTc.Properties.Mask.EditMask = "00000000000";
            txtTc.Properties.Mask.UseMaskAsDisplayFormat = true;

            gridView1.OptionsBehavior.Editable = false;
        }

        // --- 1. DÜZELTME: SEPETE EKLE (MESAJ KUTUSU YOK, LABEL GÜNCELLEME VAR) ---
        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (lueIlac.EditValue == null || txtAdet.Text == "")
            {
                MessageBox.Show("İlaç ve Adet seçmelisiniz.", "Uyarı");
                return;
            }

            string ilacAdi = lueIlac.Text;
            int adet = 0;
            int.TryParse(txtAdet.Text, out adet);
            decimal fiyat = 0;
            decimal.TryParse(txtFiyat.Text, out fiyat);

            // 1. İlaç Etkileşim Kontrolü
            if (!EtkilesimKontrol(ilacAdi)) return;

            // 2. Stok Kontrolü
            if (!StokYeterliMi(ilacAdi, adet)) return;

            // 3. Sepete Ekle
            var mevcut = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
            if (mevcut != null)
            {
                mevcut.Adet += adet;
            }
            else
            {
                _sepet.Add(new SepetItem { IlacAdi = ilacAdi, Adet = adet, BirimFiyat = fiyat });
            }

            SepetGuncelle();

            // Alanları temizle
            lueIlac.EditValue = null;
            txtAdet.Text = "";
            txtFiyat.Text = "";
            txtToplam.Text = "";
        }

        // --- SEPETİ VE TOPLAM TUTAR LABELINI GÜNCELLE ---
        void SepetGuncelle()
        {
            try
            {
                // Gridi Güncelle
                var gridler = this.Controls.Find("gridSepet", true);
                if (gridler.Length > 0)
                {
                    DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)gridler[0];
                    gc.DataSource = null;
                    gc.DataSource = _sepet;
                }

                // Toplam Tutarı Label'a Yaz (DÜZELTİLDİ)
                decimal toplam = _sepet.Sum(x => x.Toplam);
                var lbl = this.Controls.Find("lblToplamTutar", true); // Label ismi lblToplamTutar olmalı
                if (lbl.Length > 0)
                {
                    lbl[0].Text = $"{toplam:C2}"; // Örn: ₺156,00 yazar
                }
                else
                {
                    // Bulamazsa labelControl2'yi dene (ekran görüntüsündeki isim)
                    var lbl2 = this.Controls.Find("labelControl2", true);
                    if (lbl2.Length > 0) lbl2[0].Text = $"TOPLAM: {toplam:C2}";
                }
            }
            catch { }
        }

        // --- 2. DÜZELTME: FİŞ TASARIMI (DB'DEN BİLGİ ÇEKME GERİ GELDİ) ---
        private void FisTasarimi(object sender, PrintPageEventArgs e)
        {
            string eczaneAdi = "ECZANE OTOMASYONU";
            string adres = "";
            string telefon = "";
            string logoYolu = "";

            // Veritabanından Bilgileri Çek
            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand komut = new SqlCommand("Select top 1 * From Isletme WHERE KullaniciID=@uid", conn);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    eczaneAdi = dr["Ad"].ToString().ToUpper();
                    adres = dr["Adres"].ToString();
                    telefon = dr["Telefon"].ToString();
                    if (dr["LogoYolu"] != DBNull.Value) logoYolu = dr["LogoYolu"].ToString();
                }
                conn.Close();
            }
            catch { }

            // Çizim Ayarları
            Font baslikFont = new Font("Arial", 16, FontStyle.Bold);
            Font altBaslik = new Font("Arial", 10, FontStyle.Bold);
            Font icerik = new Font("Arial", 9);
            Brush firca = Brushes.Black;
            float genislik = e.PageBounds.Width;
            int y = 20;
            int h = 20;
            StringFormat merkez = new StringFormat() { Alignment = StringAlignment.Center };

            // 1. LOGO
            if (!string.IsNullOrEmpty(logoYolu) && System.IO.File.Exists(logoYolu))
            {
                try
                {
                    Image img = Image.FromFile(logoYolu);
                    e.Graphics.DrawImage(img, (int)((genislik - 80) / 2), y, 80, 60);
                    y += 70;
                }
                catch { }
            }

            // 2. BAŞLIK VE İLETİŞİM
            e.Graphics.DrawString(eczaneAdi, baslikFont, firca, new RectangleF(0, y, genislik, 30), merkez); y += 30;
            e.Graphics.DrawString(adres, icerik, firca, new RectangleF(0, y, genislik, 40), merkez); y += 40;
            e.Graphics.DrawString($"Tel: {telefon}", icerik, firca, new RectangleF(0, y, genislik, 20), merkez); y += 30;

            e.Graphics.DrawString("------------------------------------------------", icerik, firca, 10, y); y += h;

            // 3. FİŞ BİLGİLERİ
            e.Graphics.DrawString($"Tarih: {DateTime.Now}", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"TC: {txtTc.Text}", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"Hasta: {txtHastaAdi.Text}", icerik, firca, 10, y); y += h + 10;

            // 4. ÜRÜNLER
            e.Graphics.DrawString("Ürün Adı", altBaslik, firca, 10, y);
            e.Graphics.DrawString("Adet", altBaslik, firca, 180, y);
            e.Graphics.DrawString("Tutar", altBaslik, firca, 230, y);
            y += h;

            decimal genelToplam = 0;
            foreach (var item in _sepet)
            {
                e.Graphics.DrawString(item.IlacAdi, icerik, firca, 10, y);
                e.Graphics.DrawString(item.Adet.ToString(), icerik, firca, 190, y);
                e.Graphics.DrawString(item.Toplam.ToString("C2"), icerik, firca, 230, y);
                genelToplam += item.Toplam;
                y += h;
            }

            e.Graphics.DrawString("------------------------------------------------", icerik, firca, 10, y); y += h;
            e.Graphics.DrawString($"TOPLAM: {genelToplam:C2}", baslikFont, firca, 150, y);
        }

        // --- 3. DÜZELTME: ETKİLEŞİM KONTROLÜ (TRIM İLE GÜÇLENDİRİLDİ) ---
        bool EtkilesimKontrol(string yeniIlac)
        {
            // Sepet boşsa risk yoktur
            if (_sepet.Count == 0) return true;

            try
            {
                SqlConnection conn = bgl.baglanti();
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in _sepet)
                {
                    // SQL TABLOSUNUN DOLU OLDUĞUNDAN EMİN OLMALISIN!
                    // .Trim() sayesinde görünmez boşlukları temizler ve eşleşmeyi garantiler.
                    SqlCommand cmd = new SqlCommand("SELECT RiskMesaji FROM Etkilesimler WHERE (Ilac1=@p1 AND Ilac2=@p2) OR (Ilac1=@p2 AND Ilac2=@p1)", conn);
                    cmd.Parameters.AddWithValue("@p1", yeniIlac.Trim());
                    cmd.Parameters.AddWithValue("@p2", item.IlacAdi.Trim());

                    object sonuc = cmd.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string risk = sonuc.ToString();
                        conn.Close();

                        // UYARI PENCERESİ
                        DialogResult cvp = MessageBox.Show(
                            $"⚠️ İLAÇ ETKİLEŞİM RİSKİ TESPİT EDİLDİ!\n\n" +
                            $"Sepetteki İlaç: {item.IlacAdi}\n" +
                            $"Eklenen İlaç: {yeniIlac}\n\n" +
                            $"RİSK: {risk}\n\n" +
                            $"Yine de eklemek istiyor musunuz?",
                            "HAYATİ UYARI",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Stop);

                        return (cvp == DialogResult.Yes);
                    }
                }
                conn.Close();
            }
            catch { }
            return true;
        }

        // --- DİĞER METODLAR (Standart) ---
        bool StokYeterliMi(string ilacAdi, int istenenAdet)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                SqlCommand cmd = new SqlCommand("Select adet From Ilaclar where ilacAdı=@p1 AND KullaniciID=@uid", conn);
                cmd.Parameters.AddWithValue("@p1", ilacAdi);
                cmd.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                object sonuc = cmd.ExecuteScalar();
                conn.Close();
                int dbStok = (sonuc != null) ? Convert.ToInt32(sonuc) : 0;

                var sepetteki = _sepet.FirstOrDefault(x => x.IlacAdi == ilacAdi);
                int sepettekiAdet = sepetteki != null ? sepetteki.Adet : 0;

                if (dbStok < (istenenAdet + sepettekiAdet))
                {
                    MessageBox.Show($"Stok Yetersiz! Eldeki: {dbStok}, Sepette: {sepettekiAdet}", "Hata");
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        private async void btnSatisYap_Click(object sender, EventArgs e)
        {
            if (islemYapiliyor) return;
            if (_sepet.Count == 0)
            {
                // Eğer sepet boşsa ama kutularda veri varsa, önce sepete ekle
                if (lueIlac.EditValue != null && txtAdet.Text != "") btnSepeteEkle_Click(sender, e);
                else { MessageBox.Show("Sepet boş!", "Uyarı"); return; }
            }
            if (_sepet.Count == 0) return;

            islemYapiliyor = true;
            SqlConnection conn = bgl.baglanti();

            try
            {
                // Hasta Kaydı
                SqlCommand cmdHasta = new SqlCommand("Select count(*) From Hastalar where TC=@p1 AND KullaniciID=@uid", conn);
                cmdHasta.Parameters.AddWithValue("@p1", txtTc.Text);
                cmdHasta.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                if (Convert.ToInt32(cmdHasta.ExecuteScalar()) == 0)
                {
                    string tamIsim = txtHastaAdi.Text.Trim();
                    string ad = tamIsim, soyad = "";
                    int bosluk = tamIsim.LastIndexOf(' ');
                    if (bosluk > 0) { ad = tamIsim.Substring(0, bosluk); soyad = tamIsim.Substring(bosluk + 1); }
                    SqlCommand cmdEkle = new SqlCommand("Insert into Hastalar (TC, Ad, Soyad, KullaniciID) values (@p1, @p2, @p3, @uid)", conn);
                    cmdEkle.Parameters.AddWithValue("@p1", txtTc.Text);
                    cmdEkle.Parameters.AddWithValue("@p2", ad);
                    cmdEkle.Parameters.AddWithValue("@p3", soyad);
                    cmdEkle.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdEkle.ExecuteNonQuery();
                }

                // Satış İşlemleri
                foreach (var item in _sepet)
                {
                    SqlCommand cmdDus = new SqlCommand("Update Ilaclar set adet=adet-@p1 where ilacAdı=@p2 AND KullaniciID=@uid", conn);
                    cmdDus.Parameters.AddWithValue("@p1", item.Adet);
                    cmdDus.Parameters.AddWithValue("@p2", item.IlacAdi);
                    cmdDus.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdDus.ExecuteNonQuery();

                    SqlCommand cmdHareket = new SqlCommand("Insert into Hareketler (ilacAdi, adet, toplamFiyat, tarih, hastaAdi, tcNo, KullaniciID) values (@p1,@p2,@p3,@p4,@p5,@p6,@uid)", conn);
                    cmdHareket.Parameters.AddWithValue("@p1", item.IlacAdi);
                    cmdHareket.Parameters.AddWithValue("@p2", item.Adet);
                    cmdHareket.Parameters.AddWithValue("@p3", item.Toplam);
                    cmdHareket.Parameters.AddWithValue("@p4", dateTarih.DateTime);
                    cmdHareket.Parameters.AddWithValue("@p5", txtHastaAdi.Text);
                    cmdHareket.Parameters.AddWithValue("@p6", txtTc.Text);
                    cmdHareket.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    cmdHareket.ExecuteNonQuery();
                }
                conn.Close();

                MessageBox.Show("Satış Tamamlandı.");
                FisYazdir();

                _sepet.Clear();
                SepetGuncelle();
                listele();
                temizle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); if (conn.State == ConnectionState.Open) conn.Close(); }
            finally { islemYapiliyor = false; }
        }

        private void FisYazdir()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(FisTasarimi);
            PrintPreviewDialog onizleme = new PrintPreviewDialog();
            onizleme.Document = pd;
            onizleme.ShowDialog();
        }

        void listele() { try { DataTable dt = new DataTable(); SqlDataAdapter da = new SqlDataAdapter("Select * From Hareketler WHERE KullaniciID=" + MevcutKullanici.Id + " ORDER BY tarih DESC", bgl.baglanti()); da.Fill(dt); gridControl1.DataSource = dt; gridView1.BestFitColumns(); } catch { } }
        void ilacListesiGetir() { try { DataTable dt = new DataTable(); SqlDataAdapter da = new SqlDataAdapter("Select ilacAdı, fiyat From Ilaclar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti()); da.Fill(dt); lueIlac.Properties.DataSource = dt; lueIlac.Properties.ValueMember = "ilacAdı"; lueIlac.Properties.DisplayMember = "ilacAdı"; } catch { } }
        void temizle() { lueIlac.EditValue = null; txtTc.Text = ""; txtHastaAdi.Text = ""; txtAdet.Text = ""; txtFiyat.Text = ""; txtToplam.Text = ""; }
        private void lueIlac_EditValueChanged(object sender, EventArgs e) { if (lueIlac.EditValue != null) { object val = lueIlac.Properties.GetDataSourceRowByKeyValue(lueIlac.EditValue); DataRowView row = val as DataRowView; if (row != null) txtFiyat.Text = row["fiyat"].ToString(); } }
        private void txtTc_Leave(object sender, EventArgs e) { if (txtTc.Text.Length == 11) { try { SqlCommand komut = new SqlCommand("Select Ad + ' ' + Soyad From Hastalar where TC=@p1 AND KullaniciID=@uid", bgl.baglanti()); komut.Parameters.AddWithValue("@p1", txtTc.Text); komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id); SqlDataReader dr = komut.ExecuteReader(); if (dr.Read()) { txtHastaAdi.Text = dr[0].ToString(); } bgl.baglanti().Close(); } catch { } } }
        private void txtAdet_TextChanged(object sender, EventArgs e) { try { decimal f = decimal.Parse(txtFiyat.Text); int a = int.Parse(txtAdet.Text); txtToplam.Text = (f * a).ToString("N2"); } catch { } }
        private void txtFiyat_TextChanged(object sender, EventArgs e) { txtAdet_TextChanged(null, null); }
    }
}