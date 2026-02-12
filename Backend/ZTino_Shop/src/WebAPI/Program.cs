using Serilog;
using WebAPI.DependencyInjection;
using WebAPI.DependencyInjection.CrossCutting;
using WebAPI.Filters.Response;
using WebAPI.Middleware.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// ===== Extension Logging =====
builder.AddSerilogConfig();

// ===== Service Registration =====
builder.Services
    .AddControllers(options => options.Filters.Add<ApiResponseFilter>())
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpClient()
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

// ===== Development Middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ===== Request Pipeline =====
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});
app.UseRateLimiter();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ===== Data Seeding =====
try
{
    Log.Information("Starting API...");
    await app.SeedDataAsync();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
