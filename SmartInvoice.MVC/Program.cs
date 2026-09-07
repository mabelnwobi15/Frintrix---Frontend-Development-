

var builder = WebApplication.CreateBuilder(args);



// Add services
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("https://fintrix-frontend.onrender.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add session support (JWT storage)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register HttpClient for API
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

// Register your services
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

app.UseSession();       // <-- session must come before authorization
app.UseAuthorization(); // keep authorization middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();