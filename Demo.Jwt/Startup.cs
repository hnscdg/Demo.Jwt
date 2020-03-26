using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Jwt
{
    public class Startup
    {
        // ����ע��
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // or see website for more information https://www.cnblogs.com/7tiny/p/11012035.html about jwt authentication
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                
                // ���JWT��֤��
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true, //�Ƿ���֤Issure
                        ValidateAudience = true, // �Ƿ���֤Audience
                        ValidateLifetime = true, // �Ƿ���֤ʧЧʱ��
                        ClockSkew = TimeSpan.FromSeconds(10),  //����ʱ��10����
                        ValidateIssuerSigningKey = true,  // �Ƿ���֤SecurityKey
                        ValidAudience = Const.Domain, // Audience
                        ValidIssuer = Const.Domain, // Issure
                        // ��ȡScuritykey
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey))
                    };
                });
            services.AddMvc(options => { options.EnableEndpointRouting = false; }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            // ���jwt��֤
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
