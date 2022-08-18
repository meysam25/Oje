using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService
{
    public static class PaymentServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddDbContextPool<PaymentDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)) );

            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankAccountSizpayService, BankAccountSizpayService>();
            services.AddScoped<IBankAccountSizpayPaymentService, BankAccountSizpayPaymentService>();
            services.AddScoped<ISizpayCryptoService, SizpayCryptoService>();
            services.AddScoped<IBankAccountFactorService, BankAccountFactorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountDetectorService, BankAccountDetectorService>();
            services.AddScoped<IProposalFilledFormService, ProposalFilledFormService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            services.AddScoped<ITitakUserService, TitakUserService>();
            services.AddScoped<ITiTecService, TiTecService>();
            services.AddScoped<IBankAccountSadadService, BankAccountSadadService>();
            services.AddScoped<IBankAccountSadadPaymentService, BankAccountSadadPaymentService>();
            services.AddScoped<IUserFilledRegisterFormService, UserFilledRegisterFormService>();
            services.AddScoped<IBankAccountSepService, BankAccountSepService>();
            services.AddScoped<IBankAccountSepPaymentService, BankAccountSepPaymentService>();
        }

        public static void ConfigWorker(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddDbContext<PaymentDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)), ServiceLifetime.Singleton);

            services.AddSingleton<IBankAccountSizpayService, BankAccountSizpayService>();
            services.AddSingleton<IBankAccountSizpayPaymentService, BankAccountSizpayPaymentService>();
            services.AddSingleton<ISizpayCryptoService, SizpayCryptoService>();
            services.AddSingleton<IBankAccountService, BankAccountService>();
            services.AddSingleton<IWalletTransactionService, WalletTransactionService>();
            services.AddSingleton<IBankAccountFactorService, BankAccountFactorService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IBankAccountDetectorService, BankAccountDetectorService>();
            services.AddSingleton<IProposalFilledFormService, ProposalFilledFormService>();
            services.AddSingleton<ITitakUserService, TitakUserService>();
            services.AddSingleton<ITiTecService, TiTecService>();
        }
    }
}
