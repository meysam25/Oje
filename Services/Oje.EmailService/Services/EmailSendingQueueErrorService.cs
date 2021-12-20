using Microsoft.EntityFrameworkCore;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.DB;
using Oje.EmailService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Services
{
    public class EmailSendingQueueErrorService : IEmailSendingQueueErrorService
    {
        readonly EmailServiceDBContext db = null;
        public EmailSendingQueueErrorService(EmailServiceDBContext db)
        {
            this.db = db;
        }

        public void Create(long emailSendingQueueId, DateTime createDate, string message, int? emailConfigId)
        {
            if (emailSendingQueueId > 0 && !string.IsNullOrEmpty(message))
            {
                db.Entry(new EmailSendingQueueError() { CreateDate = createDate, Description = message, EmailConfigId = emailConfigId, EmailSendingQueueId = emailSendingQueueId }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
