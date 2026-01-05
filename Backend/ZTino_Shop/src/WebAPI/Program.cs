using WebAPI.DependencyInjection;
using WebAPI.Filters.Response;
using WebAPI.Middleware.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

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
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ===== Data Seeding =====
await app.SeedDataAsync();

app.Run();
