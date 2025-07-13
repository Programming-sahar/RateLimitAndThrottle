using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});


// Rate Limiting setup By .NET Core (System.Threading.RateLimiting)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 3; // فقط 3 درخواست
        options.Window = TimeSpan.FromMinutes(1); // در هر دقیقه
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0; // صف انتظار نذاریم
    });

    // 🔽 تعیین پاسخ سفارشی هنگام رد درخواست
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});



var app = builder.Build();

// Rate Limiting (.NET Core)
app.UseRateLimiter();


// ✅ Swagger UI را همیشه فعال کن
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = ""; // این خط باعث میشه وقتی آدرس root پروژه رو باز کنی، مستقیم Swagger بیاد
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
