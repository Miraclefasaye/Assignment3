using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketHubApi.Models;

namespace TicketHubApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<TicketsController> _logger;
    private const string QueueName = "tickethub"; // Lowercase as per Azure naming requirements

    public TicketsController(IConfiguration config, ILogger<TicketsController> logger)
    {
        _config = config;
        _logger = logger;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchase purchase)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationErrors = ValidatePurchase(purchase);
        if (validationErrors.Count > 0) // Using Count instead of Any()
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var connectionString = _config["AzureStorageConnectionString"];
            var queueClient = new QueueClient(connectionString, QueueName);
            await queueClient.CreateIfNotExistsAsync();

            if (!await queueClient.ExistsAsync())
            {
                return StatusCode(500, "Queue not available");
            }

            var message = JsonConvert.SerializeObject(purchase);
            await queueClient.SendMessageAsync(message);
            return Ok(new { Message = "Ticket purchase received" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing ticket");
            return StatusCode(500, $"Processing error: {ex.Message}");
        }
    }

    private static List<string> ValidatePurchase(TicketPurchase purchase)
    {
        var errors = new List<string>();

        if (purchase.Quantity <= 0)
        {
            errors.Add("Quantity must be positive");
        }

        if (string.IsNullOrWhiteSpace(purchase.Email))
        {
            errors.Add("Email required");
        }

        // Add other validations...

        return errors;
    }
}