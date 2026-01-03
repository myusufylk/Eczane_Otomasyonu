namespace Eczane_Otomasyonu
{
    partial class FrmRaporlar
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
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRaporlar));
            this.chartIlaclar = new DevExpress.XtraCharts.ChartControl();
            this.chartCiro = new DevExpress.XtraCharts.ChartControl();
            this.lblToplamHasta = new DevExpress.XtraEditors.LabelControl();
            this.lblToplamStok = new DevExpress.XtraEditors.LabelControl();
            this.dateBaslangic = new DevExpress.XtraEditors.DateEdit();
            this.dateBitis = new DevExpress.XtraEditors.DateEdit();
            this.btnFiltrele = new DevExpress.XtraEditors.SimpleButton();
            this.btnPdf = new DevExpress.XtraEditors.SimpleButton();
            this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblToplamCiro = new DevExpress.XtraEditors.LabelControl();
            this.groupCiro = new DevExpress.XtraEditors.GroupControl();
            this.groupHasta = new DevExpress.XtraEditors.GroupControl();
            this.groupStok = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartIlaclar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCiro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBaslangic.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBaslangic.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBitis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBitis.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupCiro)).BeginInit();
            this.groupCiro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupHasta)).BeginInit();
            this.groupHasta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupStok)).BeginInit();
            this.groupStok.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartIlaclar
            // 
            this.chartIlaclar.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartIlaclar.Location = new System.Drawing.Point(0, 0);
            this.chartIlaclar.Name = "chartIlaclar";
            series1.Name = "Series 1";
            series1.SeriesID = 1;
            series1.View = pieSeriesView1;
            this.chartIlaclar.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartIlaclar.Size = new System.Drawing.Size(1918, 200);
            this.chartIlaclar.TabIndex = 0;
            chartTitle1.DXFont = new DevExpress.Drawing.DXFont("Tahoma", 28F, DevExpress.Drawing.DXFontStyle.Bold);
            chartTitle1.Text = " En Çok Satan 5 İlaç";
            chartTitle1.TitleID = 0;
            this.chartIlaclar.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // chartCiro
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartCiro.Diagram = xyDiagram1;
            this.chartCiro.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartCiro.Location = new System.Drawing.Point(0, 848);
            this.chartCiro.Name = "chartCiro";
            series2.Name = "Series 1";
            series2.SeriesID = 0;
            this.chartCiro.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series2};
            this.chartCiro.Size = new System.Drawing.Size(1918, 200);
            this.chartCiro.TabIndex = 1;
            chartTitle2.DXFont = new DevExpress.Drawing.DXFont("Tahoma", 24F, DevExpress.Drawing.DXFontStyle.Bold);
            chartTitle2.Text = "Günlük Ciro Grafiği";
            chartTitle2.TitleID = 0;
            this.chartCiro.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle2});
            // 
            // lblToplamHasta
            // 
            this.lblToplamHasta.Location = new System.Drawing.Point(58, 59);
            this.lblToplamHasta.Name = "lblToplamHasta";
            this.lblToplamHasta.Size = new System.Drawing.Size(63, 13);
            this.lblToplamHasta.TabIndex = 0;
            this.lblToplamHasta.Text = "labelControl1";
            // 
            // lblToplamStok
            // 
            this.lblToplamStok.Location = new System.Drawing.Point(75, 59);
            this.lblToplamStok.Name = "lblToplamStok";
            this.lblToplamStok.Size = new System.Drawing.Size(63, 13);
            this.lblToplamStok.TabIndex = 0;
            this.lblToplamStok.Text = "labelControl2";
            // 
            // dateBaslangic
            // 
            this.dateBaslangic.EditValue = null;
            this.dateBaslangic.Location = new System.Drawing.Point(5, 26);
            this.dateBaslangic.Name = "dateBaslangic";
            this.dateBaslangic.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBaslangic.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBaslangic.Properties.NullValuePrompt = "BAŞLANGIÇ TARİHİ";
            this.dateBaslangic.Size = new System.Drawing.Size(100, 20);
            this.dateBaslangic.TabIndex = 5;
            // 
            // dateBitis
            // 
            this.dateBitis.EditValue = null;
            this.dateBitis.Location = new System.Drawing.Point(145, 26);
            this.dateBitis.Name = "dateBitis";
            this.dateBitis.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBitis.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBitis.Properties.NullValuePrompt = "BİTİŞ TARİHİ";
            this.dateBitis.Size = new System.Drawing.Size(100, 20);
            this.dateBitis.TabIndex = 6;
            // 
            // btnFiltrele
            // 
            this.btnFiltrele.Appearance.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnFiltrele.Appearance.Options.UseFont = true;
            this.btnFiltrele.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnFiltrele.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnFiltrele.ImageOptions.SvgImage")));
            this.btnFiltrele.Location = new System.Drawing.Point(52, 63);
            this.btnFiltrele.Name = "btnFiltrele";
            this.btnFiltrele.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnFiltrele.Size = new System.Drawing.Size(120, 32);
            this.btnFiltrele.TabIndex = 7;
            this.btnFiltrele.Text = "Filtrele";
            this.btnFiltrele.Click += new System.EventHandler(this.btnFiltrele_Click);
            // 
            // btnPdf
            // 
            this.btnPdf.Location = new System.Drawing.Point(12, 541);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(75, 23);
            this.btnPdf.TabIndex = 8;
            this.btnPdf.Text = "PDF Kaydet";
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(1831, 541);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 9;
            this.btnExcel.Text = "EXCEL Kaydet";
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.dateBaslangic);
            this.groupControl1.Controls.Add(this.dateBitis);
            this.groupControl1.Controls.Add(this.btnFiltrele);
            this.groupControl1.Location = new System.Drawing.Point(21, 28);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(252, 100);
            this.groupControl1.TabIndex = 10;
            this.groupControl1.Text = "ZAMAN ARALIĞI";
            this.groupControl1.Click += new System.EventHandler(this.btnFiltrele_Click);
            // 
            // lblToplamCiro
            // 
            this.lblToplamCiro.Location = new System.Drawing.Point(44, 56);
            this.lblToplamCiro.Name = "lblToplamCiro";
            this.lblToplamCiro.Size = new System.Drawing.Size(63, 13);
            this.lblToplamCiro.TabIndex = 0;
            this.lblToplamCiro.Text = "labelControl1";
            // 
            // groupCiro
            // 
            this.groupCiro.Controls.Add(this.lblToplamCiro);
            this.groupCiro.Location = new System.Drawing.Point(12, 362);
            this.groupCiro.Name = "groupCiro";
            this.groupCiro.Size = new System.Drawing.Size(200, 100);
            this.groupCiro.TabIndex = 12;
            this.groupCiro.Text = "CİRO";
            // 
            // groupHasta
            // 
            this.groupHasta.Controls.Add(this.lblToplamHasta);
            this.groupHasta.Location = new System.Drawing.Point(854, 362);
            this.groupHasta.Name = "groupHasta";
            this.groupHasta.Size = new System.Drawing.Size(200, 100);
            this.groupHasta.TabIndex = 13;
            this.groupHasta.Text = "HASTA SAYISI";
            // 
            // groupStok
            // 
            this.groupStok.Controls.Add(this.lblToplamStok);
            this.groupStok.Location = new System.Drawing.Point(1706, 362);
            this.groupStok.Name = "groupStok";
            this.groupStok.Size = new System.Drawing.Size(200, 100);
            this.groupStok.TabIndex = 14;
            this.groupStok.Text = "STOKTAKİ İLAÇ";
            // 
            // FrmRaporlar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1048);
            this.Controls.Add(this.groupStok);
            this.Controls.Add(this.groupHasta);
            this.Controls.Add(this.groupCiro);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnPdf);
            this.Controls.Add(this.chartCiro);
            this.Controls.Add(this.chartIlaclar);
            this.Name = "FrmRaporlar";
            this.Text = "FrmRaporlar";
            this.Load += new System.EventHandler(this.FrmRaporlar_Load);
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartIlaclar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCiro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBaslangic.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBaslangic.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBitis.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBitis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupCiro)).EndInit();
            this.groupCiro.ResumeLayout(false);
            this.groupCiro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupHasta)).EndInit();
            this.groupHasta.ResumeLayout(false);
            this.groupHasta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupStok)).EndInit();
            this.groupStok.ResumeLayout(false);
            this.groupStok.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartIlaclar;
        private DevExpress.XtraCharts.ChartControl chartCiro;
        private DevExpress.XtraEditors.LabelControl lblToplamHasta;
        private DevExpress.XtraEditors.LabelControl lblToplamStok;
        private DevExpress.XtraEditors.DateEdit dateBaslangic;
        private DevExpress.XtraEditors.DateEdit dateBitis;
        private DevExpress.XtraEditors.SimpleButton btnFiltrele;
        private DevExpress.XtraEditors.SimpleButton btnPdf;
        private DevExpress.XtraEditors.SimpleButton btnExcel;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblToplamCiro;
        private DevExpress.XtraEditors.GroupControl groupCiro;
        private DevExpress.XtraEditors.GroupControl groupHasta;
        private DevExpress.XtraEditors.GroupControl groupStok;
    }
}