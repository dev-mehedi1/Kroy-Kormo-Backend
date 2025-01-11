using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sob_Need_Server.Context;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Mappings;
using Sob_Need_Server.Services.Authentication;
using Sob_Need_Server.Services.CategoryService;
using Sob_Need_Server.Services.ProductService;
using Sob_Need_Server.SignalR;
using Sob_Need_Server.Utils.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddSignalR(); 

// Register db context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });


builder.Services.AddAutoMapper(typeof(MappingProfile)); // Register AutoMapper
// Register ProductService
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenHelper>();


//// Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowLocalhost", policy =>
//    {
//        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});



// Update CORS policy to allow credentials
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        // Specify the exact origin (don't use '*')
        policy.WithOrigins("http://localhost:5173")  // Vue.js running on localhost:5173
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();  // Allow credentials (cookies, authorization headers, etc.)
    });
});

var app = builder.Build();
app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS with the "AllowLocalhost" policy
app.UseCors("AllowLocalhost");

app.UseAuthorization();
// SignalR Configure
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
