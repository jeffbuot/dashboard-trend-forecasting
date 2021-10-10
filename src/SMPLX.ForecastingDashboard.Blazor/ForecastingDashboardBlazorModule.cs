﻿using System;
using System.IO;
using System.Net.Http;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SMPLX.ForecastingDashboard.Blazor.Menus;
using SMPLX.ForecastingDashboard.MongoDB;
using SMPLX.ForecastingDashboard.Localization;
using SMPLX.ForecastingDashboard.MultiTenancy;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Components.Server.BasicTheme;
using Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Blazor.Server;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Blazor.Server;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.MultiTenancy;

namespace SMPLX.ForecastingDashboard.Blazor
{
    [DependsOn(
        typeof(ForecastingDashboardApplicationModule),
        typeof(ForecastingDashboardMongoDbModule),
        typeof(ForecastingDashboardHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreComponentsServerBasicThemeModule),
        typeof(AbpIdentityBlazorServerModule),
        typeof(AbpTenantManagementBlazorServerModule),
        typeof(AbpSettingManagementBlazorServerModule)
       )]
    public class ForecastingDashboardBlazorModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(ForecastingDashboardResource),
                    typeof(ForecastingDashboardDomainModule).Assembly,
                    typeof(ForecastingDashboardDomainSharedModule).Assembly,
                    typeof(ForecastingDashboardApplicationModule).Assembly,
                    typeof(ForecastingDashboardApplicationContractsModule).Assembly,
                    typeof(ForecastingDashboardBlazorModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureSwaggerServices(context.Services);
            ConfigureMultitenancy();
            ConfigureAutoApiControllers();
            ConfigureHttpClient(context);
            ConfigureBlazorise(context);
            ConfigureRouter(context);
            ConfigureMenu(context);
        }

        private void ConfigureMultitenancy()
        {
            Configure<AbpMultiTenancyOptions>(options => options.IsEnabled = false);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                // MVC UI
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );

                //BLAZOR UI
                options.StyleBundles.Configure(
                    BlazorBasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/blazor-global-styles.css");
                        //You can remove the following line if you don't use Blazor CSS isolation for components
                        bundle.AddFiles("/SMPLX.ForecastingDashboard.Blazor.styles.css");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "ForecastingDashboard";
                });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<ForecastingDashboardDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SMPLX.ForecastingDashboard.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ForecastingDashboardDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SMPLX.ForecastingDashboard.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ForecastingDashboardApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SMPLX.ForecastingDashboard.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ForecastingDashboardApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SMPLX.ForecastingDashboard.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ForecastingDashboardBlazorModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                // options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                // options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
                // options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                // options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
                // options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                // options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
                // options.Languages.Add(new LanguageInfo("it", "it", "Italian", "it"));
                // options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                // options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                // options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
                // options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                // options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                // options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                // options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ForecastingDashboard API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private static void ConfigureHttpClient(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri("/")
            });
        }

        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }

        private void ConfigureMenu(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new ForecastingDashboardMenuContributor());
            });
        }

        private void ConfigureRouter(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AppAssembly = typeof(ForecastingDashboardBlazorModule).Assembly;
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ForecastingDashboardApplicationModule).Assembly);
            });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ForecastingDashboardBlazorModule>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            app.UseAbpRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ForecastingDashboard API");
            });
            app.UseConfiguredEndpoints();
        }
    }
}
