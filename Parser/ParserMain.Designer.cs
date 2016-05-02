namespace Parser
{
    partial class ParserMain
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
            this.codeLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.codeLabel.Location = new System.Drawing.Point(13, 13);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(35, 13);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Code:";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(16, 133);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 1;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(16, 159);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(53, 13);
            this.messageLabel.TabIndex = 2;
            this.messageLabel.Text = "Message:";
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(16, 29);
            this.codeTextBox.Multiline = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(464, 98);
            this.codeTextBox.TabIndex = 3;
            this.codeTextBox.TextChanged += new System.EventHandler(this.codeTextBox_TextChanged);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(16, 176);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(342, 72);
            this.messageTextBox.TabIndex = 4;
            this.messageTextBox.TextChanged += new System.EventHandler(this.messageTextBox_TextChanged);
            // 
            // Parser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 260);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.codeLabel);
            this.Name = "Parser";
            this.Text = "Parser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //elements of the forms screen.
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.TextBox messageTextBox;
    }
}

