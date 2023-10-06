using System.Security.Cryptography;
using System.Text;

namespace EncryptionMagic
{
    /// <summary>
    /// See https://www.bloggingforlogging.com/2018/05/20/decrypting-the-secrets-of-ansible-vault-in-powershell/ and 
    /// https://github.com/jborean93/PowerShell-AnsibleVault and most speciffically 
    /// https://github.com/jborean93/PowerShell-AnsibleVault/blob/master/AnsibleVault/Public/Get-EncryptedAnsibleVault.ps1 for the full documentation.
    /// </summary>
    public class AnsibleEncryption
    {
        const int BLOCK_SIZE_BYTES = 16;
        const string BINARY_DELIMITER = "btKXOFJ2FkiW+4cxYtPnGzBEgwiIrMFJpPu4CZ2+gMw=";

        public static string Encrypt(string fileContent, string password)
        {
            if (string.IsNullOrEmpty(fileContent))
                throw new ArgumentNullException(nameof(fileContent));
            
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            byte[] bytes_to_encrypt = Encoding.UTF8.GetBytes(fileContent);
            byte[] salt = RandomNumberGenerator.GetBytes(32);

            // Make Keys
            byte[] derivedKey = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(80);
            byte[] cipher_key = derivedKey.Take(32).ToArray();
            byte[] hmac_key = derivedKey.Skip(32).Take(32).ToArray();
            byte[] nonce = derivedKey.Skip(64).ToArray();

            // Padding (if any)
            int paddingLength = BLOCK_SIZE_BYTES - (bytes_to_encrypt.Length % 16);
            byte[] paddedBytes = new byte[bytes_to_encrypt.Length + paddingLength];

            for (int i = 0; i < paddedBytes.Length; i++)
            {
                paddedBytes[i] = i < bytes_to_encrypt.Length ? bytes_to_encrypt[i] : (byte)paddingLength;
            }

            // Encryption with PBKDF2
            var aes_counter_cipher = Aes.Create();
            aes_counter_cipher.Mode = CipherMode.ECB;
            aes_counter_cipher.Padding = PaddingMode.None;

            var counter_encryptor = aes_counter_cipher.CreateEncryptor(cipher_key, new byte[16]);
            Queue<byte> xor_mask = new();
            byte[] encrypted_bytes = new byte[paddedBytes.Length];

            for (int i = 0; i < encrypted_bytes.Length; i++)
            {
                if (xor_mask.Count == 0)
                {
                    var counter_mode_block = new byte[aes_counter_cipher.BlockSize / 8];
                    counter_encryptor.TransformBlock(nonce, 0, nonce.Length, counter_mode_block, 0);
                    for (int j = nonce.Length - 1; j >= 0; j--)
                    {
                        var current_nonce_value = nonce[j];
                        nonce[j] = current_nonce_value == 255 ? (byte)0 : (byte)(nonce[j] + 1);
                        if (nonce[j] != 0)
                            break;
                    }
                    foreach (byte b in counter_mode_block)
                    {
                        xor_mask.Enqueue(b);
                    }
                }
                var current_mask = xor_mask.Dequeue();
                encrypted_bytes[i] = (byte)(paddedBytes[i] ^ current_mask);
            }

            var hmac_sha256 = new HMACSHA256(hmac_key);
            var hmac_temp = hmac_sha256.ComputeHash(encrypted_bytes);

            var cipher_Text = string.Join('\n', new string[] { Convert.ToHexString(salt), Convert.ToHexString(hmac_temp), Convert.ToHexString(encrypted_bytes) });
            cipher_Text = Convert.ToHexString(Encoding.UTF8.GetBytes(cipher_Text));

            // Output for Ansible
            int counter = 0;
            string ansible_encryption = "$ANSIBLE_VAULT;1.1;AES256\n";

            foreach (char c in cipher_Text)
            {
                ansible_encryption = ++counter % 80 == 0 ? ansible_encryption + c + "\n" : ansible_encryption + c;
            }

            return ansible_encryption;
        }

