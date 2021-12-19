using Microsoft.EntityFrameworkCore;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class SmsSendingQueueErrorService: ISmsSendingQueueErrorService
    {
        readonly SmsDBContext db = null;
        public SmsSendingQueueErrorService(SmsDBContext db)
        {
            this.db = db;
        }

        public void Create(long smsSendingQueueId, DateTime createDate, string message, int? smsConfigId)
        {
            if(smsSendingQueueId > 0 && !string.IsNullOrEmpty(message))
            {
                db.Entry(new SmsSendingQueueError() { CreateDate = createDate, Description = message, SmsConfigId = smsConfigId, SmsSendingQueueId = smsSendingQueueId }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
