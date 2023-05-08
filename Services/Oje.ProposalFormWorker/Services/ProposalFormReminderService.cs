using NPOI.HSSF.Record.Chart;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.ProposalFormWorker.Interfaces;
using Oje.ProposalFormWorker.Models.DB;
using Oje.ProposalFormWorker.Services.EContext;
using Oje.Sms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormWorker.Services
{
    public class ProposalFormReminderService : IProposalFormReminderService
    {
        readonly ProposalFormWorkerDBContext db = null;
        readonly ISmsTrigerService SmsTrigerService = null;
        public ProposalFormReminderService
            (
                ProposalFormWorkerDBContext db,
                ISmsTrigerService SmsTrigerService
            )
        {
            this.db = db;
            this.SmsTrigerService = SmsTrigerService;
        }

        public void Notify(List<int> days)
        {
            if (days != null && days.Count > 0)
            {
                foreach (int day in days)
                {
                    var targetDate = DateTime.Now.AddDays(day);
                    var expiredReminder = db.ProposalFormReminders
                        .Where(t => t.TargetDate.Year == targetDate.Year && t.TargetDate.Month == targetDate.Month && t.TargetDate.Day == targetDate.Day)
                        .Where(t => !t.ProposalFormReminderNotifies.Any(tt => tt.CreateDate.Year == targetDate.Year && tt.CreateDate.Month == targetDate.Month && tt.CreateDate.Day == targetDate.Day))
                        .ToList();

                    foreach (var reminder in expiredReminder)
                    {
                        SmsTrigerService.CreateSmsQue
                            (
                                null,
                                UserNotificationType.ReminderExpireing,
                                new List<PPFUserTypes>() { new PPFUserTypes() { fullUserName = reminder.Fullname, mobile = "0" + reminder.Mobile, ProposalFilledFormUserType = ProposalFilledFormUserType.OwnerUser } },
                                null,
                                "یادآوری",
                                reminder.SiteSettingId,
                                null,
                                true
                            );

                        db.Entry(new ProposalFormReminderNotify()
                        {
                            CreateDate = targetDate,
                            ProposalFormReminderId = reminder.Id
                        }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                    if (expiredReminder.Count > 0)
                        db.SaveChanges();
                }
            }
        }
    }
}
