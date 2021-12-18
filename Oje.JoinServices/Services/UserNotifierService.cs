using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.JoinServices.Interfaces;
using Oje.Sms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.JoinServices.Services
{
    public class UserNotifierService: IUserNotifierService
    {
        readonly IUserNotificationTrigerService UserNotificationTrigerService = null;
        readonly ISmsTrigerService SmsTrigerService = null;
        public UserNotifierService(
                IUserNotificationTrigerService UserNotificationTrigerService,
                ISmsTrigerService SmsTrigerService
            )
        {
            this.UserNotificationTrigerService = UserNotificationTrigerService;
            this.SmsTrigerService = SmsTrigerService;
        }

        public void Notify(long? userId, UserNotificationType type, List<long> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink)
        {
            UserNotificationTrigerService.CreateNotificationForUser(userId, type, exteraUserList, objectId, title, siteSettingId, openLink);
            SmsTrigerService.CreateSmsQue(userId, type, exteraUserList, objectId, title, siteSettingId);
        }

        public UserNotificationType ConvertProposalFilledFormStatusToUserNotifiactionType(ProposalFilledFormStatus status)
        {
            switch (status)
            {
                case ProposalFilledFormStatus.New:
                    return UserNotificationType.ProposalFilledFormStatusChangedNew;
                case ProposalFilledFormStatus.W8ForConfirm:
                    return UserNotificationType.ProposalFilledFormStatusChangeW8ForConfirm;
                case ProposalFilledFormStatus.NeedSpecialist:
                    return UserNotificationType.ProposalFilledFormStatusChangeNeedSpecialist;
                case ProposalFilledFormStatus.Confirm:
                    return UserNotificationType.ProposalFilledFormStatusChangeConfirm;
                case ProposalFilledFormStatus.Issuing:
                    return UserNotificationType.ProposalFilledFormStatusChangeIssue;
                case ProposalFilledFormStatus.NotIssue:
                    return UserNotificationType.ProposalFilledFormStatusChangeNotIssue;
                default:
                    return UserNotificationType.ProposalFilledFormStatusChangedNew;
            }
        }
    }
}
