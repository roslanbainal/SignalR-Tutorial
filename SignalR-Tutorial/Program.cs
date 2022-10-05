using SignalR_Tutorial.Hubs;
using SignalR_Tutorial.MiddlewareExtensions;
using SignalR_Tutorial.SubscribeTableDependecies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// DI
builder.Services.AddSingleton<MyHub>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();

var app = builder.Build();

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHub<MyHub>("/myHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


/*
 * we must call SubscribeTableDependency() here
 * we create one middleware and call SubscribeTableDependency() method in the middleware
 */

app.UseSqlTableDependency<SubscribeProductTableDependency>(connectionString);


app.Run();
