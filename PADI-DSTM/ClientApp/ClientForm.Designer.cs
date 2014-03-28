namespace ClientApp
{
    partial class ClientForm
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
            this.NickBox = new System.Windows.Forms.TextBox();
            this.portNumb = new System.Windows.Forms.TextBox();
            this.uidText = new System.Windows.Forms.TextBox();
            this.connectBut = new System.Windows.Forms.Button();
            this.accessBut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.createBut = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NickBox
            // 
            this.NickBox.Location = new System.Drawing.Point(13, 26);
            this.NickBox.MaxLength = 14;
            this.NickBox.Name = "NickBox";
            this.NickBox.Size = new System.Drawing.Size(100, 20);
            this.NickBox.TabIndex = 0;
            // 
            // portNumb
            // 
            this.portNumb.Location = new System.Drawing.Point(119, 26);
            this.portNumb.MaxLength = 4;
            this.portNumb.Name = "portNumb";
            this.portNumb.Size = new System.Drawing.Size(35, 20);
            this.portNumb.TabIndex = 1;
            // 
            // uidText
            // 
            this.uidText.Location = new System.Drawing.Point(13, 65);
            this.uidText.MaxLength = 4;
            this.uidText.Name = "uidText";
            this.uidText.Size = new System.Drawing.Size(35, 20);
            this.uidText.TabIndex = 2;
            // 
            // connectBut
            // 
            this.connectBut.Location = new System.Drawing.Point(160, 26);
            this.connectBut.Name = "connectBut";
            this.connectBut.Size = new System.Drawing.Size(75, 20);
            this.connectBut.TabIndex = 3;
            this.connectBut.Text = "Connect";
            this.connectBut.UseVisualStyleBackColor = true;
            this.connectBut.Click += new System.EventHandler(this.connectBut_Click);
            // 
            // accessBut
            // 
            this.accessBut.Location = new System.Drawing.Point(79, 65);
            this.accessBut.Name = "accessBut";
            this.accessBut.Size = new System.Drawing.Size(75, 20);
            this.accessBut.TabIndex = 4;
            this.accessBut.Text = "Access";
            this.accessBut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "uid:";
            // 
            // createBut
            // 
            this.createBut.Location = new System.Drawing.Point(160, 65);
            this.createBut.Name = "createBut";
            this.createBut.Size = new System.Drawing.Size(75, 20);
            this.createBut.TabIndex = 6;
            this.createBut.Text = "Create";
            this.createBut.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "nick:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "port:";
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(15, 104);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.Size = new System.Drawing.Size(220, 145);
            this.textLog.TabIndex = 10;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.createBut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accessBut);
            this.Controls.Add(this.connectBut);
            this.Controls.Add(this.uidText);
            this.Controls.Add(this.portNumb);
            this.Controls.Add(this.NickBox);
            this.Name = "ClientForm";
            this.Text = "ClientApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void logText_TextChanged(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TextBox NickBox;
        private System.Windows.Forms.TextBox portNumb;
        private System.Windows.Forms.TextBox uidText;
        private System.Windows.Forms.Button connectBut;
        private System.Windows.Forms.Button accessBut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createBut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textLog;
    }
}

