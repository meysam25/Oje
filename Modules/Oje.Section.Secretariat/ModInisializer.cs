using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.Secretariat.Services.EContext;

namespace Oje.Section.Secretariat
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<SecretariatDBContext>
            (
                options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => { b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery); b.UseNetTopologySuite(); })
            );

            services.AddScoped<ISecretariatHeaderFooterService, SecretariatHeaderFooterService>();
            services.AddScoped<ISecretariatUserDigitalSignatureService, SecretariatUserDigitalSignatureService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISecretariatHeaderFooterDescriptionService, SecretariatHeaderFooterDescriptionService>();
            services.AddScoped<ISecretariatLetterCategoryService, SecretariatLetterCategoryService>();
            services.AddScoped<ISecretariatLetterService, SecretariatLetterService>();
            services.AddScoped<ISecretariatLetterUserService, SecretariatLetterUserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISecretariatLetterReciveService, SecretariatLetterReciveService>();
        }
    }
}
