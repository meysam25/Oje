using Oje.PaymentService.Models.View.SizPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface ISizpayCryptoService
    {
        SizpayCryptoEncryptResult Encrypt(string prmInput, CryptoModels keys);
    }
}
