using DD.Cryptography;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DD
{
    class Program
    {
        static void Main(string[] args)
        {
            // Put in the "Debug" folder of this Project "source.txt" file for debugging...
            byte[] key = Encoding.ASCII.GetBytes("SecREt01pAssW0rd");
            string path1 = @"source.txt";
            string path2 = @"encoded.txt";
            string path3 = @"decoded.txt";

            // Example without reuse Encryptor (ICryptoTransform).
            // Allows you to get the same results every time you encrypt the same file.
            // To decrypt the result, you need to apply the algorithm again.
            byte[][] encoded1;
            using (RC4 rc4 = RC4.Create())
            {
                encoded1 = new byte[3][];
                for (int i = 0; i < 3; i++)
                {
                    using (var source = new FileStream(path1, FileMode.Open, FileAccess.Read))
                    using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
                    using (var target = new MemoryStream())
                    {
                        crypto.CopyTo(target);
                        encoded1[i] = target.ToArray();
                    }
                }
            }

            // Example with reuse Encryptor (ICryptoTransform).
            // Allows you to get different results when encrypting even the same file.
            // To decrypt each file, you need to re-apply the algorithm in the same order.
            byte[][] encoded2;
            using (RC4 rc4 = RC4.Create())
            {
                ICryptoTransform encryptor = rc4.CreateEncryptor(key, null);
                encoded2 = new byte[3][];
                for (int i = 0; i < 3; i++)
                {
                    using (var source = new FileStream(path1, FileMode.Open, FileAccess.Read))
                    using (var crypto = new CryptoStream(source, encryptor, CryptoStreamMode.Read))
                    using (var target = new MemoryStream())
                    {
                        crypto.CopyTo(target);
                        encoded2[i] = target.ToArray();
                    }
                }
            }

            // All sorts of templates for encryption/decryption.

            // Encryption path1 to path2
            EncoderWrite(key, path1, path2);
            Console.WriteLine($"The contents of file {path1} is encrypted in file {path2}.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Decryption path2 to path3
            EncoderWrite(key, path2, path3);
            Console.WriteLine($"The contents of file {path2} is decrypted in file {path3}.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Encryption path3
            EncoderWrite(key, path3);
            Console.WriteLine($"The contents of file {path3} is encrypted.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Decryption path3
            EncoderWrite(key, path3);
            Console.WriteLine($"The contents of file {path3} is decrypted.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Encryption path1 to buffer
            byte[] buffer = EncoderRead(key, path1);
            Console.WriteLine($"The contents of file {path1} is encrypted in the buffer.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Decryption buffer 
            EncoderWrite(key, buffer);
            Console.WriteLine($"The contents of buffer is decrypted.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Encryption buffer to buffer2
            byte[] buffer2 = new byte[123];
            EncoderWrite(key, buffer, out buffer2);
            Console.WriteLine($"The contents of buffer is encrypted in the buffer2.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
            // Decryption buffer2 to path3
            EncoderWrite(key, buffer2, path3);
            Console.WriteLine($"The contents of buffer2 is decrypted in file {path3}.");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }


        /// <summary>
        /// An example of encryption/decryption from file to byte array.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourcePath">File path to read.</param>
        /// <returns></returns>
        public static byte[] EncoderRead(byte[] key, string sourcePath)
        {
            using (var rc4 = RC4.Create())
            using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            using (var target = new MemoryStream())
            {
                crypto.CopyTo(target);
                return target.ToArray();
            }
        }

        /// <summary>
        /// An example of encryption/decryption from byte array to another 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceArray"></param>
        /// <returns></returns>
        public static byte[] EncoderRead(byte[] key, byte[] sourceArray)
        {
            using (var rc4 = RC4.Create())
            using (var source = new MemoryStream(sourceArray))
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            using (var target = new MemoryStream())
            {
                crypto.CopyTo(target);
                return target.ToArray();
            }
        }

        /// <summary>
        /// An example of encryption/decryption from a byte array to a file.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourceArray">The byte array to encrypt/decrypt.</param>
        /// <param name="targetPath">File path to write.</param>
        public static void EncoderWrite(byte[] key, byte[] sourceArray, string targetPath)
        {
            using (var rc4 = RC4.Create())
            using (var source = new MemoryStream(sourceArray))
            using (var target = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            {
                crypto.CopyTo(target);
            }
        }

        /// <summary>
        /// An example of encryption/decryption from one file to another.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourcePath">File path to read.</param>
        /// <param name="targetPath">File path to write.</param>
        public static void EncoderWrite(byte[] key, string sourcePath, string targetPath)
        {
            using (var rc4 = RC4.Create())
            using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (var target = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            {
                crypto.CopyTo(target);
            }
        }

        /// <summary>
        /// An example of encryption/decryption from one array to another.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourceArray">The byte array to encrypt/decrypt.</param>
        /// <param name="targetArray">The byte array with the resulting data.</param>
        public static void EncoderWrite(byte[] key, byte[] sourceArray, out byte[] targetArray)
        {
            using (var rc4 = RC4.Create())
            using (var source = new MemoryStream(sourceArray))
            using (var target = new MemoryStream())
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            {
                crypto.CopyTo(target);
                targetArray = target.ToArray();
            }
        }

        /// <summary>
        /// An example of file encryption/decryption directly.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourcePath">File path to read/write</param>
        public static void EncoderWrite(byte[] key, string sourcePath)
        {
            using (var rc4 = RC4.Create())
            using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.ReadWrite))
            using (var target = new MemoryStream())
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            {
                crypto.CopyTo(target);
                source.Seek(0, SeekOrigin.Begin);
                target.Seek(0, SeekOrigin.Begin);
                target.CopyTo(source);
            }
        }

        /// <summary>
        /// An example of byte array encryption/decryption directly.
        /// </summary>
        /// <param name="key">The secret key to use for the algorithm.</param>
        /// <param name="sourceArray">The byte array to encrypt/decrypt.</param>
        public static void EncoderWrite(byte[] key, byte[] sourceArray)
        {
            using (var rc4 = RC4.Create())
            using (var source = new MemoryStream(sourceArray))
            using (var target = new MemoryStream())
            using (var crypto = new CryptoStream(source, rc4.CreateEncryptor(key, null), CryptoStreamMode.Read))
            {
                crypto.CopyTo(target);
                source.Seek(0, SeekOrigin.Begin);
                target.Seek(0, SeekOrigin.Begin);
                target.CopyTo(source);
            }
        }
    }
}