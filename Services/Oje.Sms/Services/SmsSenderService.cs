using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;

namespace Oje.Sms.Services
{
    public class SmsSenderService: ISmsSenderService
    {
        readonly IMagfaSMSSenderService MagfaSMSSenderService = null;
        readonly ISmsConfigService SmsConfigService = null;
        public SmsSenderService(
                IMagfaSMSSenderService MagfaSMSSenderService, 
                ISmsConfigService SmsConfigService
            )
        {
            this.MagfaSMSSenderService = MagfaSMSSenderService;
            this.SmsConfigService = SmsConfigService;
        }

        public async Task<SmsResult> Send(string mobileNumber, string message, int? siteSettingId)
        {
            var foundConfig = SmsConfigService.GetActive(siteSettingId);
            if (foundConfig == null)
                return new SmsResult() { isSuccess = false, message = "تنظیمات یافت نشد" };

            if(foundConfig.Type == Infrastructure.Enums.SmsConfigType.Magfa)
                return await MagfaSMSSenderService.Send(foundConfig, mobileNumber, message);

            return new SmsResult() { isSuccess = false, message = "پیاده سازی نشده", cId = foundConfig?.Id };
        }
    }
}
