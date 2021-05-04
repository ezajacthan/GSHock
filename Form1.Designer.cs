
namespace GShock
{
    partial class GShock
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.b1 = new System.Windows.Forms.Button();
            this.b2 = new System.Windows.Forms.Button();
            this.b3 = new System.Windows.Forms.Button();
            this.b4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pmBox = new System.Windows.Forms.CheckBox();
            this.alarmBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // b1
            // 
            this.b1.Location = new System.Drawing.Point(25, 25);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(50, 50);
            this.b1.TabIndex = 0;
            this.b1.Text = "B1";
            this.b1.UseVisualStyleBackColor = true;
            // 
            // b2
            // 
            this.b2.Location = new System.Drawing.Point(300, 25);
            this.b2.Name = "b2";
            this.b2.Size = new System.Drawing.Size(50, 50);
            this.b2.TabIndex = 1;
            this.b2.Text = "B2";
            this.b2.UseVisualStyleBackColor = true;
            // 
            // b3
            // 
            this.b3.Location = new System.Drawing.Point(300, 275);
            this.b3.Name = "b3";
            this.b3.Size = new System.Drawing.Size(50, 50);
            this.b3.TabIndex = 2;
            this.b3.Text = "B3";
            this.b3.UseVisualStyleBackColor = true;
            // 
            // b4
            // 
            this.b4.Location = new System.Drawing.Point(25, 275);
            this.b4.Name = "b4";
            this.b4.Size = new System.Drawing.Size(50, 50);
            this.b4.TabIndex = 3;
            this.b4.Text = "B4";
            this.b4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(25, 140);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(325, 61);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "11:59:59";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pmBox
            // 
            this.pmBox.AutoSize = true;
            this.pmBox.Location = new System.Drawing.Point(25, 225);
            this.pmBox.Name = "pmBox";
            this.pmBox.Size = new System.Drawing.Size(52, 24);
            this.pmBox.TabIndex = 5;
            this.pmBox.Text = "PM";
            this.pmBox.UseVisualStyleBackColor = true;
            // 
            // alarmBox
            // 
            this.alarmBox.AutoSize = true;
            this.alarmBox.Enabled = false;
            this.alarmBox.Location = new System.Drawing.Point(261, 225);
            this.alarmBox.Name = "alarmBox";
            this.alarmBox.Size = new System.Drawing.Size(97, 24);
            this.alarmBox.TabIndex = 6;
            this.alarmBox.Text = "Alarm ON";
            this.alarmBox.UseVisualStyleBackColor = true;
            // 
            // GShock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(382, 353);
            this.Controls.Add(this.alarmBox);
            this.Controls.Add(this.pmBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.b4);
            this.Controls.Add(this.b3);
            this.Controls.Add(this.b2);
            this.Controls.Add(this.b1);
            this.MaximizeBox = false;
            this.Name = "GShock";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b1;
        private System.Windows.Forms.Button b2;
        private System.Windows.Forms.Button b3;
        private System.Windows.Forms.Button b4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox pmBox;
        private System.Windows.Forms.CheckBox alarmBox;
    }
}

