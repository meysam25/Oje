using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.DB;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UpdateAllSignatures : IUpdateAllSignatures
    {
        readonly ValidatedSignatureDBContext db = null;
        public UpdateAllSignatures
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public void Update()
        {
            new UpdateEntitySignature<Models.DB.Action>(db).Update();
            new UpdateEntitySignature<User>(db).Update();
            new UpdateEntitySignature<UserRole>(db).Update();
            new UpdateEntitySignature<Role>(db).Update();
            new UpdateEntitySignature<RoleAction>(db).Update();
            new UpdateEntitySignature<SmsValidationHistory>(db).Update();
            new UpdateEntitySignature<UploadedFile>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormKey>(db).Update();
            new UpdateEntitySignature<ProposalFilledForm>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormCacheJson>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormCompany>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormDocument>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormJson>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormSiteSetting>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormStatusLog>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormStatusLogFile>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormUser>(db).Update();
            new UpdateEntitySignature<ProposalFilledFormValue>(db).Update();
            new UpdateEntitySignature<TenderFilledForm>(db).Update();
            new UpdateEntitySignature<TenderFilledFormIssue>(db).Update();
            new UpdateEntitySignature<TenderFilledFormJson>(db).Update();
            new UpdateEntitySignature<TenderFilledFormPF>(db).Update();
            new UpdateEntitySignature<TenderFilledFormPrice>(db).Update();
            new UpdateEntitySignature<TenderFilledFormsValue>(db).Update();
            new UpdateEntitySignature<TenderFilledFormKey>(db).Update();
            new UpdateEntitySignature<TenderFilledFormValidCompany>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterForm>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterFormCardPayment>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterFormCompany>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterFormJson>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterFormKey>(db).Update();
            new UpdateEntitySignature<UserFilledRegisterFormValue>(db).Update();
            new UpdateEntitySignature<UserRegisterFormDiscountCodeUse>(db).Update();
            new UpdateEntitySignature<UserRegisterFormPrice>(db).Update();
            new UpdateEntitySignature<WalletTransaction>(db).Update();
        }
    }
}
