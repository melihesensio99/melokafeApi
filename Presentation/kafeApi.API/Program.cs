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
using System.Text;



var builder = WebApplication.CreateBuilder(args);



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
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IMenuItemService), typeof(MenuItemService));
builder.Services.AddScoped(typeof(ITableRepository), typeof(TableRepository));
builder.Services.AddScoped(typeof(ITableService), typeof(TableService));
builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
builder.Services.AddScoped(typeof(IOrderItemService), typeof(OrderItemService));
builder.Services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
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

builder.Services.AddEndpointsApiExplorer();


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

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
