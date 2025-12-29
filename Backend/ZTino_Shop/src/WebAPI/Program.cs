using Asp.Versioning;
using Infrastructure.Data.Seeds;
using WebAPI.DependencyInjection;
using WebAPI.Filters.Response;
using WebAPI.Middleware.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
});

//API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add HTTP
builder.Services.AddHttpClient();

// Add DI services
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CORS
app.UseCors("AllowAll");

//Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

//Data Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedIdentityData.SeedAsync(services);
}

app.Run();
