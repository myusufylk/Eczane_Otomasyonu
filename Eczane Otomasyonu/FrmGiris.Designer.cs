using DevExpress.XtraEditors;

namespace Eczane_Otomasyonu
{
    partial class FrmGiris
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGiris));
            this.txtKullaniciAd = new DevExpress.XtraEditors.TextEdit();
            this.txtSifre = new DevExpress.XtraEditors.TextEdit();
            this.btnGiris = new DevExpress.XtraEditors.SimpleButton();
            this.btnKapat = new DevExpress.XtraEditors.SimpleButton();
            this.pnlKayit = new DevExpress.XtraEditors.PanelControl();
            this.pnlSifreUnuttum = new DevExpress.XtraEditors.PanelControl();
            this.txtKAdı_Unuttum = new DevExpress.XtraEditors.TextEdit();
            this.btnGeri_Unuttum = new DevExpress.XtraEditors.SimpleButton();
            this.btnGuncelle = new DevExpress.XtraEditors.SimpleButton();
            this.btnŞfrUnuttum = new DevExpress.XtraEditors.SimpleButton();
            this.btnKayıtOl = new DevExpress.XtraEditors.SimpleButton();
            this.txtTel_Unuttum = new DevExpress.XtraEditors.TextEdit();
            this.txtMail_Unuttum = new DevExpress.XtraEditors.TextEdit();
            this.txtYeniSifre = new DevExpress.XtraEditors.TextEdit();
            this.txtKadi_Kayit = new DevExpress.XtraEditors.TextEdit();
            this.txtSifre_Kayit = new DevExpress.XtraEditors.TextEdit();
            this.txtMail_Kayit = new DevExpress.XtraEditors.TextEdit();
            this.btnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.txtTel_Kayit = new DevExpress.XtraEditors.TextEdit();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.btnGeri_Kayit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtKullaniciAd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSifre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlKayit)).BeginInit();
            this.pnlKayit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSifreUnuttum)).BeginInit();
            this.pnlSifreUnuttum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKAdı_Unuttum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel_Unuttum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail_Unuttum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYeniSifre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKadi_Kayit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSifre_Kayit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail_Kayit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel_Kayit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtKullaniciAd
            // 
            this.txtKullaniciAd.Location = new System.Drawing.Point(293, 187);
            this.txtKullaniciAd.Name = "txtKullaniciAd";
            this.txtKullaniciAd.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtKullaniciAd.Properties.Appearance.Options.UseFont = true;
            this.txtKullaniciAd.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtKullaniciAd.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtKullaniciAd.Properties.ContextImageOptions.Image")));
            this.txtKullaniciAd.Properties.NullText = "KULLANICI ADI";
            this.txtKullaniciAd.Size = new System.Drawing.Size(165, 36);
            this.txtKullaniciAd.TabIndex = 2;
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(292, 240);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSifre.Properties.Appearance.Options.UseFont = true;
            this.txtSifre.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtSifre.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtSifre.Properties.ContextImageOptions.Image")));
            this.txtSifre.Properties.NullText = "ŞİFRE";
            this.txtSifre.Size = new System.Drawing.Size(166, 36);
            this.txtSifre.TabIndex = 3;
            // 
            // btnGiris
            // 
            this.btnGiris.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGiris.Appearance.Options.UseFont = true;
            this.btnGiris.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGiris.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnGiris.ImageOptions.Image")));
            this.btnGiris.Location = new System.Drawing.Point(292, 294);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnGiris.Size = new System.Drawing.Size(166, 23);
            this.btnGiris.TabIndex = 4;
            this.btnGiris.Text = "GİRİŞ YAP";
            this.btnGiris.Click += new System.EventHandler(this.btnGirisYap_Click);
            // 
            // btnKapat
            // 
            this.btnKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnKapat.ImageOptions.Image")));
            this.btnKapat.Location = new System.Drawing.Point(714, 0);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnKapat.Size = new System.Drawing.Size(35, 35);
            this.btnKapat.TabIndex = 5;
            this.btnKapat.Click += new System.EventHandler(this.btnKapat_Click);
            // 
            // pnlKayit
            // 
            this.pnlKayit.Controls.Add(this.btnGeri_Kayit);
            this.pnlKayit.Controls.Add(this.txtKadi_Kayit);
            this.pnlKayit.Controls.Add(this.btnKaydet);
            this.pnlKayit.Controls.Add(this.txtMail_Kayit);
            this.pnlKayit.Controls.Add(this.txtTel_Kayit);
            this.pnlKayit.Controls.Add(this.txtSifre_Kayit);
            this.pnlKayit.Location = new System.Drawing.Point(464, 33);
            this.pnlKayit.Name = "pnlKayit";
            this.pnlKayit.Size = new System.Drawing.Size(240, 300);
            this.pnlKayit.TabIndex = 6;
            this.pnlKayit.Visible = false;
            // 
            // pnlSifreUnuttum
            // 
            this.pnlSifreUnuttum.Controls.Add(this.txtYeniSifre);
            this.pnlSifreUnuttum.Controls.Add(this.txtMail_Unuttum);
            this.pnlSifreUnuttum.Controls.Add(this.txtTel_Unuttum);
            this.pnlSifreUnuttum.Controls.Add(this.txtKAdı_Unuttum);
            this.pnlSifreUnuttum.Controls.Add(this.btnGeri_Unuttum);
            this.pnlSifreUnuttum.Controls.Add(this.btnGuncelle);
            this.pnlSifreUnuttum.Location = new System.Drawing.Point(40, 28);
            this.pnlSifreUnuttum.Name = "pnlSifreUnuttum";
            this.pnlSifreUnuttum.Size = new System.Drawing.Size(240, 300);
            this.pnlSifreUnuttum.TabIndex = 7;
            this.pnlSifreUnuttum.Visible = false;
            // 
            // txtKAdı_Unuttum
            // 
            this.txtKAdı_Unuttum.EditValue = "";
            this.txtKAdı_Unuttum.Location = new System.Drawing.Point(7, 34);
            this.txtKAdı_Unuttum.Name = "txtKAdı_Unuttum";
            this.txtKAdı_Unuttum.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtKAdı_Unuttum.Properties.Appearance.Options.UseFont = true;
            this.txtKAdı_Unuttum.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtKAdı_Unuttum.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtKAdı_Unuttum.Properties.ContextImageOptions.Image")));
            this.txtKAdı_Unuttum.Properties.NullValuePrompt = "KULLANICI ADI";
            this.txtKAdı_Unuttum.Size = new System.Drawing.Size(197, 36);
            this.txtKAdı_Unuttum.TabIndex = 12;
            // 
            // btnGeri_Unuttum
            // 
            this.btnGeri_Unuttum.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnGeri_Unuttum.ImageOptions.SvgImage")));
            this.btnGeri_Unuttum.Location = new System.Drawing.Point(5, 5);
            this.btnGeri_Unuttum.Name = "btnGeri_Unuttum";
            this.btnGeri_Unuttum.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnGeri_Unuttum.Size = new System.Drawing.Size(55, 23);
            this.btnGeri_Unuttum.TabIndex = 8;
            this.btnGeri_Unuttum.Click += new System.EventHandler(this.btnGeri_Unuttum_Click);
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGuncelle.Appearance.Options.UseFont = true;
            this.btnGuncelle.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnGuncelle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnGuncelle.ImageOptions.SvgImage")));
            this.btnGuncelle.Location = new System.Drawing.Point(24, 254);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnGuncelle.Size = new System.Drawing.Size(159, 35);
            this.btnGuncelle.TabIndex = 8;
            this.btnGuncelle.Text = "GÜNCELLE";
            this.btnGuncelle.Click += new System.EventHandler(this.btnSifreGuncelle_Click);
            // 
            // btnŞfrUnuttum
            // 
            this.btnŞfrUnuttum.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnŞfrUnuttum.Appearance.Options.UseFont = true;
            this.btnŞfrUnuttum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnŞfrUnuttum.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnŞfrUnuttum.ImageOptions.Image")));
            this.btnŞfrUnuttum.Location = new System.Drawing.Point(277, 342);
            this.btnŞfrUnuttum.Name = "btnŞfrUnuttum";
            this.btnŞfrUnuttum.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnŞfrUnuttum.Size = new System.Drawing.Size(192, 36);
            this.btnŞfrUnuttum.TabIndex = 8;
            this.btnŞfrUnuttum.Text = "ŞİFREMİ UNUTTUM";
            this.btnŞfrUnuttum.Click += new System.EventHandler(this.lnkSifreUnuttum_Click);
            // 
            // btnKayıtOl
            // 
            this.btnKayıtOl.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKayıtOl.Appearance.Options.UseFont = true;
            this.btnKayıtOl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnKayıtOl.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnKayıtOl.ImageOptions.Image")));
            this.btnKayıtOl.Location = new System.Drawing.Point(277, 384);
            this.btnKayıtOl.Name = "btnKayıtOl";
            this.btnKayıtOl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnKayıtOl.Size = new System.Drawing.Size(192, 32);
            this.btnKayıtOl.TabIndex = 9;
            this.btnKayıtOl.Text = "KAYIT OL ";
            this.btnKayıtOl.Click += new System.EventHandler(this.lnkKayitOl_Click);
            // 
            // txtTel_Unuttum
            // 
            this.txtTel_Unuttum.EditValue = "";
            this.txtTel_Unuttum.Location = new System.Drawing.Point(5, 90);
            this.txtTel_Unuttum.Name = "txtTel_Unuttum";
            this.txtTel_Unuttum.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTel_Unuttum.Properties.Appearance.Options.UseFont = true;
            this.txtTel_Unuttum.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtTel_Unuttum.Properties.ContextImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("textEdit2.Properties.ContextImageOptions.SvgImage2")));
            this.txtTel_Unuttum.Properties.NullValuePrompt = "TELEFON NO ";
            this.txtTel_Unuttum.Size = new System.Drawing.Size(197, 36);
            this.txtTel_Unuttum.TabIndex = 10;
            // 
            // txtMail_Unuttum
            // 
            this.txtMail_Unuttum.EditValue = "";
            this.txtMail_Unuttum.Location = new System.Drawing.Point(7, 148);
            this.txtMail_Unuttum.Name = "txtMail_Unuttum";
            this.txtMail_Unuttum.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMail_Unuttum.Properties.Appearance.Options.UseFont = true;
            this.txtMail_Unuttum.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtMail_Unuttum.Properties.ContextImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("textEdit2.Properties.ContextImageOptions.SvgImage1")));
            this.txtMail_Unuttum.Properties.NullValuePrompt = "MAİL ADRESİ";
            this.txtMail_Unuttum.Size = new System.Drawing.Size(197, 36);
            this.txtMail_Unuttum.TabIndex = 10;
            // 
            // txtYeniSifre
            // 
            this.txtYeniSifre.EditValue = "";
            this.txtYeniSifre.Location = new System.Drawing.Point(7, 202);
            this.txtYeniSifre.Name = "txtYeniSifre";
            this.txtYeniSifre.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtYeniSifre.Properties.Appearance.Options.UseFont = true;
            this.txtYeniSifre.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtYeniSifre.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("textEdit2.Properties.ContextImageOptions.Image2")));
            this.txtYeniSifre.Properties.NullValuePrompt = "ŞİFRE";
            this.txtYeniSifre.Size = new System.Drawing.Size(199, 36);
            this.txtYeniSifre.TabIndex = 10;
            // 
            // txtKadi_Kayit
            // 
            this.txtKadi_Kayit.EditValue = "";
            this.txtKadi_Kayit.Location = new System.Drawing.Point(5, 29);
            this.txtKadi_Kayit.Name = "txtKadi_Kayit";
            this.txtKadi_Kayit.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtKadi_Kayit.Properties.Appearance.Options.UseFont = true;
            this.txtKadi_Kayit.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtKadi_Kayit.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("textEdit2.Properties.ContextImageOptions.Image")));
            this.txtKadi_Kayit.Properties.NullValuePrompt = "KULLANICI ADI";
            this.txtKadi_Kayit.Size = new System.Drawing.Size(216, 36);
            this.txtKadi_Kayit.TabIndex = 14;
            // 
            // txtSifre_Kayit
            // 
            this.txtSifre_Kayit.EditValue = "";
            this.txtSifre_Kayit.Location = new System.Drawing.Point(5, 197);
            this.txtSifre_Kayit.Name = "txtSifre_Kayit";
            this.txtSifre_Kayit.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSifre_Kayit.Properties.Appearance.Options.UseFont = true;
            this.txtSifre_Kayit.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtSifre_Kayit.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("textEdit2.Properties.ContextImageOptions.Image1")));
            this.txtSifre_Kayit.Properties.NullValuePrompt = "ŞİFRE";
            this.txtSifre_Kayit.Size = new System.Drawing.Size(216, 36);
            this.txtSifre_Kayit.TabIndex = 15;
            // 
            // txtMail_Kayit
            // 
            this.txtMail_Kayit.EditValue = "";
            this.txtMail_Kayit.Location = new System.Drawing.Point(5, 143);
            this.txtMail_Kayit.Name = "txtMail_Kayit";
            this.txtMail_Kayit.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMail_Kayit.Properties.Appearance.Options.UseFont = true;
            this.txtMail_Kayit.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtMail_Kayit.Properties.ContextImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("textEdit2.Properties.ContextImageOptions.SvgImage")));
            this.txtMail_Kayit.Properties.NullValuePrompt = "MAİL ADRESİ";
            this.txtMail_Kayit.Size = new System.Drawing.Size(216, 36);
            this.txtMail_Kayit.TabIndex = 14;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.Appearance.Options.UseFont = true;
            this.btnKaydet.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnKaydet.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.btnKaydet.Location = new System.Drawing.Point(39, 249);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnKaydet.Size = new System.Drawing.Size(159, 35);
            this.btnKaydet.TabIndex = 14;
            this.btnKaydet.Text = "KAYDET";
            this.btnKaydet.Click += new System.EventHandler(this.btnKayitOl_Click);
            // 
            // txtTel_Kayit
            // 
            this.txtTel_Kayit.EditValue = "";
            this.txtTel_Kayit.Location = new System.Drawing.Point(5, 85);
            this.txtTel_Kayit.Name = "txtTel_Kayit";
            this.txtTel_Kayit.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTel_Kayit.Properties.Appearance.Options.UseFont = true;
            this.txtTel_Kayit.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
            this.txtTel_Kayit.Properties.ContextImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("txtTel_Kayit.Properties.ContextImageOptions.SvgImage")));
            this.txtTel_Kayit.Properties.NullValuePrompt = "TELEFON NO ";
            this.txtTel_Kayit.Size = new System.Drawing.Size(216, 36);
            this.txtTel_Kayit.TabIndex = 14;
            // 
            // btnGeri_Kayit
            // 
            this.btnGeri_Kayit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnGeri_Kayit.ImageOptions.SvgImage")));
            this.btnGeri_Kayit.Location = new System.Drawing.Point(0, 5);
            this.btnGeri_Kayit.Name = "btnGeri_Kayit";
            this.btnGeri_Kayit.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnGeri_Kayit.Size = new System.Drawing.Size(55, 23);
            this.btnGeri_Kayit.TabIndex = 14;
            this.btnGeri_Kayit.Click += new System.EventHandler(this.btnGeri_Kayit_Click);
            // 
            // FrmGiris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(750, 450);
            this.Controls.Add(this.btnKayıtOl);
            this.Controls.Add(this.btnŞfrUnuttum);
            this.Controls.Add(this.pnlSifreUnuttum);
            this.Controls.Add(this.pnlKayit);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.txtKullaniciAd);
            this.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmGiris";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmGiris_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtKullaniciAd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSifre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlKayit)).EndInit();
            this.pnlKayit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSifreUnuttum)).EndInit();
            this.pnlSifreUnuttum.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtKAdı_Unuttum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel_Unuttum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail_Unuttum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYeniSifre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKadi_Kayit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSifre_Kayit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail_Kayit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel_Kayit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private TextEdit textEdit1;
        private TextEdit txtSifre;
        private TextEdit txtKullaniciAd;
        private SimpleButton btnGiris;
        private SimpleButton btnKapat;
        private PanelControl pnlKayit;
        private PanelControl pnlSifreUnuttum;
        private SimpleButton btnGeri_Unuttum;
        private SimpleButton btnGuncelle;
        private SimpleButton btnŞfrUnuttum;
        private SimpleButton btnKayıtOl;
        private TextEdit txtKAdı_Unuttum;
        private TextEdit txtTel_Unuttum;
        private TextEdit txtYeniSifre;
        private TextEdit txtMail_Unuttum;
        private TextEdit txtKadi_Kayit;
        private SimpleButton btnKaydet;
        private TextEdit txtMail_Kayit;
        private TextEdit txtSifre_Kayit;
        private TextEdit txtTel_Kayit;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private SimpleButton btnGeri_Kayit;
    }
}
