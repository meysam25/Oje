using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.JoinServices.Interfaces
{
    public interface IUserNotifierService
    {
        void Notify(long? userId, UserNotificationType type, List<long> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink);
        UserNotificationType ConvertProposalFilledFormStatusToUserNotifiactionType(ProposalFilledFormStatus status);
    }
}
