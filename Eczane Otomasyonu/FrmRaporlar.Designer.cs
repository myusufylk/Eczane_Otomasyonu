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
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView2 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.XYDiagram xyDiagram2 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
            this.chartIlaclar = new DevExpress.XtraCharts.ChartControl();
            this.chartCiro = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartIlaclar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCiro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            this.SuspendLayout();
            // 
            // chartIlaclar
            // 
            this.chartIlaclar.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartIlaclar.Location = new System.Drawing.Point(0, 0);
            this.chartIlaclar.Name = "chartIlaclar";
            series3.Name = "Series 1";
            series3.SeriesID = 1;
            series3.View = pieSeriesView2;
            this.chartIlaclar.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.chartIlaclar.Size = new System.Drawing.Size(1481, 200);
            this.chartIlaclar.TabIndex = 0;
            chartTitle3.DXFont = new DevExpress.Drawing.DXFont("Tahoma", 28F, DevExpress.Drawing.DXFontStyle.Bold);
            chartTitle3.Text = "En Çok Satan 5 İlaç";
            chartTitle3.TitleID = 0;
            this.chartIlaclar.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle3});
            // 
            // chartCiro
            // 
            xyDiagram2.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram2.AxisY.VisibleInPanesSerializable = "-1";
            this.chartCiro.Diagram = xyDiagram2;
            this.chartCiro.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartCiro.Location = new System.Drawing.Point(0, 388);
            this.chartCiro.Name = "chartCiro";
            series4.Name = "Series 1";
            series4.SeriesID = 0;
            this.chartCiro.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series4};
            this.chartCiro.Size = new System.Drawing.Size(1481, 200);
            this.chartCiro.TabIndex = 1;
            chartTitle4.DXFont = new DevExpress.Drawing.DXFont("Tahoma", 24F, DevExpress.Drawing.DXFontStyle.Bold);
            chartTitle4.Text = "Günlük Ciro Grafiği";
            chartTitle4.TitleID = 0;
            this.chartCiro.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle4});
            // 
            // FrmRaporlar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 588);
            this.Controls.Add(this.chartCiro);
            this.Controls.Add(this.chartIlaclar);
            this.Name = "FrmRaporlar";
            this.Text = "FrmRaporlar";
            this.Load += new System.EventHandler(this.FrmRaporlar_Load);
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartIlaclar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCiro)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartIlaclar;
        private DevExpress.XtraCharts.ChartControl chartCiro;
    }
}