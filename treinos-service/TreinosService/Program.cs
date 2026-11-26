using TreinosService;
using TreinosService.Infra;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// URLs dos outros serviços vêm das variáveis de ambiente.
// Timeout alto porque no Render gratuito o primeiro acesso a um serviço inativo é lento.
var alunosUrl = builder.Configuration["ALUNOS_URL"] ?? "http://localhost:5001";
var exerciciosUrl = builder.Configuration["EXERCICIOS_URL"] ?? "http://localhost:5002";
var avaliacoesUrl = builder.Configuration["AVALIACOES_URL"] ?? "http://localhost:5004";

builder.Services.AddHttpClient("alunos", client =>
{
    client.BaseAddress = new Uri(alunosUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient("exercicios", client =>
{
    client.BaseAddress = new Uri(exerciciosUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient("avaliacoes", client =>
{
    client.BaseAddress = new Uri(avaliacoesUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(60);
});

// (Extra IA) só se fizer a Etapa 10
builder.Services.AddHttpClient("ia", client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

GeradorDeServicos.ServiceProvider = builder.Services.BuildServiceProvider();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
};

forwardedHeadersOptions.KnownIPNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

// Usado para verificar se o serviço está no ar (Render / outros microsserviços).
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();

app.Run();
