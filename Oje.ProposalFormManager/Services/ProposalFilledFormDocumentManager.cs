using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormDocumentManager: IProposalFilledFormDocumentManager
    {
        ProposalFormDBContext db = null;
        public ProposalFilledFormDocumentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public void CreateChequeArr(long proposalFilledFormId, long proposalFilledFormPrice, int? siteSettingId, List<PaymentMethodDetailesCheckVM> checkArr, IFormCollection form)
        {
            if(proposalFilledFormId > 0 && proposalFilledFormPrice > 0 && siteSettingId.ToLongReturnZiro() > 0 && checkArr != null && checkArr.Count > 0 && form != null)
            {
                for(var i = 0; i < checkArr.Count; i++)
                {
                    var check = checkArr[i];
                    string currCheckNumber = form.Keys.Any(t => t == ("check[" + i + "].checkNumber")) ? form.GetStringIfExist("check[" + i + "].checkNumber") : "";
                    int bankId = (form.Keys.Any(t => t == ("check[" + i + "].bankId")) ? form.GetStringIfExist("check[" + i + "].bankId") : "0").ToIntReturnZiro();
                    if (string.IsNullOrEmpty(currCheckNumber))
                        throw BException.GenerateNewException(String.Format(BMessages.Please_Enter_CheckNumber_RowX.GetEnumDisplayName(), i));
                    if (currCheckNumber.Length > 50)
                        throw BException.GenerateNewException(BMessages.CheckNumber_CanNot_Be_Morethen_50_Chars);
                    if(bankId.ToIntReturnZiro() <= 0)
                        throw BException.GenerateNewException(String.Format(BMessages.Please_Select_Bank_RowX.GetEnumDisplayName(), i));

                    db.Entry(new ProposalFilledFormDocument() 
                    {
                        BankId = bankId,
                        Code = currCheckNumber,
                        CreateDate = DateTime.Now,
                        Price = checkArr[i].eachPaymentLong,
                        ProposalFilledFormId = proposalFilledFormId,
                        SiteSettingId = siteSettingId.Value,
                        TargetDate = checkArr[i].checkDateEn,
                        Type = Infrastructure.Enums.ProposalFilledFormDocumentType.Cheque
                    }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                if (checkArr.Count > 0)
                    db.SaveChanges();
            }
        }
    }
}
