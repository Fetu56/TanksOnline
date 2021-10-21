
namespace ClientGame
{
    partial class Form1
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
            this.gameField = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // gameField
            // 
            this.gameField.Location = new System.Drawing.Point(5, 3);
            this.gameField.Name = "gameField";
            this.gameField.Size = new System.Drawing.Size(500, 500);
            this.gameField.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 507);
            this.Controls.Add(this.gameField);
            this.Name = "Form1";
            this.Text = "Tanks";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gameField;
    }
}

