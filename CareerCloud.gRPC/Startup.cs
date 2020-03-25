using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareerCloud.gRPC.Protos;
using CareerCloud.gRPC.Services;
using CareerCloudgRPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CareerCloud.gRPC
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<ApplicantEducationService>();
                endpoints.MapGrpcService<ApplicantJobApplicationService>();
                endpoints.MapGrpcService<ApplicantProfileService>();
                endpoints.MapGrpcService<ApplicantResumeService>();
                endpoints.MapGrpcService<ApplicantSkillService>();
                endpoints.MapGrpcService<ApplicantWorkHistoryService>();
                endpoints.MapGrpcService<CompanyDescriptionService>();
                endpoints.MapGrpcService<CompanyJobDescriptionService>();
                endpoints.MapGrpcService<CompanyJobEducationService>();
                endpoints.MapGrpcService<CompanyJobService>();
                endpoints.MapGrpcService<CompanyJobSkillService>();
                endpoints.MapGrpcService<CompanyLocationService>();
                endpoints.MapGrpcService<CompanyProfileService>();
                endpoints.MapGrpcService<SecurityLoginService>();
                endpoints.MapGrpcService<SecurityLoginsLogService>();
                endpoints.MapGrpcService<SecurityLoginsRoleService>();
                endpoints.MapGrpcService<SecurityRoleService>();
                endpoints.MapGrpcService<SystemCountryCodeService>();
                endpoints.MapGrpcService<SystemLanguageCodeService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
