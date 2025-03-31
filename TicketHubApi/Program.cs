using TicketHubApi.Services;
using TicketHubApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Queue Service
builder.Services.AddScoped<IQueueService, QueueService>();

// Configure Azure Storage
var azureStorageConnectionString = builder.Configuration["AzureStorageConnectionString"];
if (string.IsNullOrEmpty(azureStorageConnectionString))
{
    throw new InvalidOperationException("Azure Storage connection string is not configured");
}

// Add CORS policy if needed (for when you add the frontend in Assignment 4)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

// Remove the weather forecast endpoint as it's not needed for this application
// app.MapGet("/weatherforecast", () => {...});

app.Run();