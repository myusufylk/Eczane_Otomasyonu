namespace Eczane_Otomasyonu
{
    partial class FrmHastalar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHastalar));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmbGuvence = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnGuncelle = new DevExpress.XtraEditors.SimpleButton();
            this.btnSil = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtAd = new DevExpress.XtraEditors.TextEdit();
            this.txtSoyad = new DevExpress.XtraEditors.TextEdit();
            this.txtTelefon = new DevExpress.XtraEditors.TextEdit();
            this.txtAdres = new DevExpress.XtraEditors.TextEdit();
            this.txtTc = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGuvence.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoyad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefon.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdres.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControl1.Location = new System.Drawing.Point(0, 848);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1918, 200);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmbGuvence);
            this.groupControl1.Controls.Add(this.btnGuncelle);
            this.groupControl1.Controls.Add(this.btnSil);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.btnKaydet);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtAd);
            this.groupControl1.Controls.Add(this.txtSoyad);
            this.groupControl1.Controls.Add(this.txtTelefon);
            this.groupControl1.Controls.Add(this.txtAdres);
            this.groupControl1.Controls.Add(this.txtTc);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupControl1.Location = new System.Drawing.Point(1679, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(239, 848);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "HASTA İŞLEMLERİ";
            // 
            // cmbGuvence
            // 
            this.cmbGuvence.EditValue = "";
            this.cmbGuvence.Location = new System.Drawing.Point(43, 237);
            this.cmbGuvence.Name = "cmbGuvence";
            this.cmbGuvence.Properties.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbGuvence.Properties.Appearance.Options.UseFont = true;
            this.cmbGuvence.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGuvence.Properties.Items.AddRange(new object[] {
            "SGK",
            "BAĞKUR",
            "EMEKLİ SANDIĞI ",
            "ÖZEL SİGORTA ",
            "YEŞİL KART",
            "YOK "});
            this.cmbGuvence.Properties.NullValuePrompt = "GÜVENCE";
            this.cmbGuvence.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbGuvence.Size = new System.Drawing.Size(153, 28);
            this.cmbGuvence.TabIndex = 14;
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnGuncelle.ImageOptions.Image")));
            this.btnGuncelle.Location = new System.Drawing.Point(192, 374);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnGuncelle.Size = new System.Drawing.Size(35, 35);
            this.btnGuncelle.TabIndex = 12;
            this.btnGuncelle.Click += new System.EventHandler(this.btnGuncelle_Click);
            // 
            // btnSil
            // 
            this.btnSil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSil.ImageOptions.Image")));
            this.btnSil.Location = new System.Drawing.Point(118, 374);
            this.btnSil.Name = "btnSil";
            this.btnSil.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnSil.Size = new System.Drawing.Size(35, 35);
            this.btnSil.TabIndex = 13;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(5, 96);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 21);
            this.labelControl5.TabIndex = 10;
            this.labelControl5.Text = "Soy İsim";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(5, 123);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(93, 21);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "Telefon No :";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(5, 159);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(53, 21);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "Adres :";
            // 
            // btnKaydet
            // 
            this.btnKaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnKaydet.ImageOptions.Image")));
            this.btnKaydet.Location = new System.Drawing.Point(19, 374);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnKaydet.Size = new System.Drawing.Size(35, 35);
            this.btnKaydet.TabIndex = 11;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(5, 68);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 21);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "İSİM";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(0, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(109, 21);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "TC Kimlik No :";
            // 
            // txtAd
            // 
            this.txtAd.Location = new System.Drawing.Point(134, 69);
            this.txtAd.Name = "txtAd";
            this.txtAd.Size = new System.Drawing.Size(100, 20);
            this.txtAd.TabIndex = 5;
            // 
            // txtSoyad
            // 
            this.txtSoyad.Location = new System.Drawing.Point(134, 95);
            this.txtSoyad.Name = "txtSoyad";
            this.txtSoyad.Size = new System.Drawing.Size(100, 20);
            this.txtSoyad.TabIndex = 4;
            // 
            // txtTelefon
            // 
            this.txtTelefon.Location = new System.Drawing.Point(134, 121);
            this.txtTelefon.Name = "txtTelefon";
            this.txtTelefon.Size = new System.Drawing.Size(100, 20);
            this.txtTelefon.TabIndex = 3;
            // 
            // txtAdres
            // 
            this.txtAdres.Location = new System.Drawing.Point(64, 160);
            this.txtAdres.Name = "txtAdres";
            this.txtAdres.Size = new System.Drawing.Size(100, 20);
            this.txtAdres.TabIndex = 2;
            // 
            // txtTc
            // 
            this.txtTc.Location = new System.Drawing.Point(134, 43);
            this.txtTc.Name = "txtTc";
            this.txtTc.Size = new System.Drawing.Size(100, 20);
            this.txtTc.TabIndex = 0;
            // 
            // FrmHastalar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1048);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.gridControl1);
            this.Name = "FrmHastalar";
            this.Text = "FrmHastalar";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGuvence.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoyad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefon.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdres.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txtAd;
        private DevExpress.XtraEditors.TextEdit txtSoyad;
        private DevExpress.XtraEditors.TextEdit txtTelefon;
        private DevExpress.XtraEditors.TextEdit txtAdres;
        private DevExpress.XtraEditors.TextEdit txtTc;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnGuncelle;
        private DevExpress.XtraEditors.SimpleButton btnSil;
        private DevExpress.XtraEditors.SimpleButton btnKaydet;
        private DevExpress.XtraEditors.ComboBoxEdit cmbGuvence;
    }
}