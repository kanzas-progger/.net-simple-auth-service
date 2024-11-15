using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAndAuthorization.API.Extensions;
using SimpleAuthAndAuthorization.Application.Services;
using SimpleAuthAndAuthorization.Core.Abstractions;
using SimpleAuthAndAuthorization.Infrastructure;
using SimpleAuthAndAuthorization.Infrastructure.Authentication;
using SimpleAuthAndAuthorization.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddSwaggerGen();


services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.AddApiAuthentication(configuration.GetSection(nameof(JwtOptions)));

services.AddDbContext<SimpleAuthAndAuthorizationDbContext>(opt =>
{
    opt.UseNpgsql(configuration.GetConnectionString("SimpleAuthAndAuthorizationDbContext"));
});

services.AddScoped<IUsersRepository, UsersRepository>();
services.AddScoped<IUsersService, UsersService>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle = "My Swagger";
});

app.UseHttpsRedirection();

app.UseExceptionMiddleware();

app.UseCookiePolicy(new CookiePolicyOptions
{
   MinimumSameSitePolicy = SameSiteMode.Strict,
   HttpOnly = HttpOnlyPolicy.Always,
   Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();