using Apex.Venues.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register database context with the framework
builder.Services.AddDbContext<VenuesDbContext>();

// Add the DbInitializer as a scoped service 
builder.Services.AddScoped<DbTestDataInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    AddTestData(app);
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Venues.Api v1");
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();


static void AddTestData(IHost app)
{
    // We must create a new scope to retrieve scoped services 
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Get the DbTestDataInitializer service 
        var initializer = services.GetRequiredService<DbTestDataInitializer>();

        // Run the initialization logic (Synchronous) 
        initializer.Initialize();
    }
}