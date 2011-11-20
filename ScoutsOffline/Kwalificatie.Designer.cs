namespace ScoutsOffline
{
    partial class Kwalificatie
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
            this.label1 = new System.Windows.Forms.Label();
            this.RollenCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KwalificatieCombo = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.ExaminatorCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LedenListBox = new System.Windows.Forms.ListBox();
            this.Toekennen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rol";
            // 
            // RollenCombo
            // 
            this.RollenCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RollenCombo.FormattingEnabled = true;
            this.RollenCombo.Location = new System.Drawing.Point(120, 12);
            this.RollenCombo.Name = "RollenCombo";
            this.RollenCombo.Size = new System.Drawing.Size(217, 21);
            this.RollenCombo.TabIndex = 1;
            this.RollenCombo.SelectedIndexChanged += new System.EventHandler(this.RollenCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Kwalificatie";
            // 
            // KwalificatieCombo
            // 
            this.KwalificatieCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KwalificatieCombo.FormattingEnabled = true;
            this.KwalificatieCombo.Location = new System.Drawing.Point(120, 40);
            this.KwalificatieCombo.Name = "KwalificatieCombo";
            this.KwalificatieCombo.Size = new System.Drawing.Size(217, 21);
            this.KwalificatieCombo.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(120, 68);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(217, 20);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Behaald op";
            // 
            // ExaminatorCombo
            // 
            this.ExaminatorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExaminatorCombo.FormattingEnabled = true;
            this.ExaminatorCombo.Location = new System.Drawing.Point(120, 95);
            this.ExaminatorCombo.Name = "ExaminatorCombo";
            this.ExaminatorCombo.Size = new System.Drawing.Size(217, 21);
            this.ExaminatorCombo.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Examinator";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Toekennen aan";
            // 
            // LedenListBox
            // 
            this.LedenListBox.FormattingEnabled = true;
            this.LedenListBox.Location = new System.Drawing.Point(120, 126);
            this.LedenListBox.Name = "LedenListBox";
            this.LedenListBox.Size = new System.Drawing.Size(217, 251);
            this.LedenListBox.TabIndex = 9;
            // 
            // Toekennen
            // 
            this.Toekennen.Location = new System.Drawing.Point(120, 384);
            this.Toekennen.Name = "Toekennen";
            this.Toekennen.Size = new System.Drawing.Size(75, 23);
            this.Toekennen.TabIndex = 10;
            this.Toekennen.Text = "Toekennen";
            this.Toekennen.UseVisualStyleBackColor = true;
            this.Toekennen.Click += new System.EventHandler(this.Toekennen_Click);
            // 
            // Kwalificatie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 419);
            this.Controls.Add(this.Toekennen);
            this.Controls.Add(this.LedenListBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ExaminatorCombo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.KwalificatieCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RollenCombo);
            this.Controls.Add(this.label1);
            this.Name = "Kwalificatie";
            this.Text = "Kwalificatie";
            this.Load += new System.EventHandler(this.Kwalificatie_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox RollenCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox KwalificatieCombo;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ExaminatorCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LedenListBox;
        private System.Windows.Forms.Button Toekennen;
    }
}