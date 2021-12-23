using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.FileService;
using Oje.Infrastructure;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services;
using Oje.PaymentService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService
{
    public static class PaymentServiceConfig
    {
        public static void Config(IServiceCollection services)
        {

            services.AddDbContextPool<PaymentDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)) );

            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankAccountSizpayService, BankAccountSizpayService>();
            services.AddScoped<IBankAccountSizpayPaymentService, BankAccountSizpayPaymentService>();
            services.AddScoped<IBankAccountFactorService, BankAccountFactorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISizpayCryptoService, SizpayCryptoService>();
            services.AddScoped<IBankAccountDetectorService, BankAccountDetectorService>();
            services.AddScoped<IProposalFilledFormService, ProposalFilledFormService>();
        }
    }
}
