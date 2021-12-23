using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class PaymentController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly IBankAccountService BankAccountService = null;
        public PaymentController(
                IBankAccountFactorService BankAccountFactorService,
                ISiteSettingService SiteSettingService,
                IBankAccountDetectorService BankAccountDetectorService,
                IBankAccountService BankAccountService
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.SiteSettingService = SiteSettingService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.BankAccountService = BankAccountService;
        }

        public ActionResult GetWayList([FromQuery] string status)
        {
            PaymentFactorVM payModel = status.GetPayModel();
            if (payModel == null || payModel.price <= 0 || string.IsNullOrEmpty(payModel.returnUrl) || payModel.objectId <= 0)
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

            var allUserGW = BankAccountService.GetAllAcountForPayment(payModel.userId, SiteSettingService.GetSiteSetting()?.Id);
            if(allUserGW == null || allUserGW.Count == 0)
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);
            ViewBag.status = status;
            ViewBag.Title = "پرداخت فاکتور به مبلغ " + payModel.price.ToString("###,###") + " ریال";
            ViewBag.price = "مبلغ " + payModel.price.ToString("###,###") + " ریال";

            return View(allUserGW);
        }

        public ActionResult CreateFactor([FromForm] string status, [FromForm] int? id)
        {
            PaymentFactorVM payModel = status.GetPayModel();
            if (payModel == null || payModel.price <= 0 || string.IsNullOrEmpty(payModel.returnUrl) || payModel.objectId <= 0)
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

            var createdFactorId = BankAccountFactorService.Create(id,payModel, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId);

            return RedirectToAction("SelectGW", "Payment", new { area = "Payment", factorId = createdFactorId });
        }

        public ActionResult SelectGW([FromQuery] string factorId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, SiteSettingService.GetSiteSetting()?.Id);

            if (foundFactor != null && foundFactor.IsPayed != true && foundFactor.PayDate == null)
            {
                var foundType = BankAccountDetectorService.GetByType(foundFactor.BankAccountId, SiteSettingService.GetSiteSetting()?.Id);
                switch (foundType)
                {
                    case Infrastructure.Enums.BankAccountType.sizpay:
                        return RedirectToAction("Pay", "Sizpay", new { area = "Payment", factorId = factorId });
                    case Infrastructure.Enums.BankAccountType.unknown:
                    default:
                        break;
                }
            }

            throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);
        }
    }
}
