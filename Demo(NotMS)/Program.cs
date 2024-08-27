using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Entity;
using OnlineCourse.Entity.Repositories;
using OnlineCourse.Entity.Repositories.Interface;
using OnlineCourse.Entity.Settings;
using OnlineCourse.Services;

var builder = WebApplication.CreateBuilder(args);
//Подключение БД
builder.Services.AddDbContext<CourseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth/LogIn";
            options.LogoutPath = "/Auth/Logout";
            options.AccessDeniedPath = "/Auth/AccessDenied";            
        });

//var startup = new Startup(builder.Configuration);

//startup.ConfigureServices(builder.Services);
builder.Services.AddOptions();
builder.Services.AddTransient<IDbContext, CourseDbContext>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IUserRepositories, UserRepositories>();
builder.Services.AddTransient<ICartRepositories, CartRepositories>();
builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

//startup.Configure(app, app.Environment, app.Services.CreateScope().ServiceProvider);
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Course/Error");
    app.UseHsts();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
/*app.MapGet("/", () => { });*/

//var dbContext = app.Services.CreateScope().ServiceProvider.GetService<CourseDbContext>();

//DataBank.Initialize(dbContext);


app.Run();