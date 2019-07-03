using System.Security.Cryptography;

namespace DD.Cryptography
{
    public sealed class RC4CryptoServiceProvider : RC4
    {
        /// <summary>
        /// Initializes a new instance of the RC4CryptoServiceProvider class.
        /// </summary>
        public RC4CryptoServiceProvider()
        {
            // Change the limits for the current implementation (5 - 256 bytes).
            LegalKeySizesValue = new KeySizes[] { new KeySizes(40, 2048, 0) };
        }
    }
}
