namespace RiskMessageThread
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
            this.placeButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.attackButton = new System.Windows.Forms.Button();
            this.drawButton = new System.Windows.Forms.Button();
            this.tradeButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // placeButton
            // 
            this.placeButton.Location = new System.Drawing.Point(143, 28);
            this.placeButton.Name = "placeButton";
            this.placeButton.Size = new System.Drawing.Size(94, 23);
            this.placeButton.TabIndex = 0;
            this.placeButton.Text = "Place army";
            this.placeButton.UseVisualStyleBackColor = true;
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(257, 28);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(114, 23);
            this.moveButton.TabIndex = 1;
            this.moveButton.Text = "Move army";
            this.moveButton.UseVisualStyleBackColor = true;
            // 
            // attackButton
            // 
            this.attackButton.Location = new System.Drawing.Point(391, 28);
            this.attackButton.Name = "attackButton";
            this.attackButton.Size = new System.Drawing.Size(75, 23);
            this.attackButton.TabIndex = 2;
            this.attackButton.Text = "Attack";
            this.attackButton.UseVisualStyleBackColor = true;
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(578, 28);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(90, 23);
            this.drawButton.TabIndex = 3;
            this.drawButton.Text = "Draw card";
            this.drawButton.UseVisualStyleBackColor = true;
            // 
            // tradeButton
            // 
            this.tradeButton.Location = new System.Drawing.Point(22, 28);
            this.tradeButton.Name = "tradeButton";
            this.tradeButton.Size = new System.Drawing.Size(101, 23);
            this.tradeButton.TabIndex = 4;
            this.tradeButton.Text = "Trade cards";
            this.tradeButton.UseVisualStyleBackColor = true;
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(481, 28);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(80, 23);
            this.doneButton.TabIndex = 5;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 62);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(697, 25);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(34, 20);
            this.toolStripStatusLabel1.Text = "Idle";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 87);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.tradeButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.attackButton);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.placeButton);
            this.Name = "Form1";
            this.Text = "RISK";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button placeButton;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button attackButton;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button tradeButton;
        private System.Windows.Forms.Button doneButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

