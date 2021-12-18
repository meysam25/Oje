using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.ProposalFormService;
using Oje.JoinServices;

namespace Oje.Section.ProposalFilledForm
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ProposalFormConfig.Config(services);
            JoinServicesConfig.Config(services);
        }
    }
}
