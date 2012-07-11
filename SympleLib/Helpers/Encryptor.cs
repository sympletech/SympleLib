using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SympleLib.Helpers
{
    public class Encryptor
    {
        /// <summary>
        /// Encrypting and Decrypting Strings
        /// </summary>
        /// <param name="passPhrase">Pass Phrase</param>
        /// <param name="saltValue">Salting String</param>
        /// <param name="initVector">Vectoring String to further obfuscate the encryption</param>
        public Encryptor(string passPhrase, string saltValue, string initVector)
        {
            this.PassPhrase = passPhrase;
            this.SaltValue = saltValue;
            this.InitVector = initVector;
        }

        public string PassPhrase { get; set; }
        public string SaltValue { get; set; }
        public string InitVector { get; set; }

        private const string HashAlgorithm = "SHA1";
        private const int PasswordIterations = 2;
        private const int KeySize = 256;

        /// <summary>
        /// Encrypts String using Encryption Settings
        /// </summary>
        public string Encrypt(string plainText)
        {
            //Convert string values into byte arrays needed to perform encryption
            byte[] plainTextBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(SaltValue);

            //Derive a password from supplied values
            var password = new PasswordDeriveBytes(
                PassPhrase, saltBytes, HashAlgorithm, PasswordIterations);

            //Generate pseudo-random bytes for the encryption key
            byte[] keyBytes = password.GetBytes(KeySize / 8);

            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};

            //Generate encryptor
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            //Create Memory Stream to handle the encrytion
            var memoryStream = new MemoryStream();

            // Define cryptographic stream 
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            //Start Encryption
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            //Drop memory stream to byte array
            byte[] cypherTextBytes = memoryStream.ToArray();

            //Close Streams
            memoryStream.Close();
            cryptoStream.Close();

            //Convert output to string
            string cypherText = Convert.ToBase64String(cypherTextBytes);
            return cypherText;
        }

        /// <summary>
        /// Decrypts CipherText using Encryption Settings
        /// </summary>
        public string DeCrypt(string cipherText)
        {
            try
            {
                //Convert string values into byte arrays needed to perform decryption
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
                byte[] saltBytes = Encoding.ASCII.GetBytes(SaltValue);

                //Derive a password from supplied values
                var password = new PasswordDeriveBytes(
                    PassPhrase, saltBytes, HashAlgorithm, PasswordIterations);

                //Generate pseudo-random bytes for the encryption key
                byte[] keyBytes = password.GetBytes(KeySize / 8);

                var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};

                //set encryption mode to Cipher Block Chaining

                //Generate decryptor
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                //Create Memory Stream to handle the decrytion
                var memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream 
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                //create byte array to hold decrypted stream
                var plaintextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plaintextBytes, 0, plaintextBytes.Length);

                //Close Streams
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                string plainText = Encoding.UTF8.GetString(plaintextBytes, 0, decryptedByteCount);
                return plainText;
            }
            catch (Exception ex)
            {
                return "there was an error decrypting the requested data.  " + ex.Message;
            }
        }

        /// <summary>
        /// Return A One Way Hash
        /// </summary>
        public static string HashPassword(string plainText)
        {
            byte[] data = Encoding.ASCII.GetBytes(plainText);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(data);

            var sbHashedPass = new StringBuilder();
            foreach (byte b in hash)
                sbHashedPass.Append(b.ToString("X2"));

            return sbHashedPass.ToString();
        }

    }
}
