using System.Text;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.Middleware;
using GestaoLogisticaAPI.Application.Services;
using FluentValidation;
using GestaoLogisticaAPI.Infrastructure.Context;
using GestaoLogisticaAPI.Infrastructure.Repositories;
using GestaoLogisticaAPI.Infrastructure.Routing;
using GestaoLogisticaAPI.Infrastructure.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GestaoLogisticaAPI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Detecta versão do servidor corretamente para que o provedor gere SQL compatível
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        // =====================================
        // Database (MySQL Pomelo) — NET 9
        // =====================================
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions => mySqlOptions.EnableStringComparisonTranslations()
            )
        );

        // =====================================
        // Controllers + Route Prefix Global
        // =====================================
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Insert(0, new ApiRoutePrefixConvention("api/v1"));
        });

        // =====================================
        // AutoMapper + FluentValidation
        // =====================================
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        // =====================================
        // Dependency Injection (Repository + Service)
        // =====================================
        builder.Services.AddScoped<IArmazemRepository, ArmazemRepository>();
        builder.Services.AddScoped<IArmazemService, ArmazemService>();

        builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
        builder.Services.AddScoped<IClienteService, ClienteService>();

        builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        builder.Services.AddScoped<IEnderecoService, EnderecoService>();

        builder.Services.AddScoped<IEntregaProdutoRepository, EntregaProdutoRepository>();
        builder.Services.AddScoped<IEntregaProdutoService, EntregaProdutoService>();

        builder.Services.AddScoped<IEntregaRepository, EntregaRepository>();
        builder.Services.AddScoped<IEntregaService, EntregaService>();

        builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        builder.Services.AddScoped<IEstoqueService, EstoqueService>();

        builder.Services.AddScoped<IMotoristaRepository, MotoristaRepository>();
        builder.Services.AddScoped<IMotoristaService, MotoristaService>();

        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();

        builder.Services.AddScoped<IRastreamentoRepository, RastreamentoRepository>();
        builder.Services.AddScoped<IRastreamentoService, RastreamentoService>();

        builder.Services.AddScoped<IRotaRepository, RotaRepository>();
        builder.Services.AddScoped<IRotaService, RotaService>();

        builder.Services.AddScoped<ITransportadoraRepository, TransportadoraRepository>();
        builder.Services.AddScoped<ITransportadoraService, TransportadoraService>();

        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        builder.Services.AddScoped<IVeiculoService, VeiculoService>();


        // =====================================
        // CORS
        // =====================================
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowVue",
                policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        // =====================================
        // JWT Auth
        // =====================================
        var secretKey = builder.Configuration["Jwt:Key"]
                        ?? throw new Exception("Configure a porra da chave JWT no appsettings.json");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey)
                    )
                };
            });

        // =====================================
        // Swagger / OpenAPI
        // =====================================
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<AuthFirstDocumentFilter>();
            
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = builder.Configuration["Api:Title"] ?? "Gestão Logística API",
                Version = builder.Configuration["Api:Version"] ?? "v1.0.0",
            });

            // --- DEFINIÇÃO DO BEARER AUTH ---
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Insira o token JWT assim: Bearer {seu_token}",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // --- SEGURANÇA GLOBAL ---
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });


        var app = builder.Build();

        // =====================================
        // Middlewares globais
        // =====================================
        app.UseMiddleware<ExceptionLoggingMiddleware>();

        //app.UseHttpsRedirection();

        app.UseCors("AllowVue");

        app.UseAuthentication();
        app.UseAuthorization();

        // =====================================
        // Swagger — Ambiente DEV
        // =====================================
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}