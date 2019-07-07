using DD.Cryptography;
using System;
using System.Security.Cryptography;

namespace DD
{
    /// <summary>
    /// Class for working with a cryptosystem configuration.
    /// </summary>
    public class CryptoSystem
    {
        /// <summary>
        /// Registration of a new crypto-provider in the system with a implementation check.
        /// </summary>
        /// <param name="force">Force implementation check.</param>
        /// <remarks>Registration is only for the duration of the program.</remarks>
        public static void Reg(bool force)
        {
            if (!force)
            { 
                var RC = CreateFromName();
                if (RC == null)
                {
                    Reg();
                    RC = CreateFromName();
                    if (RC == null)
                    {
                        throw new ArgumentNullException("Can't add a crypto-provider to the system list.");
                    }
                }
            }
            else
            {
                Reg();
            }
        }

        /// <summary>
        /// Registration of a new crypto-provider in the system.
        /// </summary>
        /// <remarks>Registration is only for the duration of the program.</remarks>
        public static void Reg()
        {
            CryptoConfig.AddAlgorithm(typeof(RC4CryptoServiceProvider), new string[] { "RC4CryptoServiceProvider" });
            CryptoConfig.AddAlgorithm(typeof(RC4), new string[] { "RC4" });
        }

        /// <summary>
        /// An example of creating an instanceof of class RC4 by name using the system class CryptoConfig.
        /// </summary>
        /// <returns>Instanceof of class RC4CryptoServiceProvider from abstract class RC4.</returns>
        public static RC4 CreateFromName()
        {
            // Type conversions are made for clarity.
            var dd = CryptoConfig.CreateFromName("RC4");
            return (RC4CryptoServiceProvider)dd;
        }

    }
}
