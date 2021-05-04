using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CSPJQ
{
    public interface INonceService
    {
        /// <summary>
        /// Gets the generated nonce.
        /// </summary>
        /// <returns>Nonce generated when the service was initialized.
        /// Must be attached to CSP header and any inline style or script
        /// you want to use.</returns>
        string GetNonce();
    }

    public class NonceService : INonceService
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private readonly string _nonce;

        public NonceService(int nonceByteAmount = 32)
        {
            byte[] nonceBytes = new byte[nonceByteAmount];
            _rng.GetBytes(nonceBytes);

            _nonce = Convert.ToBase64String(nonceBytes);
        }

        public string GetNonce()
        {
            return _nonce;
        }
    }
}
