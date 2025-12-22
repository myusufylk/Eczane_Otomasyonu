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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHareketler));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSatisYap = new DevExpress.XtraEditors.SimpleButton();
            this.dateTarih = new DevExpress.XtraEditors.DateEdit();
            this.txtToplam = new DevExpress.XtraEditors.TextEdit();
            this.txtFiyat = new DevExpress.XtraEditors.TextEdit();
            this.txtAdet = new DevExpress.XtraEditors.TextEdit();
            this.txtHastaAdi = new DevExpress.XtraEditors.TextEdit();
            this.txtTc = new DevExpress.XtraEditors.TextEdit();
            this.lueIlac = new DevExpress.XtraEditors.LookUpEdit();
            this.gridSepet = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSepeteEkle = new DevExpress.XtraEditors.SimpleButton();
            this.lblToplamTutar = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToplam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiyat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHastaAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueIlac.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSepet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(400, 583);
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
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.btnSatisYap);
            this.groupControl1.Controls.Add(this.dateTarih);
            this.groupControl1.Controls.Add(this.txtToplam);
            this.groupControl1.Controls.Add(this.txtFiyat);
            this.groupControl1.Controls.Add(this.txtAdet);
            this.groupControl1.Controls.Add(this.txtHastaAdi);
            this.groupControl1.Controls.Add(this.txtTc);
            this.groupControl1.Controls.Add(this.lueIlac);
            this.groupControl1.Location = new System.Drawing.Point(658, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(411, 258);
            this.groupControl1.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(295, 77);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(46, 21);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "TARİH";
            // 
            // btnSatisYap
            // 
            this.btnSatisYap.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSatisYap.ImageOptions.Image")));
            this.btnSatisYap.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btnSatisYap.Location = new System.Drawing.Point(135, 198);
            this.btnSatisYap.Name = "btnSatisYap";
            this.btnSatisYap.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnSatisYap.Size = new System.Drawing.Size(130, 46);
            this.btnSatisYap.TabIndex = 7;
            this.btnSatisYap.Text = "SATIŞI ONAYLA";
            this.btnSatisYap.Click += new System.EventHandler(this.btnSatisYap_Click);
            // 
            // dateTarih
            // 
            this.dateTarih.EditValue = null;
            this.dateTarih.Location = new System.Drawing.Point(282, 104);
            this.dateTarih.Name = "dateTarih";
            this.dateTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTarih.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTarih.Size = new System.Drawing.Size(100, 20);
            this.dateTarih.TabIndex = 6;
            // 
            // txtToplam
            // 
            this.txtToplam.EditValue = "TOPLAM :";
            this.txtToplam.Enabled = false;
            this.txtToplam.Location = new System.Drawing.Point(135, 154);
            this.txtToplam.Name = "txtToplam";
            this.txtToplam.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtToplam.Properties.Appearance.Options.UseFont = true;
            this.txtToplam.Size = new System.Drawing.Size(130, 28);
            this.txtToplam.TabIndex = 5;
            // 
            // txtFiyat
            // 
            this.txtFiyat.EditValue = "FİYAT :";
            this.txtFiyat.Enabled = false;
            this.txtFiyat.Location = new System.Drawing.Point(164, 36);
            this.txtFiyat.Name = "txtFiyat";
            this.txtFiyat.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFiyat.Properties.Appearance.Options.UseFont = true;
            this.txtFiyat.Size = new System.Drawing.Size(81, 28);
            this.txtFiyat.TabIndex = 4;
            // 
            // txtAdet
            // 
            this.txtAdet.EditValue = "ADET :";
            this.txtAdet.Location = new System.Drawing.Point(295, 36);
            this.txtAdet.Name = "txtAdet";
            this.txtAdet.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAdet.Properties.Appearance.Options.UseFont = true;
            this.txtAdet.Size = new System.Drawing.Size(100, 28);
            this.txtAdet.TabIndex = 3;
            this.txtAdet.TextChanged += new System.EventHandler(this.txtAdet_TextChanged);
            // 
            // txtHastaAdi
            // 
            this.txtHastaAdi.EditValue = "HASTA ADI :";
            this.txtHastaAdi.Location = new System.Drawing.Point(5, 104);
            this.txtHastaAdi.Name = "txtHastaAdi";
            this.txtHastaAdi.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtHastaAdi.Properties.Appearance.Options.UseFont = true;
            this.txtHastaAdi.Size = new System.Drawing.Size(240, 28);
            this.txtHastaAdi.TabIndex = 2;
            // 
            // txtTc
            // 
            this.txtTc.EditValue = "TC No :";
            this.txtTc.Location = new System.Drawing.Point(5, 70);
            this.txtTc.Name = "txtTc";
            this.txtTc.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTc.Properties.Appearance.Options.UseFont = true;
            this.txtTc.Size = new System.Drawing.Size(240, 28);
            this.txtTc.TabIndex = 1;
            this.txtTc.Leave += new System.EventHandler(this.txtTc_Leave);
            // 
            // lueIlac
            // 
            this.lueIlac.Location = new System.Drawing.Point(5, 36);
            this.lueIlac.Name = "lueIlac";
            this.lueIlac.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lueIlac.Properties.Appearance.Options.UseFont = true;
            this.lueIlac.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueIlac.Properties.NullText = "İLAÇ SEÇİNİZ";
            this.lueIlac.Size = new System.Drawing.Size(135, 28);
            this.lueIlac.TabIndex = 0;
            this.lueIlac.EditValueChanged += new System.EventHandler(this.lueIlac_EditValueChanged);
            // 
            // gridSepet
            // 
            this.gridSepet.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridSepet.Location = new System.Drawing.Point(1085, 0);
            this.gridSepet.MainView = this.gridView2;
            this.gridSepet.Name = "gridSepet";
            this.gridSepet.Size = new System.Drawing.Size(404, 583);
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
            this.btnSepeteEkle.Location = new System.Drawing.Point(704, 415);
            this.btnSepeteEkle.Name = "btnSepeteEkle";
            this.btnSepeteEkle.Size = new System.Drawing.Size(75, 23);
            this.btnSepeteEkle.TabIndex = 3;
            this.btnSepeteEkle.Text = "SEPETE EKLE";
            this.btnSepeteEkle.Click += new System.EventHandler(this.btnSepeteEkle_Click);
            // 
            // lblToplamTutar
            // 
            this.lblToplamTutar.Location = new System.Drawing.Point(638, 370);
            this.lblToplamTutar.Name = "lblToplamTutar";
            this.lblToplamTutar.Size = new System.Drawing.Size(63, 13);
            this.lblToplamTutar.TabIndex = 4;
            this.lblToplamTutar.Text = "labelControl2";
            // 
            // FrmHareketler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 583);
            this.Controls.Add(this.lblToplamTutar);
            this.Controls.Add(this.btnSepeteEkle);
            this.Controls.Add(this.gridSepet);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.gridControl1);
            this.Name = "FrmHareketler";
            this.Text = "FrmHareketler";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToplam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiyat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHastaAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueIlac.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSepet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LookUpEdit lueIlac;
        private DevExpress.XtraEditors.SimpleButton btnSatisYap;
        private DevExpress.XtraEditors.DateEdit dateTarih;
        private DevExpress.XtraEditors.TextEdit txtToplam;
        private DevExpress.XtraEditors.TextEdit txtFiyat;
        private DevExpress.XtraEditors.TextEdit txtAdet;
        private DevExpress.XtraEditors.TextEdit txtHastaAdi;
        private DevExpress.XtraEditors.TextEdit txtTc;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gridSepet;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private SimpleButton btnSepeteEkle;
        private LabelControl lblToplamTutar;
    }
}