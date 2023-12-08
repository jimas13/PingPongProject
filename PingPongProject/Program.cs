using Microsoft.Net.Http.Headers;
using PingPongProject.Workers;

namespace PingPongProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient("PongService", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:44368/");
            });
            builder.Services.AddHostedService<PingRequestTimedHostedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/healthy", (HttpContext httpContext) =>
            {
                return "hey";
            })
            .WithName("GetHealthy");

            app.Run();
        }
    }
}
