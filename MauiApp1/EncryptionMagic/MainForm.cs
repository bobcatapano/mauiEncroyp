//namespace EncryptionMagic
//{
//    public partial class MainForm : Form
//    {
//        public MainForm()
//        {
//            InitializeComponent();
//        }

//        private void buttonBrowse_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog openFileDialog1 = new()
//            {
//                InitialDirectory = @"C:\",
//                Title = "Browse Files to encrypt",

//                CheckFileExists = true,
//                CheckPathExists = true,

//                Filter = "All files (*.*)|*.*",
//                FilterIndex = 2,

//                ReadOnlyChecked = true,
//                ShowReadOnly = true,

//            };

//            if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
//            {
//                textBoxFileName.Text = openFileDialog1.FileName;
//                buttonEncrypt.Visible = true;
//                labelPW.Visible = true;
//                textBoxPW.Visible = true;

//                //string fileText = File.ReadAllText(textBoxFileName.Text);

//                if (textBoxFileName.Text.EndsWith(".enm"))
//                {
//                    radioButtonEncrypt.Checked = false;
//                    radioButtonDecrypt.Checked = true;
//                }
//                else
//                {
//                    radioButtonEncrypt.Checked = true;
//                    radioButtonDecrypt.Checked = false;
//                }

//                labelRPW.Visible = radioButtonEncrypt.Checked;
//                textBoxRPW.Visible = radioButtonEncrypt.Checked;

//                textBoxPW.Focus();
//            }
//        }

//        private void buttonEncrypt_Click(object sender, EventArgs e)
//        {
//            if (radioButtonEncrypt.Checked)
//            {
//                try
//                {
//                    if (string.IsNullOrEmpty(textBoxFileName.Text))
//                    {
//                        labelError.Text = "Selected file is empty.";
//                        labelError.ForeColor = Color.Red;

//                        return;
//                    }

//                    if (textBoxFileName.Text.StartsWith("$ANSIBLE_VAULT;"))
//                    {
//                        labelError.Text = "Selected file is encrypted.";
//                        labelError.ForeColor = Color.Red;

//                        return;
//                    }

//                    if (checkBoxUseBinary.Checked)
//                    {
//                        byte[] fileBytes = File.ReadAllBytes(textBoxFileName.Text);
//                        string fullFilePath = Path.GetFullPath(textBoxFileName.Text);
//                        byte[] encryptedBytes = AnsibleEncryption.Encrypt_Bytes(fileBytes, textBoxPW.Text);

//                        string encryptedFilePath = fullFilePath + ".enm";
//                        if (!File.Exists(encryptedFilePath))
//                        {
//                            File.WriteAllBytes(encryptedFilePath, encryptedBytes);

//                            textBoxFileName.Text = fullFilePath + ".enm";

//                            labelError.Text = "File encrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";
//                        }
//                        else
//                        {
//                            int iteration = 0;
//                            encryptedFilePath = fullFilePath + iteration + ".enm";
//                            while (File.Exists(encryptedFilePath))
//                            {
//                                iteration++;
//                                encryptedFilePath = fullFilePath + iteration + ".enm";
//                            }

//                            File.WriteAllBytes(encryptedFilePath, encryptedBytes);

//                            textBoxFileName.Text = fullFilePath + ".enm";

//                            labelError.Text = "File encrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";
//                        }
//                    }
//                    else
//                    {
//                        string fileText = File.ReadAllText(textBoxFileName.Text);
//                        string fullFilePath = Path.GetFullPath(textBoxFileName.Text);
//                        var keyVaultResult = AnsibleEncryption.Encrypt(fileText, textBoxPW.Text);

//                        string encryptedFilePath = fullFilePath + ".enm";
//                        if (!File.Exists(encryptedFilePath))
//                        {
//                            File.WriteAllText(encryptedFilePath, keyVaultResult);

//                            textBoxFileName.Text = fullFilePath + ".enm";

