using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.Security;

namespace Oje.Section.Security
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SecurityConfig.Config(services);
        }
    }
}
