﻿using System;

namespace SimTMDG
{
    partial class Main
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.speedComboBox = new System.Windows.Forms.ComboBox();
            this.zoomComboBox = new System.Windows.Forms.ComboBox();
            this.playButton = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.tempLoadButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRESTAPI = new System.Windows.Forms.Button();
            this.buttonTLightTemp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.vehTypeCombo = new System.Windows.Forms.ComboBox();
            this.customParamCheck = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAstar = new System.Windows.Forms.Button();
            this.astarStatus = new System.Windows.Forms.Label();
            this.DaGrid = new SimTMDG.MainForm.CustUserControl();
            this.timerSimulation = new System.Windows.Forms.Timer(this.components);
            this.aNumVal = new System.Windows.Forms.TextBox();
            this.bNumVal = new System.Windows.Forms.TextBox();
            this.sNumVal = new System.Windows.Forms.TextBox();
            this.tNumVal = new System.Windows.Forms.TextBox();
            this.pNumVal = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DaGrid, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1069, 651);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.speedComboBox);
            this.flowLayoutPanel1.Controls.Add(this.zoomComboBox);
            this.flowLayoutPanel1.Controls.Add(this.playButton);
            this.flowLayoutPanel1.Controls.Add(this.stepButton);
            this.flowLayoutPanel1.Controls.Add(this.tempLoadButton);
            this.flowLayoutPanel1.Controls.Add(this.comboBox1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(223, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(867, 30);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // speedComboBox
            // 
            this.speedComboBox.FormattingEnabled = true;
            this.speedComboBox.Items.AddRange(new object[] {
            "1x",
            "2x",
            "4x",
            "8x",
            "16x"});
            this.speedComboBox.Location = new System.Drawing.Point(3, 3);
            this.speedComboBox.Name = "speedComboBox";
            this.speedComboBox.Size = new System.Drawing.Size(121, 21);
            this.speedComboBox.TabIndex = 2;
            this.speedComboBox.SelectedIndexChanged += new System.EventHandler(this.speedComboBox_SelectedIndexChanged);
            // 
            // zoomComboBox
            // 
            this.zoomComboBox.FormattingEnabled = true;
            this.zoomComboBox.Items.AddRange(new object[] {
            "5%",
            "10%",
            "15%",
            "20%",
            "25%",
            "33%",
            "50%",
            "67%",
            "100%",
            "150%",
            "200%",
            "400%"});
            this.zoomComboBox.Location = new System.Drawing.Point(130, 3);
            this.zoomComboBox.Name = "zoomComboBox";
            this.zoomComboBox.Size = new System.Drawing.Size(121, 21);
            this.zoomComboBox.TabIndex = 3;
            this.zoomComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomComboBox_SelectedIndexChanged);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(257, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(338, 3);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(75, 23);
            this.stepButton.TabIndex = 4;
            this.stepButton.Text = "Single Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // tempLoadButton
            // 
            this.tempLoadButton.Location = new System.Drawing.Point(419, 3);
            this.tempLoadButton.Name = "tempLoadButton";
            this.tempLoadButton.Size = new System.Drawing.Size(75, 23);
            this.tempLoadButton.TabIndex = 5;
            this.tempLoadButton.Text = "Load";
            this.tempLoadButton.UseVisualStyleBackColor = true;
            this.tempLoadButton.Click += new System.EventHandler(this.tempLoadButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(500, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnRESTAPI);
            this.flowLayoutPanel2.Controls.Add(this.buttonTLightTemp);
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.textBox1);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.textBox2);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.vehTypeCombo);
            this.flowLayoutPanel2.Controls.Add(this.customParamCheck);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.aNumVal);
            this.flowLayoutPanel2.Controls.Add(this.label6);
            this.flowLayoutPanel2.Controls.Add(this.bNumVal);
            this.flowLayoutPanel2.Controls.Add(this.label7);
            this.flowLayoutPanel2.Controls.Add(this.sNumVal);
            this.flowLayoutPanel2.Controls.Add(this.label8);
            this.flowLayoutPanel2.Controls.Add(this.tNumVal);
            this.flowLayoutPanel2.Controls.Add(this.label9);
            this.flowLayoutPanel2.Controls.Add(this.pNumVal);
            this.flowLayoutPanel2.Controls.Add(this.btnAstar);
            this.flowLayoutPanel2.Controls.Add(this.astarStatus);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 39);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(214, 530);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // btnRESTAPI
            // 
            this.btnRESTAPI.Location = new System.Drawing.Point(3, 3);
            this.btnRESTAPI.Name = "btnRESTAPI";
            this.btnRESTAPI.Size = new System.Drawing.Size(206, 24);
            this.btnRESTAPI.TabIndex = 12;
            this.btnRESTAPI.Text = "Pull Traffic";
            this.btnRESTAPI.UseVisualStyleBackColor = true;
            this.btnRESTAPI.Click += new System.EventHandler(this.btnRESTAPI_Click);
            // 
            // buttonTLightTemp
            // 
            this.buttonTLightTemp.Location = new System.Drawing.Point(3, 33);
            this.buttonTLightTemp.Name = "buttonTLightTemp";
            this.buttonTLightTemp.Size = new System.Drawing.Size(211, 23);
            this.buttonTLightTemp.TabIndex = 4;
            this.buttonTLightTemp.Text = "Change Traffic Light";
            this.buttonTLightTemp.UseVisualStyleBackColor = true;
            this.buttonTLightTemp.Click += new System.EventHandler(this.buttonTLightTemp_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Add Vehicle";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "From";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 115);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "To";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(109, 141);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Vehicle Type";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // vehTypeCombo
            // 
            this.vehTypeCombo.FormattingEnabled = true;
            this.vehTypeCombo.Items.AddRange(new object[] {
            "Car",
            "Bus",
            "Truck"});
            this.vehTypeCombo.Location = new System.Drawing.Point(109, 167);
            this.vehTypeCombo.Name = "vehTypeCombo";
            this.vehTypeCombo.Size = new System.Drawing.Size(100, 21);
            this.vehTypeCombo.TabIndex = 14;
            this.vehTypeCombo.SelectionChangeCommitted += new System.EventHandler(this.vehtypeChanged);
            // 
            // customParamCheck
            // 
            this.customParamCheck.AutoSize = true;
            this.customParamCheck.Location = new System.Drawing.Point(50, 194);
            this.customParamCheck.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
            this.customParamCheck.Name = "customParamCheck";
            this.customParamCheck.Size = new System.Drawing.Size(139, 17);
            this.customParamCheck.TabIndex = 15;
            this.customParamCheck.Text = "Use Custom Parameters";
            this.customParamCheck.UseVisualStyleBackColor = true;
            this.customParamCheck.CheckedChanged += new System.EventHandler(this.customParamCheckedChange);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "a (max acc)";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 18;
            this.label6.Text = "b (max decc)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 266);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 20;
            this.label7.Text = "s0 (safe distance)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 292);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 22;
            this.label8.Text = "T (time gap)";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 318);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 25;
            this.label9.Text = "p (politeness)";
            // 
            // btnAstar
            // 
            this.btnAstar.Location = new System.Drawing.Point(3, 347);
            this.btnAstar.Name = "btnAstar";
            this.btnAstar.Size = new System.Drawing.Size(206, 23);
            this.btnAstar.TabIndex = 10;
            this.btnAstar.Text = "Add Vehicle";
            this.btnAstar.UseVisualStyleBackColor = true;
            this.btnAstar.Click += new System.EventHandler(this.btnAstar_Click);
            // 
            // astarStatus
            // 
            this.astarStatus.Location = new System.Drawing.Point(3, 373);
            this.astarStatus.Name = "astarStatus";
            this.astarStatus.Size = new System.Drawing.Size(206, 77);
            this.astarStatus.TabIndex = 11;
            this.astarStatus.Text = "status";
            // 
            // DaGrid
            // 
            this.DaGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DaGrid.BackColor = System.Drawing.Color.White;
            this.DaGrid.CellHeight = 0;
            this.DaGrid.CellSize = new System.Drawing.Size(0, 0);
            this.DaGrid.CellWidth = 0;
            this.DaGrid.Dimension = new System.Drawing.Point(64, 64);
            this.DaGrid.DrawGrid = false;
            this.DaGrid.Location = new System.Drawing.Point(226, 42);
            this.DaGrid.Margin = new System.Windows.Forms.Padding(6);
            this.DaGrid.Max_X = 64;
            this.DaGrid.Max_Y = 64;
            this.DaGrid.Name = "DaGrid";
            this.DaGrid.Size = new System.Drawing.Size(240, 240);
            this.DaGrid.TabIndex = 3;
            this.DaGrid.Load += new System.EventHandler(this.DaGrid_Load);
            this.DaGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.DaGrid_Paint);
            this.DaGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DaGrid_MouseDown);
            this.DaGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DaGrid_MouseMove);
            this.DaGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DaGrid_MouseUp);
            this.DaGrid.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.DaGrid_MouseWheel);
            this.DaGrid.Resize += new System.EventHandler(this.DaGrid_Resize);
            // 
            // timerSimulation
            // 
            this.timerSimulation.Interval = 67;
            this.timerSimulation.Tick += new System.EventHandler(this.timerSimulation_Tick);
            // 
            // aNumVal
            // 
            this.aNumVal.Location = new System.Drawing.Point(109, 217);
            this.aNumVal.Name = "aNumVal";
            this.aNumVal.Size = new System.Drawing.Size(100, 20);
            this.aNumVal.TabIndex = 26;
            this.aNumVal.Enabled = false;
            
            // 
            // bNumVal
            // 
            this.bNumVal.Location = new System.Drawing.Point(109, 243);
            this.bNumVal.Name = "bNumVal";
            this.bNumVal.Size = new System.Drawing.Size(100, 20);
            this.bNumVal.TabIndex = 27;
            this.bNumVal.Enabled = false;
            
            // 
            // sNumVal
            // 
            this.sNumVal.Location = new System.Drawing.Point(109, 269);
            this.sNumVal.Name = "sNumVal";
            this.sNumVal.Size = new System.Drawing.Size(100, 20);
            this.sNumVal.TabIndex = 28;
            this.sNumVal.Enabled = false;
            
            // 
            // tNumVal
            // 
            this.tNumVal.Location = new System.Drawing.Point(109, 295);
            this.tNumVal.Name = "tNumVal";
            this.tNumVal.Size = new System.Drawing.Size(100, 20);
            this.tNumVal.TabIndex = 29;
            this.pNumVal.Enabled = false;
            
            // 
            // pNumVal
            // 
            this.pNumVal.Location = new System.Drawing.Point(109, 321);
            this.pNumVal.Name = "pNumVal";
            this.pNumVal.Size = new System.Drawing.Size(100, 20);
            this.pNumVal.TabIndex = 30;
            this.tNumVal.Enabled = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 651);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Main";
            this.Text = "SimTMDG";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox speedComboBox;
        private System.Windows.Forms.ComboBox zoomComboBox;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.Timer timerSimulation;
        private MainForm.CustUserControl DaGrid;
        private System.Windows.Forms.Button tempLoadButton;
        private System.Windows.Forms.Button buttonTLightTemp;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnAstar;
        private System.Windows.Forms.Label astarStatus;
        private System.Windows.Forms.Button btnRESTAPI;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox vehTypeCombo;
        private System.Windows.Forms.CheckBox customParamCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox aNumVal;
        private System.Windows.Forms.TextBox bNumVal;
        private System.Windows.Forms.TextBox sNumVal;
        private System.Windows.Forms.TextBox tNumVal;
        private System.Windows.Forms.TextBox pNumVal;
    }
}

