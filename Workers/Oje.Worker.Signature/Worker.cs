using Oje.Security.Interfaces;
using Oje.ValidatedSignature.Models.DB;
using Oje.ValidatedSignature.Services.EContext;
using Oje.Worker.Signature.Interfaces;
using Oje.Worker.Signature.Services;


namespace Oje.Worker.Signature
{
    public class Worker : BackgroundService
    {
        static List<ITableListener> sdList = new();
        readonly ValidatedSignatureDBContext db = null;
        readonly IErrorService ErrorService = null;
        public Worker
            (
                ValidatedSignatureDBContext db,
                IErrorService ErrorService
            )
        {
            this.db = db;
            this.ErrorService = ErrorService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //try
            //{
            //ProposalFilledForm
            sdList.Add(new TableListener<ProposalFilledFormKey>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledForm>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormCacheJson>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormCompany>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormDocument>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormJson>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormSiteSetting>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormStatusLog>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormStatusLogFile>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormUser>(db, ErrorService).Start());
            sdList.Add(new TableListener<ProposalFilledFormValue>(db, ErrorService).Start());

            //TenderFilledForm
            sdList.Add(new TableListener<TenderFilledForm>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormIssue>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormJson>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormPF>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormPrice>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormsValue>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormKey>(db, ErrorService).Start());
            sdList.Add(new TableListener<TenderFilledFormValidCompany>(db, ErrorService).Start());

            //ProposalFilledForm
            sdList.Add(new TableListener<UserFilledRegisterForm>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserFilledRegisterFormCardPayment>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserFilledRegisterFormCompany>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserFilledRegisterFormJson>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserFilledRegisterFormKey>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserFilledRegisterFormValue>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserRegisterFormDiscountCodeUse>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserRegisterFormPrice>(db, ErrorService).Start());

            //Users
            sdList.Add(new TableListener<ValidatedSignature.Models.DB.Action>(db, ErrorService).Start());
            sdList.Add(new TableListener<User>(db, ErrorService).Start());
            sdList.Add(new TableListener<UserRole>(db, ErrorService).Start());
            sdList.Add(new TableListener<Role>(db, ErrorService).Start());
            sdList.Add(new TableListener<RoleAction>(db, ErrorService).Start());
            sdList.Add(new TableListener<SmsValidationHistory>(db, ErrorService).Start());

            //Financial
            sdList.Add(new TableListener<BankAccount>(db, ErrorService).Start());
            sdList.Add(new TableListener<BankAccountSizpay>(db, ErrorService).Start());
            sdList.Add(new TableListener<BankAccountSadad>(db, ErrorService).Start());
            sdList.Add(new TableListener<BankAccountSep>(db, ErrorService).Start());
            sdList.Add(new TableListener<BankAccountFactor>(db, ErrorService).Start());
            sdList.Add(new TableListener<WalletTransaction>(db, ErrorService).Start());


            sdList.Add(new TableListener<UploadedFile>(db, ErrorService).Start());
            //}
            //catch ( Exception ex )
            //{
            //    string errorMessage = ex.Message + Environment.NewLine;
            //    if (ex.InnerException != null)
            //        errorMessage += ex.InnerException.Message + Environment.NewLine;
            //    if (ex?.InnerException?.InnerException != null)
            //        errorMessage += ex?.InnerException?.InnerException?.Message + Environment.NewLine;

            //    File.WriteAllText(@"C:\Publish\tempErr.txt", errorMessage);
            //}



            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var item in sdList)
                item.Stop();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var item in sdList)
                {
                    try
                    {
                        item.Run();
                    }
                    catch { }
                }
                await Task.Delay(3000, stoppingToken);
            }

        }

    }
}