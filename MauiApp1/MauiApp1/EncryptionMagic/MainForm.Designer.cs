namespace EncryptionMagic
{
    partial class MainForm
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
            buttonBrowse = new Button();
            textBoxFileName = new TextBox();
            buttonEncrypt = new Button();
            textBoxPW = new TextBox();
            labelPW = new Label();
            radioButtonEncrypt = new RadioButton();
            radioButtonDecrypt = new RadioButton();
            textBoxRPW = new TextBox();
            labelRPW = new Label();
            labelError = new Label();
            checkHidePasswords = new CheckBox();
            checkBoxUseBinary = new CheckBox();
            SuspendLayout();
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(504, 33);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(75, 23);
            buttonBrowse.TabIndex = 0;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // textBoxFileName
            // 
            textBoxFileName.Location = new Point(12, 33);
            textBoxFileName.Name = "textBoxFileName";
            textBoxFileName.Size = new Size(473, 23);
            textBoxFileName.TabIndex = 1000;
            // 
            // buttonEncrypt
            // 
            buttonEncrypt.Enabled = false;
            buttonEncrypt.Location = new Point(504, 165);
            buttonEncrypt.Name = "buttonEncrypt";
            buttonEncrypt.Size = new Size(75, 23);
            buttonEncrypt.TabIndex = 3;
            buttonEncrypt.Text = "Encrypt";
            buttonEncrypt.UseVisualStyleBackColor = true;
            buttonEncrypt.Click += buttonEncrypt_Click;
            // 
            // textBoxPW
            // 
            textBoxPW.Location = new Point(12, 80);
            textBoxPW.MaxLength = 255;
            textBoxPW.Name = "textBoxPW";
            textBoxPW.PasswordChar = '*';
            textBoxPW.Size = new Size(301, 23);
            textBoxPW.TabIndex = 1;
            textBoxPW.TextChanged += textBoxPW_TextChanged;
            textBoxPW.Enter += textBoxPW_Enter;
            // 
            // labelPW
            // 
            labelPW.AutoSize = true;
            labelPW.Location = new Point(16, 62);
            labelPW.Name = "labelPW";
            labelPW.Size = new Size(57, 15);
            labelPW.TabIndex = 1004;
            labelPW.Text = "Password";
            // 
            // radioButtonEncrypt
            // 
            radioButtonEncrypt.AutoSize = true;
            radioButtonEncrypt.Checked = true;
            radioButtonEncrypt.Location = new Point(16, 8);
            radioButtonEncrypt.Name = "radioButtonEncrypt";
            radioButtonEncrypt.Size = new Size(65, 19);
            radioButtonEncrypt.TabIndex = 1005;
            radioButtonEncrypt.TabStop = true;
            radioButtonEncrypt.Text = "Encrypt";
            radioButtonEncrypt.UseVisualStyleBackColor = true;
            radioButtonEncrypt.CheckedChanged += radioButtonEncrypt_CheckedChanged;
            // 
            // radioButtonDecrypt
            // 
            radioButtonDecrypt.AutoSize = true;
            radioButtonDecrypt.Location = new Point(87, 8);
            radioButtonDecrypt.Name = "radioButtonDecrypt";
            radioButtonDecrypt.Size = new Size(66, 19);
            radioButtonDecrypt.TabIndex = 1006;
            radioButtonDecrypt.Text = "Decrypt";
            radioButtonDecrypt.UseVisualStyleBackColor = true;
            radioButtonDecrypt.CheckedChanged += radioButtonDecrypt_CheckedChanged;
            // 
            // textBoxRPW
            // 
            textBoxRPW.Location = new Point(12, 130);
            textBoxRPW.MaxLength = 255;
            textBoxRPW.Name = "textBoxRPW";
            textBoxRPW.PasswordChar = '*';
            textBoxRPW.Size = new Size(301, 23);
            textBoxRPW.TabIndex = 2;
            textBoxRPW.TextChanged += textBoxRPW_TextChanged;
            textBoxRPW.Enter += textBoxRPW_Enter;
            // 
            // labelRPW
            // 
            labelRPW.AutoSize = true;
            labelRPW.Location = new Point(16, 112);
            labelRPW.Name = "labelRPW";
            labelRPW.Size = new Size(96, 15);
            labelRPW.TabIndex = 1003;
            labelRPW.Text = "Retype Password";
            // 
            // labelError
            // 
            labelError.AutoSize = true;
            labelError.ForeColor = Color.Red;
            labelError.Location = new Point(22, 173);
            labelError.Name = "labelError";
            labelError.Size = new Size(0, 15);
            labelError.TabIndex = 1002;
            // 
            // checkHidePasswords
            // 
            checkHidePasswords.AutoSize = true;
            checkHidePasswords.Checked = true;
            checkHidePasswords.CheckState = CheckState.Checked;
            checkHidePasswords.Location = new Point(319, 82);
            checkHidePasswords.Name = "checkHidePasswords";
            checkHidePasswords.Size = new Size(109, 19);
            checkHidePasswords.TabIndex = 1001;
            checkHidePasswords.Text = "Hide Passwords";
            checkHidePasswords.UseVisualStyleBackColor = true;
            checkHidePasswords.CheckedChanged += checkHidePasswords_CheckedChanged;
            // 
            // checkBoxUseBinary
            // 
            checkBoxUseBinary.AutoSize = true;
            checkBoxUseBinary.Checked = true;
            checkBoxUseBinary.CheckState = CheckState.Checked;
            checkBoxUseBinary.Location = new Point(319, 130);
            checkBoxUseBinary.Name = "checkBoxUseBinary";
            checkBoxUseBinary.Size = new Size(141, 19);
            checkBoxUseBinary.TabIndex = 1007;
            checkBoxUseBinary.Text = "Use binary encryption";
            checkBoxUseBinary.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(591, 208);
            Controls.Add(checkBoxUseBinary);
            Controls.Add(checkHidePasswords);
            Controls.Add(labelError);
            Controls.Add(labelRPW);
            Controls.Add(textBoxRPW);
            Controls.Add(radioButtonDecrypt);
            Controls.Add(radioButtonEncrypt);
            Controls.Add(labelPW);
            Controls.Add(textBoxPW);
            Controls.Add(buttonEncrypt);
            Controls.Add(textBoxFileName);
            Controls.Add(buttonBrowse);
            Name = "MainForm";
            Text = "EncryptionMagic";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonBrowse;
        private TextBox textBoxFileName;
        private Button buttonEncrypt;
        private TextBox textBoxPW;
        private Label labelPW;
        private RadioButton radioButtonEncrypt;
        private RadioButton radioButtonDecrypt;
        private TextBox textBoxRPW;
        private Label labelRPW;
        private Label labelError;
        private CheckBox checkHidePasswords;
        private CheckBox checkBoxUseBinary;
    }
}