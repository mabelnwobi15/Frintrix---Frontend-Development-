var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Add session support (JWT storage)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Retrieve backend API URL from Render environment variable or fall back to local URL
var apiBaseUrl = builder.Configuration["BACKEND_URL"] 
    ?? builder.Configuration["API_BASE_URL"] 
    ?? "https://fintrix-api.onrender.com/";

// Ensure trailing slash for HttpClient route matching
if (!apiBaseUrl.EndsWith("/"))
{
    apiBaseUrl += "/";
}

// Register HttpClient for API communication
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://fintrix-api.onrender.com/");
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Register application services
builder.Services.AddScoped<SmartInvoice.MVC.Services.ClientService>();
builder.Services.AddScoped<SmartInvoice.MVC.Services.InvoiceService>();
builder.Services.AddScoped<SmartInvoice.MVC.Services.PaymentService>();

// Needed to access session inside services
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();
