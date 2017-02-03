namespace Cina219test
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

            this._cina219_1.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDown_cal1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label_voltage_bus1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_voltage_shunt1 = new System.Windows.Forms.Label();
            this.label_calibration1 = new System.Windows.Forms.Label();
            this.label_current1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_power1 = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button_single = new System.Windows.Forms.Button();
            this.label_address1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_voltage_bus2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label_voltage_shunt2 = new System.Windows.Forms.Label();
            this.numericUpDown_cal2 = new System.Windows.Forms.NumericUpDown();
            this.label_calibration2 = new System.Windows.Forms.Label();
            this.label_current2 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label_power2 = new System.Windows.Forms.Label();
            this.label_address2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cal1)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cal2)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown_cal1
            // 
            this.numericUpDown_cal1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown_cal1.Hexadecimal = true;
            this.numericUpDown_cal1.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_cal1.Location = new System.Drawing.Point(158, 67);
            this.numericUpDown_cal1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown_cal1.Name = "numericUpDown_cal1";
            this.numericUpDown_cal1.Size = new System.Drawing.Size(56, 22);
            this.numericUpDown_cal1.TabIndex = 4;
            this.numericUpDown_cal1.ValueChanged += new System.EventHandler(this.numericUpDown_cal_ValueChanged);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.label_voltage_bus1, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label_voltage_shunt1, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.numericUpDown_cal1, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.label_calibration1, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.label_current1, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_power1, 1, 3);
            this.tableLayoutPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel.Location = new System.Drawing.Point(19, 35);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(257, 92);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // label_voltage_bus1
            // 
            this.label_voltage_bus1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_voltage_bus1.AutoSize = true;
            this.label_voltage_bus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_voltage_bus1.Location = new System.Drawing.Point(106, 0);
            this.label_voltage_bus1.Name = "label_voltage_bus1";
            this.label_voltage_bus1.Size = new System.Drawing.Size(46, 16);
            this.label_voltage_bus1.TabIndex = 0;
            this.label_voltage_bus1.Text = "0.0000";
            this.label_voltage_bus1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 28);
            this.label4.TabIndex = 7;
            this.label4.Text = "Calibration";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Voltage(bus):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Voltage(shunt):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current(mA):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_voltage_shunt1
            // 
            this.label_voltage_shunt1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_voltage_shunt1.AutoSize = true;
            this.label_voltage_shunt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_voltage_shunt1.Location = new System.Drawing.Point(106, 16);
            this.label_voltage_shunt1.Name = "label_voltage_shunt1";
            this.label_voltage_shunt1.Size = new System.Drawing.Size(46, 16);
            this.label_voltage_shunt1.TabIndex = 3;
            this.label_voltage_shunt1.Text = "0.0000";
            this.label_voltage_shunt1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label_calibration1
            // 
            this.label_calibration1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_calibration1.AutoSize = true;
            this.label_calibration1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_calibration1.Location = new System.Drawing.Point(106, 64);
            this.label_calibration1.Name = "label_calibration1";
            this.label_calibration1.Size = new System.Drawing.Size(46, 28);
            this.label_calibration1.TabIndex = 6;
            this.label_calibration1.Text = "0.0000";
            this.label_calibration1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_current1
            // 
            this.label_current1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_current1.AutoSize = true;
            this.label_current1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_current1.Location = new System.Drawing.Point(106, 32);
            this.label_current1.Name = "label_current1";
            this.label_current1.Size = new System.Drawing.Size(46, 16);
            this.label_current1.TabIndex = 5;
            this.label_current1.Text = "0.0000";
            this.label_current1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Power(mW):";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_power1
            // 
            this.label_power1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_power1.AutoSize = true;
            this.label_power1.Location = new System.Drawing.Point(106, 48);
            this.label_power1.Name = "label_power1";
            this.label_power1.Size = new System.Drawing.Size(46, 16);
            this.label_power1.TabIndex = 9;
            this.label_power1.Text = "0.0000";
            this.label_power1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(165, 179);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 4;
            this.button_start.Text = "&Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_single
            // 
            this.button_single.Location = new System.Drawing.Point(291, 179);
            this.button_single.Name = "button_single";
            this.button_single.Size = new System.Drawing.Size(75, 23);
            this.button_single.TabIndex = 5;
            this.button_single.Text = "&Single";
            this.button_single.UseVisualStyleBackColor = true;
            this.button_single.Click += new System.EventHandler(this.button_single_Click);
            // 
            // label_address1
            // 
            this.label_address1.AutoSize = true;
            this.label_address1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label_address1.Location = new System.Drawing.Point(15, 9);
            this.label_address1.Name = "label_address1";
            this.label_address1.Size = new System.Drawing.Size(54, 16);
            this.label_address1.TabIndex = 6;
            this.label_address1.Text = "Adress:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label_voltage_bus2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_voltage_shunt2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_cal2, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label_calibration2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label_current2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_power2, 1, 3);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(316, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(257, 92);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // label_voltage_bus2
            // 
            this.label_voltage_bus2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_voltage_bus2.AutoSize = true;
            this.label_voltage_bus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_voltage_bus2.Location = new System.Drawing.Point(106, 0);
            this.label_voltage_bus2.Name = "label_voltage_bus2";
            this.label_voltage_bus2.Size = new System.Drawing.Size(46, 16);
            this.label_voltage_bus2.TabIndex = 0;
            this.label_voltage_bus2.Text = "0.0000";
            this.label_voltage_bus2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 28);
            this.label8.TabIndex = 7;
            this.label8.Text = "Calibration";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "Voltage(bus):";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 16);
            this.label10.TabIndex = 2;
            this.label10.Text = "Voltage(shunt):";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 16);
            this.label11.TabIndex = 4;
            this.label11.Text = "Current(mA):";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_voltage_shunt2
            // 
            this.label_voltage_shunt2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_voltage_shunt2.AutoSize = true;
            this.label_voltage_shunt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_voltage_shunt2.Location = new System.Drawing.Point(106, 16);
            this.label_voltage_shunt2.Name = "label_voltage_shunt2";
            this.label_voltage_shunt2.Size = new System.Drawing.Size(46, 16);
            this.label_voltage_shunt2.TabIndex = 3;
            this.label_voltage_shunt2.Text = "0.0000";
            this.label_voltage_shunt2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // numericUpDown_cal2
            // 
            this.numericUpDown_cal2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown_cal2.Hexadecimal = true;
            this.numericUpDown_cal2.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_cal2.Location = new System.Drawing.Point(158, 67);
            this.numericUpDown_cal2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown_cal2.Name = "numericUpDown_cal2";
            this.numericUpDown_cal2.Size = new System.Drawing.Size(56, 22);
            this.numericUpDown_cal2.TabIndex = 4;
            this.numericUpDown_cal2.ValueChanged += new System.EventHandler(this.numericUpDown_cal2_ValueChanged);
            // 
            // label_calibration2
            // 
            this.label_calibration2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_calibration2.AutoSize = true;
            this.label_calibration2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_calibration2.Location = new System.Drawing.Point(106, 64);
            this.label_calibration2.Name = "label_calibration2";
            this.label_calibration2.Size = new System.Drawing.Size(46, 28);
            this.label_calibration2.TabIndex = 6;
            this.label_calibration2.Text = "0.0000";
            this.label_calibration2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_current2
            // 
            this.label_current2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_current2.AutoSize = true;
            this.label_current2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_current2.Location = new System.Drawing.Point(106, 32);
            this.label_current2.Name = "label_current2";
            this.label_current2.Size = new System.Drawing.Size(46, 16);
            this.label_current2.TabIndex = 5;
            this.label_current2.Text = "0.0000";
            this.label_current2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 16);
            this.label15.TabIndex = 8;
            this.label15.Text = "Power(mW):";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_power2
            // 
            this.label_power2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_power2.AutoSize = true;
            this.label_power2.Location = new System.Drawing.Point(106, 48);
            this.label_power2.Name = "label_power2";
            this.label_power2.Size = new System.Drawing.Size(46, 16);
            this.label_power2.TabIndex = 9;
            this.label_power2.Text = "0.0000";
            this.label_power2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label_address2
            // 
            this.label_address2.AutoSize = true;
            this.label_address2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label_address2.Location = new System.Drawing.Point(313, 9);
            this.label_address2.Name = "label_address2";
            this.label_address2.Size = new System.Drawing.Size(54, 16);
            this.label_address2.TabIndex = 9;
            this.label_address2.Text = "Adress:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 224);
            this.Controls.Add(this.label_address2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label_address1);
            this.Controls.Add(this.button_single);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "CINA219 Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cal1)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cal2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown_cal1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label_voltage_bus1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_voltage_shunt1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_current1;
        private System.Windows.Forms.Label label_calibration1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_power1;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_single;
        private System.Windows.Forms.Label label_address1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_voltage_bus2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_voltage_shunt2;
        private System.Windows.Forms.NumericUpDown numericUpDown_cal2;
        private System.Windows.Forms.Label label_calibration2;
        private System.Windows.Forms.Label label_current2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label_power2;
        private System.Windows.Forms.Label label_address2;
    }
}

