using ClinicAPI.Auth;
using ClinicAPI.Models;
using ClinicAPI.Packages;
using ClinicAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IJwtManager, JwtManager>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<PKG_DOCTORS>();

// Configure MailjetSettings
builder.Services.Configure<MailjetSettings>(builder.Configuration.GetSection("Mailjet"));
builder.Services.AddScoped<IPKG_EMAIL_CONF, PKG_EMAIL_CONF>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// Swagger Configuration
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //builder.Services.Configure<MailjetSettings>(builder.Configuration.GetSection("Mailjet"));
    //builder.Services.AddScoped<IPKG_EMAIL_CONF, PKG_EMAIL_CONF>();



    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // Configure Swagger to display enum names for DoctorCategory as dropdown
    option.MapType<DoctorCategory>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(DoctorCategory))
            .Select(name => new OpenApiString(name))
            .Cast<IOpenApiAny>()
            .ToList()
    });


    //option.MapType<Role>(() => new OpenApiSchema
    //{
    //    Type = "string",
    //    Enum = Enum.GetNames(typeof(Role))
    //    .Select(name => new OpenApiString(name))
    //    .Cast<IOpenApiAny>()
    //    .ToList()
    //});

    option.MapType<Role>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(Role))
        .Select(name => new OpenApiString(name))
        .Cast<IOpenApiAny>()
        .ToList()
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp"); // Enable CORS policy here

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
