using Oje.AccountService.Interfaces;
using Oje.EmailService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Sms.Interfaces;

namespace Oje.JoinServices.Services
{
    public class UserNotifierService: IUserNotifierService
    {
        readonly IUserNotificationTrigerService UserNotificationTrigerService = null;
        readonly ISmsTrigerService SmsTrigerService = null;
        readonly IEmailTrigerService EmailTrigerService = null;
        public UserNotifierService(
                IUserNotificationTrigerService UserNotificationTrigerService,
                ISmsTrigerService SmsTrigerService,
                IEmailTrigerService EmailTrigerService
            )
        {
            this.UserNotificationTrigerService = UserNotificationTrigerService;
            this.SmsTrigerService = SmsTrigerService;
            this.EmailTrigerService = EmailTrigerService;
        }

        public void Notify(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink, object exteraParameter = null)
        {
            UserNotificationTrigerService.CreateNotificationForUser(userId, type, exteraUserList, objectId, title, siteSettingId, openLink, exteraParameter);
            SmsTrigerService.CreateSmsQue(userId, type, exteraUserList, objectId, title, siteSettingId, exteraParameter);
            EmailTrigerService.CreateEmailQue(userId, type, exteraUserList, objectId, title, siteSettingId, exteraParameter);
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
