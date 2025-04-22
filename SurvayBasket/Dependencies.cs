using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using SurvayBasket.Health.cs;
using SurvayBasket.Settigns.cs;


namespace SurvayBasket
{
    public static class Dependencies
    {
    
        public static IServiceCollection AddDependacies (this IServiceCollection service, IConfiguration configuration)
        {

            // Add services to the container.

            service.AddControllers();

            service.AddScoped<IPollService, PollService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IQuestionService, QuestionService>();
            service.AddScoped<IVoteService, VoteService>();
            service.AddScoped<ICacheService, CacheService>();
            service.AddScoped<IEmailSender, EmailService>();
            service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IUserService, UserService>();

            /**********************/
            var allowedorigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
            service.AddCors(options =>
                options.AddDefaultPolicy(builder => 
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithOrigins(allowedorigins!)));

            /**********************/
            service.AddExceptionHandler<GlobalExeptionHandler>().
                     AddProblemDetails();

            /**********************/
            service.AddIdentity<AppUser,AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


   

            /**********************/
            var connectionstring = configuration.GetConnectionString("DefualtConnection") ??
            throw new InvalidOperationException(" connection string 'DefualtConnection' was not found !!");
            service.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionstring));


            /**********************/
            service.AddSwagger ()
                   .AddMapster ()
                   .Addfluentvalidatuon()
                   .AddAuthConfig(configuration);


            /**********************/
            service.AddHealthChecks()
                   .AddSqlServer(name: "database", connectionString: configuration.GetConnectionString("DefualtConnection")!)
              //     .AddHangfire(op => op.MinimumAvailableServers = 1)
                   .AddCheck<MailProviderHealthCheack>(name:"Mail Provider");


            return service; 
        }

        private static IServiceCollection AddSwagger(this IServiceCollection service)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
         

            return service;
        }

        private static IServiceCollection AddMapster(this IServiceCollection service)
        {

            //mapster
            var mappingconfig = TypeAdapterConfig.GlobalSettings;
            mappingconfig.Scan(Assembly.GetExecutingAssembly());
            service.AddSingleton<IMapper>(new Mapper(mappingconfig));

            return service;
        }
        private static IServiceCollection Addfluentvalidatuon(this IServiceCollection service)
        {


            // fluent validatuon
            service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            service.AddFluentValidationAutoValidation();

            return service;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();

            // services.Configure<Jwtoptions>(configuration.GetSection(nameof(Jwtoptions)));

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
              //  options.Lockout.AllowedForNewUsers = false;
            });



            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();



            services.AddOptions<Jwtoptions>()
                    .BindConfiguration(nameof(Jwtoptions))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();


            services.AddOptions<EmailSettings>()
                    .BindConfiguration(nameof(EmailSettings))
                    .ValidateOnStart();

            var settings = configuration.GetSection(nameof(Jwtoptions)).Get<Jwtoptions>(); // the same like the injection of jwtoptions





            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
                    ValidIssuer = settings?.Issuer,
                    ValidAudience = settings?.Audience
                };
            });

            return services;
        }
    }
}
