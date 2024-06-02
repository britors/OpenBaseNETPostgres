using OpenBaseNET.Infra.CrossCutting;
using OpenBaseNET.Presentation.Api;
using System.Reflection;

var ascii = Figgle.FiggleFonts.Standard.Render("OpenBaseNET");
Console.WriteLine(ascii);
Console.WriteLine("Starting application...");
Console.WriteLine("Postgresql Flavor...");
Console.WriteLine($"Version {Assembly.GetEntryAssembly()?.GetName().Version}");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ControllerMiddleware>();
await app.RunAsync();
