namespace Engine.WinForms
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.raster = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loadLabel = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.FPSLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.drawPointsCheckBox = new System.Windows.Forms.CheckBox();
            this.drawEdgesCheckBox = new System.Windows.Forms.CheckBox();
            this.drawFacesCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.raster)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.raster, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // raster
            // 
            this.raster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.raster.Location = new System.Drawing.Point(3, 3);
            this.raster.Name = "raster";
            this.raster.Size = new System.Drawing.Size(594, 444);
            this.raster.TabIndex = 0;
            this.raster.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.drawPointsCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.drawFacesCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.drawEdgesCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.FPSLabel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(603, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 444);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loadLabel);
            this.groupBox1.Controls.Add(this.loadButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load File";
            // 
            // loadLabel
            // 
            this.loadLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadLabel.AutoSize = true;
            this.loadLabel.Location = new System.Drawing.Point(9, 45);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(72, 13);
            this.loadLabel.TabIndex = 1;
            this.loadLabel.Text = "No file loaded";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(6, 19);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Load .off file";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // FPSLabel
            // 
            this.FPSLabel.AutoSize = true;
            this.FPSLabel.Location = new System.Drawing.Point(93, 129);
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(53, 13);
            this.FPSLabel.TabIndex = 3;
            this.FPSLabel.Text = "FPSLabel";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // drawPointsCheckBox
            // 
            this.drawPointsCheckBox.AutoSize = true;
            this.drawPointsCheckBox.Location = new System.Drawing.Point(3, 109);
            this.drawPointsCheckBox.Name = "drawPointsCheckBox";
            this.drawPointsCheckBox.Size = new System.Drawing.Size(83, 17);
            this.drawPointsCheckBox.TabIndex = 4;
            this.drawPointsCheckBox.Text = "Draw Points";
            this.drawPointsCheckBox.UseVisualStyleBackColor = true;
            this.drawPointsCheckBox.CheckedChanged += new System.EventHandler(this.refreshScreenEvent);
            // 
            // drawEdgesCheckBox
            // 
            this.drawEdgesCheckBox.AutoSize = true;
            this.drawEdgesCheckBox.Location = new System.Drawing.Point(3, 132);
            this.drawEdgesCheckBox.Name = "drawEdgesCheckBox";
            this.drawEdgesCheckBox.Size = new System.Drawing.Size(84, 17);
            this.drawEdgesCheckBox.TabIndex = 5;
            this.drawEdgesCheckBox.Text = "Draw Edges";
            this.drawEdgesCheckBox.UseVisualStyleBackColor = true;
            this.drawEdgesCheckBox.CheckedChanged += new System.EventHandler(this.refreshScreenEvent);
            // 
            // drawFacesCheckBox
            // 
            this.drawFacesCheckBox.AutoSize = true;
            this.drawFacesCheckBox.Location = new System.Drawing.Point(92, 109);
            this.drawFacesCheckBox.Name = "drawFacesCheckBox";
            this.drawFacesCheckBox.Size = new System.Drawing.Size(83, 17);
            this.drawFacesCheckBox.TabIndex = 6;
            this.drawFacesCheckBox.Text = "Draw Faces";
            this.drawFacesCheckBox.UseVisualStyleBackColor = true;
            this.drawFacesCheckBox.CheckedChanged += new System.EventHandler(this.refreshScreenEvent);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "Form1";
            this.Text = "3D Engine";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.refreshScreenEvent);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.raster)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox raster;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label loadLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label FPSLabel;
        private System.Windows.Forms.CheckBox drawPointsCheckBox;
        private System.Windows.Forms.CheckBox drawFacesCheckBox;
        private System.Windows.Forms.CheckBox drawEdgesCheckBox;
    }
}

