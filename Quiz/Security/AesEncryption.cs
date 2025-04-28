using Quiz.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quiz.Security
{
    public static class AesEncryption
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static Tuple<byte[], byte[]> DeriveKeyAndIv(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = deriveBytes.GetBytes(KeySize / 8);
                byte[] ivBytes = deriveBytes.GetBytes(BlockSize / 8);
                return Tuple.Create(keyBytes, ivBytes);
            }
        }

        public static byte[] EncryptQuizModel(QuizModel quizModel, string password)
        {
            if (quizModel == null || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("QuizModel or password cannot be null or empty.");
            }

            byte[] salt = GenerateSalt();
            Tuple<byte[], byte[]> keyIv = DeriveKeyAndIv(password, salt);
            byte[] key = keyIv.Item1;
            byte[] iv = keyIv.Item2;

            byte[] encryptedQuizData;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    // Serialize the QuizModel to a byte array using System.Text.Json
                    byte[] quizData = JsonSerializer.SerializeToUtf8Bytes(quizModel);

                    // Write the salt and then the encrypted data
                    memoryStream.Write(salt, 0, salt.Length);
                    cryptoStream.Write(quizData, 0, quizData.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedQuizData = memoryStream.ToArray();
                }
            }

            return encryptedQuizData;
        }

        public static QuizModel DecryptQuizModel(byte[] encryptedData, string password)
        {
            if (encryptedData == null || encryptedData.Length == 0 || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Encrypted data or password cannot be null or empty.");
            }

            byte[] salt = new byte[32];
            if (encryptedData.Length < salt.Length)
            {
                throw new ArgumentException("Encrypted data is too short to contain the salt.");
            }
            Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);

            Tuple<byte[], byte[]> keyIv = DeriveKeyAndIv(password, salt);
            byte[] key = keyIv.Item1;
            byte[] iv = keyIv.Item2;

            QuizModel decryptedQuizModel = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var memoryStream = new MemoryStream(encryptedData, salt.Length, encryptedData.Length - salt.Length))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    try
                    {
                        // Deserialize the QuizModel from a byte array using System.Text.Json
                        decryptedQuizModel = JsonSerializer.Deserialize<QuizModel>(cryptoStream);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Decryption error: {ex.Message}");
                        return null;
                    }
                }
            }

            return decryptedQuizModel;
        }
    }
}
