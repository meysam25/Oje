using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Infrastructure.Interfac
{
    public interface IUserNotifierService
    {
        void Notify(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink, object exteraParameter = null, bool isModal = false);
        UserNotificationType ConvertProposalFilledFormStatusToUserNotifiactionType(ProposalFilledFormStatus status);
    }
}
