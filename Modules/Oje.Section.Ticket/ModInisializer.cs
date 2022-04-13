using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.Ticket.Services.EContext;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Services;

namespace Oje.Section.Ticket
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<TicketDBContext>(
               options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           );

            services.AddScoped<ITicketCategoryService, TicketCategoryService>();
            services.AddScoped<ITicketUserService, TicketUserService>();
            services.AddScoped<ITicketUserAnswerService, TicketUserAnswerService>();
        }
    }
}
