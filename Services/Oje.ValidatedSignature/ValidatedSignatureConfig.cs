using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Services;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature
{
    public static class ValidatedSignatureConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<ValidatedSignatureDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IProposalFilledFormService, ProposalFilledFormService>();
            services.AddScoped<IUploadedFileService, UploadedFileService>();
            services.AddScoped<ITenderFilledFormService, TenderFilledFormService>();
            services.AddScoped<IUserFilledRegisterFormService, UserFilledRegisterFormService>();
            services.AddScoped<IUserRegisterFormPriceService, UserRegisterFormPriceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUpdateAllSignatures, UpdateAllSignatures>();
            services.AddScoped<ISmsValidationHistoryService, SmsValidationHistoryService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
        }

        public static void ConfigForWorker(IServiceCollection services)
        {
            services.AddDbContext<ValidatedSignatureDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)), ServiceLifetime.Singleton);

            
            //services.AddSingleton<IProposalFilledFormService, ProposalFilledFormService>();
            //services.AddSingleton<IUploadedFileService, UploadedFileService>();
            //services.AddSingleton<ITenderFilledFormService, TenderFilledFormService>();
        }
    }
}
