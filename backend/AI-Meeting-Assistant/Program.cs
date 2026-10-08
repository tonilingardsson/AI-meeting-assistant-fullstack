using AI_Meeting_Assistant.Services;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(
    $"Environment: {builder.Environment.EnvironmentName}");

Console.WriteLine(
    $"Gemini API key loaded: {!string.IsNullOrWhiteSpace(builder.Configuration["Gemini:ApiKey"])}");

Console.WriteLine(
    $"Gemini model loaded: {!string.IsNullOrWhiteSpace(builder.Configuration["Gemini:Model"])}");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<AgendaService>();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<GeminiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
