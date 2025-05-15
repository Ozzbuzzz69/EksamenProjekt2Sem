using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using EksamenProjektTest.EFDbContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<OrderService>();
builder.Services.AddSingleton<CampaignOfferService>();
builder.Services.AddSingleton<SandwichService>();
builder.Services.AddSingleton<WarmMealService>();
builder.Services.AddTransient<GenericDbService<Sandwich>>();
builder.Services.AddTransient<GenericDbService<WarmMeal>>();
builder.Services.AddTransient<GenericDbService<CampaignOffer>>();
builder.Services.AddTransient<GenericDbService<Order>>();

builder.Services.AddTransient<GenericDbService<User>>();
builder.Services.AddSingleton<UserService>();

//builder.Services.AddScoped<GenericDbService<User>>();
//builder.Services.AddScoped<UserService>();

// Add DB context
builder.Services.AddDbContext<FoodContext>();

builder.Services.Configure<CookiePolicyOptions>(options => {
    // This lambda determines whether user consent for non-essential cookies is needed for a given request. options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;

});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions => {
    cookieOptions.LoginPath = "/LogInAndOut/LogIn";

});
builder.Services.AddMvc().AddRazorPagesOptions(options => {
    options.Conventions.AuthorizeFolder("/Order");

}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
//    await userService.SeedMockUsersAsync();
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
