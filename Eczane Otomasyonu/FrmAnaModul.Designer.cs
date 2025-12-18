namespace Eczane_Otomasyonu
{
    partial class FrmAnaModul
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAnaModul));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnIlaclar = new DevExpress.XtraBars.BarButtonItem();
            this.btnMusteriler = new DevExpress.XtraBars.BarButtonItem();
            this.btnPersonel = new DevExpress.XtraBars.BarButtonItem();
            this.btnSatislar = new DevExpress.XtraBars.BarButtonItem();
            this.btnCikis = new DevExpress.XtraBars.BarButtonItem();
            this.btnRaporlar = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.pnlTahmin = new DevExpress.XtraEditors.GroupControl();
            this.cmbIlaclar = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnPanelKapat = new DevExpress.XtraEditors.SimpleButton();
            this.lblTahminSonuc = new DevExpress.XtraEditors.LabelControl();
            this.btnTahminHesapla = new DevExpress.XtraEditors.SimpleButton();
            this.dateTahminBitis = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnTahminAc = new DevExpress.XtraEditors.SimpleButton();
            this.flowSohbet = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnGonder = new DevExpress.XtraEditors.SimpleButton();
            this.txtMesaj = new DevExpress.XtraEditors.TextEdit();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBildirim = new DevExpress.XtraEditors.SimpleButton();
            this.lstBildirimler = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTahmin)).BeginInit();
            this.pnlTahmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIlaclar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTahminBitis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTahminBitis.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMesaj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstBildirimler)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.btnIlaclar,
            this.btnMusteriler,
            this.btnPersonel,
            this.btnSatislar,
            this.btnCikis,
            this.btnRaporlar});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 8;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(1904, 150);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // btnIlaclar
            // 
            this.btnIlaclar.Caption = "İLAÇLAR";
            this.btnIlaclar.Id = 1;
            this.btnIlaclar.Name = "btnIlaclar";
            this.btnIlaclar.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnIlaclar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnIlaclar_ItemClick);
            // 
            // btnMusteriler
            // 
            this.btnMusteriler.Caption = "HASTALAR";
            this.btnMusteriler.Id = 2;
            this.btnMusteriler.Name = "btnMusteriler";
            this.btnMusteriler.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnMusteriler.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHastalar_ItemClick);
            // 
            // btnPersonel
            // 
            this.btnPersonel.Caption = "barButtonItem1";
            this.btnPersonel.Id = 3;
            this.btnPersonel.Name = "btnPersonel";
            // 
            // btnSatislar
            // 
            this.btnSatislar.Caption = "SATIŞLAR";
            this.btnSatislar.Id = 5;
            this.btnSatislar.Name = "btnSatislar";
            this.btnSatislar.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnSatislar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSatislar_ItemClick);
            // 
            // btnCikis
            // 
            this.btnCikis.Caption = "ÇIKIŞ";
            this.btnCikis.Id = 6;
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnCikis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCikis_ItemClick);
            // 
            // btnRaporlar
            // 
            this.btnRaporlar.Caption = "RAPORLAR";
            this.btnRaporlar.Id = 7;
            this.btnRaporlar.Name = "btnRaporlar";
            this.btnRaporlar.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnRaporlar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRaporlar_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
            this.ribbonPage1.Name = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnIlaclar);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnMusteriler);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnSatislar);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnRaporlar);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.btnCikis);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 1014);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1904, 27);
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // pnlTahmin
            // 
            this.pnlTahmin.Controls.Add(this.cmbIlaclar);
            this.pnlTahmin.Controls.Add(this.btnPanelKapat);
            this.pnlTahmin.Controls.Add(this.lblTahminSonuc);
            this.pnlTahmin.Controls.Add(this.btnTahminHesapla);
            this.pnlTahmin.Controls.Add(this.dateTahminBitis);
            this.pnlTahmin.Controls.Add(this.labelControl2);
            this.pnlTahmin.Controls.Add(this.labelControl1);
            this.pnlTahmin.Location = new System.Drawing.Point(1599, 632);
            this.pnlTahmin.Name = "pnlTahmin";
            this.pnlTahmin.Size = new System.Drawing.Size(288, 312);
            this.pnlTahmin.TabIndex = 3;
            this.pnlTahmin.Text = "                  YAPAY ZEKA İLE SATIŞ TAHMİNİ";
            this.pnlTahmin.Visible = false;
            // 
            // cmbIlaclar
            // 
            this.cmbIlaclar.Location = new System.Drawing.Point(135, 58);
            this.cmbIlaclar.MenuManager = this.ribbon;
            this.cmbIlaclar.Name = "cmbIlaclar";
            this.cmbIlaclar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbIlaclar.Size = new System.Drawing.Size(100, 20);
            this.cmbIlaclar.TabIndex = 7;
            // 
            // btnPanelKapat
            // 
            this.btnPanelKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPanelKapat.ImageOptions.Image")));
            this.btnPanelKapat.Location = new System.Drawing.Point(248, 26);
            this.btnPanelKapat.Name = "btnPanelKapat";
            this.btnPanelKapat.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnPanelKapat.Size = new System.Drawing.Size(35, 35);
            this.btnPanelKapat.TabIndex = 6;
            this.btnPanelKapat.Text = "simpleButton1";
            this.btnPanelKapat.Click += new System.EventHandler(this.btnPanelKapat_Click);
            // 
            // lblTahminSonuc
            // 
            this.lblTahminSonuc.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTahminSonuc.Appearance.Options.UseFont = true;
            this.lblTahminSonuc.Location = new System.Drawing.Point(76, 238);
            this.lblTahminSonuc.Name = "lblTahminSonuc";
            this.lblTahminSonuc.Size = new System.Drawing.Size(148, 21);
            this.lblTahminSonuc.TabIndex = 5;
            this.lblTahminSonuc.Text = "Sonuç Bekleniyor...";
            // 
            // btnTahminHesapla
            // 
            this.btnTahminHesapla.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTahminHesapla.Appearance.Options.UseFont = true;
            this.btnTahminHesapla.Location = new System.Drawing.Point(106, 191);
            this.btnTahminHesapla.Name = "btnTahminHesapla";
            this.btnTahminHesapla.Size = new System.Drawing.Size(118, 23);
            this.btnTahminHesapla.TabIndex = 4;
            this.btnTahminHesapla.Text = "TAHMİN ET";
            this.btnTahminHesapla.Click += new System.EventHandler(this.btnTahminHesapla_Click);
            // 
            // dateTahminBitis
            // 
            this.dateTahminBitis.EditValue = null;
            this.dateTahminBitis.Location = new System.Drawing.Point(135, 117);
            this.dateTahminBitis.MenuManager = this.ribbon;
            this.dateTahminBitis.Name = "dateTahminBitis";
            this.dateTahminBitis.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTahminBitis.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTahminBitis.Size = new System.Drawing.Size(100, 20);
            this.dateTahminBitis.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(18, 116);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(87, 21);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "TARİHİNİZ :";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(30, 56);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 21);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "İLAÇ ADI :";
            // 
            // btnTahminAc
            // 
            this.btnTahminAc.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTahminAc.Appearance.Options.UseFont = true;
            this.btnTahminAc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTahminAc.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnTahminAc.ImageOptions.Image")));
            this.btnTahminAc.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btnTahminAc.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.btnTahminAc.Location = new System.Drawing.Point(1705, 950);
            this.btnTahminAc.Name = "btnTahminAc";
            this.btnTahminAc.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnTahminAc.Size = new System.Drawing.Size(167, 49);
            this.btnTahminAc.TabIndex = 4;
            this.btnTahminAc.Text = "SATIŞ TAHMİNİ";
            this.btnTahminAc.Click += new System.EventHandler(this.btnTahminAc_Click);
            // 
            // flowSohbet
            // 
            this.flowSohbet.AutoScroll = true;
            this.flowSohbet.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowSohbet.Location = new System.Drawing.Point(5, 5);
            this.flowSohbet.Name = "flowSohbet";
            this.flowSohbet.Size = new System.Drawing.Size(403, 225);
            this.flowSohbet.TabIndex = 7;
            this.flowSohbet.WrapContents = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.btnGonder);
            this.panelControl1.Controls.Add(this.txtMesaj);
            this.panelControl1.Controls.Add(this.flowSohbet);
            this.panelControl1.Location = new System.Drawing.Point(12, 578);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(413, 263);
            this.panelControl1.TabIndex = 8;
            this.panelControl1.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(378, 5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton1.Size = new System.Drawing.Size(35, 35);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnGonder
            // 
            this.btnGonder.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnGonder.ImageOptions.SvgImage")));
            this.btnGonder.Location = new System.Drawing.Point(364, 229);
            this.btnGonder.Name = "btnGonder";
            this.btnGonder.Size = new System.Drawing.Size(44, 34);
            this.btnGonder.TabIndex = 9;
            this.btnGonder.Click += new System.EventHandler(this.btnGonder_Click);
            // 
            // txtMesaj
            // 
            this.txtMesaj.Location = new System.Drawing.Point(5, 236);
            this.txtMesaj.MenuManager = this.ribbon;
            this.txtMesaj.Name = "txtMesaj";
            this.txtMesaj.Size = new System.Drawing.Size(353, 20);
            this.txtMesaj.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(17, 920);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 63);
            this.button1.TabIndex = 9;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBildirim
            // 
            this.btnBildirim.AutoSize = true;
            this.btnBildirim.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBildirim.ImageOptions.Image")));
            this.btnBildirim.Location = new System.Drawing.Point(141, 960);
            this.btnBildirim.Name = "btnBildirim";
            this.btnBildirim.Size = new System.Drawing.Size(38, 36);
            this.btnBildirim.TabIndex = 12;
            this.btnBildirim.Click += new System.EventHandler(this.btnBildirim_Click);
            // 
            // lstBildirimler
            // 
            this.lstBildirimler.Location = new System.Drawing.Point(189, 888);
            this.lstBildirimler.Name = "lstBildirimler";
            this.lstBildirimler.Size = new System.Drawing.Size(403, 120);
            this.lstBildirimler.TabIndex = 13;
            this.lstBildirimler.Visible = false;
            // 
            // FrmAnaModul
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.lstBildirimler);
            this.Controls.Add(this.btnBildirim);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTahminAc);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.pnlTahmin);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.IsMdiContainer = true;
            this.Name = "FrmAnaModul";
            this.Text = "FrmAnaModul";
            this.Load += new System.EventHandler(this.FrmAnaModul_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTahmin)).EndInit();
            this.pnlTahmin.ResumeLayout(false);
            this.pnlTahmin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIlaclar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTahminBitis.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTahminBitis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMesaj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstBildirimler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem btnIlaclar;
        private DevExpress.XtraBars.BarButtonItem btnMusteriler;
        private DevExpress.XtraBars.BarButtonItem btnPersonel;
        private DevExpress.XtraBars.BarButtonItem btnSatislar;
        private DevExpress.XtraBars.BarButtonItem btnCikis;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btnRaporlar;
        private DevExpress.XtraEditors.GroupControl pnlTahmin;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnTahminAc;
        private DevExpress.XtraEditors.SimpleButton btnPanelKapat;
        private DevExpress.XtraEditors.LabelControl lblTahminSonuc;
        private DevExpress.XtraEditors.SimpleButton btnTahminHesapla;
        private DevExpress.XtraEditors.DateEdit dateTahminBitis;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.FlowLayoutPanel flowSohbet;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnGonder;
        private DevExpress.XtraEditors.TextEdit txtMesaj;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraEditors.ComboBoxEdit cmbIlaclar;
        private DevExpress.XtraEditors.ListBoxControl lstBildirimler;
        private DevExpress.XtraEditors.SimpleButton btnBildirim;
    }
}