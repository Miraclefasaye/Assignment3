// Services/IQueueService.cs
public interface IQueueService
{
    Task SendMessageAsync(TicketPurchase purchase);
}

// Services/QueueService.cs
public class QueueService : IQueueService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<QueueService> _logger;

    public QueueService(IConfiguration configuration, ILogger<QueueService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendMessageAsync(TicketPurchase purchase)
    {
        try
        {
            var connectionString = _configuration["AzureStorageConnectionString"];
            var queueName = "tickethub";

            var queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            {
                var message = JsonSerializer.Serialize(purchase);
                await queueClient.SendMessageAsync(message);
                _logger.LogInformation("Message sent to queue");
            }
            else
            {
                _logger.LogError("Queue does not exist");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to queue");
            throw;
        }
    }
}