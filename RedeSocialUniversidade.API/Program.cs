using RedeSocialUniversidade.Domain.Interface;
using RedeSocialUniversidade.Infra.Repository;
using RedeSocialUniversidade.Application.Services;
using RedeSocialUniversidade.Infra;
using System.Text.Json.Serialization;
using RedeSocialUniversidade.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurações básicas
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

// Registro dos repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPostagemRepository, PostagemRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();

// Registro dos services
builder.Services.AddScoped<UsuarioAppService>();
builder.Services.AddScoped<PostagemAppService>();
builder.Services.AddScoped<EventoAppService>();



builder.Services.AddDbContext<SqlContext>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();