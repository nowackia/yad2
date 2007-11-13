namespace Client.UI {
    partial class BuildStripe {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.scrollingPanel = new System.Windows.Forms.Panel();
            this.buttonDown = new System.Windows.Forms.Extended.PictureButton();
            this.buttonUp = new System.Windows.Forms.Extended.PictureButton();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.scrollingPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scrollingPanel
            // 
            this.scrollingPanel.BackColor = System.Drawing.SystemColors.ControlText;
            this.scrollingPanel.Controls.Add(this.flowLayoutPanel1);
            this.scrollingPanel.Location = new System.Drawing.Point(0, 0);
            this.scrollingPanel.Name = "scrollingPanel";
            this.scrollingPanel.Size = new System.Drawing.Size(78, 200);
            this.scrollingPanel.TabIndex = 6;
            // 
            // buttonDown
            // 
            this.buttonDown.Image = null;
            this.buttonDown.Location = new System.Drawing.Point(3, 250);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(72, 14);
            this.buttonDown.TabIndex = 8;
            this.buttonDown.Text = "down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Image = null;
            this.buttonUp.Location = new System.Drawing.Point(3, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(72, 14);
            this.buttonUp.TabIndex = 7;
            this.buttonUp.Text = "up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.scrollingPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(3, 23);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(72, 221);
            this.contentPanel.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.contentPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonDown, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonUp, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(78, 267);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(78, 200);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // BuildStripe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BuildStripe";
            this.Size = new System.Drawing.Size(78, 267);
            this.scrollingPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel scrollingPanel;
        private System.Windows.Forms.Extended.PictureButton buttonDown;
        private System.Windows.Forms.Extended.PictureButton buttonUp;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
