using AutoMapper;
using APIProject.Domain;
using APIProject.Service.Interfaces;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIProject.Service.Interface;
using System.Text;
using APIProject.Service.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using APIProject.Service;
using APIProject.Middleware;

using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Http;
using APIProject.Service.Library;
using APIProject.Service.MailService;
using Microsoft.AspNetCore.HttpOverrides;
using APIProject.Job;

namespace APIProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {
            

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddAutoMapper(typeof(Startup));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMail, Mail>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
                //.AllowCredentials());
            });
            services.AddAutoMapper(typeof(APIProject.Service.MappingProfile));
            services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            //check token [Authorize]
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("App", new OpenApiInfo { Title = "App API", Version = "App" });
                options.SwaggerDoc("Web", new OpenApiInfo { Title = "Web API", Version = "Web" });

                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT containing userid claim",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                var security =
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "bearerAuth",
                                    Type = ReferenceType.SecurityScheme
                                },
                                UnresolvedReference = true
                            },
                            new List<string>()
                        }
                    };
                options.AddSecurityRequirement(security);
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            });
            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ConfigureCoreAndRepositoryService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //sử dụng swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/App/swagger.json", "App API");
                c.SwaggerEndpoint("/swagger/Web/swagger.json", "Web API");
                c.RoutePrefix = string.Empty;
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
            });
            app.UseMiddleware<JWTMiddleware>();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=LoginWeb}/{id?}");
            });
            app.UseStaticFiles();

            // Using service Get Ip From customer 
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });


        }
        private void ConfigureCoreAndRepositoryService(IServiceCollection services)
        {
            // basse
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IServices<>), typeof(BaseService<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductItemRepository, ProductItemRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();


            services.AddScoped<IProductStorageHistoryRepository, ProductStorageHistoryRepository>();
            services.AddScoped<IProductStorageRepository, ProductStorageRepository>();

            services.AddScoped<IProductStorageService, ProductStorageService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IReceiveAddressRepository, ReceiveAddressRepository>();
            services.AddScoped<IReceiveAddressService, ReceiveAddressService>();

            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IWardRepository, WardRepository>();
            services.AddScoped<IAddressService, AddressService>();

            services.AddScoped<IMemberPointHistoryRepository, MemberPointHistoryRepository>();

            services.AddScoped<IOrderComplainImageRepository, OrderComplainImageRepository>();

            services.AddScoped<IPushNotificationService, PushNotificationService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IUploadFileService, UploadFileService>();

            services.AddScoped<IHomeService, HomeService>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IStorageImportRepository, StorageImportRepository>();
            services.AddScoped<IStorageImportService, StorageImportService>();

            services.AddScoped<IStorageExportRepository, StorageExportRepository>();
            services.AddScoped<IStorageExportService, StorageExportService>();

            services.AddScoped<IProductStorageHistoryRepository, ProductStorageHistoryRepository>();
            services.AddScoped<IStorageImportDetailRepository, StorageImportDetailRepository>();
            services.AddScoped<IStorageExportDetailRepository, StorageExportDetailRepository>();

            services.AddScoped<IVnpayService, VnpayService>();
            services.AddScoped<VnPayLibrary>();

            services.AddScoped<IMail, Mail>();

            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsRepository, NewsRepository>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IStatisticService, StatisticService>();

            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IConfigService, ConfigService>();

            services.AddScoped<IHttpRequestService, HttpRequestService>();
            services.AddScoped<ISocketService, SocketService>();


            // Add Mapter Singler 
            var mp = new MapperConfiguration((MapperContext) => MapperContext.AddProfile(new MappingProfile()));
            services.AddSingleton(mp.CreateMapper());

        }
    }
}
