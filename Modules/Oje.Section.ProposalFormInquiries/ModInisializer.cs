using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.ProposalFormManager;
using Oje.FireInsuranceManager;

namespace Oje.Section.ProposalFormInquiries
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ProposalFormConfig.Config(services);
            FireInsuranceManagerConfig.Config(services);
        }
    }
}
