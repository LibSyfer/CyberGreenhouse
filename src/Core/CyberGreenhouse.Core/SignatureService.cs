using System.Security.Cryptography;
using System.Text;

namespace CyberGreenhouse.Core
{
    public class SignatureService : IDisposable
    {
        private readonly HMAC _hmac;

        public SignatureService(string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            _hmac = new HMACSHA256(keyBytes);
        }

        public string SignData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signature = _hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(signature);
        }

        public bool VerifyData(string data, string signatureBase64)
        {
            string computedSignature = SignData(data);
            return computedSignature == signatureBase64;
        }

        public void Dispose()
        {
            _hmac.Dispose();
        }
    }
}
