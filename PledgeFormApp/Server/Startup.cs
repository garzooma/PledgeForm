using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace PledgeFormApp.Server
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllersWithViews();
      services.AddRazorPages();
      services.AddTransient<AppDb>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));
      services.AddScoped<IPledgersRepository, PledgersRepository>();
      services.AddScoped<IDonationsRepository, DonationsRepository>();
      services.AddScoped<IInstallmentsRepository, InstallmentsRepository>();
      services.AddScoped<IEnvelopesRepository, EnvelopesRepository>();
      services.AddScoped<IDisplayEnvelopesRepository, DisplayEnvelopesRepository>();
      services.AddSwaggerDocument();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebAssemblyDebugging();
        app.UseOpenApi();
        app.UseSwaggerUi3();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseBlazorFrameworkFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("index.html");
      });
    }
  }
}