        public static byte[] Encrypt_Bytes(byte[] fileContent, string password)
        {
            if (fileContent == null)
                throw new ArgumentNullException(nameof(fileContent));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            //byte[] fileContent = Encoding.UTF8.GetBytes(fileContent_1);
            byte[] salt = RandomNumberGenerator.GetBytes(32);

            // Make Keys
            byte[] derivedKey = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(80);
            byte[] cipher_key = derivedKey.Take(32).ToArray();
            byte[] hmac_key = derivedKey.Skip(32).Take(32).ToArray();
            byte[] nonce = derivedKey.Skip(64).ToArray();

            // Padding (if any)
            int paddingLength = BLOCK_SIZE_BYTES - (fileContent.Length % 16);
            byte[] paddedBytes = new byte[fileContent.Length + paddingLength];

            for (int i = 0; i < paddedBytes.Length; i++)
            {
                paddedBytes[i] = i < fileContent.Length ? fileContent[i] : (byte)paddingLength;
            }

            // Encryption with PBKDF2
            var aes_counter_cipher = Aes.Create();
            aes_counter_cipher.Mode = CipherMode.ECB;
            aes_counter_cipher.Padding = PaddingMode.None;

            var counter_encryptor = aes_counter_cipher.CreateEncryptor(cipher_key, new byte[16]);
            Queue<byte> xor_mask = new();
            byte[] encrypted_bytes = new byte[paddedBytes.Length];

            for (int i = 0; i < encrypted_bytes.Length; i++)
            {
                if (xor_mask.Count == 0)
                {
                    var counter_mode_block = new byte[aes_counter_cipher.BlockSize / 8];
                    counter_encryptor.TransformBlock(nonce, 0, nonce.Length, counter_mode_block, 0);
                    for (int j = nonce.Length - 1; j >= 0; j--)
                    {
                        var current_nonce_value = nonce[j];
                        nonce[j] = current_nonce_value == 255 ? (byte)0 : (byte)(nonce[j] + 1);
                        if (nonce[j] != 0)
                            break;
                    }
                    foreach (byte b in counter_mode_block)
                    {
                        xor_mask.Enqueue(b);
                    }
                }
                var current_mask = xor_mask.Dequeue();
                encrypted_bytes[i] = (byte)(paddedBytes[i] ^ current_mask);
            }

            var hmac_sha256 = new HMACSHA256(hmac_key);
            var hmac_temp = hmac_sha256.ComputeHash(encrypted_bytes);

            // Decode the Base64-encoded delimiter into a byte array
            byte[] delimiter = Convert.FromBase64String(BINARY_DELIMITER);

            using var ms = new MemoryStream();
            
            ms.Write(salt, 0, salt.Length);
            ms.Write(delimiter, 0, delimiter.Length);
            ms.Write(hmac_temp, 0, hmac_temp.Length);
            ms.Write(delimiter, 0, delimiter.Length);
            ms.Write(encrypted_bytes, 0, encrypted_bytes.Length);

            return ms.ToArray();
        }

        static List<int> IndexAll(byte[] array, byte[] pattern, int startIndex = 0)
        {
            List<int> indexes = new List<int>();
            int i = startIndex;
            int j = 0;
            int n = array.Length;
            int m = pattern.Length;

            while (i < n)
            {
                if (array[i] == pattern[j])
                {
                    j++;
                }
                else
                {
                    i -= j;
                    j = 0;
                }
                i++;

                if (j == m)
                {
                    indexes.Add(i - m);
                    j = 0;
                }
            }

            return indexes;
        }

        public static string Generate13ByteDelimiter()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            byte[] guidBytes1 = guid1.ToByteArray();
            byte[] guidBytes2 = guid2.ToByteArray();

            byte[] combinedGuidBytes = new byte[32];
            Array.Copy(guidBytes1, 0, combinedGuidBytes, 0, 16);
            Array.Copy(guidBytes2, 0, combinedGuidBytes, 16, 16);

            // Convert the combined byte array to a Base64 string
            string base64String = Convert.ToBase64String(combinedGuidBytes);

            return base64String;
        }

