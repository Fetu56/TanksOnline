
namespace LoginPass
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
            this.log = new System.Windows.Forms.Button();
            this.reg = new System.Windows.Forms.Button();
            this.textEm = new System.Windows.Forms.TextBox();
            this.textPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.login = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(52, 189);
            this.log.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(132, 40);
            this.log.TabIndex = 0;
            this.log.Text = "Login";
            this.log.UseVisualStyleBackColor = true;
            this.log.Click += new System.EventHandler(this.log_Click);
            // 
            // reg
            // 
            this.reg.Location = new System.Drawing.Point(191, 189);
            this.reg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.reg.Name = "reg";
            this.reg.Size = new System.Drawing.Size(43, 40);
            this.reg.TabIndex = 1;
            this.reg.Text = "Reg";
            this.reg.UseVisualStyleBackColor = true;
            this.reg.Click += new System.EventHandler(this.reg_Click);
            // 
            // textEm
            // 
            this.textEm.Location = new System.Drawing.Point(52, 80);
            this.textEm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textEm.Name = "textEm";
            this.textEm.Size = new System.Drawing.Size(181, 23);
            this.textEm.TabIndex = 2;
            // 
            // textPass
            // 
            this.textPass.Location = new System.Drawing.Point(52, 138);
            this.textPass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textPass.Name = "textPass";
            this.textPass.PasswordChar = '*';
            this.textPass.Size = new System.Drawing.Size(181, 23);
            this.textPass.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // login
            // 
            this.login.AutoSize = true;
            this.login.Location = new System.Drawing.Point(120, 22);
            this.login.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(37, 15);
            this.login.TabIndex = 6;
            this.login.Text = "Login";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "RESET PASSWORD?";
            this.label4.Click += new System.EventHandler(this.resetPass_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 267);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.login);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textPass);
            this.Controls.Add(this.textEm);
            this.Controls.Add(this.reg);
            this.Controls.Add(this.log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button log;
        private System.Windows.Forms.Button reg;
        private System.Windows.Forms.TextBox textEm;
        private System.Windows.Forms.TextBox textPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label login;
        private System.Windows.Forms.Label label4;
    }
}

