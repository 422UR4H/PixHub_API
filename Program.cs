using PixHub.Data;
using PixHub.Middlewares;
using PixHub.Services;
using Microsoft.EntityFrameworkCore;
using PixHub.Repositories;
using Prometheus;
using PixHub.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string host = builder.Configuration["Database:Host"] ?? string.Empty;
    string port = builder.Configuration["Database:Port"] ?? string.Empty;
    string username = builder.Configuration["Database:Username"] ?? string.Empty;
    string database = builder.Configuration["Database:Database"] ?? string.Empty;
    string password = builder.Configuration["Database:Password"] ?? string.Empty;

    string connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PixKeyService>();
builder.Services.AddScoped<PixKeyRepository>();
builder.Services.AddScoped<PaymentsService>();
builder.Services.AddScoped<PaymentsRepository>();
builder.Services.AddScoped<PaymentProviderService>();
builder.Services.AddScoped<PaymentProviderRepository>();
builder.Services.AddScoped<PaymentProviderAccountService>();
builder.Services.AddScoped<PaymentProviderAccountRepository>();

builder.Services.AddSingleton<MessageService>();

IConfigurationSection queueConfigurationSection = builder.Configuration.GetSection("QueueSettings");
builder.Services.Configure<QueueConfig>(queueConfigurationSection);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics(options => options.AddCustomLabel("host", context => context.Request.Host.Host));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();
