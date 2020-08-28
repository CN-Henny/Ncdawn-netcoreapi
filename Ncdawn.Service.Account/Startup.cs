using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Pivotal.Discovery.Client;
using Ncdawn.EFS.DBContext;
using Ncdawn.feignclient.Model;
using Ncdawn.EFS.Response;

namespace Ncdawn.EFS.Service.Account
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //配置跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", w =>
                {
                    w.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            })
                .AddSingleton<SupportFilter>();
            //配置IOC注册E:\启梦\源码\NetCore框架\自写\Ncdawn.EFS\CommonHelper\AMapHelper.cs
            //注册数据库
            services.AddDbContext<MSSqlDBContext>(options =>
                options.UseSqlServer(Configuration.GetSection("ConnectionStrings")["SqlConnection"]));
            services.AddRouting(options => options.LowercaseUrls = true);
            //注册Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = " API",
                    Description = "by bj eland"
                });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Ncdawn.EFS.xml");
                var xmlPath2 = Path.Combine(basePath, "Ncdawn.Service.Account.xml");
                options.IncludeXmlComments(xmlPath);
                options.IncludeXmlComments(xmlPath2);
            });
            //使服务可以注册到springcloud的EnableEurekaServer注册中心
            services.AddDiscoveryClient(Configuration);
            //注册Controller
            services.AddMvc().AddControllersAsServices();

            var builder = new ContainerBuilder();

            //注册仓储
            builder.RegisterAssemblyTypes(typeof(IDependency).Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            //注册仓储服务
            builder.RegisterAssemblyTypes(typeof(Ncdawn.Service.Account.IBaseService).Assembly)
             .AsImplementedInterfaces()
              .PropertiesAutowired()
             .InstancePerLifetimeScope();

            ////注册仓储服务
            //builder.RegisterAssemblyTypes(typeof(Ncdawn.CommonService.IBaseService).Assembly)
            // .AsImplementedInterfaces()
            //  .PropertiesAutowired()
            // .InstancePerLifetimeScope();

            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            //app.UseCors("AllowAllOrigin");
            app.UseMvc();
            //使服务可以注册到springcloud的EnableEurekaServer注册中心
            app.UseDiscoveryClient();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

        }
    }
}
