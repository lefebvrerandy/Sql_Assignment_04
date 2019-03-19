namespace Sql_Assignment_04
{
    partial class form_title
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart_main = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cb_Product = new System.Windows.Forms.ComboBox();
            this.cb_Class = new System.Windows.Forms.ComboBox();
            this.lbl_Class = new System.Windows.Forms.Label();
            this.lbl_Product = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_City = new System.Windows.Forms.Label();
            this.cb_City = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart_main)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_main
            // 
            chartArea2.Name = "ChartArea1";
            this.chart_main.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_main.Legends.Add(legend2);
            this.chart_main.Location = new System.Drawing.Point(12, 56);
            this.chart_main.Name = "chart_main";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Test";
            this.chart_main.Series.Add(series2);
            this.chart_main.Size = new System.Drawing.Size(1400, 497);
            this.chart_main.TabIndex = 0;
            this.chart_main.Text = "chartMain";
            title2.Name = "Title1";
            title2.Text = "Chart Title";
            this.chart_main.Titles.Add(title2);
            // 
            // cb_Product
            // 
            this.cb_Product.FormattingEnabled = true;
            this.cb_Product.Location = new System.Drawing.Point(626, 29);
            this.cb_Product.Name = "cb_Product";
            this.cb_Product.Size = new System.Drawing.Size(169, 21);
            this.cb_Product.TabIndex = 1;
            // 
            // cb_Class
            // 
            this.cb_Class.FormattingEnabled = true;
            this.cb_Class.Location = new System.Drawing.Point(451, 29);
            this.cb_Class.Name = "cb_Class";
            this.cb_Class.Size = new System.Drawing.Size(169, 21);
            this.cb_Class.TabIndex = 2;
            // 
            // lbl_Class
            // 
            this.lbl_Class.AutoSize = true;
            this.lbl_Class.Location = new System.Drawing.Point(448, 10);
            this.lbl_Class.Name = "lbl_Class";
            this.lbl_Class.Size = new System.Drawing.Size(72, 13);
            this.lbl_Class.TabIndex = 3;
            this.lbl_Class.Text = "Product Class";
            // 
            // lbl_Product
            // 
            this.lbl_Product.AutoSize = true;
            this.lbl_Product.Location = new System.Drawing.Point(623, 10);
            this.lbl_Product.Name = "lbl_Product";
            this.lbl_Product.Size = new System.Drawing.Size(44, 13);
            this.lbl_Product.TabIndex = 4;
            this.lbl_Product.Text = "Product";
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Location = new System.Drawing.Point(13, 13);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(206, 13);
            this.lbl_title.TabIndex = 5;
            this.lbl_title.Text = "Developed by: Randy Lefebvre - 6842256";
            // 
            // lbl_City
            // 
            this.lbl_City.AutoSize = true;
            this.lbl_City.Location = new System.Drawing.Point(798, 10);
            this.lbl_City.Name = "lbl_City";
            this.lbl_City.Size = new System.Drawing.Size(24, 13);
            this.lbl_City.TabIndex = 7;
            this.lbl_City.Text = "City";
            // 
            // cb_City
            // 
            this.cb_City.FormattingEnabled = true;
            this.cb_City.Location = new System.Drawing.Point(801, 29);
            this.cb_City.Name = "cb_City";
            this.cb_City.Size = new System.Drawing.Size(169, 21);
            this.cb_City.TabIndex = 6;
            // 
            // form_title
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 565);
            this.Controls.Add(this.lbl_City);
            this.Controls.Add(this.cb_City);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.lbl_Product);
            this.Controls.Add(this.lbl_Class);
            this.Controls.Add(this.cb_Class);
            this.Controls.Add(this.cb_Product);
            this.Controls.Add(this.chart_main);
            this.Name = "form_title";
            this.Text = "Food Mart 2008";
            ((System.ComponentModel.ISupportInitialize)(this.chart_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cb_Product;
        private System.Windows.Forms.ComboBox cb_Class;
        private System.Windows.Forms.Label lbl_Class;
        private System.Windows.Forms.Label lbl_Product;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_City;
        private System.Windows.Forms.ComboBox cb_City;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_main;
    }
}

