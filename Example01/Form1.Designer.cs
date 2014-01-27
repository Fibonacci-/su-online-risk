namespace Example01
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.initializationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redistributeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fortificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(719, 353);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.initializationToolStripMenuItem,
            this.redistributeToolStripMenuItem,
            this.attackToolStripMenuItem,
            this.fortificationToolStripMenuItem,
            this.doneToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(721, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // initializationToolStripMenuItem
            // 
            this.initializationToolStripMenuItem.Name = "initializationToolStripMenuItem";
            this.initializationToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.initializationToolStripMenuItem.Text = "Initialization";
            // 
            // redistributeToolStripMenuItem
            // 
            this.redistributeToolStripMenuItem.Name = "redistributeToolStripMenuItem";
            this.redistributeToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.redistributeToolStripMenuItem.Text = "Redistribute";
            // 
            // attackToolStripMenuItem
            // 
            this.attackToolStripMenuItem.Name = "attackToolStripMenuItem";
            this.attackToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.attackToolStripMenuItem.Text = "Attack";
            // 
            // fortificationToolStripMenuItem
            // 
            this.fortificationToolStripMenuItem.Name = "fortificationToolStripMenuItem";
            this.fortificationToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.fortificationToolStripMenuItem.Text = "Fortification";
            // 
            // doneToolStripMenuItem
            // 
            this.doneToolStripMenuItem.Name = "doneToolStripMenuItem";
            this.doneToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.doneToolStripMenuItem.Text = "Done";
            this.doneToolStripMenuItem.Click += new System.EventHandler(this.doneToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 383);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(721, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 405);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem initializationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redistributeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fortificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doneToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

