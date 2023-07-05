using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Middlewares;
using GenericRepositoryAndUnitofWork.Repositories;
using GenericRepositoryAndUnitofWork.UnitofWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter access token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Config CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Identity config
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BookStoreContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<BookStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStore"));
});

builder.Services.AddMvc();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<SecondMiddleware>();
builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<LoggingMiddleware>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))

    };
});

var app = builder.Build();

app.UseErrorHandlingMiddleware(); // Custom middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.Use(async (context, next) =>
//{
//    // Do work that can write to the Response.
//    //await context.Response.WriteAsync("Default Middleware!\n");
//    context.Items.Add("DataFromDefault", "Default Middleware!\n");
//    await next.Invoke();
//    // Do logging or other work that doesn't write to the Response.
//});

//app.UseStaticFiles("about.html"); 

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/about", async context =>
//    {
//        await context.Response.WriteAsync("Trang gioi thieu");
//    });

//    endpoints.MapGet("/product", async context =>
//    {
//        await context.Response.WriteAsync("Trang San Pham");
//    });
//});


//app.UseMiddleware<FirstMiddleware>();
//app.UseMiddleware<SecondMiddleware>();



//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Terminate Middleware!");
//});


//app.UseLoggingMiddleware();







app.Run();
