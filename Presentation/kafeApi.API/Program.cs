using FluentValidation;
using FluentValidation.AspNetCore;
using kafeApi.API.Middlewares;
using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemsDto;
using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.OrderItemDtos;
using KafeApi.Application.Dtos.TableDtos;
using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Helpers;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Mapping;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Services.Concrete;
using KafeApi.Application.Validators.Category;
using KafeApi.Application.Validators.Table;
using KafeApi.Persistence.Context;
using KafeApi.Persistence.Context.Identity;
using KafeApi.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// 1. Serilog initialization (DO THIS FIRST)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

builder.Services.AddDbContext<AppDbContext>(options =>
options
.UseSqlServer(builder.Configuration
.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppIdentityDbContext>(options => options
.UseSqlServer(builder.Configuration
.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();



builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddScoped<TokenHelpers>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateCategoryDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateCategoryDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateMenuItemDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateMenuItemDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateTableDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateTableDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateOrderDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateOrderDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateOrderItemDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateOrderItemDto));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(RegisterDto));


builder.Services.AddAutoMapper(typeof(GeneralMapping));


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();


app.MapScalarApiReference(options =>
{
    options.Title = "Kafe API V1";
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
}
);




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<SerilogMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        identityContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Veritabani migrasyonu sirasinda bir hata olustu.");
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
