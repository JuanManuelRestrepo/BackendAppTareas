using ApiSampleFinal.Automapper;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services;
using System.Net;
using System.Net.Security;

namespace ApiSampleFinal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Agregar servicios al contenedor
            builder.Services.AddServices();
            builder.Services.AddRepositories(configuration);

            // Configurar la conexión a la base de datos
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Defaultconnection"),
              b => b.MigrationsAssembly("ApiSampleFinal")));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configuración de serialización JSON
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            // Configuración de AutoMapper
            var mappingConfiguration = new MapperConfiguration(m => m.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfiguration.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // Configuración de CORS: Permitir solicitudes desde el frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORS_Policy", builder =>
                {
                    // Permite solicitudes desde el frontend en http://127.0.0.1:5500
                    builder.WithOrigins("http://127.0.0.1:5500")  // URL del frontend
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            // Configuración de Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configuración SSL para desarrollo: Asegúrate de que la validación del certificado se permita en desarrollo
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                return app.Environment.IsDevelopment() ? true : errors == SslPolicyErrors.None;
            };

            // Configuración de la canalización de solicitudes HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Usar CORS para permitir solicitudes desde el frontend
            app.UseCors("CORS_Policy");

            // Habilitar redirección HTTPS si el entorno lo requiere
            app.UseHttpsRedirection();

            // Configurar la autorización y los controladores
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
