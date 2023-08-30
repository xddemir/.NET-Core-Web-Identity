using Microsoft.AspNetCore.Authorization;
using WebApp.IdentityExamaple.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", opt =>
{
    opt.Cookie.Name = "MyCookieAuth";
    opt.LoginPath = "/Account/Login";
    opt.ExpireTimeSpan = TimeSpan.FromSeconds(200);
    // opt.AccessDeniedPath=""
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("HRManagerOnly", policy =>
    {
        policy.RequireClaim("Department", "HR")
            .RequireClaim("Manager")
            .Requirements.Add(new HRManagerProbationRequirement(3));
    });
    
    option.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim("Admin");
    });
    
    option.AddPolicy("MustBelongToHrDepartment", policy =>
    {
        policy.RequireClaim("Department", "HR");
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

builder.Services.AddHttpClient("MyWebApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7271/");
});

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();