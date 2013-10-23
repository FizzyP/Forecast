namespace ForecastTool
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_precision = new System.Windows.Forms.ComboBox();
            this.comboBox_outputResolution = new System.Windows.Forms.ComboBox();
            this.button_selectSource = new System.Windows.Forms.Button();
            this.button_forecast = new System.Windows.Forms.Button();
            this.openFileDialog_selectSource = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Precision";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output Resolution";
            // 
            // comboBox_precision
            // 
            this.comboBox_precision.FormattingEnabled = true;
            this.comboBox_precision.Location = new System.Drawing.Point(116, 6);
            this.comboBox_precision.Name = "comboBox_precision";
            this.comboBox_precision.Size = new System.Drawing.Size(84, 21);
            this.comboBox_precision.TabIndex = 2;
            // 
            // comboBox_outputResolution
            // 
            this.comboBox_outputResolution.FormattingEnabled = true;
            this.comboBox_outputResolution.Location = new System.Drawing.Point(116, 28);
            this.comboBox_outputResolution.Name = "comboBox_outputResolution";
            this.comboBox_outputResolution.Size = new System.Drawing.Size(84, 21);
            this.comboBox_outputResolution.TabIndex = 3;
            // 
            // button_selectSource
            // 
            this.button_selectSource.Location = new System.Drawing.Point(15, 57);
            this.button_selectSource.Name = "button_selectSource";
            this.button_selectSource.Size = new System.Drawing.Size(89, 23);
            this.button_selectSource.TabIndex = 4;
            this.button_selectSource.Text = "Select Source";
            this.button_selectSource.UseVisualStyleBackColor = true;
            this.button_selectSource.Click += new System.EventHandler(this.button_selectSource_Click);
            // 
            // button_forecast
            // 
            this.button_forecast.BackColor = System.Drawing.Color.Chartreuse;
            this.button_forecast.Location = new System.Drawing.Point(116, 57);
            this.button_forecast.Name = "button_forecast";
            this.button_forecast.Size = new System.Drawing.Size(84, 23);
            this.button_forecast.TabIndex = 5;
            this.button_forecast.Text = "Forecast";
            this.button_forecast.UseVisualStyleBackColor = false;
            this.button_forecast.Click += new System.EventHandler(this.button_forecast_Click);
            // 
            // openFileDialog_selectSource
            // 
            this.openFileDialog_selectSource.RestoreDirectory = true;
            this.openFileDialog_selectSource.Title = "Select Source";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 91);
            this.Controls.Add(this.button_forecast);
            this.Controls.Add(this.button_selectSource);
            this.Controls.Add(this.comboBox_outputResolution);
            this.Controls.Add(this.comboBox_precision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Forecast v0.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_precision;
        private System.Windows.Forms.ComboBox comboBox_outputResolution;
        private System.Windows.Forms.Button button_selectSource;
        private System.Windows.Forms.Button button_forecast;
        private System.Windows.Forms.OpenFileDialog openFileDialog_selectSource;
    }
}

