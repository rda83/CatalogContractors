using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using CatalogContractors.Database;
using CatalogContractors.Models.Repositories;
using CatalogContractors.App.Repositories;

namespace CatalogContractors.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("CatalogContractorsDB"));
            services.AddTransient<ITypeContactRepository, TypeContactRepository>();
            services.AddTransient<IContractorRepository, ContractorRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();

            #region versioning

            services.AddRouting(options => options.LowercaseUrls = true);

                services.AddScoped<IUrlHelper>(x =>
                {
                    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                    var factory = x.GetRequiredService<IUrlHelperFactory>();
                    return factory.GetUrlHelper(actionContext);
                });

                services.AddControllers()
                       .AddNewtonsoftJson(options =>
                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                services.AddApiVersioning(
                   config =>
                   {
                       config.ReportApiVersions = true;
                       config.AssumeDefaultVersionWhenUnspecified = true;
                       config.DefaultApiVersion = new ApiVersion(1, 0);
                       config.ApiVersionReader = new HeaderApiVersionReader("api-version");
                   });

                services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            #endregion

            #region swagger

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Справочник контрагентов",
                        Description = "Справочник контрагентов ASP.NET Core Web API",
                        Contact = new OpenApiContact
                        {
                            Name = "Rostovtsev D.",
                            Email = string.Empty,
                            Url = new Uri("https://github.com/rda83"),
                        },
                    });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog contractors");
                c.RoutePrefix = string.Empty;
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
