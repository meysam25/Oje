using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class CaptchaResult
    {
        public CaptchaResult()
        {
            Uid = Guid.NewGuid();
        }
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid Uid { get; set; }
    }
}
