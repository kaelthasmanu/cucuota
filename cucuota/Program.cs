using System.Text;
using cucuota;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var builderconf = new ConfigurationBuilder();
        var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builderconf.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true
        );
        var jwtOptions = builder.Configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>();

        IConfiguration config = builderconf.Build();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHostedService<AddCuotaBackground>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy  =>
                {
                    policy.WithOrigins("https://localhost/").AllowAnyOrigin();
                });
        });
        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                //convert the string signing key to byte array
                byte[] signingKeyBytes = Encoding.UTF8
                    .GetBytes(jwtOptions.SigningKey);

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
        builder.Services.AddAuthorization();

        builder.Services.Configure<WorkingFiles>(builder.Configuration.GetSection("WorkingFiles"));
        builder.Services.Configure<ConfigLDAP>(builder.Configuration.GetSection("LDAPServer"));

        builder.Services.AddScoped<ChangeCantQuota>();
        builder.Services.AddScoped<LDAPUtils>();
        builder.Services.AddSingleton<UpdateDataCuota>();

        var app = builder.Build();

        app.MapPost("/tokens/connect", (HttpContext ctx, JwtOptions jwtOptions) 
            => TokenEndpoint.Connect(ctx, jwtOptions));

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors(MyAllowSpecificOrigins);

        app.MapControllers();

        app.Run(config.GetSection("URLListen").GetSection("base_url").Value);
    }
}


