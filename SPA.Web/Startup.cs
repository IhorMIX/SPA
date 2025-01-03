using Microsoft.EntityFrameworkCore;
using SPA.BLL.Services;
using SPA.BLL.Services.Interfaces;
using SPA.DAL;
using SPA.DAL.Repositories;
using SPA.DAL.Repositories.Interfaces;
using SPA.Web.Helpers;

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
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") ?? Configuration.GetConnectionString("ConnectionString");

        services.AddDbContext<SPADbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddAutoMapper(typeof(Startup));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        
        services.AddSingleton<TokenHelper>();
        
        services.AddControllers();
        
        services.AddAutoMapper(typeof(Startup));
        
        services.AddJwtAuth();
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
        
        app.UseStaticFiles();
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}