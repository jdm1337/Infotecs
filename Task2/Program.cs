using Task2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<TaskScheduleService>();
builder.Services.AddSingleton<FeedService>();


builder.WebHost.UseStaticWebAssets();

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();
var feedService = app.Services.GetRequiredService<FeedService>();
feedService.Start();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
