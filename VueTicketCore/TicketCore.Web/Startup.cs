using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using TicketCore.Common;
using TicketCore.Data;
using TicketCore.Services.AwsHelper;
using TicketCore.Services.MailHelper;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Resources;

namespace TicketCore.Web
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
            services.AddCommandServices(Configuration);
            services.AddServicesQueries(Configuration);
            services.AddScoped<AuditFilterAttribute>();
            services.AddSingleton<LocalizationService>();
            services.AddAutoMapper(typeof(Startup).Assembly);

            var connection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<VueTicketDbContext>(options => options.UseSqlServer(connection));
            services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<AppSettingsProperties>(Configuration.GetSection("ApplicationSettings"));
            


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IMailingService, MailingService>();

            #region Registering ResourcesPath
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            #endregion

            #region AWS S3 Settings
            services.Configure<AwsSettings>(Configuration.GetSection("AwsSettings"));
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                Credentials = new BasicAWSCredentials(Configuration.GetValue<string>("AwsSettings:AccessKey"), Configuration.GetValue<string>("AwsSettings:SecretKey")),
                Region = RegionEndpoint.APSouth1
            });
            services.AddSingleton<IAwsS3HelperService, AwsS3HelperService>();
            services.Configure<AwsS3BucketOptions>(Configuration.GetSection(nameof(AwsS3BucketOptions)))
                .AddSingleton(x => x.GetRequiredService<IOptions<AwsS3BucketOptions>>().Value);
            #endregion
            services.AddMvc()
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                  options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
              })
              .AddSessionStateTempDataProvider()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization(options =>
              {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                  {
                      var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                      return factory.Create("SharedResource", assemblyName.Name);
                  };
              });


            // For FileUpload
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                x.ValueLengthLimit = int.MaxValue; //not recommended value
                x.MemoryBufferThreshold = Int32.MaxValue;
            });

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                options.Cookie.Name = ".VueTicket.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            //  Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.OnAppendCookie = (context) =>
                {
                    context.IssueCookie = true;
                };
            });


            #region Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("hi"),
                    new CultureInfo("mr"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture("en");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
               // options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });
            #endregion

            services.AddControllersWithViews(
                //config =>
                //{
                //    config.Filters.Add(typeof(AuditFilterAttribute));
                //}
                )
                .AddRazorRuntimeCompilation();

            services.AddControllers();

            // using Memory Cache 
            services.AddMemoryCache();

            #region Registering AddDNTCaptcha
            //  AddDNTCaptcha
            services.AddDNTCaptcha(options =>

                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false)
                    .WithEncryptionKey("9F3baE2KFTM7m0C^tt%^Ag")
                    .AbsoluteExpiration(minutes: 7)
                    .WithNoise(0.015f, 0.015f, 1, 0.0f)


            );


            

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();
       

            // Localization
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRouting();
            app.UseAuthorization();

            // Enabling Session
            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Portal}/{action=Login}/{id?}");
            });
        }
    }
}
