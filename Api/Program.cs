using Api.Extension;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(
    options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    
builder.Services.AddPostgreSqlDbContext(builder.Configuration);
builder.Services.AddPostgreSqlIdentityContext();
builder.Services.AddConfigureIdentityOptions();
builder.Services.AddJwtTokenGenerator();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerGenCustomConfig();
builder.Services.AddCors();
builder.Services.AddShoppingCartService();
builder.Services.AddOrdersService();
builder.Services.AddPaymentService();

var app = builder.Build();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o =>
    o.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("*")
);
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
await app.Services.InitializeRoleAsync();
app.Run();