//                            labelError.Text = "File encrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";
//                        }
//                        else
//                        {
//                            int iteration = 0;
//                            encryptedFilePath = fullFilePath + iteration + ".enm";
//                            while (File.Exists(encryptedFilePath))
//                            {
//                                iteration++;
//                                encryptedFilePath = fullFilePath + iteration + ".enm";
//                            }

//                            File.WriteAllText(encryptedFilePath, keyVaultResult);

//                            textBoxFileName.Text = fullFilePath + ".enm";

//                            labelError.Text = "File encrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";
//                        }
//                    }
//                }
//                catch
//                {
//                    labelError.Text = "There was an error encrypting the file.";
//                    labelError.ForeColor = Color.Red;
//                }
//            }

//            if (radioButtonDecrypt.Checked)
//            {
//                try
//                {
//                    string fullFilePath = Path.GetFullPath(textBoxFileName.Text);

//                    if (!fullFilePath.EndsWith(".enm"))
//                    {
//                        labelError.Text = "Only .enm files can be decrypted.";
//                        labelError.ForeColor = Color.Red;

//                        return;
//                    }

//                    if (checkBoxUseBinary.Checked)
//                    {
//                        byte[] encryptedFileBytes = File.ReadAllBytes(textBoxFileName.Text);
//                        byte[] decryptedBytes = AnsibleEncryption.Decrypt_Bytes(encryptedFileBytes, textBoxPW.Text);

//                        string newFilePath = new string(fullFilePath.SkipLast(4).ToArray());
//                        if (!File.Exists(newFilePath))
//                        {
//                            File.WriteAllBytes(newFilePath, decryptedBytes);

//                            labelError.Text = "File decrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";

//                            return;
//                        }
//                        else
//                        {
//                            string decryptedFilePath = newFilePath + ".decrypted";
//                            if (!File.Exists(decryptedFilePath))
//                            {
//                                File.WriteAllBytes(decryptedFilePath, decryptedBytes);

//                                labelError.Text = "File Decrypted";
//                                labelError.ForeColor = Color.Green;

//                                textBoxFileName.Text = "";
//                                textBoxPW.Text = "";
//                                textBoxRPW.Text = "";

//                                return;
//                            }
//                            else
//                            {
//                                //ToDo: Iterate like encrypt
//                                int iteration = 0;
//                                decryptedFilePath = fullFilePath + iteration + ".enm";
//                                while (File.Exists(decryptedFilePath))
//                                {
//                                    iteration++;
//                                    decryptedFilePath = fullFilePath + iteration + ".enm";
//                                }

//                                File.WriteAllBytes(decryptedFilePath, decryptedBytes);

//                                textBoxFileName.Text = fullFilePath + ".enm";

//                                labelError.Text = "File Decrypted";
//                                labelError.ForeColor = Color.Green;

//                                textBoxFileName.Text = "";
//                                textBoxPW.Text = "";
//                                textBoxRPW.Text = "";

//                                return;

//                            }
//                        }

//                    }
//                    else
//                    {
//                        string fileText = File.ReadAllText(textBoxFileName.Text);
//                        var keyVaultResult = AnsibleEncryption.Decrypt(fileText, textBoxPW.Text);

//                        string newFilePath = new string(fullFilePath.SkipLast(4).ToArray());
//                        if (!File.Exists(newFilePath))
//                        {
//                            File.WriteAllText(newFilePath, keyVaultResult);

//                            labelError.Text = "File decrypted.";
//                            labelError.ForeColor = Color.Green;

//                            textBoxFileName.Text = "";
//                            textBoxPW.Text = "";
//                            textBoxRPW.Text = "";
//                        }
//                        else
//                        {
//                            string decryptedFilePath = newFilePath + ".decrypted";
//                            if (!File.Exists(decryptedFilePath))
//                            {
//                                File.WriteAllText(decryptedFilePath, keyVaultResult);

//                                labelError.Text = "File Decrypted";
//                                labelError.ForeColor = Color.Green;

