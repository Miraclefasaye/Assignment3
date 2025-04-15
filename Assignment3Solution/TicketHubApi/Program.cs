using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketHubApi.Data; // <-- Replace with your actual DbContext namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticket Hub API", Version = "v1" });
});

// Load secret config (optional for dev)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("secret.json", optional: true);
}

// ✅ Add database context
builder.Services.AddDbContext<TicketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Hub API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