        public static string Decrypt(string encryptedFileContent, string password)
        {
            if (string.IsNullOrEmpty(encryptedFileContent)) throw new ArgumentNullException(nameof(encryptedFileContent));

            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            if (!encryptedFileContent.StartsWith("$ANSIBLE_VAULT")) throw new ArgumentException("Vault text does not start with the header `$ANSIBLE_VAULT;");

            var vault_text = encryptedFileContent;

            var vault_lines = vault_text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var header = vault_lines[0].Trim().Split(';');

            Version version;
            try
            {
                version = new Version(header[1].Trim());
            }
            catch 
            {
                throw new Exception("Cannot parse vault version.  Is this an Ansible vault?");
            }
            
            if ((version.Major == 1 && version.Minor < 1) || (version.Major > 1) || (version.Major == 1 && version.Minor >= 2))
            {
                throw new NotSupportedException("Currently only 1.1 and 1.2 is supported by this tool");
            }

            string[] cipher_text = string.Join("", vault_lines.Skip(1)).Trim().ToLower().Split("0a", StringSplitOptions.None);

            string salt_hex_string = cipher_text[0];
            string hmac_hex_hex_string = cipher_text[1];
            string encrypted_hex_bytes_string = cipher_text[2]; 

            byte[] salt = HexToBytes(DecodeHexString(salt_hex_string));
            string hmac_hex_string = DecodeHexString(hmac_hex_hex_string);
            byte[] encrypted_bytes = HexToBytes(DecodeHexString(encrypted_hex_bytes_string));

            byte[] derivedKey = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(80);
            byte[] cipher_key = derivedKey.Take(32).ToArray();
            byte[] hmac_key = derivedKey.Skip(32).Take(32).ToArray();
            byte[] nonce = derivedKey.Skip(64).ToArray();

            HMACSHA256 hmac_sha256 = new HMACSHA256(hmac_key);
            byte[] hmac_temp = hmac_sha256.ComputeHash(encrypted_bytes);
            string actual_hmac = Convert.ToHexString(hmac_temp);

            if (actual_hmac.ToLower() !=  hmac_hex_string.ToLower())
            {
                throw new Exception("HMAC verification failed, was the wrong password entered?");
            }

            // Invoke-AESCTRCycle
#pragma warning disable SYSLIB0021 // Type or member is obsolete --> We are only decrypting here
            AesCryptoServiceProvider counter_cipher = new()
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };
#pragma warning restore SYSLIB0021 // Type or member is obsolete --> We are only decrypting here

            ICryptoTransform counter_encryptor = counter_cipher.CreateEncryptor(cipher_key, new byte[BLOCK_SIZE_BYTES]);
            Queue<byte> xor_mask = new();
            var output = new byte[encrypted_bytes.Length];
            for(int i = 0; i < encrypted_bytes.Length; i++)
            {
                if (xor_mask.Count == 0)
                {
                    byte[] counter_mode_block = new byte[counter_cipher.BlockSize / 8];
                    counter_encryptor.TransformBlock(nonce, 0, nonce.Length, counter_mode_block, 0);

                    for (int j = nonce.Length - 1; j >= 0; j--)
                    {
                        var current_nonce_value = nonce[j];
                        nonce[j] = (byte)(current_nonce_value == 255 ? 0 : nonce[j] + 1);

                        if (nonce[j] != 0) break;
                    }

                    foreach(byte counter_byte in counter_mode_block) xor_mask.Enqueue(counter_byte);
                }

                byte current_mask = xor_mask.Dequeue();
                output[i] = (byte)(encrypted_bytes[i] ^ current_mask);
            }

            byte[] unpadded_bytes = RemovePkcs7Padding(output);
            string decrypted_string = Encoding.UTF8.GetString(unpadded_bytes);

