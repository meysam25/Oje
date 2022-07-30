using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.RegisterForm.Services.EContext;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Services;

namespace Oje.Section.RegisterForm
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<RegisterFormDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => { b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery); b.UseNetTopologySuite(); }));

            services.AddScoped<IUserRegisterFormService, UserRegisterFormService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRegisterFormRequiredDocumentTypeService, UserRegisterFormRequiredDocumentTypeService>();
            services.AddScoped<IUserRegisterFormRequiredDocumentService, UserRegisterFormRequiredDocumentService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserFilledRegisterFormService, UserFilledRegisterFormService>();
            services.AddScoped<IProvincService, ProvincService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IUserFilledRegisterFormJsonService, UserFilledRegisterFormJsonService>();
            services.AddScoped<IUserFilledRegisterFormKeyService, UserFilledRegisterFormKeyService>();
            services.AddScoped<IUserFilledRegisterFormValueService, UserFilledRegisterFormValueService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAgentRefferService, AgentRefferService>();
            services.AddScoped<IUserRegisterFormPriceService, UserRegisterFormPriceService>();
            services.AddScoped<IUserFilledRegisterFormCardPaymentService, UserFilledRegisterFormCardPaymentService>();
            services.AddScoped<IUserRegisterFormDiscountCodeService, UserRegisterFormDiscountCodeService>();
            services.AddScoped<IUserRegisterFormCompanyService, UserRegisterFormCompanyService>();
            services.AddScoped<IUserRegisterFormPrintDescrptionService, UserRegisterFormPrintDescrptionService>();
        }
    }
}
