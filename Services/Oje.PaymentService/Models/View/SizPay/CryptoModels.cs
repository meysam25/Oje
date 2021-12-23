using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    public class CryptoModels
    {
        private readonly string _Key;
        private readonly string _IV;
        private readonly string _FngrKey;

        public CryptoModels(string prmKey, string prmIV, string prmFngrKey)
        {
            _Key = prmKey;
            _IV = prmIV;
            _FngrKey = prmFngrKey;
        }

        public string FngrKey
        {
            get { return _FngrKey; }
        }

        public string IV
        {
            get { return _IV; }
        }

        public string Key
        {
            get { return _Key; }
        }
    }
}
