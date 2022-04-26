using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Services;
using Oje.Section.WebMain.Services.EContext;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Section.WebMain.Hubs;

namespace Oje.Section.WebMain
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SupportHUb>("/support");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<WebMainDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ITopMenuService, TopMenuService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IPageLeftRightDesignService, PageLeftRightDesignService>();
            services.AddScoped<IPageLeftRightDesignItemService, PageLeftRightDesignItemService>();
            services.AddScoped<IFooterExteraLinkService, FooterExteraLinkService>();
            services.AddScoped<IFooterGroupExteraLinkService, FooterGroupExteraLinkService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IOurObjectService, OurObjectService>();
            services.AddScoped<IAutoAnswerOnlineChatMessageService, AutoAnswerOnlineChatMessageService>();
            services.AddScoped<IAutoAnswerOnlineChatMessageLikeService, AutoAnswerOnlineChatMessageLikeService>();
            services.AddScoped<ISubscribeEmailService, SubscribeEmailService>();
            services.AddScoped<IPageManifestService, PageManifestService>();
            services.AddScoped<IPageManifestItemService, PageManifestItemService>();
        }
    }
}
