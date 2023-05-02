using Configurations.Middlewares;
using Serilog;

namespace WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        IsLocal = Configuration.GetValue<bool?>("Local").HasValue && Configuration.GetValue<bool?>("Local").Value == true;
    }

    public IConfiguration Configuration { get; }
    public bool IsLocal { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Error()
        .Enrich.FromLogContext()
        .CreateLogger();
        services.AddSingleton(Log.Logger);

        services.AddHttpContextAccessor();


        services.AddControllers();

        services.AddInfrastructureServices(Configuration);
        services.AddApplicationServices(Configuration);

        if (IsLocal)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        services.AddCors(options =>
        {
            options.AddPolicy(name: "apiCorsPolicy",
                builder =>
                {
                    builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("apiCorsPolicy");
        app.UseMiddleware<ExceptionHandlingMiddleware>();


        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        if (IsLocal)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}