using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class FireInsuranceCoverageTitleManager : IFireInsuranceCoverageTitleManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public FireInsuranceCoverageTitleManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public List<FireInsuranceCoverageTitle> GetBy(List<int> allIds)
        {
            return db.FireInsuranceCoverageTitles
               .Include(t => t.FireInsuranceCoverageActivityDangerLevels)
               .Where(t => t.IsActive == true && allIds.Contains(t.Id))
               .AsNoTracking()
               .ToList();
        }

        public object GetInquiryExteraFilterCtrls()
        {
            var allCtrls = new List<object>();
            var allPanels = new List<object>();
            var allCoves = db.FireInsuranceCoverageTitles
                .Where(t => t.IsActive == true)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    type = t.EffectOn,
                    hasA = t.FireInsuranceCoverageActivityDangerLevels.Any(tt => tt.IsActive == true),
                }).ToList();
            
            for(int i = 0; i < allCoves.Count; i++)
            {
                var cover = allCoves[i];
                if (cover.hasA == true)
                {
                    allCtrls.Add(new 
                    {
                        parentCL = "col-xl-3 col-lg-4 col-md-6 col-sm-6 col-xs-12",
                        name = "exteraQuestions[" + i + "].value",
                        type = "dropDown2",
                        textfield = "title",
                        valuefield = "id",
                        dataurl = "/ProposalFormInquiries/FireInsurance/GetActivityList",
                        ph  = BMessages.Please_Select_Your_Activity.GetEnumDisplayName(),
                        label = cover.title
                    });
                    allCtrls.Add(new
                    {
                        name = "exteraQuestions[" + i + "].id",
                        type = "hidden",
                        dfaultValue = cover.id
                    });
                } else if (cover.type == Infrastructure.Enums.FireInsuranceCoverageEffectOn.InputByUser)
                {
                    allCtrls.Add(new 
                    {
                        parentCL = "col-xl-3 col-lg-4 col-md-6 col-sm-6 col-xs-12",
                        name = "exteraQuestions[" + i + "].value",
                        type = "text",
                        ph = "لطفا سرمایه را وارد کنید",
                        label  = cover.title
                    });
                    allCtrls.Add(new
                    {
                        name = "exteraQuestions[" + i + "].id",
                        type = "hidden",
                        dfaultValue = cover.id
                    });
                } else
                {
                    allCtrls.Add(new
                    {
                        parentCL = "col-xl-3 col-lg-4 col-md-6 col-sm-6 col-xs-12",
                        name = "exteraQuestions[" + i + "]",
                        type = "dropDown",
                        textfield = "title",
                        valuefield = "id",
                        dataurl = "/Core/BaseData/Get/IsActive",
                        label = cover.title
                    });
                    allCtrls.Add(new
                    {
                        name = "exteraQuestions[" + i + "].id",
                        type = "hidden",
                        dfaultValue = cover.id
                    });
                }
            }

            if (allCoves.Count > 0)
                allPanels.Add(new
                {
                    id = "exteraCoverPanel",
                    title = "پوشش اضافی",
                    @class = "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
                    ctrls = allCtrls
                });

            return new
            {
                panels = allPanels
            };
        }
    }
}