//                                textBoxFileName.Text = "";
//                                textBoxPW.Text = "";
//                                textBoxRPW.Text = "";
//                            }
//                            else
//                            {
//                                int iteration = 0;
//                                decryptedFilePath = fullFilePath + iteration + ".decrypted";
//                                while (File.Exists(decryptedFilePath))
//                                {
//                                    iteration++;
//                                    decryptedFilePath = fullFilePath + iteration + ".decrypted";
//                                }

//                                File.WriteAllText(decryptedFilePath, keyVaultResult);

//                                textBoxFileName.Text = fullFilePath + ".decrypted";

//                                labelError.Text = "File encrypted.";
//                                labelError.ForeColor = Color.Green;

//                                textBoxFileName.Text = "";
//                                textBoxPW.Text = "";
//                                textBoxRPW.Text = "";
//                            }
//                        }
//                    }

//                }
//                catch 
//                {
//                    labelError.Text = "Error decrypting file.";
//                    labelError.ForeColor = Color.Red;
//            


//            }
//        }

//        private void textBoxPW_TextChanged(object sender, EventArgs e)
//        {
//            if (textBoxPW.Text.Length == 0)
//                return;
//            bool valid = FormIsValid();
//            buttonEncrypt.Enabled = valid;
//            labelError.Text = valid ? "" : GetErrorMessage();
//        }

//        private void radioButtonDecrypt_CheckedChanged(object sender, EventArgs e)
//        {
//            buttonEncrypt.Text = radioButtonDecrypt.Checked ? "Decrypt" : "Encrypt";
//            labelRPW.Visible = radioButtonEncrypt.Checked;
//            textBoxRPW.Visible = radioButtonEncrypt.Checked;
//        }

//        private void radioButtonEncrypt_CheckedChanged(object sender, EventArgs e)
//        {
//            buttonEncrypt.Text = radioButtonEncrypt.Checked ? "Encrypt" : "Decrypt";
//            labelRPW.Visible = radioButtonEncrypt.Checked;
//            textBoxRPW.Visible = radioButtonEncrypt.Checked;
//        }

//        private void textBoxRPW_TextChanged(object sender, EventArgs e)
//        {
//            if (textBoxPW.Text.Length == 0)
//                return;
//            bool valid = FormIsValid();
//            buttonEncrypt.Enabled = valid;
//            labelError.Text = valid ? "" : GetErrorMessage();
//        }

//        bool FormIsValid()
//        {
//            if (radioButtonEncrypt.Checked)
//                return PasswordLengthValid() && PasswordRetypeLengthValid() && PasswordsMatch();

//            if (radioButtonDecrypt.Checked)
//                return PasswordLengthValid();

//            return false;
//        }

//        bool PasswordLengthValid()
//        {
//            return textBoxPW.Text.Length >= 6;
//        }

//        bool PasswordRetypeLengthValid()
//        {
//            return textBoxRPW.Text.Length >= 6;
//        }

//        bool PasswordsMatch()
//        {
//            return textBoxPW.Text == textBoxRPW.Text;
//        }

//        string GetErrorMessage()
//        {
//            if (!PasswordLengthValid())
//            {
//                return "Password must be at least 6 characters.";
//            }

//            if (!PasswordRetypeLengthValid())
//            {
//                return "Retype Password must be at least 6 characters.";
//            }

//            if (!PasswordsMatch())
//            {
//                return "Password and Retype must match.";
//            }

//            return string.Empty;
//        }

//        private void checkHidePasswords_CheckedChanged(object sender, EventArgs e)
//        {
//            textBoxPW.PasswordChar = checkHidePasswords.Checked ? '*' : (char)0;
//            textBoxRPW.PasswordChar = checkHidePasswords.Checked ? '*' : (char)0;
//        }

//        private void textBoxPW_Enter(object sender, EventArgs e)
//        {
//            Clipboard.Clear();
//        }

//        private void textBoxRPW_Enter(object sender, EventArgs e)
//        {
//            Clipboard.Clear();
//        }
//    }
//}