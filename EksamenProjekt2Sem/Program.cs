using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using EksamenProjektTest.EFDbContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddTransient<GenericDbService<Order>>();
builder.Services.AddTransient<GenericDbService<User>>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CampaignOfferService>();
builder.Services.AddTransient<GenericDbService<CampaignOffer>>();
builder.Services.AddSingleton<SandwichService>();
builder.Services.AddTransient<GenericDbService<Sandwich>>();
builder.Services.AddSingleton<WarmMealService>();
builder.Services.AddTransient<GenericDbService<WarmMeal>>();





//Getting mock data into the database og så skal de andre services ud kommenteres når de er skal bruges

//builder.Services.AddScoped<GenericDbService<User>>();
//builder.Services.AddScoped<UserService>();

//builder.Services.AddScoped<GenericDbService<Sandwich>>();
//builder.Services.AddScoped<SandwichService>();

//builder.Services.AddScoped<GenericDbService<WarmMeal>>();
//builder.Services.AddScoped<WarmMealService>();

//builder.Services.AddScoped<GenericDbService<CampaignOffer>>();
//builder.Services.AddScoped<CampaignOfferService>();

// Add DB context
builder.Services.AddDbContext<FoodContext>();

builder.Services.Configure<CookiePolicyOptions>(options => {
    // This lambda determines whether user consent for non-essential cookies is needed for a given request. options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;

});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions =>
{
    cookieOptions.LoginPath = "/LogInAndOut/LogIn";
    cookieOptions.AccessDeniedPath = "/LogInAndOut/AccessDenied";

});




var app = builder.Build();

//Getting mock data into the database

//using (var scope = app.Services.CreateScope())
//{
//    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
//    var userService2 = scope.ServiceProvider.GetRequiredService<SandwichService>();
//    var userService3 = scope.ServiceProvider.GetRequiredService<WarmMealService>();
//    var userService4 = scope.ServiceProvider.GetRequiredService<CampaignOfferService>();

//    await userService.SeedMockUsersAsync();
//    await userService2.SeedSandwichAsync();
//    await userService3.SeedWarmMealAsync();
//    await userService4.SeedCampaignAsync();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
