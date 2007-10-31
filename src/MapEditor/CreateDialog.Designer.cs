namespace MapEditor
{
    partial class CreateDialog
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
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.btnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).BeginInit();
            this.SuspendLayout();
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(12, 9);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(38, 13);
            this.widthLabel.TabIndex = 0;
            this.widthLabel.Text = "Width:";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(9, 35);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(41, 13);
            this.heightLabel.TabIndex = 1;
            this.heightLabel.Text = "Height:";
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(56, 7);
            this.width.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.width.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(46, 20);
            this.width.TabIndex = 2;
            this.width.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // height
            // 
            this.height.Location = new System.Drawing.Point(56, 33);
            this.height.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.height.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(46, 20);
            this.height.TabIndex = 3;
            this.height.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // btnCreate
            // 
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Location = new System.Drawing.Point(12, 59);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(90, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // CreateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(116, 96);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.height);
            this.Controls.Add(this.width);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.Name = "CreateDialog";
            this.Text = "CreateDialog";
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.Button btnCreate;
    }
}