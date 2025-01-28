using Microsoft.EntityFrameworkCore;
using SPA.BLL.Services;
using SPA.BLL.Services.Interfaces;
using SPA.DAL;
using SPA.DAL.Repositories;
using SPA.DAL.Repositories.Interfaces;
using SPA.Web.Extensions;
using SPA.Web.Helpers;
using Newtonsoft.Json.Converters;

namespace SPA.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(opt => 
            opt.SerializerSettings.Converters.Add(new StringEnumConverter()));
               
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") ?? Configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<SPADbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddAutoMapper(typeof(Startup));
        
        services.AddScoped<TokenHelper>();
        services.AddJwtAuth();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
    }
    
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
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlingMiddleware>(); 
        
        app.UseRouting();
        app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}