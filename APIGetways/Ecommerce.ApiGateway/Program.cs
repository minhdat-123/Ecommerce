var builder = WebApplication.CreateBuilder(args);

// 1. Load YARP configuration from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Removed default Minimal API endpoint mapping

// 2. Enable YARP reverse proxy middleware
app.MapReverseProxy();

app.Run();
