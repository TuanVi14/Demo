using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication (cookie) and authorization
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.LoginPath = "/Account/Login";
});

// Add EF Core SQL Server
builder.Services.AddDbContext<Demo.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=DemoDb;Trusted_Connection=True;"));

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

// Ensure DB created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Demo.Data.ApplicationDbContext>();
    db.Database.EnsureCreated();

    var connection = db.Database.GetDbConnection();
    try
    {
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"
IF EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'CustomerAddresses' AND s.name = 'dbo'
)
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM sys.columns
        WHERE object_id = OBJECT_ID(N'dbo.CustomerAddresses')
          AND name = 'Country'
    )
    BEGIN
        ALTER TABLE dbo.CustomerAddresses
        ADD Country nvarchar(max) NOT NULL DEFAULT('')
    END
END";
        command.ExecuteNonQuery();
    }
    finally
    {
        connection.Close();
    }

    // Seed default admin user if none exists
    if (!db.Users.Any())
    {
        db.Users.Add(new Demo.Models.User
        {
            Username = "admin",
            PasswordHash = Demo.Models.User.HashPassword("123"),
            Roles = "Admin",
            Address = "Admin HQ",
            CustomerGroup = "VIP"
        });
        db.SaveChanges();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
