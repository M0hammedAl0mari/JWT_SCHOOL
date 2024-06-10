using lastTest.Repository;
using lastTest.DataBase; // Ensure you have this using statement for MyContext
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using lastTest.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using lastTest.Data;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database context
builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddControllersWithViews();


// Register your repositories here
builder.Services.AddScoped<UserRepository>();

// Register your repositories here
builder.Services.AddScoped<TeacherRepository>();

// Register your repositories here
builder.Services.AddScoped<TokenService>();

// Register your repositories here
builder.Services.AddScoped<CourseRepository>();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options => {
//        options.LoginPath = "/Access/login";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure the JWT Bearer token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Extract the token from the cookie if it exists
            if (context.Request.Cookies.TryGetValue("AuthToken", out string token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // This prevents the automatic 401 unauthorized result
            context.HandleResponse();

            // Redirect to login page
            context.Response.Redirect("/Access/Login");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            // Handle 403 Forbidden: redirect to the login page
            context.Response.Redirect("/Access/Login");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Teacher"));
    options.AddPolicy("StudentOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Student"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is used
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Teacher}/{action=Index}/{id?}");
app.Run();
