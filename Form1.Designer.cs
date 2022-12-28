namespace PulseAirPrint
{
    partial class PulseAirPrintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PulseAirPrintForm));
            this.thruBox = new System.Windows.Forms.ComboBox();
            this.drillSizeBox = new System.Windows.Forms.ComboBox();
            this.paSide = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rotaryPDBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nGearToInsideDim = new System.Windows.Forms.TextBox();
            this.nGearToOutsideDim = new System.Windows.Forms.TextBox();
            this.insideDim = new System.Windows.Forms.TextBox();
            this.gearToInsideDim = new System.Windows.Forms.TextBox();
            this.gearToOutsideDim = new System.Windows.Forms.TextBox();
            this.okBTN = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // thruBox
            // 
            this.thruBox.FormattingEnabled = true;
            this.thruBox.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.thruBox.Location = new System.Drawing.Point(896, 69);
            this.thruBox.Name = "thruBox";
            this.thruBox.Size = new System.Drawing.Size(121, 21);
            this.thruBox.TabIndex = 0;
            this.thruBox.Text = "No";
            // 
            // drillSizeBox
            // 
            this.drillSizeBox.FormattingEnabled = true;
            this.drillSizeBox.Items.AddRange(new object[] {
            "0.3125"});
            this.drillSizeBox.Location = new System.Drawing.Point(896, 97);
            this.drillSizeBox.Name = "drillSizeBox";
            this.drillSizeBox.Size = new System.Drawing.Size(121, 21);
            this.drillSizeBox.TabIndex = 1;
            this.drillSizeBox.Text = "0.3125";
            // 
            // paSide
            // 
            this.paSide.FormattingEnabled = true;
            this.paSide.Items.AddRange(new object[] {
            "Tailstock",
            "GearSide"});
            this.paSide.Location = new System.Drawing.Point(896, 125);
            this.paSide.Name = "paSide";
            this.paSide.Size = new System.Drawing.Size(121, 21);
            this.paSide.TabIndex = 2;
            this.paSide.Text = "Tailstock";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(837, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Is it thru?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(837, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Drill Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(759, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "What side gets drilled?";
            // 
            // rotaryPDBox
            // 
            this.rotaryPDBox.Location = new System.Drawing.Point(896, 153);
            this.rotaryPDBox.Name = "rotaryPDBox";
            this.rotaryPDBox.Size = new System.Drawing.Size(100, 20);
            this.rotaryPDBox.TabIndex = 6;
            this.rotaryPDBox.Text = "3.14";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(828, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rotary PD";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(713, 273);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // nGearToInsideDim
            // 
            this.nGearToInsideDim.Location = new System.Drawing.Point(21, 81);
            this.nGearToInsideDim.Name = "nGearToInsideDim";
            this.nGearToInsideDim.Size = new System.Drawing.Size(30, 20);
            this.nGearToInsideDim.TabIndex = 9;
            this.nGearToInsideDim.Text = "1";
            // 
            // nGearToOutsideDim
            // 
            this.nGearToOutsideDim.Location = new System.Drawing.Point(21, 107);
            this.nGearToOutsideDim.Name = "nGearToOutsideDim";
            this.nGearToOutsideDim.Size = new System.Drawing.Size(30, 20);
            this.nGearToOutsideDim.TabIndex = 10;
            this.nGearToOutsideDim.Text = "1";
            // 
            // insideDim
            // 
            this.insideDim.Location = new System.Drawing.Point(339, 32);
            this.insideDim.Name = "insideDim";
            this.insideDim.Size = new System.Drawing.Size(30, 20);
            this.insideDim.TabIndex = 11;
            this.insideDim.Text = "1";
            // 
            // gearToInsideDim
            // 
            this.gearToInsideDim.Location = new System.Drawing.Point(663, 81);
            this.gearToInsideDim.Name = "gearToInsideDim";
            this.gearToInsideDim.Size = new System.Drawing.Size(30, 20);
            this.gearToInsideDim.TabIndex = 12;
            this.gearToInsideDim.Text = "1";
            // 
            // gearToOutsideDim
            // 
            this.gearToOutsideDim.Location = new System.Drawing.Point(663, 128);
            this.gearToOutsideDim.Name = "gearToOutsideDim";
            this.gearToOutsideDim.Size = new System.Drawing.Size(30, 20);
            this.gearToOutsideDim.TabIndex = 13;
            this.gearToOutsideDim.Text = "1";
            // 
            // okBTN
            // 
            this.okBTN.Location = new System.Drawing.Point(913, 226);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(75, 23);
            this.okBTN.TabIndex = 14;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // PulseAirPrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 281);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.gearToOutsideDim);
            this.Controls.Add(this.gearToInsideDim);
            this.Controls.Add(this.insideDim);
            this.Controls.Add(this.nGearToOutsideDim);
            this.Controls.Add(this.nGearToInsideDim);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rotaryPDBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paSide);
            this.Controls.Add(this.drillSizeBox);
            this.Controls.Add(this.thruBox);
            this.Name = "PulseAirPrintForm";
            this.Text = "Pulse Air Print Form";
            this.Load += new System.EventHandler(this.PulseAirPrintFormed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.ComboBox thruBox;
        public System.Windows.Forms.ComboBox drillSizeBox;
        public System.Windows.Forms.ComboBox paSide;
        public System.Windows.Forms.TextBox rotaryPDBox;
        public System.Windows.Forms.TextBox nGearToInsideDim;
        public System.Windows.Forms.TextBox nGearToOutsideDim;
        public System.Windows.Forms.TextBox insideDim;
        public System.Windows.Forms.TextBox gearToInsideDim;
        public System.Windows.Forms.TextBox gearToOutsideDim;
        public System.Windows.Forms.Button okBTN;
    }
}