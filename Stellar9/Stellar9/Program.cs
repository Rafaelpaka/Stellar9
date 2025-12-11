using Stellar9.Repositories;
using Stellar9.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PortoRepository>();
builder.Services.AddScoped<NaveService>();
builder.Services.AddScoped<ViagemService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;

        options.JsonSerializerOptions.WriteIndented = false;

        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SpacePort 9 API",
        Version = "v1",
        Description = "API de Logística Interplanetária para gerenciar naves e viagens espaciais",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "SpacePort 9 Team",
            Email = "contato@spaceport9.space"
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpacePort 9 API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<PortoRepository>();

    var navesIniciais = new[]
    {
        new Stellar9.modelos.Nave
        {
            Id = Guid.NewGuid(),
            Nome = "Millennium Falcon",
            Modelo = "YT-1300",
            CapacidadeCargaKG = 100000,
            Status = Stellar9.modelos.Nave.StatusNave.EmOrbita
        },
        new Stellar9.modelos.Nave
        {
            Id = Guid.NewGuid(),
            Nome = "Enterprise",
            Modelo = "NCC-1701",
            CapacidadeCargaKG = 500000,
            Status = Stellar9.modelos.Nave.StatusNave.EmOrbita
        },
        new Stellar9.modelos.Nave
        {
            Id = Guid.NewGuid(),
            Nome = "Serenity",
            Modelo = "Firefly",
            CapacidadeCargaKG = 75000,
            Status = Stellar9.modelos.Nave.StatusNave.Manutencao
        }
    };

    foreach (var nave in navesIniciais)
    {
        repository.AdicionarNave(nave);
    }

    Console.WriteLine($"Seed completo: {navesIniciais.Length} naves adicionadas ao sistema.");
}

app.Run();
