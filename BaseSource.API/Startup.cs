using System.Configuration;
using BaseSource.API.Extensions;
using BaseSource.API.Middleware;
using BaseSource.Application.Extensions;

namespace BaseSource.API
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
            var authServer = Configuration.GetSection("AuthServer");
            services.AddPersistenceContexts();
            services.AddRepositories();

            services.AddCors(options =>
            {
                options.AddPolicy("OpenCQRS", builder =>
                {
                    var origins = Configuration.GetSection("AllowedHosts").Value.Split(";");
                    builder.WithOrigins(origins);
                    builder.SetIsOriginAllowed(origin => true);
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.WithExposedHeaders("*");
                });
            });

            services.AddEDILayer(Configuration);
            services.AddApplicationLayer(Configuration);
            services.AddEvent(Configuration);
            services.AddCurrentUserService();
            services.ConfigurationLocalization(Configuration);
            services.AddEssentials();
            services.AddContextInfrastructure(Configuration);
            services.AddSharedInfrastructure(Configuration);

            services.AddControllers();


            services.AddDistributedMemoryCache(configuration =>
            {
                configuration.ExpirationScanFrequency = System.TimeSpan.FromDays(1);
            });
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseCors("OpenCQRS");
            app.ConfigureSwagger();
            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCustomExceptionHandler();
            app.UseRequestLocalizationByCulture();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
