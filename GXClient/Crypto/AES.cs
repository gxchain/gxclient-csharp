using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace gxclient.Crypto
{
    public static class AES
    {
        public static byte[] EncryptWithChecksum(PrivateKey privateKey, PublicKey publicKey, UInt64 nonce, string message)
        {
            byte[] shareSecret = privateKey.SharedSecret(publicKey);
            byte[] sharedSecretUTF8Bytes = Encoding.UTF8.GetBytes(Hex.BytesToHex(shareSecret));
            byte[] nonceBytes = Encoding.UTF8.GetBytes("" + nonce);
            byte[] seed = nonceBytes.Concat(sharedSecretUTF8Bytes).ToArray();
            string hash = Hex.BytesToHex(Hash.SHA512(seed));
            byte[] key = Hex.HexToBytes(hash.Substring(0, 64));
            byte[] iv = Hex.HexToBytes(hash.Substring(64, 32));
            byte[] encodedMsg = Encoding.UTF8.GetBytes(message);
            byte[] checksum = Hash.SHA256(encodedMsg).Take(4).ToArray();

            return EncryptStringToBytes(checksum.Concat(encodedMsg).ToArray(), key, iv);
        }

        public static string DecryptWithChecksum(PrivateKey privateKey, PublicKey publicKey, UInt64 nonce, byte[] payload)
        {
            byte[] shareSecret = privateKey.SharedSecret(publicKey);
            byte[] sharedSecretUTF8Bytes = Encoding.UTF8.GetBytes(Hex.BytesToHex(shareSecret));
            byte[] nonceBytes = Encoding.UTF8.GetBytes("" + nonce);
            byte[] seed = nonceBytes.Concat(sharedSecretUTF8Bytes).ToArray();
            string hash = Hex.BytesToHex(Hash.SHA512(seed));
            byte[] key = Hex.HexToBytes(hash.Substring(0, 64));
            byte[] iv = Hex.HexToBytes(hash.Substring(64, 32));
            byte[] decryptedMsg = DecryptStringFromBytes(payload, key, iv);
            byte[] checksum = decryptedMsg.Take(4).ToArray();
            byte[] message = decryptedMsg.Skip(4).ToArray();
            byte[] newChecksum = Hash.SHA256(message).Take(4).ToArray();
            if (!checksum.SequenceEqual(newChecksum))
            {
                throw new Exception($"Invalid checksum, expected {Hex.BytesToHex(newChecksum)}, got {Hex.BytesToHex(checksum)}");
            }
            return Encoding.UTF8.GetString(message);
        }

        static byte[] EncryptStringToBytes(byte[] payload, byte[] Key, byte[] IV)
        {
            byte[] encrypted;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                encrypted = encryptor.TransformFinalBlock(payload, 0, payload.Length);

                //// Create the streams used for encryption.
                //using (MemoryStream msEncrypt = new MemoryStream())
                //{
                //    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                //    {
                //        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                //        {
                //            //Write all data to the stream.
                //            swEncrypt.Write(plainText);
                //        }
                //        encrypted = msEncrypt.ToArray();
                //    }
                //}
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static byte[] DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            // Declare the string used to hold
            // the decrypted text.
            byte[] resultArray = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                resultArray = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

                //Console.WriteLine(Hex.BytesToHex(resultArray));

                //plaintext = Encoding.UTF8.GetString(resultArray);

                // Create the streams used for decryption.
                //using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                //{
                //    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                //    {
                //        using (StreamReader srDecrypt = new StreamReader(csDecrypt,Encoding.UTF8))
                //        {
                //            // Read the decrypted bytes from the decrypting stream
                //            // and place them in a string.
                //            plaintext = srDecrypt.ReadToEnd();
                //        }
                //    }
                //}

            }
            return resultArray;

        }
    }
}
