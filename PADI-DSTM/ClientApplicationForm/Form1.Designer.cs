namespace ClientApplicationForm
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
            this.CreatePadInt = new System.Windows.Forms.Button();
            this.Access = new System.Windows.Forms.Button();
            this.Fail = new System.Windows.Forms.Button();
            this.Freeze = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Button();
            this.Recover = new System.Windows.Forms.Button();
            this.TxAbort = new System.Windows.Forms.Button();
            this.CreateTextBox = new System.Windows.Forms.TextBox();
            this.AccessTextBox = new System.Windows.Forms.TextBox();
            this.PadIntIdC = new System.Windows.Forms.TextBox();
            this.PadIntIdA = new System.Windows.Forms.TextBox();
            this.URLTextBox = new System.Windows.Forms.TextBox();
            this.URL = new System.Windows.Forms.TextBox();
            this.Log = new System.Windows.Forms.TextBox();
            this.TxBegin = new System.Windows.Forms.Button();
            this.TxCommit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CreatePadInt
            // 
            this.CreatePadInt.Location = new System.Drawing.Point(250, 21);
            this.CreatePadInt.Name = "CreatePadInt";
            this.CreatePadInt.Size = new System.Drawing.Size(75, 23);
            this.CreatePadInt.TabIndex = 0;
            this.CreatePadInt.Text = "Create";
            this.CreatePadInt.UseVisualStyleBackColor = true;
            this.CreatePadInt.Click += new System.EventHandler(this.CreatePadInt_Click);
            // 
            // Access
            // 
            this.Access.Location = new System.Drawing.Point(250, 63);
            this.Access.Name = "Access";
            this.Access.Size = new System.Drawing.Size(75, 23);
            this.Access.TabIndex = 1;
            this.Access.Text = "Access";
            this.Access.UseVisualStyleBackColor = true;
            this.Access.Click += new System.EventHandler(this.AccessPadInt_Click);
            // 
            // Fail
            // 
            this.Fail.Location = new System.Drawing.Point(250, 109);
            this.Fail.Name = "Fail";
            this.Fail.Size = new System.Drawing.Size(75, 23);
            this.Fail.TabIndex = 2;
            this.Fail.Text = "Fail";
            this.Fail.UseVisualStyleBackColor = true;
            this.Fail.Click += new System.EventHandler(this.Fail_Click);
            // 
            // Freeze
            // 
            this.Freeze.Location = new System.Drawing.Point(250, 153);
            this.Freeze.Name = "Freeze";
            this.Freeze.Size = new System.Drawing.Size(75, 23);
            this.Freeze.TabIndex = 3;
            this.Freeze.Text = "Freeze";
            this.Freeze.UseVisualStyleBackColor = true;
            this.Freeze.Click += new System.EventHandler(this.Freeze_Click);
            // 
            // Status
            // 
            this.Status.Location = new System.Drawing.Point(250, 191);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(75, 23);
            this.Status.TabIndex = 4;
            this.Status.Text = "Status";
            this.Status.UseVisualStyleBackColor = true;
            this.Status.Click += new System.EventHandler(this.Status_Click);
            // 
            // Recover
            // 
            this.Recover.Location = new System.Drawing.Point(250, 229);
            this.Recover.Name = "Recover";
            this.Recover.Size = new System.Drawing.Size(75, 23);
            this.Recover.TabIndex = 5;
            this.Recover.Text = "Recover";
            this.Recover.UseVisualStyleBackColor = true;
            this.Recover.Click += new System.EventHandler(this.Recover_Click);
            // 
            // TxAbort
            // 
            this.TxAbort.Location = new System.Drawing.Point(250, 269);
            this.TxAbort.Name = "TxAbort";
            this.TxAbort.Size = new System.Drawing.Size(75, 23);
            this.TxAbort.TabIndex = 6;
            this.TxAbort.Text = "Abort";
            this.TxAbort.UseVisualStyleBackColor = true;
            this.TxAbort.Click += new System.EventHandler(this.TxAbort_Click);
            // 
            // CreateTextBox
            // 
            this.CreateTextBox.Location = new System.Drawing.Point(76, 23);
            this.CreateTextBox.Name = "CreateTextBox";
            this.CreateTextBox.Size = new System.Drawing.Size(147, 20);
            this.CreateTextBox.TabIndex = 7;
            // 
            // AccessTextBox
            // 
            this.AccessTextBox.Location = new System.Drawing.Point(76, 63);
            this.AccessTextBox.Name = "AccessTextBox";
            this.AccessTextBox.Size = new System.Drawing.Size(147, 20);
            this.AccessTextBox.TabIndex = 8;
            // 
            // PadIntIdC
            // 
            this.PadIntIdC.Location = new System.Drawing.Point(13, 23);
            this.PadIntIdC.Name = "PadIntIdC";
            this.PadIntIdC.ReadOnly = true;
            this.PadIntIdC.Size = new System.Drawing.Size(46, 20);
            this.PadIntIdC.TabIndex = 9;
            this.PadIntIdC.Tag = "";
            this.PadIntIdC.Text = "ID :";
            // 
            // PadIntIdA
            // 
            this.PadIntIdA.Location = new System.Drawing.Point(13, 63);
            this.PadIntIdA.Name = "PadIntIdA";
            this.PadIntIdA.ReadOnly = true;
            this.PadIntIdA.Size = new System.Drawing.Size(46, 20);
            this.PadIntIdA.TabIndex = 10;
            this.PadIntIdA.TabStop = false;
            this.PadIntIdA.Text = "ID :";
            // 
            // URLTextBox
            // 
            this.URLTextBox.Location = new System.Drawing.Point(76, 109);
            this.URLTextBox.Name = "URLTextBox";
            this.URLTextBox.Size = new System.Drawing.Size(147, 20);
            this.URLTextBox.TabIndex = 11;
            // 
            // URL
            // 
            this.URL.Location = new System.Drawing.Point(13, 109);
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
            this.Log.Size = new System.Drawing.Size(210, 224);
            this.Log.TabIndex = 13;
            // 
            // TxBegin
            // 
            this.TxBegin.Location = new System.Drawing.Point(250, 309);
            this.TxBegin.Name = "TxBegin";
            this.TxBegin.Size = new System.Drawing.Size(75, 23);
            this.TxBegin.TabIndex = 14;
            this.TxBegin.Text = "Begin";
            this.TxBegin.UseVisualStyleBackColor = true;
            this.TxBegin.Click += new System.EventHandler(this.TxBegin_Click);
            // 
            // TxCommit
            // 
            this.TxCommit.Location = new System.Drawing.Point(250, 354);
            this.TxCommit.Name = "TxCommit";
            this.TxCommit.Size = new System.Drawing.Size(75, 23);
            this.TxCommit.TabIndex = 15;
            this.TxCommit.Text = "Commit";
            this.TxCommit.UseVisualStyleBackColor = true;
            this.TxCommit.Click += new System.EventHandler(this.TxCommit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 389);
            this.Controls.Add(this.TxCommit);
            this.Controls.Add(this.TxBegin);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.URLTextBox);
            this.Controls.Add(this.PadIntIdA);
            this.Controls.Add(this.PadIntIdC);
            this.Controls.Add(this.AccessTextBox);
            this.Controls.Add(this.CreateTextBox);
            this.Controls.Add(this.TxAbort);
            this.Controls.Add(this.Recover);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Freeze);
            this.Controls.Add(this.Fail);
            this.Controls.Add(this.Access);
            this.Controls.Add(this.CreatePadInt);
            this.Name = "Form1";
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
        private System.Windows.Forms.TextBox CreateTextBox;
        private System.Windows.Forms.TextBox AccessTextBox;
        private System.Windows.Forms.TextBox PadIntIdC;
        private System.Windows.Forms.TextBox PadIntIdA;
        private System.Windows.Forms.TextBox URLTextBox;
        private System.Windows.Forms.TextBox URL;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.Button TxBegin;
        private System.Windows.Forms.Button TxCommit;
    }
}

