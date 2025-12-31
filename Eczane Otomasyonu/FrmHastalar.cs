using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;

namespace Eczane_Otomasyonu
{
    public partial class FrmHastalar : DevExpress.XtraEditors.XtraForm
    {
        SqlBaglantisi bgl = new SqlBaglantisi();

        // Seçilen satırın ID'sini hafızada tutmak için değişken
        string secilenHastaID = "";

        public FrmHastalar()
        {
            InitializeComponent();

            // --- DÜZELTME 1: MANUEL BUTON BAĞLAMALARI SİLİNDİ ---
            // Visual Studio zaten butonlara çift tıklayınca bu bağlamayı yapıyor.
            // Buraya tekrar yazarsan kod 2 kere çalışır ve hata verir.
            // Sadece Load ve Grid olaylarını bırakıyoruz.

            this.Load += FrmHastalar_Load;
            gridView1.RowClick += gridView1_RowClick;

            // Eğer grid satır seçimi değişirse ID'yi yakalamak için (Garanti olsun)
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;
        }

        // --- FORM YÜKLENİRKEN ---
        private void FrmHastalar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();

            // Grid Ayarı
            gridView1.OptionsBehavior.Editable = false;

            // --- TC KİMLİK AYARI ---
            txtTc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtTc.Properties.Mask.EditMask = @"\d{11}";
            txtTc.Properties.MaxLength = 11;

            // --- TELEFON AYARI ---
            txtTelefon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            txtTelefon.Properties.Mask.EditMask = "(999) 000-00-00";

            // --- GÜVENCE KUTUSU ---
            cmbGuvence.Properties.Items.Clear();
            cmbGuvence.Properties.Items.AddRange(new string[] { "SGK", "Bağkur", "Emekli Sandığı", "Özel Sigorta", "Yeşil Kart", "Yok" });
            cmbGuvence.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        // --- LİSTELEME ---
        void listele()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select * From Hastalar WHERE KullaniciID=" + MevcutKullanici.Id, bgl.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
            }
            catch { }
        }

        void temizle()
        {
            txtTc.Text = "";
            txtAd.Text = "";
            txtSoyad.Text = "";
            txtTelefon.Text = "";
            cmbGuvence.Text = "";
            txtAdres.Text = "";
            secilenHastaID = ""; // ID'yi de sıfırla
        }

        // --- GRID SATIRINA TIKLAYINCA VERİLERİ ÇEK ---
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            VeriAktar();
        }

        // Ok tuşlarıyla gezerken de verileri çeksin
        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            VeriAktar();
        }

        void VeriAktar()
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                try
                {
                    // ID'yi hafızaya alıyoruz (Güncelleme için şart!)
                    secilenHastaID = dr["ID"].ToString();

                    txtTc.Text = dr["TC"].ToString();
                    txtAd.Text = dr["Ad"].ToString();
                    txtSoyad.Text = dr["Soyad"].ToString();
                    txtTelefon.Text = dr["Telefon"].ToString();
                    cmbGuvence.Text = dr["Guvence"].ToString();
                    txtAdres.Text = dr["Adres"].ToString();
                }
                catch { }
            }
        }

        // --- KAYDET ---
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = bgl.baglanti();
                // Önce aynı TC var mı kontrol et (Mükerrer kaydı önle)
                SqlCommand kontrol = new SqlCommand("Select Count(*) From Hastalar Where TC=@p1 AND KullaniciID=@uid", conn);
                kontrol.Parameters.AddWithValue("@p1", txtTc.Text);
                kontrol.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                int sayi = Convert.ToInt32(kontrol.ExecuteScalar());

                if (sayi > 0)
                {
                    MessageBox.Show("Bu TC Kimlik numarasıyla zaten bir hasta kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conn.Close();
                    return;
                }

                SqlCommand komut = new SqlCommand("insert into Hastalar (TC, Ad, Soyad, Telefon, Guvence, Adres, KullaniciID) values (@p1, @p2, @p3, @p4, @p5, @p6, @uid)", conn);
                komut.Parameters.AddWithValue("@p1", txtTc.Text);
                komut.Parameters.AddWithValue("@p2", txtAd.Text);
                komut.Parameters.AddWithValue("@p3", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p4", txtTelefon.Text);
                komut.Parameters.AddWithValue("@p5", cmbGuvence.Text);
                komut.Parameters.AddWithValue("@p6", txtAdres.Text);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                komut.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Hasta Başarıyla Kaydedildi ✅", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
                temizle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        // --- SİL ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (secilenHastaID == "")
            {
                MessageBox.Show("Lütfen silinecek hastayı listeden seçiniz.", "Uyarı");
                return;
            }

            if (MessageBox.Show("Bu hasta kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection conn = bgl.baglanti();
                    SqlCommand komut = new SqlCommand("Delete From Hastalar where ID=@p1 AND KullaniciID=@uid", conn);
                    komut.Parameters.AddWithValue("@p1", secilenHastaID);
                    komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);
                    komut.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Kayıt Silindi 🗑️", "Bilgi");
                    listele();
                    temizle();
                }
                catch (Exception ex) { MessageBox.Show("Silme Hatası: " + ex.Message); }
            }
        }

        // --- GÜNCELLE (DÜZELTİLEN KISIM) ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Bir satır seçili mi?
            if (secilenHastaID == "")
            {
                MessageBox.Show("Lütfen güncellenecek hastayı listeden seçiniz.", "Uyarı");
                return;
            }

            try
            {
                SqlConnection conn = bgl.baglanti();

                // 2. DÜZELTME: Güncellemeyi TC'ye göre değil, ID'ye göre yapıyoruz.
                // Böylece TC numarasındaki hataları bile düzeltebilirsin.
                SqlCommand komut = new SqlCommand("Update Hastalar set TC=@p1, Ad=@p2, Soyad=@p3, Telefon=@p4, Guvence=@p5, Adres=@p6 where ID=@id AND KullaniciID=@uid", conn);

                komut.Parameters.AddWithValue("@p1", txtTc.Text);
                komut.Parameters.AddWithValue("@p2", txtAd.Text);
                komut.Parameters.AddWithValue("@p3", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p4", txtTelefon.Text);
                komut.Parameters.AddWithValue("@p5", cmbGuvence.Text);
                komut.Parameters.AddWithValue("@p6", txtAdres.Text);

                // Kilit Nokta: ID'yi kullanıyoruz
                komut.Parameters.AddWithValue("@id", secilenHastaID);
                komut.Parameters.AddWithValue("@uid", MevcutKullanici.Id);

                int sonuc = komut.ExecuteNonQuery();
                conn.Close();

                if (sonuc > 0)
                {
                    MessageBox.Show("Bilgiler Başarıyla Güncellendi ✅", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listele();
                    temizle(); // Temizle en son çağrılmalı
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız! Kayıt bulunamadı veya yetkiniz yok.", "Hata");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme Hatası: " + ex.Message);
            }
        }
    }
}