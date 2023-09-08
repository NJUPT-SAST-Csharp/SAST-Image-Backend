using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SastImgAPI.Filters;
using SastImgAPI.Models;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Models.Validators;
using SastImgAPI.Options;
using SastImgAPI.Services;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure logging program.
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Logging.ClearProviders().AddSerilog();

// Add controllers.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Set JsonSerializerOptions
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add DbContext.
builder.Services.AddDbContext<DatabaseContext>(
    options =>
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
            .UseSnakeCaseNamingConvention()
);

// Add Redis caching.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "SastImg";
});

// Add Identity
builder.Services.AddIdentityCore<User>(options =>
{
    //options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultPhoneProvider;
    options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultPhoneProvider;
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(
    options => options.TokenLifespan = TimeSpan.FromMinutes(5)
);

builder.Services.AddDataProtection();

// Configure the Identity info.
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>()
    .AddSignInManager<SignInManager<User>>();

// Add Validators
builder.Services.AddSingleton<IValidator<RegisterRequestDto>, RegisterValidator>();
builder.Services.AddSingleton<IValidator<LoginRequestDto>, LoginValidator>();
builder.Services.AddSingleton<IValidator<PasswordResetRequestDto>, PasswordResetValidator>();
builder.Services.AddSingleton<IValidator<AlbumRequestDto>, AlbumValidation>();
builder.Services.AddSingleton<IValidator<EmailConfirmRequestDto>, EmailConfirmValidator>();
builder.Services.AddSingleton<IValidator<ImageRequestDto>, ImageValidator>();
builder.Services.AddSingleton<IValidator<ProfileRequestDto>, ProfileValidator>();
builder.Services.AddSingleton<IValidator<TagRequestDto>, TagValidator>();
builder.Services.AddSingleton<IValidator<CategoryRequestDto>, CategoryValidator>();

// Add configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.Configure<EmailSendOption>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("File"));

// Add Jwt
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        SymmetricSecurityKey secKey =
            new(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!.SecKey
                )
            );
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secKey,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256, SecurityAlgorithms.RsaSha256 }
        };
    });

// Add filters.
builder.Services.Configure<MvcOptions>(opts =>
{
    opts.Filters.Add<GlobalExceptionFilter>();
    opts.Filters.Add<LoggerExceptionFilter>();
});

//Add and configure swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        Description = "Authorization Header \r\nExample:'Bearer 123456789'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    };
    options.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    options.AddSecurityRequirement(requirement);
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});

// Add localizer service
builder.Services.AddLocalization();

// Add Custom services
builder.Services.AddTransient<EmailTokenSender>();
builder.Services.AddTransient<JwtTokenGenerator>();
builder.Services.AddTransient<ImageAccessor>();
builder.Services.AddSingleton<CacheAuthAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var languages = builder.Configuration.GetSection("SupportLanguages").Get<string[]>()!;
app.UseRequestLocalization(
    new RequestLocalizationOptions()
        .AddSupportedCultures(languages)
        .AddSupportedUICultures(languages)
        .SetDefaultCulture(languages[0])
);

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
