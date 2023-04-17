using Jose;
using Jose.keys;
using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class CTestRemoveMe
    {
        public static string EC2()
        {
            string result = "";

            var eccPem2 = @"
-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEwXIv/MBBx4bwhjVcU6eX4Xr+V4Ly
baewE4lecIeQSoFj4cwEx1A9FLC30N/R7DPJ4rvq+pUhCbGiAsC8TG2JvQ==
-----END PUBLIC KEY-----

";

            var key2 = ECDsa.Create();
            key2.ImportFromPem(eccPem2);

            var m2 = key2.ExportParameters(false);
            var payload = new Dictionary<string, object>()
             {
                 { "asdfasdg", "mr.x@contoso.com" },
                 { "asdfffasdg", 1300819380 }
             };

            var publicKey = EccKey.New(m2.Q.X, m2.Q.Y, null, CngKeyUsages.KeyAgreement);
            result = Jose.JWT.Encode(payload, publicKey, JweAlgorithm.ECDH_ES_A256KW, JweEncryption.A256GCM);

            return result;
        }
    }
}
