using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalChat.Hubs;
using SignalChat.Libs;
using System;

namespace SignalChat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //注册SignalR服务
            services.AddSignalR();
            //services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            //授权
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Home/Entrance";
                options.Cookie.Name = "AspnetcoreSessionId";
                options.Cookie.Path = "/"; //cookie所在的目录，asp.net默认为"/"，就是根目录
                options.Cookie.HttpOnly = true; //设置了HttpOnly属性，js脚本将无法读取到cookie信息，能有效的防止XSS攻击窃取cookie内容，增加cookie的安全性
                options.Cookie.Expiration = new TimeSpan(8, 0, 0); //cookie过期时间
                options.ExpireTimeSpan = new TimeSpan(8, 0, 0);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //不知道怎么用时先在msdn上面找到该对象，然后看它所拥有的方法和扩展方法，最后根据方法使用即可
            app.UseAuthentication();
            app.UseSignalR(route=> {
                route.MapHub<ChatHub>("/chat");
            });            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Entrance}/{id?}");
            });
        }
    }
}
