namespace ClientApplicationForm
{
    partial class GUI
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
            this.CreatePadInt = new System.Windows.Forms.Button();
            this.Access = new System.Windows.Forms.Button();
            this.Fail = new System.Windows.Forms.Button();
            this.Freeze = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Button();
            this.Recover = new System.Windows.Forms.Button();
            this.TxAbort = new System.Windows.Forms.Button();
            this.masterIPtxt = new System.Windows.Forms.TextBox();
            this.uidTxt = new System.Windows.Forms.TextBox();
            this.URLTextBox = new System.Windows.Forms.TextBox();
            this.URL = new System.Windows.Forms.TextBox();
            this.Log = new System.Windows.Forms.TextBox();
            this.TxBegin = new System.Windows.Forms.Button();
            this.TxCommit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.masterPortTxt = new System.Windows.Forms.TextBox();
            this.InitButton = new System.Windows.Forms.Button();
            this.readButton = new System.Windows.Forms.Button();
            this.writeButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.valueTxt = new System.Windows.Forms.TextBox();
            this.padintStack = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CreatePadInt
            // 
            this.CreatePadInt.Location = new System.Drawing.Point(114, 61);
            this.CreatePadInt.Name = "CreatePadInt";
            this.CreatePadInt.Size = new System.Drawing.Size(75, 23);
            this.CreatePadInt.TabIndex = 0;
            this.CreatePadInt.Text = "Create";
            this.CreatePadInt.UseVisualStyleBackColor = true;
            this.CreatePadInt.Click += new System.EventHandler(this.CreatePadInt_Click);
            // 
            // Access
            // 
            this.Access.Location = new System.Drawing.Point(195, 60);
            this.Access.Name = "Access";
            this.Access.Size = new System.Drawing.Size(75, 23);
            this.Access.TabIndex = 1;
            this.Access.Text = "Access";
            this.Access.UseVisualStyleBackColor = true;
            this.Access.Click += new System.EventHandler(this.AccessPadInt_Click);
            // 
            // Fail
            // 
            this.Fail.Location = new System.Drawing.Point(144, 309);
            this.Fail.Name = "Fail";
            this.Fail.Size = new System.Drawing.Size(56, 23);
            this.Fail.TabIndex = 2;
            this.Fail.Text = "Fail";
            this.Fail.UseVisualStyleBackColor = true;
            this.Fail.Click += new System.EventHandler(this.Fail_Click);
            // 
            // Freeze
            // 
            this.Freeze.Location = new System.Drawing.Point(84, 309);
            this.Freeze.Name = "Freeze";
            this.Freeze.Size = new System.Drawing.Size(54, 23);
            this.Freeze.TabIndex = 3;
            this.Freeze.Text = "Freeze";
            this.Freeze.UseVisualStyleBackColor = true;
            this.Freeze.Click += new System.EventHandler(this.Freeze_Click);
            // 
            // Status
            // 
            this.Status.Location = new System.Drawing.Point(13, 309);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(61, 23);
            this.Status.TabIndex = 4;
            this.Status.Text = "Status";
            this.Status.UseVisualStyleBackColor = true;
            this.Status.Click += new System.EventHandler(this.Status_Click);
            // 
            // Recover
            // 
            this.Recover.Location = new System.Drawing.Point(206, 309);
            this.Recover.Name = "Recover";
            this.Recover.Size = new System.Drawing.Size(59, 23);
            this.Recover.TabIndex = 5;
            this.Recover.Text = "Recover";
            this.Recover.UseVisualStyleBackColor = true;
            this.Recover.Click += new System.EventHandler(this.Recover_Click);
            // 
            // TxAbort
            // 
            this.TxAbort.Location = new System.Drawing.Point(195, 122);
            this.TxAbort.Name = "TxAbort";
            this.TxAbort.Size = new System.Drawing.Size(75, 23);
            this.TxAbort.TabIndex = 6;
            this.TxAbort.Text = "TxAbort";
            this.TxAbort.UseVisualStyleBackColor = true;
            this.TxAbort.Click += new System.EventHandler(this.TxAbort_Click);
            // 
            // masterIPtxt
            // 
            this.masterIPtxt.Location = new System.Drawing.Point(76, 12);
            this.masterIPtxt.MaxLength = 15;
            this.masterIPtxt.Name = "masterIPtxt";
            this.masterIPtxt.Size = new System.Drawing.Size(92, 20);
            this.masterIPtxt.TabIndex = 7;
            this.masterIPtxt.Text = "localhost";
            // 
            // uidTxt
            // 
            this.uidTxt.Location = new System.Drawing.Point(76, 63);
            this.uidTxt.MaxLength = 4;
            this.uidTxt.Name = "uidTxt";
            this.uidTxt.Size = new System.Drawing.Size(32, 20);
            this.uidTxt.TabIndex = 8;
            // 
            // URLTextBox
            // 
            this.URLTextBox.Location = new System.Drawing.Point(65, 283);
            this.URLTextBox.MaxLength = 40;
            this.URLTextBox.Name = "URLTextBox";
            this.URLTextBox.Size = new System.Drawing.Size(200, 20);
            this.URLTextBox.TabIndex = 11;
            // 
            // URL
            // 
            this.URL.Location = new System.Drawing.Point(13, 283);
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Size = new System.Drawing.Size(46, 20);
            this.URL.TabIndex = 12;
            this.URL.Text = "URL :";
            // 
            // Log
            // 
            this.Log.Location = new System.Drawing.Point(13, 153);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.Size = new System.Drawing.Size(176, 121);
            this.Log.TabIndex = 13;
            // 
            // TxBegin
            // 
            this.TxBegin.Location = new System.Drawing.Point(33, 124);
            this.TxBegin.Name = "TxBegin";
            this.TxBegin.Size = new System.Drawing.Size(75, 23);
            this.TxBegin.TabIndex = 14;
            this.TxBegin.Text = "TxBegin";
            this.TxBegin.UseVisualStyleBackColor = true;
            this.TxBegin.Click += new System.EventHandler(this.TxBegin_Click);
            // 
            // TxCommit
            // 
            this.TxCommit.Location = new System.Drawing.Point(114, 124);
            this.TxCommit.Name = "TxCommit";
            this.TxCommit.Size = new System.Drawing.Size(75, 23);
            this.TxCommit.TabIndex = 15;
            this.TxCommit.Text = "TxCommit";
            this.TxCommit.UseVisualStyleBackColor = true;
            this.TxCommit.Click += new System.EventHandler(this.TxCommit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "uid:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Master IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Master Port:";
            // 
            // masterPortTxt
            // 
            this.masterPortTxt.Location = new System.Drawing.Point(76, 37);
            this.masterPortTxt.MaxLength = 4;
            this.masterPortTxt.Name = "masterPortTxt";
            this.masterPortTxt.Size = new System.Drawing.Size(32, 20);
            this.masterPortTxt.TabIndex = 19;
            this.masterPortTxt.Text = "8086";
            // 
            // InitButton
            // 
            this.InitButton.Location = new System.Drawing.Point(195, 10);
            this.InitButton.Name = "InitButton";
            this.InitButton.Size = new System.Drawing.Size(75, 23);
            this.InitButton.TabIndex = 20;
            this.InitButton.Text = "Init";
            this.InitButton.UseVisualStyleBackColor = true;
            this.InitButton.Click += new System.EventHandler(this.InitButton_Click);
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(195, 93);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(75, 23);
            this.readButton.TabIndex = 21;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.ReadPadInt);
            // 
            // writeButton
            // 
            this.writeButton.Location = new System.Drawing.Point(114, 92);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(75, 23);
            this.writeButton.TabIndex = 22;
            this.writeButton.Text = "Write";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.WritePadInt);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "value:";
            // 
            // valueTxt
            // 
            this.valueTxt.Location = new System.Drawing.Point(76, 90);
            this.valueTxt.MaxLength = 4;
            this.valueTxt.Name = "valueTxt";
            this.valueTxt.Size = new System.Drawing.Size(32, 20);
            this.valueTxt.TabIndex = 24;
            // 
            // padintStack
            // 
            this.padintStack.Location = new System.Drawing.Point(195, 153);
            this.padintStack.Multiline = true;
            this.padintStack.Name = "padintStack";
            this.padintStack.ReadOnly = true;
            this.padintStack.Size = new System.Drawing.Size(70, 121);
            this.padintStack.TabIndex = 25;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 337);
            this.Controls.Add(this.padintStack);
            this.Controls.Add(this.valueTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.InitButton);
            this.Controls.Add(this.masterPortTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxCommit);
            this.Controls.Add(this.TxBegin);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.URLTextBox);
            this.Controls.Add(this.uidTxt);
            this.Controls.Add(this.masterIPtxt);
            this.Controls.Add(this.TxAbort);
            this.Controls.Add(this.Recover);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Freeze);
            this.Controls.Add(this.Fail);
            this.Controls.Add(this.Access);
            this.Controls.Add(this.CreatePadInt);
            this.Name = "GUI";
            this.Text = "PADI-DSTM Application";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreatePadInt;
        private System.Windows.Forms.Button Access;
        private System.Windows.Forms.Button Fail;
        private System.Windows.Forms.Button Freeze;
        private System.Windows.Forms.Button Status;
        private System.Windows.Forms.Button Recover;
        private System.Windows.Forms.Button TxAbort;
        private System.Windows.Forms.TextBox masterIPtxt;
        private System.Windows.Forms.TextBox uidTxt;
        private System.Windows.Forms.TextBox URLTextBox;
        private System.Windows.Forms.TextBox URL;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.Button TxBegin;
        private System.Windows.Forms.Button TxCommit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox masterPortTxt;
        private System.Windows.Forms.Button InitButton;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox valueTxt;
        private System.Windows.Forms.TextBox padintStack;
    }
}

