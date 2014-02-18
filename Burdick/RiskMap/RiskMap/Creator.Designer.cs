namespace RiskMap
{
    partial class Creator
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newContinentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editContinentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTerritoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.openImageToolStripMenuItem,
            this.newContinentToolStripMenuItem,
            this.editContinentToolStripMenuItem,
            this.editTerritoryToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(834, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.loadToolStripMenuItem.Text = "Save";
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            this.openImageToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.openImageToolStripMenuItem.Text = "Open Image";
            this.openImageToolStripMenuItem.Click += new System.EventHandler(this.openImageToolStripMenuItem_Click);
            // 
            // newContinentToolStripMenuItem
            // 
            this.newContinentToolStripMenuItem.Name = "newContinentToolStripMenuItem";
            this.newContinentToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.newContinentToolStripMenuItem.Text = "New Continent";
            this.newContinentToolStripMenuItem.Click += new System.EventHandler(this.newContinentToolStripMenuItem_Click);
            // 
            // editContinentToolStripMenuItem
            // 
            this.editContinentToolStripMenuItem.Name = "editContinentToolStripMenuItem";
            this.editContinentToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.editContinentToolStripMenuItem.Text = "Edit Continent";
            this.editContinentToolStripMenuItem.Click += new System.EventHandler(this.editContinentToolStripMenuItem_Click);
            // 
            // editTerritoryToolStripMenuItem
            // 
            this.editTerritoryToolStripMenuItem.Name = "editTerritoryToolStripMenuItem";
            this.editTerritoryToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.editTerritoryToolStripMenuItem.Text = "Edit Territory";
            this.editTerritoryToolStripMenuItem.Click += new System.EventHandler(this.editTerritoryToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(0, 0);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Creator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 463);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Creator";
            this.Text = "Risk Map Creator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editContinentToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem newContinentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTerritoryToolStripMenuItem;
    }
}