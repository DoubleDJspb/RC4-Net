using DD.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RC4Test
{
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// These test vectors are not official, but they are taken from the wiki and there is hope for their veracity.
        /// </summary>
        /// <see cref="https://en.wikipedia.org/wiki/RC4#Test_vectors"/>
        [TestMethod]
        public void WikiTest()
        {
            string[] Key = new string[] { "Key", "Wiki", "Secret" };
            string[] Plaintext = new string[] { "Plaintext", "pedia", "Attack at dawn" };

            byte[][] Expected = new byte[][] {
                new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 },
                new byte[] { 0x10, 0x21, 0xBF, 0x04, 0x20 },
                new byte[] { 0x45, 0xA0, 0x1F, 0x64, 0x5F, 0xC3, 0x5B, 0x38, 0x35, 0x52, 0x54, 0x4B, 0x9B, 0xF5 } };

            for (int i = 0; i < Key.Length; i++)
            {
                byte[] key = Encoding.ASCII.GetBytes(Key[i]);
                byte[] plaintext = Encoding.ASCII.GetBytes(Plaintext[i]);
                byte[] encoded = EncoderRead(key, plaintext);

                var enc = encoded.Aggregate("0x", (s, b) => s += $"{b:X2}");
                var exp = Expected[i].Aggregate("0x", (s, b) => s += $"{b:X2}");

                Assert.IsTrue(Enumerable.SequenceEqual(Expected[i], encoded));
            }
        }

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
    }
}