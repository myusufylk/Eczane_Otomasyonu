using DevExpress.XtraEditors;
namespace Eczane_Otomasyonu
{
    partial class FrmHareketler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHareketler));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSatisYap = new DevExpress.XtraEditors.SimpleButton();
            this.gridSepet = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.btnSepeteEkle = new DevExpress.XtraEditors.SimpleButton();
            this.lueIlac = new DevExpress.XtraEditors.LookUpEdit();
            this.btnReceteYukle = new DevExpress.XtraEditors.SimpleButton();
            this.lblToplamTutar = new DevExpress.XtraEditors.LabelControl();
            this.dateTarih = new DevExpress.XtraEditors.DateEdit();
            this.txtBarkod = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtTc = new DevExpress.XtraEditors.TextEdit();
            this.txtFiyat = new DevExpress.XtraEditors.TextEdit();
            this.txtAdet = new DevExpress.XtraEditors.TextEdit();
            this.txtHastaAdi = new DevExpress.XtraEditors.TextEdit();
            this.btnRiskAnaliz = new DevExpress.XtraEditors.SimpleButton();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSepet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueIlac.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiyat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHastaAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(400, 568);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "Aranacak KELİMEYİ GİRİNİZ";
            // 
            // btnSatisYap
            // 
            this.btnSatisYap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSatisYap.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSatisYap.ImageOptions.Image")));
            this.btnSatisYap.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btnSatisYap.Location = new System.Drawing.Point(1276, 514);
            this.btnSatisYap.Name = "btnSatisYap";
            this.btnSatisYap.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnSatisYap.Size = new System.Drawing.Size(200, 46);
            this.btnSatisYap.TabIndex = 7;
            this.btnSatisYap.Text = "SATIŞI ONAYLA";
            this.btnSatisYap.Click += new System.EventHandler(this.btnSatisYap_Click);
            // 
            // gridSepet
            // 
            this.gridSepet.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridSepet.Location = new System.Drawing.Point(1084, 0);
            this.gridSepet.MainView = this.gridView2;
            this.gridSepet.Name = "gridSepet";
            this.gridSepet.Size = new System.Drawing.Size(404, 568);
            this.gridSepet.TabIndex = 2;
            this.gridSepet.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridSepet;
            this.gridView2.Name = "gridView2";
            // 
            // btnSepeteEkle
            // 
            this.btnSepeteEkle.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSepeteEkle.Appearance.Options.UseFont = true;
            this.btnSepeteEkle.Location = new System.Drawing.Point(16, 410);
            this.btnSepeteEkle.Name = "btnSepeteEkle";
            this.btnSepeteEkle.Size = new System.Drawing.Size(389, 23);
            this.btnSepeteEkle.TabIndex = 3;
            this.btnSepeteEkle.Text = "SEPETE EKLE";
            this.btnSepeteEkle.Click += new System.EventHandler(this.btnSepeteEkle_Click);
            // 
            // lueIlac
            // 
            this.lueIlac.Location = new System.Drawing.Point(4, 144);
            this.lueIlac.Name = "lueIlac";
            this.lueIlac.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lueIlac.Properties.Appearance.Options.UseFont = true;
            this.lueIlac.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueIlac.Properties.NullText = "";
            this.lueIlac.Properties.NullValuePrompt = "İLAÇ SEÇİNİZ";
            this.lueIlac.Size = new System.Drawing.Size(401, 28);
            this.lueIlac.TabIndex = 0;
            this.lueIlac.EditValueChanged += new System.EventHandler(this.lueIlac_EditValueChanged);
            // 
            // btnReceteYukle
            // 
            this.btnReceteYukle.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnReceteYukle.Appearance.Options.UseFont = true;
            this.btnReceteYukle.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnReceteYukle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnReceteYukle.ImageOptions.SvgImage")));
            this.btnReceteYukle.Location = new System.Drawing.Point(4, 83);
            this.btnReceteYukle.Name = "btnReceteYukle";
            this.btnReceteYukle.Size = new System.Drawing.Size(401, 38);
            this.btnReceteYukle.TabIndex = 6;
            this.btnReceteYukle.Text = "REÇETE OKU";
            this.btnReceteYukle.Click += new System.EventHandler(this.btnReceteYukle_Click);
            // 
            // lblToplamTutar
            // 
            this.lblToplamTutar.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamTutar.Appearance.Options.UseFont = true;
            this.lblToplamTutar.Location = new System.Drawing.Point(173, 444);
            this.lblToplamTutar.Name = "lblToplamTutar";
            this.lblToplamTutar.Size = new System.Drawing.Size(0, 21);
            this.lblToplamTutar.TabIndex = 4;
            // 
            // dateTarih
            // 
            this.dateTarih.EditValue = null;
            this.dateTarih.Location = new System.Drawing.Point(207, 369);
            this.dateTarih.Name = "dateTarih";
            this.dateTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTarih.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTarih.Size = new System.Drawing.Size(100, 20);
            this.dateTarih.TabIndex = 6;
            // 
            // txtBarkod
            // 
            this.txtBarkod.EditValue = "";
            this.txtBarkod.Location = new System.Drawing.Point(4, 33);
            this.txtBarkod.Name = "txtBarkod";
            this.txtBarkod.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtBarkod.Properties.Appearance.Options.UseFont = true;
            this.txtBarkod.Properties.NullValuePrompt = "BARKOD OKUTUNUZ";
            this.txtBarkod.Size = new System.Drawing.Size(401, 28);
            this.txtBarkod.TabIndex = 5;
            this.txtBarkod.EditValueChanged += new System.EventHandler(this.txtBarkod_EditValueChanged);
            this.txtBarkod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarkod_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(155, 368);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(46, 21);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "TARİH";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.pictureEdit1);
            this.groupControl1.Controls.Add(this.btnRiskAnaliz);
            this.groupControl1.Controls.Add(this.txtTc);
            this.groupControl1.Controls.Add(this.txtFiyat);
            this.groupControl1.Controls.Add(this.txtAdet);
            this.groupControl1.Controls.Add(this.txtHastaAdi);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtBarkod);
            this.groupControl1.Controls.Add(this.dateTarih);
            this.groupControl1.Controls.Add(this.lblToplamTutar);
            this.groupControl1.Controls.Add(this.btnReceteYukle);
            this.groupControl1.Controls.Add(this.lueIlac);
            this.groupControl1.Controls.Add(this.btnSepeteEkle);
            this.groupControl1.Location = new System.Drawing.Point(578, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(431, 541);
            this.groupControl1.TabIndex = 1;
            // 
            // txtTc
            // 
            this.txtTc.Location = new System.Drawing.Point(16, 492);
            this.txtTc.Name = "txtTc";
            this.txtTc.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTc.Properties.Appearance.Options.UseFont = true;
            this.txtTc.Properties.NullValuePrompt = "TC NO :";
            this.txtTc.Size = new System.Drawing.Size(183, 28);
            this.txtTc.TabIndex = 19;
            // 
            // txtFiyat
            // 
            this.txtFiyat.Location = new System.Drawing.Point(4, 204);
            this.txtFiyat.Name = "txtFiyat";
            this.txtFiyat.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFiyat.Properties.Appearance.Options.UseFont = true;
            this.txtFiyat.Properties.NullValuePrompt = "FİYAT :";
            this.txtFiyat.Size = new System.Drawing.Size(178, 28);
            this.txtFiyat.TabIndex = 18;
            // 
            // txtAdet
            // 
            this.txtAdet.EditValue = "";
            this.txtAdet.Location = new System.Drawing.Point(227, 204);
            this.txtAdet.Name = "txtAdet";
            this.txtAdet.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAdet.Properties.Appearance.Options.UseFont = true;
            this.txtAdet.Properties.NullValuePrompt = "ADET";
            this.txtAdet.Size = new System.Drawing.Size(178, 28);
            this.txtAdet.TabIndex = 17;
            // 
            // txtHastaAdi
            // 
            this.txtHastaAdi.EditValue = "";
            this.txtHastaAdi.Location = new System.Drawing.Point(214, 492);
            this.txtHastaAdi.Name = "txtHastaAdi";
            this.txtHastaAdi.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtHastaAdi.Properties.Appearance.Options.UseFont = true;
            this.txtHastaAdi.Properties.NullValuePrompt = "HASTA ADI :";
            this.txtHastaAdi.Size = new System.Drawing.Size(183, 28);
            this.txtHastaAdi.TabIndex = 15;
            // 
            // btnRiskAnaliz
            // 
            this.btnRiskAnaliz.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnRiskAnaliz.Appearance.Options.UseFont = true;
            this.btnRiskAnaliz.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightBottom;
            this.btnRiskAnaliz.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRiskAnaliz.ImageOptions.SvgImage")));
            this.btnRiskAnaliz.Location = new System.Drawing.Point(16, 439);
            this.btnRiskAnaliz.Name = "btnRiskAnaliz";
            this.btnRiskAnaliz.Size = new System.Drawing.Size(389, 35);
            this.btnRiskAnaliz.TabIndex = 20;
            this.btnRiskAnaliz.Text = " Risk Analizi";
            this.btnRiskAnaliz.Click += new System.EventHandler(this.btnRiskAnaliz_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(139, 253);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(152, 96);
            this.pictureEdit1.TabIndex = 21;
            // 
            // FrmHareketler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1488, 568);
            this.Controls.Add(this.btnSatisYap);
            this.Controls.Add(this.gridSepet);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.gridControl1);
            this.Name = "FrmHareketler";
            this.Text = "FrmHareketler";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSepet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueIlac.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiyat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHastaAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnSatisYap;
        private DevExpress.XtraGrid.GridControl gridSepet;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private SimpleButton btnSepeteEkle;
        private LookUpEdit lueIlac;
        private SimpleButton btnReceteYukle;
        private LabelControl lblToplamTutar;
        private DateEdit dateTarih;
        private TextEdit txtBarkod;
        private LabelControl labelControl1;
        private GroupControl groupControl1;
        private TextEdit txtAdet;
        private TextEdit txtFiyat;
        private TextEdit txtHastaAdi;
        private TextEdit txtTc;
        private SimpleButton btnRiskAnaliz;
        private PictureEdit pictureEdit1;
    }
}