using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Services;
using Oje.Section.Blog.Services.EContext;

namespace Oje.Section.Blog
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<BlogDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<IBlogLastLikeAndViewService, BlogLastLikeAndViewService>();
            services.AddScoped<IBlogReviewService, BlogReviewService>();
            services.AddScoped<IBlogTagService, BlogTagService>();
            services.AddScoped<IBlogTagService, BlogTagService>();
        }
    }
}
