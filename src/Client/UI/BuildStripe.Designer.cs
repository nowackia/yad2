namespace Yad.UI.Client {
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonDown = new System.Windows.Forms.Extended.PictureButton();
            this.buttonUp = new System.Windows.Forms.Extended.PictureButton();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.scrollingPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
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
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.BackgroundImage = global::Yad.Properties.Resources.UI_Down;
            this.buttonDown.Image = null;
            this.buttonDown.Location = new System.Drawing.Point(3, 250);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.PressedImage = global::Yad.Properties.Resources.UI_Down_Pressed;
            this.buttonDown.Size = new System.Drawing.Size(72, 14);
            this.buttonDown.TabIndex = 8;
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.BackgroundImage = global::Yad.Properties.Resources.UI_Up;
            this.buttonUp.Image = null;
            this.buttonUp.Location = new System.Drawing.Point(3, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.PressedImage = global::Yad.Properties.Resources.UI_Up_Pressed;
            this.buttonUp.Size = new System.Drawing.Size(72, 14);
            this.buttonUp.TabIndex = 7;
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
            this.contentPanel.SizeChanged += new System.EventHandler(this.contentPanel_SizeChanged);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.BackgroundImage = global::Yad.Properties.Resources.UI_Background;
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.contentPanel, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonDown, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUp, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(78, 267);
            this.tableLayoutPanelMain.TabIndex = 9;
            // 
            // BuildStripe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Yad.Properties.Resources.UI_Background;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "BuildStripe";
            this.Size = new System.Drawing.Size(78, 267);
            this.scrollingPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel scrollingPanel;
        private System.Windows.Forms.Extended.PictureButton buttonDown;
        private System.Windows.Forms.Extended.PictureButton buttonUp;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
