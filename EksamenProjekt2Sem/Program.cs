using EksamenProjekt2Sem.AppDbContext;
using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<OrderService, OrderService>();
builder.Services.AddSingleton<CampaignOfferService, CampaignOfferService>();
builder.Services.AddSingleton<UserService, UserService>();
builder.Services.AddSingleton<SandwichService, SandwichService>();
builder.Services.AddSingleton<WarmMealService, WarmMealService>();

// Add DB context
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<GenericDbService<OrderService>, GenericDbService<OrderService>>();
builder.Services.AddTransient<GenericDbService<CampaignOfferService>, GenericDbService<CampaignOfferService>>();
builder.Services.AddTransient<GenericDbService<UserService>, GenericDbService<UserService>>();
builder.Services.AddTransient<GenericDbService<SandwichService>, GenericDbService<SandwichService>>();
builder.Services.AddTransient<GenericDbService<WarmMealService>, GenericDbService<WarmMealService>>();

/*builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=(localdb)\\mssqllocaldb;Database=MyAppDb;Trusted_Connection=True;")));
*/

builder.Services.Configure<CookiePolicyOptions>(options => {
    // This lambda determines whether user consent for non-essential cookies is needed for a given request. options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;

});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions => {
    cookieOptions.LoginPath = "/Login/LogInPage";

});
builder.Services.AddMvc().AddRazorPagesOptions(options => {
    options.Conventions.AuthorizeFolder("/Item");

}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

var app = builder.Build();

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
