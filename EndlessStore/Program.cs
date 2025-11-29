using Confluent.Kafka;
using EndlessStore.Inventory.API.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var opts = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;

    var config = new ProducerConfig
    {
        BootstrapServers = opts.BootstrapServers,
        ClientId = opts.ClientId,
        Acks = opts.Producer.Acks switch
        {
            "-1" => Acks.All,
            "0" => Acks.None,
            "1" => Acks.Leader,
            _ => Acks.All
        },
        EnableIdempotence = opts.Producer.EnableIdempotence,
        CompressionType = opts.Producer.CompressionType switch
        {
            "gzip" => CompressionType.Gzip,
            "snappy" => CompressionType.Snappy,
            "lz4" => CompressionType.Lz4,
            "zstd" => CompressionType.Zstd,
            _ => CompressionType.None
        },
        LingerMs = opts.Producer.LingerMs,
        BatchSize = opts.Producer.BatchSize,
        MessageSendMaxRetries = opts.Producer.Retries
    };

    return new ProducerBuilder<string, string>(config).Build();
});

builder.Services.AddScoped<EndlessStore.Inventory.Core.Application.Services.IKafkaService, EndlessStore.Inventory.Infrastructure.Kafka.KafkaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