            return decrypted_string;
        }

        public static byte[] Decrypt_Bytes(byte[] encryptedData, string password)
        {
            if (encryptedData == null || encryptedData.Length == 0) throw new ArgumentNullException(nameof(encryptedData));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            // Decode the Base64-encoded delimiter into a byte array
            byte[] delimiter = Convert.FromBase64String(BINARY_DELIMITER);

            // Get a list of all occurrences of the delimiter in the encrypted data
            List<int> delimiterIndexes = IndexAll(encryptedData, delimiter);

            if (delimiterIndexes.Count != 2)
            {
                throw new InvalidOperationException("Invalid file to decrypt.");
            }

            int index1 = delimiterIndexes[0];
            int index2 = delimiterIndexes[1];

            byte[] salt = new byte[index1];
            byte[] hmac_hex_bytes = new byte[index2 - index1 - delimiter.Length];
            byte[] encrypted_bytes = new byte[encryptedData.Length - index2 - delimiter.Length];

            Array.Copy(encryptedData, 0, salt, 0, salt.Length);
            Array.Copy(encryptedData, index1 + delimiter.Length, hmac_hex_bytes, 0, hmac_hex_bytes.Length);
            Array.Copy(encryptedData, index2 + delimiter.Length, encrypted_bytes, 0, encrypted_bytes.Length);

            // Convert hmac_hex_bytes to hex string
            string hmac_hex_string = Convert.ToHexString(hmac_hex_bytes).ToLower();

            byte[] derivedKey = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(80);
            byte[] cipher_key = derivedKey.Take(32).ToArray();
            byte[] hmac_key = derivedKey.Skip(32).Take(32).ToArray();
            byte[] nonce = derivedKey.Skip(64).ToArray();

            HMACSHA256 hmac_sha256 = new HMACSHA256(hmac_key);
            byte[] hmac_temp = hmac_sha256.ComputeHash(encrypted_bytes);
            string actual_hmac = Convert.ToHexString(hmac_temp);

            if (actual_hmac.ToLower() != hmac_hex_string.ToLower())
            {
                throw new Exception("HMAC verification failed, was the wrong password entered?");
            }

            // Invoke-AESCTRCycle
#pragma warning disable SYSLIB0021 // Type or member is obsolete --> We are only decrypting here
            AesCryptoServiceProvider counter_cipher = new()
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };
#pragma warning restore SYSLIB0021 // Type or member is obsolete --> We are only decrypting here

            ICryptoTransform counter_encryptor = counter_cipher.CreateEncryptor(cipher_key, new byte[BLOCK_SIZE_BYTES]);
            Queue<byte> xor_mask = new();
            var output = new byte[encrypted_bytes.Length];
            for (int i = 0; i < encrypted_bytes.Length; i++)
            {
                if (xor_mask.Count == 0)
                {
                    byte[] counter_mode_block = new byte[counter_cipher.BlockSize / 8];
                    counter_encryptor.TransformBlock(nonce, 0, nonce.Length, counter_mode_block, 0);

                    for (int j = nonce.Length - 1; j >= 0; j--)
                    {
                        var current_nonce_value = nonce[j];
                        nonce[j] = (byte)(current_nonce_value == 255 ? 0 : nonce[j] + 1);

                        if (nonce[j] != 0) break;
                    }

                    foreach (byte counter_byte in counter_mode_block) xor_mask.Enqueue(counter_byte);
                }

                byte current_mask = xor_mask.Dequeue();
                output[i] = (byte)(encrypted_bytes[i] ^ current_mask);
            }

            byte[] unpadded_bytes = RemovePkcs7Padding(output);
            return unpadded_bytes;
        }

        static string DecodeHexString(string doubleHexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < doubleHexString.Length; i += 2)
            {
                string hs = doubleHexString.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return sb.ToString();
        }

        static byte[] HexToBytes(string hexString)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                bytes.Add((byte)Convert.ToUInt32(hs, 16));
            }
            return bytes.ToArray();
        }

        static byte[] RemovePkcs7Padding(byte[] value)
        {
            byte last_byte = value[value.Length - 1];

            if (last_byte > BLOCK_SIZE_BYTES || value.Length == 1)
                return value;
            
            for(int i = value.Length - 1; i >= value.Length - last_byte; i--)
            {
                if (value[i] != last_byte)
                    return value;
            }

            int unpadded_size = value.Length - last_byte;
            byte[] unpadded_bytes = new byte[unpadded_size];
            Buffer.BlockCopy(value, 0, unpadded_bytes, 0, unpadded_size);

            return unpadded_bytes;
        }
    }
}
