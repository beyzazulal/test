var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services for controllers
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication services (assuming JWT authentication)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer",
            ValidAudience = "yourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
        };
    });

// Register your BL services
builder.Services.AddScoped<UserBL>();
builder.Services.AddScoped<TeamLeaderBL>();

// Register DAL (if necessary, for example, as a singleton if it doesn't contain state)
builder.Services.AddSingleton<DAL.DAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Optional: sets Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Add this to ensure the app uses authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
